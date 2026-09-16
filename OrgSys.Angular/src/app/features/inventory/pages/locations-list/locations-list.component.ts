import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { PageHeaderComponent } from '../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../shared/components/toast/toast.service';
import { Stock } from '../../models/stock.model';
import { StockService } from '../../services/stock.service';
import { WarehouseLocation } from '../../models/warehouse.model';
import { WarehouseLocationService } from '../../services/warehouse-location.service';

/** New screen (brief §4.2/§68) — no legacy MVC equivalent to replace, Locations didn't exist before. */
@Component({
  selector: 'app-locations-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './locations-list.component.html',
})
export class LocationsListComponent {
  private readonly service = inject(WarehouseLocationService);
  private readonly stockService = inject(StockService);
  private readonly toast = inject(ToastService);

  readonly stocks = signal<Stock[]>([]);
  readonly stockId = signal(0);
  readonly locations = signal<WarehouseLocation[]>([]);
  readonly loading = signal(false);

  constructor() {
    this.stockService.getList({ pageSize: 500 }).subscribe((r) => {
      const list = r.response ?? [];
      this.stocks.set(list);
      if (list.length && !this.stockId()) {
        this.stockId.set(list[0].id);
        this.load();
      }
    });
  }

  onStockChange(): void {
    this.load();
  }

  private load(): void {
    if (!this.stockId()) {
      this.locations.set([]);
      return;
    }
    this.loading.set(true);
    this.service.getByStock(this.stockId()).subscribe({
      next: (result) => {
        this.loading.set(false);
        this.locations.set(result.response ?? []);
      },
      error: () => {
        this.loading.set(false);
        this.toast.error('Could not load warehouse locations.');
      },
    });
  }
}
