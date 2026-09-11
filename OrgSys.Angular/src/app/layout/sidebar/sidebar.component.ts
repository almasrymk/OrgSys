import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { NavigationEnd, Router, RouterModule } from '@angular/router';
import { HasPermissionDirective } from '../../core/auth/has-permission.directive';
import { MENU, MenuItem } from '../../core/config/menu.config';
import { LayoutService } from '../layout.service';

/**
 * Dore's two-pane menu (see OrgSys/Views/Shared/_MainMenu.cshtml and
 * OrgSys/wwwroot/js/dore.script.js §03.05): an icon-only rail (`.main-menu`) plus a labelled
 * panel (`.sub-menu`) showing the children of whichever rail item is active. MVC matches the
 * panel to the rail item via a `data-link` attribute compared against the active controller name;
 * here the same idea is done with the current route instead of a controller name.
 */
@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule, HasPermissionDirective],
  templateUrl: './sidebar.component.html',
})
export class SidebarComponent {
  readonly menu: MenuItem[] = MENU;
  readonly layout = inject(LayoutService);

  private readonly router = inject(Router);
  private readonly manualGroup = signal<number | null>(null);
  private readonly currentUrl = signal(this.router.url);

  readonly activeGroupIndex = computed(() => {
    const manual = this.manualGroup();
    if (manual !== null) {
      return manual;
    }
    const url = this.currentUrl();
    return this.menu.findIndex((item) =>
      item.children?.some((child) => child.route && (url === child.route || url.startsWith(child.route + '/') || url.startsWith(child.route + '?'))),
    );
  });

  constructor() {
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.currentUrl.set(event.urlAfterRedirects);
        this.manualGroup.set(null);
      }
    });
  }

  onRailClick(item: MenuItem, index: number, event: Event): void {
    if (item.route) {
      return;
    }
    event.preventDefault();
    this.manualGroup.set(this.activeGroupIndex() === index ? null : index);
  }
}
