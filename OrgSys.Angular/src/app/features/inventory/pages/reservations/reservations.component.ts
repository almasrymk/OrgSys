import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { isApiSuccess } from '../../../../core/models/api-result.model';
import { ConfirmDialogService } from '../../../../shared/components/confirm-dialog/confirm-dialog.service';
import { PageHeaderComponent } from '../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../shared/components/toast/toast.service';
import { Product, Stock } from '../../../invoices/models/invoice-lookups.model';
import { ProductService, StockService } from '../../../invoices/services/invoice-lookups.service';
import { ReservationStatus, StockReservation } from '../../models/warehouse.model';
import { StockReservationService } from '../../services/stock-reservation.service';

/** New screen (brief §14/§68) — reservations never existed before this pass. */
@Component({
  selector: 'app-reservations',
  standalone: true,
  imports: [CommonModule, FormsModule, PageHeaderComponent],
  templateUrl: './reservations.component.html',
})
export class ReservationsComponent {
  private readonly service = inject(StockReservationService);
  private readonly stockService = inject(StockService);
  private readonly productService = inject(ProductService);
  private readonly toast = inject(ToastService);
  private readonly confirmDialog = inject(ConfirmDialogService);

  readonly ReservationStatus = ReservationStatus;

  readonly stocks = signal<Stock[]>([]);
  readonly products = signal<Product[]>([]);
  readonly status = signal<ReservationStatus | null>(ReservationStatus.Active);
  readonly reservations = signal<StockReservation[]>([]);
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
    this.service.getList(null, null, this.status()).subscribe({
      next: (result) => {
        this.loading.set(false);
        this.reservations.set(result.response ?? []);
      },
      error: () => {
        this.loading.set(false);
        this.toast.error('Could not load reservations.');
      },
    });
  }

  async release(reservation: StockReservation): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(`Release reservation #${reservation.id}?`, 'Release reservation');
    if (!confirmed) return;

    this.service.release(reservation.id).subscribe({
      next: (result) => {
        if (isApiSuccess(result)) {
          this.toast.success('Reservation released.');
          this.search();
        } else {
          this.toast.error(result.errors?.[0]?.messageError ?? 'Release failed.');
        }
      },
      error: () => this.toast.error('Release failed.'),
    });
  }

  async fulfill(reservation: StockReservation): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(`Mark reservation #${reservation.id} as fulfilled?`, 'Fulfill reservation');
    if (!confirmed) return;

    this.service.fulfill(reservation.id).subscribe({
      next: (result) => {
        if (isApiSuccess(result)) {
          this.toast.success('Reservation fulfilled.');
          this.search();
        } else {
          this.toast.error(result.errors?.[0]?.messageError ?? 'Fulfill failed.');
        }
      },
      error: () => this.toast.error('Fulfill failed.'),
    });
  }
}
