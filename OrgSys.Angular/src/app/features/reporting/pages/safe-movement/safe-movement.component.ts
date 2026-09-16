import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Dealer } from '../../../parties';
import { DealerService } from '../../../parties';
import { PageHeaderComponent } from '../../../../shared/components/page-header/page-header.component';
import { PaginationComponent } from '../../../../shared/components/pagination/pagination.component';
import { ToastService } from '../../../../shared/components/toast/toast.service';
import { SafeMovementRow } from '../../models/report.model';
import { CashBox } from '../../models/report-lookups.model';
import { FinancialReportService } from '../../services/report.service';
import { CashBoxService } from '../../services/report-lookups.service';

const PAGE_SIZE = 25;

function startOfYear(): string {
  return `${new Date().getFullYear()}-01-01`;
}
function endOfYear(): string {
  return `${new Date().getFullYear()}-12-31`;
}

/** Replaces Areas/Reports/Views/Finances/SafeMovement.cshtml. "Safe" = CashBox, resolved via CashBox.FinancialAccountId == Financial.FinancialAccountId (see GetSafeMovementReportQueryHandler). */
@Component({
  selector: 'app-safe-movement',
  standalone: true,
  imports: [CommonModule, FormsModule, PageHeaderComponent, PaginationComponent],
  templateUrl: './safe-movement.component.html',
})
export class SafeMovementComponent {
  private readonly service = inject(FinancialReportService);
  private readonly cashBoxService = inject(CashBoxService);
  private readonly dealerService = inject(DealerService);
  private readonly toast = inject(ToastService);

  readonly cashBoxes = signal<CashBox[]>([]);
  readonly dealers = signal<Dealer[]>([]);

  readonly fromDate = signal(startOfYear());
  readonly toDate = signal(endOfYear());
  readonly cashBoxId = signal(0);
  readonly dealerId = signal(0);

  readonly rows = signal<SafeMovementRow[]>([]);
  readonly loading = signal(false);
  readonly page = signal(1);
  readonly pageCount = signal(1);

  constructor() {
    this.cashBoxService.getList({ pageSize: 500 }).subscribe((r) => this.cashBoxes.set(r.response ?? []));
    this.dealerService.getList({ pageSize: 2000 }).subscribe((r) => this.dealers.set(r.response ?? []));
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
      .safeMovement({
        fromDate: this.fromDate(),
        toDate: this.toDate(),
        dealerId: this.dealerId(),
        cashBoxId: this.cashBoxId(),
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
          this.toast.error('Could not load the safe movement report.');
        },
      });
  }
}
