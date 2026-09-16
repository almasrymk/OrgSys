import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { isApiSuccess } from '../../../../core/models/api-result.model';
import { ConfirmDialogService } from '../../../../shared/components/confirm-dialog/confirm-dialog.service';
import { PageHeaderComponent } from '../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../shared/components/toast/toast.service';
import { Stock } from '../../models/stock.model';
import { StockService } from '../../services/stock.service';
import { DocumentStatus, InventoryReceipt } from '../../models/warehouse.model';
import { InventoryReceiptService } from '../../services/inventory-receipt.service';

/** New screen (brief §9/§68) — a standalone receipt document, independent of Purchasing's own
 * document model. No legacy MVC equivalent (the old Addition/TypeId-1 Transaction screen stays as-is). */
@Component({
  selector: 'app-receipts-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './receipts-list.component.html',
})
export class ReceiptsListComponent {
  private readonly service = inject(InventoryReceiptService);
  private readonly stockService = inject(StockService);
  private readonly toast = inject(ToastService);
  private readonly confirmDialog = inject(ConfirmDialogService);

  readonly DocumentStatus = DocumentStatus;

  readonly stocks = signal<Stock[]>([]);
  readonly stockId = signal(0);
  readonly receipts = signal<InventoryReceipt[]>([]);
  readonly loading = signal(false);

  constructor() {
    this.stockService.getList({ pageSize: 500 }).subscribe((r) => this.stocks.set(r.response ?? []));
    this.search();
  }

  stockName(id: number): string {
    return this.stocks().find((s) => s.id === id)?.name ?? `#${id}`;
  }

  search(): void {
    this.loading.set(true);
    this.service.getList(this.stockId() || null, null).subscribe({
      next: (result) => {
        this.loading.set(false);
        this.receipts.set(result.response ?? []);
      },
      error: () => {
        this.loading.set(false);
        this.toast.error('Could not load inventory receipts.');
      },
    });
  }

  async post(receipt: InventoryReceipt): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(`Post receipt "${receipt.code}"? This cannot be edited afterward.`, 'Post receipt');
    if (!confirmed) return;

    this.service.post(receipt.id).subscribe({
      next: (result) => {
        if (isApiSuccess(result)) {
          this.toast.success('Receipt posted.');
          this.search();
        } else {
          this.toast.error(result.errors?.[0]?.messageError ?? 'Post failed.');
        }
      },
      error: () => this.toast.error('Post failed.'),
    });
  }

  async cancel(receipt: InventoryReceipt): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(`Cancel draft receipt "${receipt.code}"?`, 'Cancel receipt');
    if (!confirmed) return;

    this.service.cancel(receipt.id).subscribe({
      next: (result) => {
        if (isApiSuccess(result)) {
          this.toast.success('Receipt cancelled.');
          this.search();
        } else {
          this.toast.error(result.errors?.[0]?.messageError ?? 'Cancel failed.');
        }
      },
      error: () => this.toast.error('Cancel failed.'),
    });
  }
}
