import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../../core/auth/auth.service';
import { ApiResult, isApiSuccess } from '../../../../core/models/api-result.model';
import { PageHeaderComponent } from '../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../shared/components/toast/toast.service';
import { FinancialAccount } from '../../../treasury/financial-accounts/models/financial-account.model';
import { FinancialAccountService } from '../../../treasury/financial-accounts/services/financial-account.service';
import { Currency } from '../../../master-data/currencies/models/currency.model';
import { CurrencyService } from '../../../master-data/currencies/services/currency.service';
import { CUSTODY_STATUS_LABEL, Custody, CustodyStatus } from '../../models/custody.model';
import { CustodyService } from '../../services/custody.service';

@Component({
  selector: 'app-custody-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './custody-form.component.html',
})
export class CustodyFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly auth = inject(AuthService);
  private readonly toast = inject(ToastService);
  private readonly service = inject(CustodyService);
  private readonly currencyService = inject(CurrencyService);
  private readonly financialAccountService = inject(FinancialAccountService);

  readonly CustodyStatus = CustodyStatus;
  readonly statusLabel = CUSTODY_STATUS_LABEL;
  readonly saving = signal(false);
  readonly custody = signal<Custody | null>(null);
  readonly currencies = signal<Currency[]>([]);
  readonly financialAccounts = signal<FinancialAccount[]>([]);

  readonly form = this.fb.nonNullable.group({
    holderId: [0, [Validators.required, Validators.min(1)]],
    purpose: ['', Validators.required],
    currencyId: [0, [Validators.required, Validators.min(1)]],
    rate: [1, [Validators.required, Validators.min(0.000001)]],
    issuedAmount: [0, [Validators.required, Validators.min(0.01)]],
    dueDate: [''],
    notes: [''],
    code: [''],
    financialAccountId: [0],
    counterAccountId: [0],
    settleAmount: [0],
    returnAmount: [0],
    toHolderId: [0],
    transferReason: [''],
  });

  constructor() {
    this.currencyService.getList({ pageSize: 500 }).subscribe((r) => this.currencies.set(r.response ?? []));
    this.financialAccountService.getList({ pageSize: 500 }).subscribe((r) => this.financialAccounts.set(r.response ?? []));
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) this.load(id);
  }

  get isNew(): boolean {
    return this.custody() == null && !this.route.snapshot.paramMap.get('id');
  }

  private load(id: number): void {
    this.service.getById(id).subscribe({
      next: (result) => {
        if (result.response) {
          this.custody.set(result.response);
          this.form.patchValue({
            holderId: result.response.holderId,
            purpose: result.response.purpose,
            currencyId: result.response.currencyId,
            rate: result.response.rate,
            issuedAmount: result.response.issuedAmount,
            dueDate: result.response.dueDate?.slice(0, 10) ?? '',
            notes: result.response.notes ?? '',
            code: result.response.code ?? '',
          });
          this.form.disable();
          this.form.controls.financialAccountId.enable();
          this.form.controls.counterAccountId.enable();
          this.form.controls.settleAmount.enable();
          this.form.controls.returnAmount.enable();
          this.form.controls.toHolderId.enable();
          this.form.controls.transferReason.enable();
        } else {
          this.toast.error('Custody not found.');
          this.router.navigate(['/advances/custodies']);
        }
      },
      error: () => this.toast.error('Could not load custody.'),
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.saving.set(true);
    const value = this.form.getRawValue();
    this.service
      .create({
        holderId: value.holderId,
        purpose: value.purpose,
        currencyId: value.currencyId,
        rate: value.rate,
        issuedAmount: value.issuedAmount,
        dueDate: value.dueDate || null,
        createUserId: this.auth.currentUser()?.id ?? 0,
        createDate: new Date().toISOString(),
        branchId: null,
        notes: value.notes || null,
        code: value.code || null,
      })
      .subscribe({
        next: (result) => {
          this.saving.set(false);
          if (isApiSuccess(result) && result.response) {
            this.toast.success('Custody created as Draft.');
            this.router.navigate(['/advances/custodies', result.response]);
          } else {
            this.toast.error(result.errors?.[0]?.messageError ?? 'Could not create custody.');
          }
        },
        error: () => {
          this.saving.set(false);
          this.toast.error('Could not create custody.');
        },
      });
  }

  approve(): void {
    const id = this.custody()?.id;
    if (!id) return;
    this.service.approve(id).subscribe({
      next: (result) => this.afterAction(result, 'Custody approved.'),
      error: () => this.toast.error('Approve failed.'),
    });
  }

  issue(): void {
    const current = this.custody();
    if (!current) return;
    const value = this.form.getRawValue();
    if (value.financialAccountId <= 0 || value.counterAccountId <= 0) {
      this.toast.error('Financial account and counter GL account are required to issue.');
      return;
    }
    this.service
      .issue(current.id, {
        id: current.id,
        financialAccountId: value.financialAccountId,
        counterAccountId: value.counterAccountId,
        issueDate: new Date().toISOString(),
        createUserId: this.auth.currentUser()?.id ?? 0,
        branchId: current.branchId,
        shiftId: null,
      })
      .subscribe({
        next: (result) => this.afterAction(result, 'Custody issued.'),
        error: () => this.toast.error('Issue failed.'),
      });
  }

  settle(): void {
    const current = this.custody();
    if (!current) return;
    const amount = this.form.getRawValue().settleAmount;
    this.service.settle(current.id, amount).subscribe({
      next: (result) => this.afterAction(result, 'Settlement recorded.'),
      error: () => this.toast.error('Settle failed.'),
    });
  }

  returnAmount(): void {
    const current = this.custody();
    if (!current) return;
    const value = this.form.getRawValue();
    if (value.financialAccountId <= 0 || value.counterAccountId <= 0) {
      this.toast.error('Financial account and counter GL account are required to return cash.');
      return;
    }
    this.service
      .returnAmount(current.id, {
        id: current.id,
        amount: value.returnAmount,
        financialAccountId: value.financialAccountId,
        counterAccountId: value.counterAccountId,
        returnDate: new Date().toISOString(),
        createUserId: this.auth.currentUser()?.id ?? 0,
        branchId: current.branchId,
        shiftId: null,
      })
      .subscribe({
        next: (result) => this.afterAction(result, 'Return recorded.'),
        error: () => this.toast.error('Return failed.'),
      });
  }

  close(): void {
    const id = this.custody()?.id;
    if (!id) return;
    this.service.close(id).subscribe({
      next: (result) => this.afterAction(result, 'Custody closed.'),
      error: () => this.toast.error('Close failed.'),
    });
  }

  cancel(): void {
    const id = this.custody()?.id;
    if (!id) return;
    this.service.cancel(id).subscribe({
      next: (result) => this.afterAction(result, 'Custody cancelled.'),
      error: () => this.toast.error('Cancel failed.'),
    });
  }

  transfer(): void {
    const current = this.custody();
    if (!current) return;
    const value = this.form.getRawValue();
    this.service
      .transfer(current.id, value.toHolderId, value.transferReason, this.auth.currentUser()?.id ?? 0, new Date().toISOString())
      .subscribe({
        next: (result) => this.afterAction(result, 'Custody transferred.'),
        error: () => this.toast.error('Transfer failed.'),
      });
  }

  private afterAction(result: ApiResult, success: string): void {
    if (isApiSuccess(result)) {
      this.toast.success(success);
      const id = this.custody()?.id;
      if (id) this.load(id);
    } else {
      this.toast.error(result.errors?.[0]?.messageError ?? 'Action failed.');
    }
  }
}
