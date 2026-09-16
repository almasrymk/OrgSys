import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { ConfirmDialogService } from '../../../../../shared/components/confirm-dialog/confirm-dialog.service';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { PaginationComponent } from '../../../../../shared/components/pagination/pagination.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { Status } from '../../../../../shared/models/status.enum';
import { PriceList } from '../../models/price-list.model';
import { PriceListService } from '../../services/price-list.service';

const PAGE_SIZE = 20;

@Component({
  selector: 'app-price-list-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, PageHeaderComponent, PaginationComponent],
  templateUrl: './price-list-list.component.html',
})
export class PriceListListComponent {
  readonly Status = Status;
  readonly priceLists = signal<PriceList[]>([]);
  readonly loading = signal(false);
  readonly page = signal(1);
  readonly pageCount = signal(1);
  readonly keySearch = signal('');

  constructor(
    private readonly priceListService: PriceListService,
    private readonly toast: ToastService,
    private readonly confirmDialog: ConfirmDialogService,
  ) {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.priceListService.search({ keySearch: this.keySearch(), page: this.page(), pageSize: PAGE_SIZE }).subscribe({
      next: (result) => {
        this.loading.set(false);
        this.priceLists.set(result.response ?? []);
        this.pageCount.set(result.pageCount || 1);
      },
      error: () => {
        this.loading.set(false);
        this.toast.error('Could not load price lists.');
      },
    });
  }

  onSearch(): void {
    this.page.set(1);
    this.load();
  }

  onPageChange(page: number): void {
    this.page.set(page);
    this.load();
  }

  async onDelete(priceList: PriceList): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(`Delete "${priceList.name}"? This cannot be undone.`, 'Delete price list');
    if (!confirmed) return;

    this.priceListService.delete(priceList.id).subscribe({
      next: (result) => {
        if (isApiSuccess(result)) {
          this.toast.success('Price list deleted.');
          this.load();
        } else {
          this.toast.error(result.errors?.[0]?.messageError ?? 'Delete failed.');
        }
      },
      error: () => this.toast.error('Delete failed.'),
    });
  }
}
