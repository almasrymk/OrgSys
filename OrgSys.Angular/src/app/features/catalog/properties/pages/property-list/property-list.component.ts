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
import { AttributeDataType, Property } from '../../models/property.model';
import { PropertyService } from '../../services/property.service';

const PAGE_SIZE = 20;

@Component({
  selector: 'app-property-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, PageHeaderComponent, PaginationComponent],
  templateUrl: './property-list.component.html',
})
export class PropertyListComponent {
  readonly Status = Status;
  readonly AttributeDataType = AttributeDataType;
  readonly properties = signal<Property[]>([]);
  readonly loading = signal(false);
  readonly page = signal(1);
  readonly pageCount = signal(1);
  readonly keySearch = signal('');

  constructor(
    private readonly propertyService: PropertyService,
    private readonly toast: ToastService,
    private readonly confirmDialog: ConfirmDialogService,
  ) {
    this.load();
  }

  dataTypeLabel(dataType: AttributeDataType): string {
    return AttributeDataType[dataType] ?? String(dataType);
  }

  load(): void {
    this.loading.set(true);
    this.propertyService.search({ keySearch: this.keySearch(), page: this.page(), pageSize: PAGE_SIZE }).subscribe({
      next: (result) => {
        this.loading.set(false);
        this.properties.set(result.response ?? []);
        this.pageCount.set(result.pageCount || 1);
      },
      error: () => {
        this.loading.set(false);
        this.toast.error('Could not load properties.');
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

  async onDelete(property: Property): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(`Delete "${property.name}"? This cannot be undone.`, 'Delete property');
    if (!confirmed) return;

    this.propertyService.delete(property.id).subscribe({
      next: (result) => {
        if (isApiSuccess(result)) {
          this.toast.success('Property deleted.');
          this.load();
        } else {
          this.toast.error(result.errors?.[0]?.messageError ?? 'Delete failed.');
        }
      },
      error: () => this.toast.error('Delete failed.'),
    });
  }
}
