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
import { Invoice } from '../../models/invoice.model';
import { InvoiceService } from '../../services/invoice.service';
import { InvoiceTypeService } from '../../services/invoice-type.service';

const PAGE_SIZE = 20;

/** Replaces Areas/Invoices/Views/Invoice/{Index,List}.cshtml. One component drives all 4 InvoiceTypes. */
@Component({
  selector: 'app-invoice-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, PageHeaderComponent, PaginationComponent, HasPermissionDirective],
  templateUrl: './invoice-list.component.html',
})
export class InvoiceListComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly service = inject(InvoiceService);
  private readonly invoiceTypeService = inject(InvoiceTypeService);
  private readonly toast = inject(ToastService);
  private readonly confirmDialog = inject(ConfirmDialogService);
  readonly permissionService = inject(PermissionService);

  readonly Status = Status;
  readonly typeId = Number(this.route.snapshot.data['typeId'] ?? this.route.snapshot.paramMap.get('typeId'));

  readonly title = signal('Invoices');
  readonly invoices = signal<Invoice[]>([]);
  readonly loading = signal(false);
  readonly page = signal(1);
  readonly pageCount = signal(1);
  readonly keySearch = signal('');

  constructor() {
    this.invoiceTypeService.getById(this.typeId).subscribe((r) => {
      if (r.response?.name) this.title.set(`${r.response.group ?? ''} ${r.response.name}`.trim());
    });
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.service.search({ keySearch: this.keySearch(), typeId: this.typeId, page: this.page(), pageSize: PAGE_SIZE }).subscribe({
      next: (result) => {
        this.loading.set(false);
        this.invoices.set(result.response ?? []);
        this.pageCount.set(result.pageCount || 1);
      },
      error: () => {
        this.loading.set(false);
        this.toast.error('Could not load invoices.');
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

  canEdit(invoice: Invoice): boolean {
    return invoice.status !== Status.Cancel;
  }

  canCancel(invoice: Invoice): boolean {
    return invoice.status === Status.New && this.permissionService.can('Invoices.All');
  }

  canRedo(invoice: Invoice): boolean {
    return invoice.status === Status.Cancel && this.permissionService.can('Invoices.All');
  }

  async cancel(invoice: Invoice): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(`Cancel invoice "${invoice.code}"?`, 'Cancel invoice');
    if (!confirmed) return;

    this.service.cancel(invoice.id).subscribe({
      next: (result) => this.handleAction(result, 'Invoice cancelled.'),
      error: () => this.toast.error('Cancel failed.'),
    });
  }

  redo(invoice: Invoice): void {
    this.service.redo(invoice.id).subscribe({
      next: (result) => this.handleAction(result, 'Invoice restored.'),
      error: () => this.toast.error('Redo failed.'),
    });
  }

  async onDelete(invoice: Invoice): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(`Delete invoice "${invoice.code}"? This cannot be undone.`, 'Delete invoice');
    if (!confirmed) return;

    this.service.delete(invoice.id).subscribe({
      next: (result) => this.handleAction(result, 'Invoice deleted.'),
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
