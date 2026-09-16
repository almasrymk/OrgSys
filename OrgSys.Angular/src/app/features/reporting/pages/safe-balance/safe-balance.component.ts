import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { PageHeaderComponent } from '../../../../shared/components/page-header/page-header.component';
import { PaginationComponent } from '../../../../shared/components/pagination/pagination.component';
import { ToastService } from '../../../../shared/components/toast/toast.service';
import { SafeBalanceRow } from '../../models/report.model';
import { CashBox } from '../../models/report-lookups.model';
import { FinancialReportService } from '../../services/report.service';
import { CashBoxService } from '../../services/report-lookups.service';

const PAGE_SIZE = 25;

/** Replaces Areas/Reports/Views/Finances/SafeBalance.cshtml — never actually finished in MVC (see GetSafeBalanceReportQueryHandler); one row per safe with its running balance as of a date. */
@Component({
  selector: 'app-safe-balance',
  standalone: true,
  imports: [CommonModule, FormsModule, PageHeaderComponent, PaginationComponent],
  templateUrl: './safe-balance.component.html',
})
export class SafeBalanceComponent {
  private readonly service = inject(FinancialReportService);
  private readonly cashBoxService = inject(CashBoxService);
  private readonly toast = inject(ToastService);

  readonly cashBoxes = signal<CashBox[]>([]);
  readonly toDate = signal(new Date().toISOString().slice(0, 10));
  readonly cashBoxId = signal(0);

  readonly rows = signal<SafeBalanceRow[]>([]);
  readonly loading = signal(false);
  readonly page = signal(1);
  readonly pageCount = signal(1);

  constructor() {
    this.cashBoxService.getList({ pageSize: 500 }).subscribe((r) => this.cashBoxes.set(r.response ?? []));
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
    this.service.safeBalance({ toDate: this.toDate(), cashBoxId: this.cashBoxId(), page: this.page(), pageSize: PAGE_SIZE }).subscribe({
      next: (result) => {
        this.loading.set(false);
        this.rows.set(result.rows);
        this.pageCount.set(result.pageCount);
      },
      error: () => {
        this.loading.set(false);
        this.toast.error('Could not load the safe balance report.');
      },
    });
  }
}
