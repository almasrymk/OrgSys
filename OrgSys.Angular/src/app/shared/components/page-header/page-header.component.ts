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
}
