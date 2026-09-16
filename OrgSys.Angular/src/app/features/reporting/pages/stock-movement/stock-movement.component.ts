import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Product, Stock } from '../../../invoices/models/invoice-lookups.model';
import { ProductService, StockService } from '../../../invoices/services/invoice-lookups.service';
import { PageHeaderComponent } from '../../../../shared/components/page-header/page-header.component';
import { PaginationComponent } from '../../../../shared/components/pagination/pagination.component';
import { ToastService } from '../../../../shared/components/toast/toast.service';
import { MovementRow } from '../../models/report.model';
import { WarehouseReportService } from '../../services/report.service';

const PAGE_SIZE = 25;

function startOfYear(): string {
  return `${new Date().getFullYear()}-01-01`;
}
function endOfYear(): string {
  return `${new Date().getFullYear()}-12-31`;
}

/** Replaces Areas/Reports/Views/Warehouses/{StockMovement,ProductMovement}.cshtml — one screen for both (see WarehouseReportController.Movement). */
@Component({
  selector: 'app-stock-movement',
  standalone: true,
  imports: [CommonModule, FormsModule, PageHeaderComponent, PaginationComponent],
  templateUrl: './stock-movement.component.html',
})
export class StockMovementComponent {
  private readonly service = inject(WarehouseReportService);
  private readonly stockService = inject(StockService);
  private readonly productService = inject(ProductService);
  private readonly toast = inject(ToastService);

  readonly stocks = signal<Stock[]>([]);
  readonly products = signal<Product[]>([]);

  readonly fromDate = signal(startOfYear());
  readonly toDate = signal(endOfYear());
  readonly stockId = signal(0);
  readonly productId = signal(0);

  readonly rows = signal<MovementRow[]>([]);
  readonly loading = signal(false);
  readonly page = signal(1);
  readonly pageCount = signal(1);

  constructor() {
    this.stockService.getList({ pageSize: 500 }).subscribe((r) => this.stocks.set(r.response ?? []));
    this.productService.getList({ pageSize: 5000 }).subscribe((r) => this.products.set(r.response ?? []));
    this.search();
  }

  search(): void {
    this.page.set(1);
    this.load();
  }

  onPageChange(page: number): void {
    this.page.set(page);
    this.load();
  }

  private load(): void {
    this.loading.set(true);
    this.service
      .movement({
        fromDate: this.fromDate(),
        toDate: this.toDate(),
        stockId: this.stockId(),
        productId: this.productId(),
        page: this.page(),
        pageSize: PAGE_SIZE,
      })
      .subscribe({
        next: (result) => {
          this.loading.set(false);
          this.rows.set(result.rows);
          this.pageCount.set(result.pageCount);
        },
        error: () => {
          this.loading.set(false);
          this.toast.error('Could not load the movement report.');
        },
      });
  }
}
