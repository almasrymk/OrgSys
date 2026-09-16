import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { Account } from '../../../../accounting/accounts/models/account.model';
import { AccountService } from '../../../../accounting/accounts/services/account.service';
import { Currency } from '../../../../master-data/currencies/models/currency.model';
import { CurrencyService } from '../../../../master-data/currencies/services/currency.service';
import { FinancialAccountType } from '../../models/financial-account.model';
import { Bank, BankBranch } from '../../models/bank-lookup.model';
import { FinancialAccountService } from '../../services/financial-account.service';
import { BankService, BankBranchService } from '../../services/bank-lookup.service';
import { BranchService } from '../../services/branch-lookup.service';

interface BranchOption {
  id: number;
  name: string | null;
}

/** Replaces Areas/Setting/Views/FinancialAccount/Save.cshtml — one form, fields swap by financialAccountType. */
@Component({
  selector: 'app-financial-account-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './financial-account-form.component.html',
})
export class FinancialAccountFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly financialAccountService = inject(FinancialAccountService);
  private readonly accountService = inject(AccountService);
  private readonly currencyService = inject(CurrencyService);
  private readonly bankService = inject(BankService);
  private readonly bankBranchService = inject(BankBranchService);
  private readonly branchService = inject(BranchService);
  private readonly toast = inject(ToastService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  readonly financialAccountType = this.route.snapshot.data['financialAccountType'] as FinancialAccountType;
  readonly title = this.route.snapshot.data['title'] as string;
  readonly basePath = this.route.snapshot.data['basePath'] as string;
  readonly isBank = this.financialAccountType === FinancialAccountType.Bank;

  readonly id = signal<number | null>(null);
  readonly code = signal<string | null>(null);
  readonly saving = signal(false);
  readonly glAccounts = signal<Account[]>([]);
  readonly currencies = signal<Currency[]>([]);
  readonly branches = signal<BranchOption[]>([]);
  readonly banks = signal<Bank[]>([]);
  readonly allBankBranches = signal<BankBranch[]>([]);

  readonly form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
    accountId: [0, [Validators.required, Validators.min(1)]],
    currencyId: [0],
    isActive: [true],
    branchId: [0],
    bankId: [0],
    bankBranchId: [0],
    accountNumber: [''],
    iban: [''],
    swiftCode: [''],
  });

  constructor() {
    this.accountService.getList({ pageSize: 5000 }).subscribe((r) => this.glAccounts.set((r.response ?? []).filter((a) => a.isPostable)));
    this.currencyService.getList({ pageSize: 500 }).subscribe((r) => this.currencies.set(r.response ?? []));

    if (!this.isBank) {
      this.branchService.getList({ pageSize: 500 }).subscribe((r) => this.branches.set(r.response ?? []));
    } else {
      this.bankService.getList({ pageSize: 500 }).subscribe((r) => this.banks.set(r.response ?? []));
      this.bankBranchService.getList({ pageSize: 2000 }).subscribe((r) => this.allBankBranches.set(r.response ?? []));
    }

    const editId = this.route.snapshot.paramMap.get('id');
    if (editId) {
      const id = Number(editId);
      this.id.set(id);
      this.financialAccountService.getById(id).subscribe((result) => {
        const account = result.response;
        if (!account) return;

        this.form.patchValue({
          name: account.name,
          accountId: account.accountId ?? 0,
          currencyId: account.currencyId ?? 0,
          isActive: account.isActive,
          branchId: account.branchId ?? 0,
          bankId: account.bankId ?? 0,
          bankBranchId: account.bankBranchId ?? 0,
          accountNumber: account.accountNumber ?? '',
          iban: account.iban ?? '',
          swiftCode: account.swiftCode ?? '',
        });
        this.code.set(account.code);
      });
    } else {
      this.financialAccountService.getMaxCodeNumber(this.financialAccountType).subscribe((max) => this.code.set(String((max || 0) + 1)));
    }
  }

  branchesForSelectedBank(): BankBranch[] {
    const bankId = this.form.controls.bankId.value;
    return this.allBankBranches().filter((b) => b.bankId === bankId);
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving.set(true);
    const value = this.form.getRawValue();
    const id = this.id() ?? undefined;

    const codeNumber = Number(this.code() ?? 0);

    const payload = {
      id,
      name: value.name,
      financialAccountType: this.financialAccountType,
      typeId: this.financialAccountType,
      code: this.code() ?? String(codeNumber),
      codeNumber,
      accountId: value.accountId,
      currencyId: value.currencyId || null,
      isActive: value.isActive,
      ...(this.isBank
        ? { bankId: value.bankId || null, bankBranchId: value.bankBranchId || null, accountNumber: value.accountNumber, iban: value.iban, swiftCode: value.swiftCode }
        : { branchId: value.branchId || null }),
    };

    const request$ = id ? this.financialAccountService.update(payload) : this.financialAccountService.create(payload);

    request$.subscribe({
      next: (result) => {
        this.saving.set(false);
        if (isApiSuccess(result)) {
          this.toast.success('Saved.');
          this.router.navigateByUrl(this.basePath);
        } else {
          this.toast.error(result.errors?.[0]?.messageError ?? 'Save failed.');
        }
      },
      error: () => {
        this.saving.set(false);
        this.toast.error('Save failed.');
      },
    });
  }
}
