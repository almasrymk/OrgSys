import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { isApiSuccess } from '../../../../core/models/api-result.model';
import { PageHeaderComponent } from '../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../shared/components/toast/toast.service';
import { Stock } from '../../models/stock.model';
import { StockService } from '../../services/stock.service';
import { CreateWarehouseLocationRequest } from '../../models/warehouse.model';
import { WarehouseLocationService } from '../../services/warehouse-location.service';

@Component({
  selector: 'app-locations-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './locations-form.component.html',
})
export class LocationsFormComponent {
  private readonly service = inject(WarehouseLocationService);
  private readonly stockService = inject(StockService);
  private readonly toast = inject(ToastService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  readonly stocks = signal<Stock[]>([]);
  readonly saving = signal(false);

  readonly model: CreateWarehouseLocationRequest = {
    stockId: 0,
    code: '',
    name: null,
    parentLocationId: null,
    locationType: null,
    isReceivable: true,
    isPickable: true,
  };

  constructor() {
    this.stockService.getList({ pageSize: 500 }).subscribe((r) => this.stocks.set(r.response ?? []));

    const stockIdParam = this.route.snapshot.queryParamMap.get('stockId');
    if (stockIdParam) this.model.stockId = Number(stockIdParam);
  }

  save(): void {
    if (!this.model.stockId || !this.model.code.trim()) {
      this.toast.error('Warehouse and code are required.');
      return;
    }

    this.saving.set(true);
    this.service.create(this.model).subscribe({
      next: (result) => {
        this.saving.set(false);
        if (isApiSuccess(result)) {
          this.toast.success('Location created.');
          this.router.navigate(['/inventory/locations']);
        } else {
          this.toast.error(result.errors?.[0]?.messageError ?? 'Could not create the location.');
        }
      },
      error: () => {
        this.saving.set(false);
        this.toast.error('Could not create the location.');
      },
    });
  }
}
