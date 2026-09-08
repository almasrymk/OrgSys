import { CommonModule } from '@angular/common';
import { Component, computed, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HasPermissionDirective } from '../../../../../core/auth/has-permission.directive';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { ConfirmDialogService } from '../../../../../shared/components/confirm-dialog/confirm-dialog.service';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { PaginationComponent } from '../../../../../shared/components/pagination/pagination.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { Status } from '../../../../../shared/models/status.enum';
import { Country } from '../../../countries/models/country.model';
import { CountryService } from '../../../countries/services/country.service';
import { Bank } from '../../models/bank.model';
import { BankService } from '../../services/bank.service';

const PAGE_SIZE = 20;

/** Replaces Areas/Setting/Views/Bank/{Index,List}.cshtml. BankDto has no CountryName, so Country is resolved client-side from a loaded lookup list. */
@Component({
  selector: 'app-bank-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, PageHeaderComponent, PaginationComponent, HasPermissionDirective],
  templateUrl: './bank-list.component.html',
})
export class BankListComponent {
  readonly Status = Status;

  readonly banks = signal<Bank[]>([]);
  readonly countries = signal<Country[]>([]);
  readonly countryNameById = computed(() => new Map(this.countries().map((c) => [c.id, c.name])));
  readonly loading = signal(false);
  readonly page = signal(1);
  readonly pageCount = signal(1);
  readonly keySearch = signal('');

  constructor(
    private readonly bankService: BankService,
    private readonly countryService: CountryService,
    private readonly toast: ToastService,
    private readonly confirmDialog: ConfirmDialogService,
  ) {
    this.countryService.getList({ pageSize: 500 }).subscribe((r) => this.countries.set(r.response ?? []));
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.bankService.search({ keySearch: this.keySearch(), page: this.page(), pageSize: PAGE_SIZE }).subscribe({
      next: (result) => {
        this.loading.set(false);
        this.banks.set(result.response ?? []);
        this.pageCount.set(result.pageCount || 1);
      },
      error: () => {
        this.loading.set(false);
        this.toast.error('Could not load banks.');
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

  async onDelete(bank: Bank): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(`Delete "${bank.name}"? This cannot be undone.`, 'Delete bank');
    if (!confirmed) return;

    this.bankService.delete(bank.id).subscribe({
      next: (result) => {
        if (isApiSuccess(result)) {
          this.toast.success('Bank deleted.');
          this.load();
        } else {
          this.toast.error(result.errors?.[0]?.messageError ?? 'Delete failed.');
        }
      },
      error: () => this.toast.error('Delete failed.'),
    });
  }
}
