import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { HasPermissionDirective } from '../../../../../core/auth/has-permission.directive';
import { PermissionService } from '../../../../../core/auth/permission.service';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { ConfirmDialogService } from '../../../../../shared/components/confirm-dialog/confirm-dialog.service';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { PaginationComponent } from '../../../../../shared/components/pagination/pagination.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { Status } from '../../../../../shared/models/status.enum';
import { Financial } from '../../models/financial.model';
import { FinancialService } from '../../services/financial.service';
import { FinancialTypeService } from '../../services/financial-type.service';

const PAGE_SIZE = 20;

/**
 * Replaces Areas/Financials/Views/Financial/{Index,List}.cshtml. Every row here is created already
 * Posted (see PostFinancialTransactionRequest's docstring) — Reverse is the only lifecycle action
 * that ever applies in practice; Cancel/Redo/Edit/Delete only exist for a never-reached Draft state
 * and are not offered here to avoid dead buttons (mirrors the actual server-side gating).
 */
@Component({
  selector: 'app-financial-transaction-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, PageHeaderComponent, PaginationComponent, HasPermissionDirective],
  templateUrl: './financial-transaction-list.component.html',
})
export class FinancialTransactionListComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly financialService = inject(FinancialService);
  private readonly financialTypeService = inject(FinancialTypeService);
  private readonly toast = inject(ToastService);
  private readonly confirmDialog = inject(ConfirmDialogService);
  readonly permissionService = inject(PermissionService);

  readonly Status = Status;
  readonly financialTypeId = Number(this.route.snapshot.paramMap.get('typeId'));

  readonly title = signal('Financial');
  readonly transactions = signal<Financial[]>([]);
  readonly loading = signal(false);
  readonly page = signal(1);
  readonly pageCount = signal(1);
  readonly keySearch = signal('');

  constructor() {
    this.financialTypeService.getById(this.financialTypeId).subscribe((r) => {
      if (r.response?.name) this.title.set(r.response.name);
    });
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.financialService
      .search({ keySearch: this.keySearch(), typeId: this.financialTypeId, page: this.page(), pageSize: PAGE_SIZE })
      .subscribe({
        next: (result) => {
          this.loading.set(false);
          this.transactions.set(result.response ?? []);
          this.pageCount.set(result.pageCount || 1);
        },
        error: () => {
          this.loading.set(false);
          this.toast.error('Could not load transactions.');
        },
      });
  }

  onSearch(): void {
    this.page.set(1);
    this.load();
  }

  onPageChange(page: number): void {
    this.page.set(page);
    this.load();
  }

  canReverse(t: Financial): boolean {
    return t.posted && t.status !== Status.Reversed && !t.financialTransferId && this.permissionService.can('Financial.Reverse');
  }

  async reverse(t: Financial): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(
      `Reverse transaction "${t.code}"? This books a new, balancing Journal Entry.`,
      'Reverse transaction',
    );
    if (!confirmed) return;

    this.financialService.reverse(t.id).subscribe({
      next: (result) => {
        if (isApiSuccess(result)) {
          this.toast.success('Transaction reversed.');
          this.load();
        } else {
          this.toast.error(result.errors?.[0]?.messageError ?? 'Reverse failed.');
        }
      },
      error: () => this.toast.error('Reverse failed.'),
    });
  }
}
