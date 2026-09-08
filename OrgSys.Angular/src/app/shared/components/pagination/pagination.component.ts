import { CommonModule } from '@angular/common';
import { Component, computed, input, output } from '@angular/core';

/** Replaces OrgSys.App's Views/Shared/PagedList.cshtml partial. */
@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './pagination.component.html',
})
export class PaginationComponent {
  page = input.required<number>();
  pageCount = input.required<number>();
  pageChange = output<number>();

  readonly pages = computed(() => {
    const count = Math.max(this.pageCount(), 1);
    return Array.from({ length: count }, (_, i) => i + 1);
  });

  goTo(page: number): void {
    if (page < 1 || page > this.pageCount() || page === this.page()) return;
    this.pageChange.emit(page);
  }
}
