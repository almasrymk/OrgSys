import { CommonModule } from '@angular/common';
import { Component, computed, HostListener, inject, signal } from '@angular/core';
import { NavigationEnd, Router, RouterModule } from '@angular/router';
import { HasPermissionDirective } from '../../core/auth/has-permission.directive';
import { MENU, MenuItem } from '../../core/config/menu.config';
import { LayoutService } from '../layout.service';

/**
 * Dore's two-pane menu (see OrgSys/Views/Shared/_MainMenu.cshtml and
 * OrgSys/wwwroot/js/dore.script.js §03.05): an icon-only rail (`.main-menu`) plus a labelled
 * panel (`.sub-menu`) for the children of the rail item the user opened.
 *
 * The labelled pane is a flyout: it opens on a parent rail click and closes on an outside
 * click or after navigation — same idea as Dore's document-click handler around `sub-hidden`.
 */
@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule, HasPermissionDirective],
  templateUrl: './sidebar.component.html',
})
export class SidebarComponent {
  readonly menu: MenuItem[] = MENU;
  private readonly layout = inject(LayoutService);
  private readonly router = inject(Router);
  private readonly openedGroup = signal<number | null>(null);
  private readonly currentUrl = signal(this.router.url);

  readonly openedGroupIndex = this.openedGroup.asReadonly();

  readonly urlGroupIndex = computed(() => {
    const url = this.currentUrl();
    return this.menu.findIndex((item) => this.groupMatchesUrl(item, url));
  });

  constructor() {
    this.syncSubmenuVisibility();
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.currentUrl.set(event.urlAfterRedirects);
        this.closeSubmenu();
      }
    });
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    const target = event.target as HTMLElement | null;
    if (!target) {
      return;
    }
    if (target.closest('.menu, .menu-button, .menu-button-mobile')) {
      return;
    }
    this.closeSubmenu();
  }

  isRailActive(item: MenuItem, index: number): boolean {
    if (item.route) {
      return false;
    }
    return this.urlGroupIndex() === index || this.openedGroup() === index;
  }

  onRailClick(item: MenuItem, index: number, event: Event): void {
    if (item.route) {
      return;
    }
    event.preventDefault();
    event.stopPropagation();
    const next = this.openedGroup() === index ? null : index;
    this.openedGroup.set(next);
    if (next !== null) {
      this.layout.revealSubmenu();
    }
    this.syncSubmenuVisibility();
  }

  private closeSubmenu(): void {
    if (this.openedGroup() === null) {
      return;
    }
    this.openedGroup.set(null);
    this.syncSubmenuVisibility();
  }

  private groupMatchesUrl(item: MenuItem, url: string): boolean {
    if (!item.children?.length) {
      return false;
    }
    return item.children.some((child) => {
      if (!child.route) {
        return false;
      }
      return url === child.route || url.startsWith(child.route + '/') || url.startsWith(child.route + '?');
    });
  }

  private syncSubmenuVisibility(): void {
    this.layout.setHasSubmenu(this.openedGroup() !== null);
  }
}
