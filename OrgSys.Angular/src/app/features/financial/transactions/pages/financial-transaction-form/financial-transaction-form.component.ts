import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../../../core/auth/auth.service';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { Account } from '../../../../accounting/accounts/models/account.model';
import { AccountService } from '../../../../accounting/accounts/services/account.service';
import { Currency } from '../../../../administration/currencies/models/currency.model';
import { CurrencyService } from '../../../../administration/currencies/services/currency.service';
import { FinancialAccount } from '../../../financial-accounts/models/financial-account.model';
import { FinancialAccountService } from '../../../financial-accounts/services/financial-account.service';
import { Dealer, DealerType } from '../../../../customers-suppliers/dealers/models/dealer.model';
import { DealerService } from '../../../../customers-suppliers/dealers/services/dealer.service';
import { FinancialReferenceType, FinancialTransactionDirection, FinancialType } from '../../models/financial.model';
import { FinancialService } from '../../services/financial.service';
import { FinancialTypeService } from '../../services/financial-type.service';

/**
 * Replaces Areas/Financials/Views/Financial/Save.cshtml for every FinancialType except Opening
 * Balance (a separate Draft-then-Post flow, out of scope here — see financial.service.ts). One
 * form, behavior driven entirely by the loaded FinancialType (title/icon, Direction lock, and
 * whether a Customer/Supplier/Expense/Income reference replaces the manual Counter Account picker)
 * — mirrors the same "type param drives everything" pattern used for Journal.
 *
 * Reference types are intentionally limited to Customer/Supplier/Expense/Income here — these are the
 * only ones PostTransactionCommandHandler resolves against a real linked account (see its
 * ResolveReference). Invoice/Payment/Transfer/Employee/Loan/Cheque/PaymentGateway references are not
 * offered; picking "Other" and a manual Counter Account covers every FinancialType that isn't
 * Receipt/Payment.
 */
@Component({
  selector: 'app-financial-transaction-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './financial-transaction-form.component.html',
})
export class FinancialTransactionFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly auth = inject(AuthService);
  private readonly toast = inject(ToastService);
  private readonly financialService = inject(FinancialService);
  private readonly financialTypeService = inject(FinancialTypeService);
  private readonly financialAccountService = inject(FinancialAccountService);
  private readonly currencyService = inject(CurrencyService);
  private readonly accountService = inject(AccountService);
  private readonly dealerService = inject(DealerService);

  readonly FinancialReferenceType = FinancialReferenceType;
  readonly FinancialTransactionDirection = FinancialTransactionDirection;

  readonly financialTypeId = Number(this.route.snapshot.paramMap.get('typeId'));
  readonly financialType = signal<FinancialType | null>(null);
  readonly saving = signal(false);

  readonly financialAccounts = signal<FinancialAccount[]>([]);
  readonly currencies = signal<Currency[]>([]);
  readonly glAccounts = signal<Account[]>([]);
  readonly customers = signal<Dealer[]>([]);
  readonly suppliers = signal<Dealer[]>([]);

  /** Receipt(2) / Payment(3) only — see PostTransactionCommandHandler.ResolveReference. */
  readonly showsReference = this.financialTypeId === 2 || this.financialTypeId === 3;

  readonly expenseAccounts = computed(() => this.glAccounts().filter((a) => a.accountTypeName === 'Expense'));
  readonly incomeAccounts = computed(() => this.glAccounts().filter((a) => a.accountTypeName === 'Revenue'));

  readonly form = this.fb.nonNullable.group({
    financialAccountId: [0, [Validators.required, Validators.min(1)]],
    direction: [FinancialTransactionDirection.In as number, Validators.required],
    amount: [0, [Validators.required, Validators.min(0.01)]],
    currencyId: [0, [Validators.required, Validators.min(1)]],
    exchangeRate: [1, [Validators.required, Validators.min(0.0001)]],
    transactionDate: [new Date().toISOString().slice(0, 10), Validators.required],
    referenceType: [FinancialReferenceType.Other as number],
    referenceId: [0],
    counterAccountId: [0],
    description: [''],
  });

  /**
   * Deliberately a plain method, not `computed()` — it reads a ReactiveFormsModule FormControl's
   * `.value`, a plain mutable property, not a signal, so `computed()` would never see it change and
   * would cache whatever it evaluated to on the very first read (caught live: it cached `true` from
   * the form's initial Other-typed state and never re-evaluated after switching to Customer/Supplier/
   * Expense/Income, silently blocking every Receipt/Payment submit needing a reference). Angular's
   * template change detection calls this every cycle regardless, so a plain method is correctly
   * reactive here without needing `toSignal(control.valueChanges)`.
   */
  referenceNeedsCounterAccount(): boolean {
    const ref = this.form.controls.referenceType.value;
    return ref !== FinancialReferenceType.Customer && ref !== FinancialReferenceType.Supplier && ref !== FinancialReferenceType.Expense && ref !== FinancialReferenceType.Income;
  }

  constructor() {
    this.financialTypeService.getById(this.financialTypeId).subscribe((r) => {
      const type = r.response;
      this.financialType.set(type);
      if (type && type.inOut !== 0) {
        const locked = type.inOut > 0 ? FinancialTransactionDirection.In : FinancialTransactionDirection.Out;
        this.form.controls.direction.setValue(locked);
        this.form.controls.direction.disable();
      }
    });

    this.financialAccountService.getList({ pageSize: 2000 }).subscribe((r) => this.financialAccounts.set((r.response ?? []).filter((a) => a.isActive)));
    this.currencyService.getList({ pageSize: 500 }).subscribe((r) => this.currencies.set(r.response ?? []));
    this.accountService.getList({ pageSize: 5000 }).subscribe((r) => this.glAccounts.set((r.response ?? []).filter((a) => a.isPostable)));

    if (this.showsReference) {
      this.dealerService.getList({ typeId: DealerType.Client, pageSize: 2000 }).subscribe((r) => this.customers.set(r.response ?? []));
      this.dealerService.getList({ typeId: DealerType.Supplier, pageSize: 2000 }).subscribe((r) => this.suppliers.set(r.response ?? []));
    }
  }

  onCurrencyChange(currencyId: number): void {
    const currency = this.currencies().find((c) => c.id === currencyId);
    if (currency) this.form.controls.exchangeRate.setValue(currency.rate);
  }

  onReferenceTypeChange(): void {
    this.form.controls.referenceId.setValue(0);
  }

  referenceOptions(): { id: number; label: string }[] {
    switch (this.form.controls.referenceType.value) {
      case FinancialReferenceType.Customer:
        return this.customers().map((d) => ({ id: d.id, label: d.name ?? '' }));
      case FinancialReferenceType.Supplier:
        return this.suppliers().map((d) => ({ id: d.id, label: d.name ?? '' }));
      case FinancialReferenceType.Expense:
        return this.expenseAccounts().map((a) => ({ id: a.id, label: `${a.code} ${a.name}` }));
      case FinancialReferenceType.Income:
        return this.incomeAccounts().map((a) => ({ id: a.id, label: `${a.code} ${a.name}` }));
      default:
        return [];
    }
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.getRawValue();

    if (this.showsReference && value.referenceType !== FinancialReferenceType.Other && !value.referenceId) {
      this.toast.error('Select a reference.');
      return;
    }
    if (this.referenceNeedsCounterAccount() && !value.counterAccountId) {
      this.toast.error('Select a counter account.');
      return;
    }

    this.saving.set(true);

    const isDealerReference = value.referenceType === FinancialReferenceType.Customer || value.referenceType === FinancialReferenceType.Supplier;

    this.financialService
      .postTransaction({
        financialAccountId: value.financialAccountId,
        financialTypeId: this.financialTypeId,
        direction: value.direction,
        amount: value.amount,
        currencyId: value.currencyId,
        exchangeRate: value.exchangeRate,
        transactionDate: value.transactionDate,
        referenceType: this.showsReference ? value.referenceType : FinancialReferenceType.Other,
        referenceId: this.showsReference && value.referenceType !== FinancialReferenceType.Other ? value.referenceId : null,
        referenceNumber: null,
        counterAccountId: value.counterAccountId,
        dealerId: isDealerReference ? value.referenceId : null,
        description: value.description || null,
        createUserId: this.auth.currentUser()?.id ?? 0,
        branchId: null,
        shiftId: null,
      })
      .subscribe({
        next: (result) => {
          this.saving.set(false);
          if (isApiSuccess(result)) {
            this.toast.success('Transaction posted.');
            this.router.navigate(['/financial/transactions', this.financialTypeId]);
          } else {
            this.toast.error(result.errors?.[0]?.messageError ?? 'Post failed.');
          }
        },
        error: () => {
          this.saving.set(false);
          this.toast.error('Post failed.');
        },
      });
  }
}
