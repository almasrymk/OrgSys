import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HasPermissionDirective } from '../../../../core/auth/has-permission.directive';
import { PermissionService } from '../../../../core/auth/permission.service';
import { ApiResult, isApiSuccess } from '../../../../core/models/api-result.model';
import { ConfirmDialogService } from '../../../../shared/components/confirm-dialog/confirm-dialog.service';
import { PageHeaderComponent } from '../../../../shared/components/page-header/page-header.component';
import { PaginationComponent } from '../../../../shared/components/pagination/pagination.component';
import { ToastService } from '../../../../shared/components/toast/toast.service';
import { Status } from '../../../../shared/models/status.enum';
import { Inventory } from '../../models/inventory.model';
import { InventoryService } from '../../services/inventory.service';

const PAGE_SIZE = 20;

/** Replaces Areas/Transactions/Views/Inventory/{Index,List}.cshtml. No TypeId dimension — one screen. */
@Component({
  selector: 'app-inventory-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, PageHeaderComponent, PaginationComponent, HasPermissionDirective],
  templateUrl: './inventory-list.component.html',
})
export class InventoryListComponent {
  private readonly service = inject(InventoryService);
  private readonly toast = inject(ToastService);
  private readonly confirmDialog = inject(ConfirmDialogService);
  readonly permissionService = inject(PermissionService);

  readonly Status = Status;

  readonly inventories = signal<Inventory[]>([]);
  readonly loading = signal(false);
  readonly page = signal(1);
  readonly pageCount = signal(1);
  readonly keySearch = signal('');

  constructor() {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.service.search({ keySearch: this.keySearch(), page: this.page(), pageSize: PAGE_SIZE }).subscribe({
      next: (result) => {
        this.loading.set(false);
        this.inventories.set(result.response ?? []);
        this.pageCount.set(result.pageCount || 1);
      },
      error: () => {
        this.loading.set(false);
        this.toast.error('Could not load inventory counts.');
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

  canEdit(inv: Inventory): boolean {
    return inv.status !== Status.Cancel;
  }

  canCancel(inv: Inventory): boolean {
    return inv.status === Status.New && this.permissionService.can('Inventory.Cancel,Inventory.All');
  }

  canRedo(inv: Inventory): boolean {
    return inv.status === Status.Cancel && this.permissionService.can('Inventory.Redo,Inventory.All');
  }

  async cancel(inv: Inventory): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(`Cancel inventory count "${inv.code}"?`, 'Cancel inventory count');
    if (!confirmed) return;

    this.service.cancel(inv.id).subscribe({
      next: (result) => this.handleAction(result, 'Inventory count cancelled.'),
      error: () => this.toast.error('Cancel failed.'),
    });
  }

  redo(inv: Inventory): void {
    this.service.redo(inv.id).subscribe({
      next: (result) => this.handleAction(result, 'Inventory count restored.'),
      error: () => this.toast.error('Redo failed.'),
    });
  }

  async createAdjustment(inv: Inventory): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(
      `Create adjustment transactions for "${inv.code}" from its counted differences?`,
      'Create adjustment',
    );
    if (!confirmed) return;

    this.service.createAdjustment(inv.id).subscribe({
      next: (result) => this.handleAction(result, 'Adjustment transactions created.'),
      error: () => this.toast.error('Unable to create adjustment. Check inventory differences.'),
    });
  }

  async onDelete(inv: Inventory): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(`Delete inventory count "${inv.code}"? This cannot be undone.`, 'Delete inventory count');
    if (!confirmed) return;

    this.service.delete(inv.id).subscribe({
      next: (result) => this.handleAction(result, 'Inventory count deleted.'),
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
