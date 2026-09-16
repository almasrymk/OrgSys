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
import { Brand } from '../../models/brand.model';
import { BrandService } from '../../services/brand.service';

const PAGE_SIZE = 20;

@Component({
  selector: 'app-brand-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, PageHeaderComponent, PaginationComponent],
  templateUrl: './brand-list.component.html',
})
export class BrandListComponent {
  readonly Status = Status;
  readonly brands = signal<Brand[]>([]);
  readonly loading = signal(false);
  readonly page = signal(1);
  readonly pageCount = signal(1);
  readonly keySearch = signal('');

  constructor(
    private readonly brandService: BrandService,
    private readonly toast: ToastService,
    private readonly confirmDialog: ConfirmDialogService,
  ) {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.brandService.search({ keySearch: this.keySearch(), page: this.page(), pageSize: PAGE_SIZE }).subscribe({
      next: (result) => {
        this.loading.set(false);
        this.brands.set(result.response ?? []);
        this.pageCount.set(result.pageCount || 1);
      },
      error: () => {
        this.loading.set(false);
        this.toast.error('Could not load brands.');
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

  async onDelete(brand: Brand): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(`Delete "${brand.name}"? This cannot be undone.`, 'Delete brand');
    if (!confirmed) return;

    this.brandService.delete(brand.id).subscribe({
      next: (result) => {
        if (isApiSuccess(result)) {
          this.toast.success('Brand deleted.');
          this.load();
        } else {
          this.toast.error(result.errors?.[0]?.messageError ?? 'Delete failed.');
        }
      },
      error: () => this.toast.error('Delete failed.'),
    });
  }
}
