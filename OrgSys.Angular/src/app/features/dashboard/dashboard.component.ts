import { Component } from '@angular/core';
import { PageHeaderComponent } from '../../shared/components/page-header/page-header.component';

/** Placeholder for HomeController.Dashboard — replace as the dashboard feature is migrated. */
@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [PageHeaderComponent],
  template: `
    <app-page-header title="Dashboard"></app-page-header>
    <div class="card">
      <div class="card-body">Welcome to OrgSys.</div>
    </div>
  `,
})
export class DashboardComponent {}
