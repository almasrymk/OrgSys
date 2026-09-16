import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { HasPermissionDirective } from '../../../../../core/auth/has-permission.directive';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { ConfirmDialogService } from '../../../../../shared/components/confirm-dialog/confirm-dialog.service';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { PaginationComponent } from '../../../../../shared/components/pagination/pagination.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { Status } from '../../../../../shared/models/status.enum';
import { Dealer, DealerType } from '../../models/dealer.model';
import { DealerService } from '../../services/dealer.service';

const PAGE_SIZE = 20;

/** Replaces Areas/Setting/Views/Dealer/{Index,List}.cshtml. One component drives both "Customers" and "Suppliers". */
@Component({
  selector: 'app-dealer-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, PageHeaderComponent, PaginationComponent, HasPermissionDirective],
  templateUrl: './dealer-list.component.html',
})
export class DealerListComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly service = inject(DealerService);
  private readonly toast = inject(ToastService);
  private readonly confirmDialog = inject(ConfirmDialogService);

  readonly Status = Status;
  readonly dealerType = this.route.snapshot.data['dealerType'] as DealerType;
  readonly title = this.route.snapshot.data['title'] as string;
  readonly permissionPrefix = this.route.snapshot.data['permissionPrefix'] as string;
  readonly basePath = this.route.snapshot.data['basePath'] as string;

  readonly dealers = signal<Dealer[]>([]);
  readonly loading = signal(false);
  readonly page = signal(1);
  readonly pageCount = signal(1);
  readonly keySearch = signal('');

  constructor() {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.service.search({ keySearch: this.keySearch(), typeId: this.dealerType, page: this.page(), pageSize: PAGE_SIZE }).subscribe({
      next: (result) => {
        this.loading.set(false);
        this.dealers.set(result.response ?? []);
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

  async onDelete(dealer: Dealer): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(`Delete "${dealer.name}"? This cannot be undone.`, `Delete ${this.title.slice(0, -1)}`);
    if (!confirmed) return;

    this.service.delete(dealer.id).subscribe({
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
