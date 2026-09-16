import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../../../../core/auth/auth.service';
import { HasPermissionDirective } from '../../../../../core/auth/has-permission.directive';
import { PermissionService } from '../../../../../core/auth/permission.service';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { ConfirmDialogService } from '../../../../../shared/components/confirm-dialog/confirm-dialog.service';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { Status } from '../../../../../shared/models/status.enum';
import { OpeningBalance } from '../../models/opening-balance.model';
import { OpeningBalanceService } from '../../services/opening-balance.service';

/** Replaces Areas/Financials/Views/Financial/Index.cshtml filtered to TypeId=1 (Opening Balance). */
@Component({
  selector: 'app-opening-balance-list',
  standalone: true,
  imports: [CommonModule, RouterModule, PageHeaderComponent, HasPermissionDirective],
  templateUrl: './opening-balance-list.component.html',
})
export class OpeningBalanceListComponent {
  private readonly service = inject(OpeningBalanceService);
  private readonly toast = inject(ToastService);
  private readonly confirmDialog = inject(ConfirmDialogService);
  private readonly auth = inject(AuthService);
  readonly permissionService = inject(PermissionService);

  readonly Status = Status;
  readonly rows = signal<OpeningBalance[]>([]);
  readonly loading = signal(false);

  constructor() {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.service.search({ typeId: 1, page: 1, pageSize: 100 }).subscribe({
      next: (result) => {
        this.loading.set(false);
        this.rows.set(result.response ?? []);
      },
      error: () => {
        this.loading.set(false);
        this.toast.error('Could not load opening balances.');
      },
    });
  }

  isDraft(row: OpeningBalance): boolean {
    return !row.posted && row.status === Status.New;
  }

  canPost(row: OpeningBalance): boolean {
    return this.isDraft(row) && this.permissionService.can('Financial.Post');
  }

  canReverse(row: OpeningBalance): boolean {
    return row.posted && row.status !== Status.Reversed && this.permissionService.can('Financial.Reverse');
  }

  canDelete(row: OpeningBalance): boolean {
    return this.isDraft(row) && this.permissionService.can('Financial.Delete');
  }

  post(row: OpeningBalance): void {
    const userId = this.auth.currentUser()?.id ?? 0;
    this.service.post(row.id, userId).subscribe({
      next: (result) => {
        if (isApiSuccess(result)) {
          this.toast.success('Opening balance posted.');
          this.load();
        } else {
          this.toast.error(result.errors?.[0]?.messageError ?? 'Post failed.');
        }
      },
      error: () => this.toast.error('Post failed.'),
    });
  }

  async reverse(row: OpeningBalance): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(`Reverse this Opening Balance of ${row.amount}?`, 'Reverse opening balance');
    if (!confirmed) return;

    this.service.reverse(row.id).subscribe({
      next: (result) => {
        if (isApiSuccess(result)) {
          this.toast.success('Opening balance reversed.');
          this.load();
        } else {
          this.toast.error(result.errors?.[0]?.messageError ?? 'Reverse failed.');
        }
      },
      error: () => this.toast.error('Reverse failed.'),
    });
  }

  async onDelete(row: OpeningBalance): Promise<void> {
    const confirmed = await this.confirmDialog.confirm('Delete this Draft Opening Balance?', 'Delete opening balance');
    if (!confirmed) return;

    this.service.delete(row.id).subscribe({
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
