import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../../../core/auth/auth.service';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { FinancialAccount } from '../../../financial-accounts/models/financial-account.model';
import { FinancialAccountService } from '../../../financial-accounts/services/financial-account.service';
import { Currency } from '../../../../administration/currencies/models/currency.model';
import { CurrencyService } from '../../../../administration/currencies/services/currency.service';
import { OpeningBalanceService } from '../../services/opening-balance.service';

/**
 * Replaces Areas/Financials/Views/Financial/Save.cshtml's Opening Balance branch
 * (FinancialController.SaveOpeningBalanceDraft). Saves a Draft only — Posting is a separate action
 * from the list, mirroring Journal's Draft/Post/Reverse split. The FiscalYear field MVC shows is
 * UI-only there too (never persisted, not checked by CreateFinancialCommandValidator — see
 * FinancialDto.FiscalYearId's own comment) so it's omitted here; Date alone drives fiscal-period
 * resolution server-side, same as every other Financial/Journal save.
 */
@Component({
  selector: 'app-opening-balance-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './opening-balance-form.component.html',
})
export class OpeningBalanceFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly auth = inject(AuthService);
  private readonly toast = inject(ToastService);
  private readonly service = inject(OpeningBalanceService);
  private readonly financialAccountService = inject(FinancialAccountService);
  private readonly currencyService = inject(CurrencyService);

  readonly id = signal<number | null>(null);
  readonly saving = signal(false);
  readonly financialAccounts = signal<FinancialAccount[]>([]);
  readonly currencies = signal<Currency[]>([]);

  private originalCreateUserId: number | null = null;
  private originalCreateDate: string | null = null;

  readonly form = this.fb.nonNullable.group({
    financialAccountId: [0, [Validators.required, Validators.min(1)]],
    date: [new Date().toISOString().slice(0, 10), Validators.required],
    amount: [0, [Validators.required, Validators.min(0.01)]],
    currencyId: [0, [Validators.required, Validators.min(1)]],
    rate: [1, [Validators.required, Validators.min(0.0001)]],
    notes: [''],
  });

  constructor() {
    this.financialAccountService.getList({ pageSize: 2000 }).subscribe((r) => this.financialAccounts.set((r.response ?? []).filter((a) => a.isActive)));
    this.currencyService.getList({ pageSize: 500 }).subscribe((r) => this.currencies.set(r.response ?? []));

    const editId = this.route.snapshot.paramMap.get('id');
    if (editId) {
      const id = Number(editId);
      this.id.set(id);
      this.service.getById(id).subscribe((result) => {
        const row = result.response;
        if (!row) return;

        this.form.patchValue({
          financialAccountId: row.financialAccountId,
          date: row.date?.slice(0, 10) ?? '',
          amount: row.amount,
          currencyId: row.currencyId,
          rate: row.rate,
          notes: row.notes ?? '',
        });
        this.originalCreateUserId = row.createUserId;
        this.originalCreateDate = row.createDate;
      });
    }
  }

  onCurrencyChange(currencyId: number): void {
    const currency = this.currencies().find((c) => c.id === currencyId);
    if (currency) this.form.controls.rate.setValue(currency.rate);
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving.set(true);
    const value = this.form.getRawValue();
    const id = this.id() ?? undefined;
    const currentUserId = this.auth.currentUser()?.id;
    const nowIso = new Date().toISOString();

    const payload = {
      ...(id ? { id } : {}),
      financialAccountId: value.financialAccountId,
      financialTypeId: 1 as const,
      typeId: 1 as const,
      direction: 1 as const,
      referenceType: 0 as const,
      paymentTypeId: 1 as const,
      date: value.date,
      amount: value.amount,
      amountByDefaultCurrency: value.amount * value.rate,
      currencyId: value.currencyId,
      rate: value.rate,
      notes: value.notes || null,
      createUserId: id ? (this.originalCreateUserId ?? undefined) : currentUserId,
      createDate: id ? (this.originalCreateDate ?? undefined) : nowIso,
      ...(id ? { modifyUserId: currentUserId, modifyDate: nowIso } : {}),
    };

    const request$ = id ? this.service.update(payload) : this.service.create(payload);

    request$.subscribe({
      next: (result) => {
        this.saving.set(false);
        if (isApiSuccess(result)) {
          this.toast.success('Opening balance saved as Draft.');
          this.router.navigateByUrl('/financial/opening-balances');
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
