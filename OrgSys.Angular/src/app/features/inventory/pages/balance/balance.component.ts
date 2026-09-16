import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { PageHeaderComponent } from '../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../shared/components/toast/toast.service';
import { Product, Stock } from '../../../invoices/models/invoice-lookups.model';
import { ProductService, StockService } from '../../../invoices/services/invoice-lookups.service';
import { StockBalance } from '../../models/warehouse.model';
import { StockBalanceService } from '../../services/stock-balance.service';

/**
 * The hardened InventoryBalance projection (brief §8) — distinct from the existing
 * /reports/warehouse/balance screen, which sums the legacy ledger on the fly. This reads the new
 * materialized balance row directly, including Reserved/Available (which the legacy report has no
 * concept of).
 */
@Component({
  selector: 'app-inventory-balance',
  standalone: true,
  imports: [CommonModule, FormsModule, PageHeaderComponent],
  templateUrl: './balance.component.html',
})
export class BalanceComponent {
  private readonly service = inject(StockBalanceService);
  private readonly stockService = inject(StockService);
  private readonly productService = inject(ProductService);
  private readonly toast = inject(ToastService);

  readonly stocks = signal<Stock[]>([]);
  readonly products = signal<Product[]>([]);
  readonly stockId = signal(0);
  readonly productId = signal(0);
  readonly balances = signal<StockBalance[]>([]);
  readonly loading = signal(false);

  constructor() {
    this.stockService.getList({ pageSize: 500 }).subscribe((r) => this.stocks.set(r.response ?? []));
    this.productService.getList({ pageSize: 5000 }).subscribe((r) => this.products.set(r.response ?? []));
    this.search();
  }

  productName(id: number): string {
    return this.products().find((p) => p.id === id)?.name ?? `#${id}`;
  }

  stockName(id: number): string {
    return this.stocks().find((s) => s.id === id)?.name ?? `#${id}`;
  }

  search(): void {
    this.loading.set(true);
    this.service.getList(this.stockId() || null, this.productId() || null).subscribe({
      next: (result) => {
        this.loading.set(false);
        this.balances.set(result.response ?? []);
      },
      error: () => {
        this.loading.set(false);
        this.toast.error('Could not load the inventory balance.');
      },
    });
  }
}
