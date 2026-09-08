import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HasPermissionDirective } from '../../../../../core/auth/has-permission.directive';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { ConfirmDialogService } from '../../../../../shared/components/confirm-dialog/confirm-dialog.service';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { PaginationComponent } from '../../../../../shared/components/pagination/pagination.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { Status } from '../../../../../shared/models/status.enum';
import { Country } from '../../models/country.model';
import { CountryService } from '../../services/country.service';

const PAGE_SIZE = 20;

/** Replaces Areas/Setting/Views/Country/{Index,List}.cshtml. */
@Component({
  selector: 'app-country-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, PageHeaderComponent, PaginationComponent, HasPermissionDirective],
  templateUrl: './country-list.component.html',
})
export class CountryListComponent {
  readonly Status = Status;

  readonly countries = signal<Country[]>([]);
  readonly loading = signal(false);
  readonly page = signal(1);
  readonly pageCount = signal(1);
  readonly keySearch = signal('');

  constructor(
    private readonly countryService: CountryService,
    private readonly toast: ToastService,
    private readonly confirmDialog: ConfirmDialogService,
  ) {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.countryService.search({ keySearch: this.keySearch(), page: this.page(), pageSize: PAGE_SIZE }).subscribe({
      next: (result) => {
        this.loading.set(false);
        this.countries.set(result.response ?? []);
        this.pageCount.set(result.pageCount || 1);
      },
      error: () => {
        this.loading.set(false);
        this.toast.error('Could not load countries.');
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

  async onDelete(country: Country): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(
      `Delete "${country.name}"? This cannot be undone.`,
      'Delete country',
    );
    if (!confirmed) return;

    this.countryService.delete(country.id).subscribe({
      next: (result) => {
        if (isApiSuccess(result)) {
          this.toast.success('Country deleted.');
          this.load();
        } else {
          this.toast.error(result.errors?.[0]?.messageError ?? 'Delete failed.');
        }
      },
      error: () => this.toast.error('Delete failed.'),
    });
  }
}
