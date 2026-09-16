import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../../../core/auth/auth.service';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { Currency } from '../../../../administration/currencies/models/currency.model';
import { CurrencyService } from '../../../../administration/currencies/services/currency.service';
import { FinancialAccount } from '../../../financial-accounts/models/financial-account.model';
import { FinancialAccountService } from '../../../financial-accounts/services/financial-account.service';
import { FinancialTransferService } from '../../services/financial-transfer.service';

/** Replaces Areas/Financials/Views/FinancialTransfer/Save.cshtml — CashBox/Bank in either direction. */
@Component({
  selector: 'app-financial-transfer-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './financial-transfer-form.component.html',
})
export class FinancialTransferFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly transferService = inject(FinancialTransferService);
  private readonly financialAccountService = inject(FinancialAccountService);
  private readonly currencyService = inject(CurrencyService);
  private readonly auth = inject(AuthService);
  private readonly toast = inject(ToastService);
  private readonly router = inject(Router);

  readonly saving = signal(false);
  readonly financialAccounts = signal<FinancialAccount[]>([]);
  readonly currencies = signal<Currency[]>([]);

  readonly form = this.fb.nonNullable.group({
    fromFinancialAccountId: [0, [Validators.required, Validators.min(1)]],
    toFinancialAccountId: [0, [Validators.required, Validators.min(1)]],
    amount: [0, [Validators.required, Validators.min(0.01)]],
    currencyId: [0, [Validators.required, Validators.min(1)]],
    exchangeRate: [1, [Validators.required, Validators.min(0.0001)]],
    transactionDate: [new Date().toISOString().slice(0, 10), Validators.required],
    description: [''],
  });

  constructor() {
    this.financialAccountService.getList({ pageSize: 2000 }).subscribe((r) => this.financialAccounts.set((r.response ?? []).filter((a) => a.isActive)));
    this.currencyService.getList({ pageSize: 500 }).subscribe((r) => this.currencies.set(r.response ?? []));
  }

  onCurrencyChange(currencyId: number): void {
    const currency = this.currencies().find((c) => c.id === currencyId);
    if (currency) this.form.controls.exchangeRate.setValue(currency.rate);
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.getRawValue();
    if (value.fromFinancialAccountId === value.toFinancialAccountId) {
      this.toast.error('Source and destination accounts must be different.');
      return;
    }

    this.saving.set(true);

    this.transferService
      .post({
        fromFinancialAccountId: value.fromFinancialAccountId,
        toFinancialAccountId: value.toFinancialAccountId,
        amount: value.amount,
        currencyId: value.currencyId,
        exchangeRate: value.exchangeRate,
        transactionDate: value.transactionDate,
        description: value.description || null,
        createUserId: this.auth.currentUser()?.id ?? 0,
        branchId: null,
        shiftId: null,
      })
      .subscribe({
        next: (result) => {
          this.saving.set(false);
          if (isApiSuccess(result)) {
            this.toast.success('Transfer posted.');
            this.router.navigateByUrl('/financial/transfers');
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
