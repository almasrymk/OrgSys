import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HasPermissionDirective } from '../../../../../core/auth/has-permission.directive';
import { PermissionService } from '../../../../../core/auth/permission.service';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { ConfirmDialogService } from '../../../../../shared/components/confirm-dialog/confirm-dialog.service';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { Status } from '../../../../../shared/models/status.enum';
import { FinancialTransfer } from '../../models/financial-transfer.model';
import { FinancialTransferService } from '../../services/financial-transfer.service';

/** Replaces Areas/Financials/Views/FinancialTransfer/{Index,List}.cshtml. No permission keys of its own — reuses Financial.*. */
@Component({
  selector: 'app-financial-transfer-list',
  standalone: true,
  imports: [CommonModule, RouterModule, PageHeaderComponent, HasPermissionDirective],
  templateUrl: './financial-transfer-list.component.html',
})
export class FinancialTransferListComponent {
  private readonly transferService = inject(FinancialTransferService);
  private readonly toast = inject(ToastService);
  private readonly confirmDialog = inject(ConfirmDialogService);
  readonly permissionService = inject(PermissionService);

  readonly Status = Status;
  readonly transfers = signal<FinancialTransfer[]>([]);
  readonly loading = signal(false);

  constructor() {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.transferService.getList().subscribe({
      next: (result) => {
        this.loading.set(false);
        this.transfers.set(result.response ?? []);
      },
      error: () => {
        this.loading.set(false);
        this.toast.error('Could not load transfers.');
      },
    });
  }

  canReverse(t: FinancialTransfer): boolean {
    return t.status !== Status.Reversed && this.permissionService.can('Financial.Reverse');
  }

  async reverse(t: FinancialTransfer): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(
      `Reverse the transfer of ${t.amount} from "${t.fromFinancialAccountName}" to "${t.toFinancialAccountName}"?`,
      'Reverse transfer',
    );
    if (!confirmed) return;

    this.transferService.reverse(t.id).subscribe({
      next: (result) => {
        if (isApiSuccess(result)) {
          this.toast.success('Transfer reversed.');
          this.load();
        } else {
          this.toast.error(result.errors?.[0]?.messageError ?? 'Reverse failed.');
        }
      },
      error: () => this.toast.error('Reverse failed.'),
    });
  }
}
