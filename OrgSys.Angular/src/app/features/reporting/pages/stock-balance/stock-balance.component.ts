import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Product, Stock } from '../../../invoices/models/invoice-lookups.model';
import { ProductService, StockService } from '../../../invoices/services/invoice-lookups.service';
import { PageHeaderComponent } from '../../../../shared/components/page-header/page-header.component';
import { PaginationComponent } from '../../../../shared/components/pagination/pagination.component';
import { ToastService } from '../../../../shared/components/toast/toast.service';
import { BalanceRow } from '../../models/report.model';
import { Classification } from '../../models/report-lookups.model';
import { WarehouseReportService } from '../../services/report.service';
import { ClassificationService } from '../../services/report-lookups.service';

const PAGE_SIZE = 25;

/** Replaces Areas/Reports/Views/Warehouses/{StockBalance,ProductBalance}.cshtml — one screen for both (see WarehouseReportController.Balance). */
@Component({
  selector: 'app-stock-balance',
  standalone: true,
  imports: [CommonModule, FormsModule, PageHeaderComponent, PaginationComponent],
  templateUrl: './stock-balance.component.html',
})
export class StockBalanceComponent {
  private readonly service = inject(WarehouseReportService);
  private readonly stockService = inject(StockService);
  private readonly productService = inject(ProductService);
  private readonly classificationService = inject(ClassificationService);
  private readonly toast = inject(ToastService);

  readonly stocks = signal<Stock[]>([]);
  readonly products = signal<Product[]>([]);
  readonly classifications = signal<Classification[]>([]);

  readonly toDate = signal(new Date().toISOString().slice(0, 10));
  readonly stockId = signal(0);
  readonly productId = signal(0);
  readonly classificationId = signal(0);
  readonly sortByStock = signal(true);

  readonly rows = signal<BalanceRow[]>([]);
  readonly loading = signal(false);
  readonly page = signal(1);
  readonly pageCount = signal(1);

  constructor() {
    this.stockService.getList({ pageSize: 500 }).subscribe((r) => this.stocks.set(r.response ?? []));
    this.productService.getList({ pageSize: 5000 }).subscribe((r) => this.products.set(r.response ?? []));
    this.classificationService.getList({ pageSize: 500 }).subscribe((r) => this.classifications.set(r.response ?? []));
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
      .balance({
        toDate: this.toDate(),
        stockId: this.stockId(),
        productId: this.productId(),
        classificationId: this.classificationId(),
        sortByStock: this.sortByStock(),
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
          this.toast.error('Could not load the balance report.');
        },
      });
  }
}
