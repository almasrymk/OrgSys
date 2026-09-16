import { Injectable, computed, signal } from '@angular/core';

/**
 * Tracks the Dore menu state that OrgSys.App keys off `#app-container` classes
 * (see wwwroot/js/dore.script.js §03.05's `allMenuClassNames`).
 *
 * - `menu-sub-hidden`: hamburger collapsed the labelled pane (user preference).
 * - `sub-hidden`: the active rail item has no children (Dashboard), so Dore
 *   slides the second pane away instead of leaving an empty column open.
 */
@Injectable({ providedIn: 'root' })
export class LayoutService {
  private readonly menuSubHidden = signal(false);
  private readonly hasSubmenu = signal(false);
  private readonly mobileOpen = signal(false);

  readonly appContainerClass = computed(() => {
    const classes = ['menu-default'];
    if (this.menuSubHidden()) classes.push('menu-sub-hidden');
    if (!this.hasSubmenu()) classes.push('sub-hidden');
    if (this.mobileOpen()) classes.push('menu-mobile');
    return classes.join(' ');
  });

  setHasSubmenu(has: boolean): void {
    this.hasSubmenu.set(has);
  }

  revealSubmenu(): void {
    this.menuSubHidden.set(false);
  }

  toggleDesktopMenu(): void {
    this.menuSubHidden.update((v) => !v);
  }

  toggleMobileMenu(): void {
    this.mobileOpen.update((v) => !v);
  }
}
