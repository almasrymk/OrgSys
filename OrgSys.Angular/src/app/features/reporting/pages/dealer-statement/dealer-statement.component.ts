import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Dealer } from '../../../parties';
import { DealerService } from '../../../parties';
import { PageHeaderComponent } from '../../../../shared/components/page-header/page-header.component';
import { PaginationComponent } from '../../../../shared/components/pagination/pagination.component';
import { ToastService } from '../../../../shared/components/toast/toast.service';
import { DealerStatementRow } from '../../models/report.model';
import { DealerReportService } from '../../services/report.service';

const PAGE_SIZE = 25;

function startOfYear(): string {
  return `${new Date().getFullYear()}-01-01`;
}

/** Replaces Areas/Reports/Views/{Sales,Purchases}/{Clients,Suppliers}Statment.cshtml — one screen for both, driven by route :dealerTypeId. Shows a running Balance per row, opening-balance row first (see GetDealerStatementReportQueryHandler for the running-total logic). */
@Component({
  selector: 'app-dealer-statement',
  standalone: true,
  imports: [CommonModule, FormsModule, PageHeaderComponent, PaginationComponent],
  templateUrl: './dealer-statement.component.html',
})
export class DealerStatementComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly service = inject(DealerReportService);
  private readonly dealerService = inject(DealerService);
  private readonly toast = inject(ToastService);

  readonly dealerTypeId = Number(this.route.snapshot.paramMap.get('dealerTypeId'));
  readonly title = computed(() => (this.dealerTypeId === 1 ? 'Clients Statement' : 'Suppliers Statement'));

  readonly dealers = signal<Dealer[]>([]);
  readonly fromDate = signal(startOfYear());
  readonly toDate = signal(new Date().toISOString().slice(0, 10));
  readonly dealerId = signal(0);

  readonly rows = signal<DealerStatementRow[]>([]);
  readonly loading = signal(false);
  readonly page = signal(1);
  readonly pageCount = signal(1);

  constructor() {
    this.dealerService.getList({ typeId: this.dealerTypeId, pageSize: 2000 }).subscribe((r) => this.dealers.set(r.response ?? []));
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
      .statement({
        dealerTypeId: this.dealerTypeId,
        fromDate: this.fromDate(),
        toDate: this.toDate(),
        dealerId: this.dealerId(),
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
          this.toast.error('Could not load the statement.');
        },
      });
  }
}
