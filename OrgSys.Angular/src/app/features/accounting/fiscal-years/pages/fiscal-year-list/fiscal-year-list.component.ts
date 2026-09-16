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
import { FiscalYear, FiscalYearStatus } from '../../models/fiscal-year.model';
import { FiscalYearService } from '../../services/fiscal-year.service';

const PAGE_SIZE = 20;

/** Replaces Areas/Setting/Views/FiscalYear/{Index,List}.cshtml. */
@Component({
  selector: 'app-fiscal-year-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, PageHeaderComponent, PaginationComponent, HasPermissionDirective],
  templateUrl: './fiscal-year-list.component.html',
})
export class FiscalYearListComponent {
  readonly Status = Status;
  readonly FiscalYearStatus = FiscalYearStatus;

  readonly fiscalYears = signal<FiscalYear[]>([]);
  readonly loading = signal(false);
  readonly page = signal(1);
  readonly pageCount = signal(1);
  readonly keySearch = signal('');

  constructor(
    private readonly fiscalYearService: FiscalYearService,
    private readonly toast: ToastService,
    private readonly confirmDialog: ConfirmDialogService,
  ) {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.fiscalYearService.search({ keySearch: this.keySearch(), page: this.page(), pageSize: PAGE_SIZE }).subscribe({
      next: (result) => {
        this.loading.set(false);
        this.fiscalYears.set(result.response ?? []);
        this.pageCount.set(result.pageCount || 1);
      },
      error: () => {
        this.loading.set(false);
        this.toast.error('Could not load fiscal years.');
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

  async onDelete(fiscalYear: FiscalYear): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(
      `Delete "${fiscalYear.name}"? This cannot be undone.`,
      'Delete fiscal year',
    );
    if (!confirmed) return;

    this.fiscalYearService.delete(fiscalYear.id).subscribe({
      next: (result) => {
        if (isApiSuccess(result)) {
          this.toast.success('Fiscal year deleted.');
          this.load();
        } else {
          this.toast.error(result.errors?.[0]?.messageError ?? 'Delete failed.');
        }
      },
      error: () => this.toast.error('Delete failed.'),
    });
  }
}
