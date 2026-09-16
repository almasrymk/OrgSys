import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { PageHeaderComponent } from '../../../../shared/components/page-header/page-header.component';
import { PaginationComponent } from '../../../../shared/components/pagination/pagination.component';
import { ToastService } from '../../../../shared/components/toast/toast.service';
import { SalesBalanceRow } from '../../models/report.model';
import { SalesReportService } from '../../services/report.service';

const PAGE_SIZE = 31;

function startOfYear(): string {
  return `${new Date().getFullYear()}-01-01`;
}

/** Replaces Areas/Reports/Views/Sales/SalesBalance.cshtml — a daily Sales-invoice summary (In/Out/Net per day), not a per-dealer balance despite the name (see GetSalesBalanceReportQueryHandler). */
@Component({
  selector: 'app-sales-balance',
  standalone: true,
  imports: [CommonModule, FormsModule, PageHeaderComponent, PaginationComponent],
  templateUrl: './sales-balance.component.html',
})
export class SalesBalanceComponent {
  private readonly service = inject(SalesReportService);
  private readonly toast = inject(ToastService);

  readonly fromDate = signal(startOfYear());
  readonly toDate = signal(new Date().toISOString().slice(0, 10));

  readonly rows = signal<SalesBalanceRow[]>([]);
  readonly loading = signal(false);
  readonly page = signal(1);
  readonly pageCount = signal(1);

  constructor() {
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
    this.service.balance({ fromDate: this.fromDate(), toDate: this.toDate(), page: this.page(), pageSize: PAGE_SIZE }).subscribe({
      next: (result) => {
        this.loading.set(false);
        this.rows.set(result.rows);
        this.pageCount.set(result.pageCount);
      },
      error: () => {
        this.loading.set(false);
        this.toast.error('Could not load the sales balance report.');
      },
    });
  }
}
