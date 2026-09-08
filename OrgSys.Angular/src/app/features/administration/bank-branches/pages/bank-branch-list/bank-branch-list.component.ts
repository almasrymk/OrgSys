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
import { BankBranch } from '../../models/bank-branch.model';
import { BankBranchService } from '../../services/bank-branch.service';

const PAGE_SIZE = 20;

/** Replaces Areas/Setting/Views/BankBranch/{Index,List}.cshtml. */
@Component({
  selector: 'app-bank-branch-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, PageHeaderComponent, PaginationComponent, HasPermissionDirective],
  templateUrl: './bank-branch-list.component.html',
})
export class BankBranchListComponent {
  readonly Status = Status;

  readonly bankBranches = signal<BankBranch[]>([]);
  readonly loading = signal(false);
  readonly page = signal(1);
  readonly pageCount = signal(1);
  readonly keySearch = signal('');

  constructor(
    private readonly bankBranchService: BankBranchService,
    private readonly toast: ToastService,
    private readonly confirmDialog: ConfirmDialogService,
  ) {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.bankBranchService.search({ keySearch: this.keySearch(), page: this.page(), pageSize: PAGE_SIZE }).subscribe({
      next: (result) => {
        this.loading.set(false);
        this.bankBranches.set(result.response ?? []);
        this.pageCount.set(result.pageCount || 1);
      },
      error: () => {
        this.loading.set(false);
        this.toast.error('Could not load bank branches.');
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

  async onDelete(bankBranch: BankBranch): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(`Delete "${bankBranch.name}"? This cannot be undone.`, 'Delete bank branch');
    if (!confirmed) return;

    this.bankBranchService.delete(bankBranch.id).subscribe({
      next: (result) => {
        if (isApiSuccess(result)) {
          this.toast.success('Bank branch deleted.');
          this.load();
        } else {
          this.toast.error(result.errors?.[0]?.messageError ?? 'Delete failed.');
        }
      },
      error: () => this.toast.error('Delete failed.'),
    });
  }
}
