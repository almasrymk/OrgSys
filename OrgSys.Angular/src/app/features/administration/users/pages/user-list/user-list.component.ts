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
import { User } from '../../models/user.model';
import { UserService } from '../../services/user.service';

const PAGE_SIZE = 20;

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, PageHeaderComponent, PaginationComponent],
  templateUrl: './user-list.component.html',
})
export class UserListComponent {
  readonly Status = Status;
  readonly users = signal<User[]>([]);
  readonly loading = signal(false);
  readonly page = signal(1);
  readonly pageCount = signal(1);
  readonly keySearch = signal('');

  constructor(
    private readonly userService: UserService,
    private readonly toast: ToastService,
    private readonly confirmDialog: ConfirmDialogService,
  ) {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.userService.search({ keySearch: this.keySearch(), page: this.page(), pageSize: PAGE_SIZE }).subscribe({
      next: (result) => {
        this.loading.set(false);
        this.users.set(result.response ?? []);
        this.pageCount.set(result.pageCount || 1);
      },
      error: () => {
        this.loading.set(false);
        this.toast.error('Could not load users.');
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

  async onDelete(user: User): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(`Delete "${user.userName}"? This cannot be undone.`, 'Delete user');
    if (!confirmed) return;

    this.userService.delete(user.id).subscribe({
      next: (result) => {
        if (isApiSuccess(result)) {
          this.toast.success('User deleted.');
          this.load();
        } else {
          this.toast.error(result.errors?.[0]?.messageError ?? 'Delete failed.');
        }
      },
      error: () => this.toast.error('Delete failed.'),
    });
  }
}
