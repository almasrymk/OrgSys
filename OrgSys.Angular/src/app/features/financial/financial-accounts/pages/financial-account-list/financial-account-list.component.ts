import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { HasPermissionDirective } from '../../../../../core/auth/has-permission.directive';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { ConfirmDialogService } from '../../../../../shared/components/confirm-dialog/confirm-dialog.service';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { PaginationComponent } from '../../../../../shared/components/pagination/pagination.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { Status } from '../../../../../shared/models/status.enum';
import { FinancialAccount, FinancialAccountType } from '../../models/financial-account.model';
import { FinancialAccountService } from '../../services/financial-account.service';

const PAGE_SIZE = 20;

/**
 * Replaces Areas/Setting/Views/FinancialAccount/{Index,List}.cshtml. One component drives both
 * "Cash Boxes" and "Bank Accounts" (route data supplies financialAccountType/title/permissionPrefix)
 * — same one-screen-per-type-family pattern as Journal's FinancialType-driven form.
 */
@Component({
  selector: 'app-financial-account-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, PageHeaderComponent, PaginationComponent, HasPermissionDirective],
  templateUrl: './financial-account-list.component.html',
})
export class FinancialAccountListComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly financialAccountService = inject(FinancialAccountService);
  private readonly toast = inject(ToastService);
  private readonly confirmDialog = inject(ConfirmDialogService);

  readonly Status = Status;
  readonly FinancialAccountType = FinancialAccountType;

  readonly financialAccountType = this.route.snapshot.data['financialAccountType'] as FinancialAccountType;
  readonly title = this.route.snapshot.data['title'] as string;
  readonly permissionPrefix = this.route.snapshot.data['permissionPrefix'] as string;
  readonly basePath = this.route.snapshot.data['basePath'] as string;

  readonly accounts = signal<FinancialAccount[]>([]);
  readonly loading = signal(false);
  readonly page = signal(1);
  readonly pageCount = signal(1);
  readonly keySearch = signal('');

  readonly isBank = computed(() => this.financialAccountType === FinancialAccountType.Bank);

  constructor() {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.financialAccountService
      .search({ keySearch: this.keySearch(), typeId: this.financialAccountType, page: this.page(), pageSize: PAGE_SIZE })
      .subscribe({
        next: (result) => {
          this.loading.set(false);
          this.accounts.set(result.response ?? []);
          this.pageCount.set(result.pageCount || 1);
        },
        error: () => {
          this.loading.set(false);
          this.toast.error(`Could not load ${this.title.toLowerCase()}.`);
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

  async onDelete(account: FinancialAccount): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(`Delete "${account.name}"? This cannot be undone.`, `Delete ${this.title.slice(0, -1)}`);
    if (!confirmed) return;

    this.financialAccountService.delete(account.id).subscribe({
      next: (result) => {
        if (isApiSuccess(result)) {
          this.toast.success('Deleted.');
          this.load();
        } else {
          this.toast.error(result.errors?.[0]?.messageError ?? 'Delete failed.');
        }
      },
      error: () => this.toast.error('Delete failed.'),
    });
  }
}
