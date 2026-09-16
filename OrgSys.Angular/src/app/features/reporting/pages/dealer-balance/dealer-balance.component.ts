import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Dealer } from '../../../parties';
import { DealerService } from '../../../parties';
import { PageHeaderComponent } from '../../../../shared/components/page-header/page-header.component';
import { PaginationComponent } from '../../../../shared/components/pagination/pagination.component';
import { ToastService } from '../../../../shared/components/toast/toast.service';
import { DealerBalanceRow } from '../../models/report.model';
import { DealerReportService } from '../../services/report.service';

const PAGE_SIZE = 25;

/** Replaces Areas/Reports/Views/{Sales,Purchases}/{Clients,Suppliers}Balance.cshtml — one screen for both, driven by route :dealerTypeId (1=Client, 2=Supplier), same "type-multiplexed screen" pattern used throughout this app. */
@Component({
  selector: 'app-dealer-balance',
  standalone: true,
  imports: [CommonModule, FormsModule, PageHeaderComponent, PaginationComponent],
  templateUrl: './dealer-balance.component.html',
})
export class DealerBalanceComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly service = inject(DealerReportService);
  private readonly dealerService = inject(DealerService);
  private readonly toast = inject(ToastService);

  readonly dealerTypeId = Number(this.route.snapshot.paramMap.get('dealerTypeId'));
  readonly title = computed(() => (this.dealerTypeId === 1 ? 'Clients Balance' : 'Suppliers Balance'));

  readonly dealers = signal<Dealer[]>([]);
  readonly toDate = signal(new Date().toISOString().slice(0, 10));
  readonly dealerId = signal(0);

  readonly rows = signal<DealerBalanceRow[]>([]);
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
      .balance({ dealerTypeId: this.dealerTypeId, toDate: this.toDate(), dealerId: this.dealerId(), page: this.page(), pageSize: PAGE_SIZE })
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
