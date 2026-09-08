import { CommonModule } from '@angular/common';
import { Component, computed, signal } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { HasPermissionDirective } from '../../../../../core/auth/has-permission.directive';
import { PermissionService } from '../../../../../core/auth/permission.service';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { ConfirmDialogService } from '../../../../../shared/components/confirm-dialog/confirm-dialog.service';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { TreeViewComponent, TreeNodeAction } from '../../../../../shared/components/tree-view/tree-view.component';
import { TreeNode, buildTree } from '../../../../../shared/components/tree-view/tree-node.model';
import { Account } from '../../models/account.model';
import { AccountService } from '../../services/account.service';

const MAX_ACCOUNTS = 5000;

/** Replaces Areas/Setting/Views/Account/{Index,_AccountTreeNode}.cshtml. */
@Component({
  selector: 'app-account-list',
  standalone: true,
  imports: [CommonModule, RouterModule, PageHeaderComponent, TreeViewComponent, HasPermissionDirective],
  templateUrl: './account-list.component.html',
})
export class AccountListComponent {
  readonly loading = signal(false);
  readonly accounts = signal<Account[]>([]);
  readonly tree = computed<TreeNode<Account>[]>(() =>
    buildTree(
      this.accounts(),
      (a) => a.id,
      (a) => a.parentId,
      (a) => `${a.code ?? ''} ${a.name ?? ''}`.trim(),
      (a) => a.accountTypeName ?? undefined,
    ),
  );

  constructor(
    private readonly accountService: AccountService,
    private readonly toast: ToastService,
    private readonly confirmDialog: ConfirmDialogService,
    private readonly router: Router,
    readonly permissionService: PermissionService,
  ) {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.accountService.getList({ pageSize: MAX_ACCOUNTS }).subscribe({
      next: (result) => {
        this.loading.set(false);
        this.accounts.set(result.response ?? []);
      },
      error: () => {
        this.loading.set(false);
        this.toast.error('Could not load the chart of accounts.');
      },
    });
  }

  async onTreeAction(event: { action: TreeNodeAction; node: TreeNode<Account> }): Promise<void> {
    const { action, node } = event;

    if (action === 'add-child') {
      this.router.navigate(['/accounting/accounts/new'], { queryParams: { parentId: node.id } });
      return;
    }

    if (action === 'edit') {
      this.router.navigate(['/accounting/accounts', node.id, 'edit']);
      return;
    }

    const confirmed = await this.confirmDialog.confirm(
      `Delete "${node.data.name}"? This cannot be undone.`,
      'Delete account',
    );
    if (!confirmed) return;

    this.accountService.delete(node.id).subscribe({
      next: (result) => {
        if (isApiSuccess(result)) {
          this.toast.success('Account deleted.');
          this.load();
        } else {
          this.toast.error(result.errors?.[0]?.messageError ?? 'Delete failed.');
        }
      },
      error: () => this.toast.error('Delete failed.'),
    });
  }
}
