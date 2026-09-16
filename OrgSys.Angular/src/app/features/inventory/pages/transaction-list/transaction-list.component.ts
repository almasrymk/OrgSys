import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { HasPermissionDirective } from '../../../../core/auth/has-permission.directive';
import { PermissionService } from '../../../../core/auth/permission.service';
import { ApiResult, isApiSuccess } from '../../../../core/models/api-result.model';
import { ConfirmDialogService } from '../../../../shared/components/confirm-dialog/confirm-dialog.service';
import { PageHeaderComponent } from '../../../../shared/components/page-header/page-header.component';
import { PaginationComponent } from '../../../../shared/components/pagination/pagination.component';
import { ToastService } from '../../../../shared/components/toast/toast.service';
import { Status } from '../../../../shared/models/status.enum';
import { Transaction, TransactionTypeId } from '../../models/transaction.model';
import { TransactionService } from '../../services/transaction.service';
import { TransactionTypeService } from '../../services/transaction-type.service';

const PAGE_SIZE = 20;

/** Replaces Areas/Transactions/Views/Transaction/{Index,List}.cshtml. One component drives all TransactionTypes. */
@Component({
  selector: 'app-transaction-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, PageHeaderComponent, PaginationComponent, HasPermissionDirective],
  templateUrl: './transaction-list.component.html',
})
export class TransactionListComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly service = inject(TransactionService);
  private readonly transactionTypeService = inject(TransactionTypeService);
  private readonly toast = inject(ToastService);
  private readonly confirmDialog = inject(ConfirmDialogService);
  readonly permissionService = inject(PermissionService);

  readonly Status = Status;
  readonly TransactionTypeId = TransactionTypeId;
  readonly typeId = Number(this.route.snapshot.data['typeId'] ?? this.route.snapshot.paramMap.get('typeId'));

  readonly title = signal('Transactions');
  readonly transactions = signal<Transaction[]>([]);
  readonly loading = signal(false);
  readonly page = signal(1);
  readonly pageCount = signal(1);
  readonly keySearch = signal('');

  constructor() {
    this.transactionTypeService.getById(this.typeId).subscribe((r) => {
      if (r.response?.name) this.title.set(r.response.name);
    });
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.service.search({ keySearch: this.keySearch(), typeId: this.typeId, page: this.page(), pageSize: PAGE_SIZE }).subscribe({
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

  canEdit(t: Transaction): boolean {
    return t.status !== Status.Cancel;
  }

  canCancel(t: Transaction): boolean {
    return t.status === Status.New && this.permissionService.can('Transactions.All');
  }

  canRedo(t: Transaction): boolean {
    return t.status === Status.Cancel && this.permissionService.can('Transactions.All');
  }

  async cancel(t: Transaction): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(`Cancel transaction "${t.code}"?`, 'Cancel transaction');
    if (!confirmed) return;

    this.service.cancel(t.id).subscribe({
      next: (result) => this.handleAction(result, 'Transaction cancelled.'),
      error: () => this.toast.error('Cancel failed.'),
    });
  }

  redo(t: Transaction): void {
    this.service.redo(t.id).subscribe({
      next: (result) => this.handleAction(result, 'Transaction restored.'),
      error: () => this.toast.error('Redo failed.'),
    });
  }

  async onDelete(t: Transaction): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(`Delete transaction "${t.code}"? This cannot be undone.`, 'Delete transaction');
    if (!confirmed) return;

    this.service.delete(t.id).subscribe({
      next: (result) => this.handleAction(result, 'Transaction deleted.'),
      error: () => this.toast.error('Delete failed.'),
    });
  }

  private handleAction(result: ApiResult, successMessage: string): void {
    if (isApiSuccess(result)) {
      this.toast.success(successMessage);
      this.load();
    } else {
      this.toast.error(result.errors?.[0]?.messageError ?? 'The action failed.');
    }
  }
}
