import { CommonModule } from '@angular/common';
import { Component, input } from '@angular/core';
import { RouterModule } from '@angular/router';

export interface Breadcrumb {
  label: string;
  route?: string;
}

/** Replaces the repeated breadcrumb + <h1> block at the top of every OrgSys.App Index/Save view. */
@Component({
  selector: 'app-page-header',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './page-header.component.html',
})
export class PageHeaderComponent {
  title = input.required<string>();
  breadcrumbs = input<Breadcrumb[]>([]);
  /** Dore icon class (e.g. 'iconsminds-coins'), matching the icon shown next to the <h1> on the
   * equivalent OrgSys.App view (see ViewBag.FinancialTypeIcon in Areas/Financials/Views). */
  icon = input<string>();
}
