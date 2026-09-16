import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { isApiSuccess } from '../../../../core/models/api-result.model';
import { ConfirmDialogService } from '../../../../shared/components/confirm-dialog/confirm-dialog.service';
import { PageHeaderComponent } from '../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../shared/components/toast/toast.service';
import { CUSTODY_STATUS_LABEL, Custody, CustodyStatus } from '../../models/custody.model';
import { CustodyService } from '../../services/custody.service';

@Component({
  selector: 'app-custody-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './custody-list.component.html',
})
export class CustodyListComponent {
  private readonly service = inject(CustodyService);
  private readonly toast = inject(ToastService);
  private readonly confirmDialog = inject(ConfirmDialogService);

  readonly CustodyStatus = CustodyStatus;
  readonly statusLabel = CUSTODY_STATUS_LABEL;
  readonly rows = signal<Custody[]>([]);
  readonly loading = signal(false);
  readonly statusFilter = signal<CustodyStatus | 0>(0);

  constructor() {
    this.search();
  }

  search(): void {
    this.loading.set(true);
    const status = this.statusFilter() === 0 ? null : this.statusFilter();
    this.service.getList(null, status).subscribe({
      next: (result) => {
        this.loading.set(false);
        this.rows.set(result.response ?? []);
      },
      error: () => {
        this.loading.set(false);
        this.toast.error('Could not load custodies.');
      },
    });
  }

  async approve(row: Custody): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(`Approve custody "${row.code ?? row.id}"?`, 'Approve');
    if (!confirmed) return;
    this.service.approve(row.id).subscribe({
      next: (result) => {
        if (isApiSuccess(result)) {
          this.toast.success('Custody approved.');
          this.search();
        } else {
          this.toast.error(result.errors?.[0]?.messageError ?? 'Approve failed.');
        }
      },
      error: () => this.toast.error('Approve failed.'),
    });
  }

  async cancel(row: Custody): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(`Cancel draft custody "${row.code ?? row.id}"?`, 'Cancel');
    if (!confirmed) return;
    this.service.cancel(row.id).subscribe({
      next: (result) => {
        if (isApiSuccess(result)) {
          this.toast.success('Custody cancelled.');
          this.search();
        } else {
          this.toast.error(result.errors?.[0]?.messageError ?? 'Cancel failed.');
        }
      },
      error: () => this.toast.error('Cancel failed.'),
    });
  }
}
