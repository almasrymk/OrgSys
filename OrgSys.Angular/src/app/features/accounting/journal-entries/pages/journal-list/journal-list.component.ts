import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HasPermissionDirective } from '../../../../../core/auth/has-permission.directive';
import { PermissionService } from '../../../../../core/auth/permission.service';
import { ApiResult, isApiSuccess } from '../../../../../core/models/api-result.model';
import { ConfirmDialogService } from '../../../../../shared/components/confirm-dialog/confirm-dialog.service';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { PaginationComponent } from '../../../../../shared/components/pagination/pagination.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { Status } from '../../../../../shared/models/status.enum';
import { Journal } from '../../models/journal.model';
import { JournalService } from '../../services/journal.service';

const PAGE_SIZE = 20;

/**
 * Replaces Areas/Financials/Views/Journal/{Index,List}.cshtml. Workflow-action visibility mirrors
 * the state machine enforced server-side in Application/Commands/.../Journal/Command/*CommandHandler.cs
 * — this is UX only, the API re-validates every transition (see docs/ANGULAR_MIGRATION_INVENTORY.md §5).
 */
@Component({
  selector: 'app-journal-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, PageHeaderComponent, PaginationComponent, HasPermissionDirective],
  templateUrl: './journal-list.component.html',
})
export class JournalListComponent {
  readonly Status = Status;

  readonly journals = signal<Journal[]>([]);
  readonly loading = signal(false);
  readonly page = signal(1);
  readonly pageCount = signal(1);
  readonly keySearch = signal('');

  constructor(
    private readonly journalService: JournalService,
    private readonly toast: ToastService,
    private readonly confirmDialog: ConfirmDialogService,
    private readonly permissionService: PermissionService,
  ) {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.journalService.search({ keySearch: this.keySearch(), page: this.page(), pageSize: PAGE_SIZE }).subscribe({
      next: (result) => {
        this.loading.set(false);
        this.journals.set(result.response ?? []);
        this.pageCount.set(result.pageCount || 1);
      },
      error: () => {
        this.loading.set(false);
        this.toast.error('Could not load journal entries.');
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

  statusLabel(journal: Journal): string {
    if (journal.status === Status.Reversed) return 'Reversed';
    if (journal.status === Status.Cancel) return 'Cancelled';
    if (journal.status === Status.New) return journal.posted ? 'Posted' : 'Draft';
    return journal.posted ? `Posted (${Status[journal.status]})` : Status[journal.status];
  }

  statusBadgeClass(journal: Journal): string {
    if (journal.status === Status.Reversed) return 'badge-secondary';
    if (journal.status === Status.Cancel) return 'badge-warning';
    if (journal.posted) return 'badge-success';
    return 'badge-light';
  }

  private isDraft(journal: Journal): boolean {
    return !journal.refranceTable && !journal.posted && journal.status === Status.New;
  }

  canEdit(journal: Journal): boolean {
    return this.isDraft(journal);
  }

  canPost(journal: Journal): boolean {
    return this.isDraft(journal) && this.permissionService.can('Journal.Post');
  }

  canCancel(journal: Journal): boolean {
    return this.isDraft(journal) && this.permissionService.can('Journal.Cancel');
  }

  canDelete(journal: Journal): boolean {
    return this.isDraft(journal) && this.permissionService.can('Journal.Delete');
  }

  canReverse(journal: Journal): boolean {
    return !journal.refranceTable && journal.posted && journal.status === Status.New && this.permissionService.can('Journal.Reverse');
  }

  canRedo(journal: Journal): boolean {
    return !journal.refranceTable && !journal.posted && journal.status === Status.Cancel && this.permissionService.can('Journal.Redo');
  }

  post(journal: Journal): void {
    this.journalService.post(journal.id).subscribe({
      next: (result) => this.handleActionResult(result, 'Journal entry posted.'),
      error: () => this.toast.error('Post failed.'),
    });
  }

  async cancel(journal: Journal): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(`Cancel journal entry "${journal.code}"?`, 'Cancel journal');
    if (!confirmed) return;

    this.journalService.cancel(journal.id).subscribe({
      next: (result) => this.handleActionResult(result, 'Journal entry cancelled.'),
      error: () => this.toast.error('Cancel failed.'),
    });
  }

  redo(journal: Journal): void {
    this.journalService.redo(journal.id).subscribe({
      next: (result) => this.handleActionResult(result, 'Journal entry restored.'),
      error: () => this.toast.error('Redo failed.'),
    });
  }

  async reverse(journal: Journal): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(
      `Reverse journal entry "${journal.code}"? This books a new, balancing entry.`,
      'Reverse journal',
    );
    if (!confirmed) return;

    this.journalService.reverse(journal.id).subscribe({
      next: (result) => this.handleActionResult(result, 'Journal entry reversed.'),
      error: () => this.toast.error('Reverse failed.'),
    });
  }

  async onDelete(journal: Journal): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(
      `Delete journal entry "${journal.code}"? This cannot be undone.`,
      'Delete journal',
    );
    if (!confirmed) return;

    this.journalService.delete(journal.id).subscribe({
      next: (result) => this.handleActionResult(result, 'Journal entry deleted.'),
      error: () => this.toast.error('Delete failed.'),
    });
  }

  private handleActionResult(result: ApiResult, successMessage: string): void {
    if (isApiSuccess(result)) {
      this.toast.success(successMessage);
      this.load();
    } else {
      this.toast.error(result.errors?.[0]?.messageError ?? 'The action failed.');
    }
  }
}
