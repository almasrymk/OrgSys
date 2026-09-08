import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HasPermissionDirective } from '../../core/auth/has-permission.directive';
import { MENU, MenuItem } from '../../core/config/menu.config';

/** Structured replacement for OrgSys.App's Views/Shared/_MainMenu.cshtml. */
@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule, HasPermissionDirective],
  templateUrl: './sidebar.component.html',
})
export class SidebarComponent {
  readonly menu: MenuItem[] = MENU;
}
