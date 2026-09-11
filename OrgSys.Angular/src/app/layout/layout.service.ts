import { Injectable, signal } from '@angular/core';

/**
 * Tracks the Dore menu state that OrgSys.App keys off `#app-container` classes
 * (see wwwroot/js/dore.script.js §03.05's `allMenuClassNames`). Only the two states the
 * header's hamburger buttons actually drive are reproduced: the desktop toggle collapses the
 * labelled sub-menu panel down to the icon rail, the mobile toggle slides the whole menu in/out.
 */
@Injectable({ providedIn: 'root' })
export class LayoutService {
  private readonly subHidden = signal(false);
  private readonly mobileOpen = signal(false);

  readonly appContainerClass = () => {
    const classes = ['menu-default'];
    if (this.subHidden()) classes.push('menu-sub-hidden');
    if (this.mobileOpen()) classes.push('menu-mobile');
    return classes.join(' ');
  };

  toggleDesktopMenu(): void {
    this.subHidden.update((v) => !v);
  }

  toggleMobileMenu(): void {
    this.mobileOpen.update((v) => !v);
  }
}
