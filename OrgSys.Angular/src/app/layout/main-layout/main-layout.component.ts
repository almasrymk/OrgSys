import { Component, HostBinding } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ConfirmDialogComponent } from '../../shared/components/confirm-dialog/confirm-dialog.component';
import { ToastContainerComponent } from '../../shared/components/toast/toast-container.component';
import { FooterComponent } from '../footer/footer.component';
import { HeaderComponent } from '../header/header.component';
import { LayoutService } from '../layout.service';
import { SidebarComponent } from '../sidebar/sidebar.component';

/**
 * Reproduces OrgSys.App's `<body id="app-container" class="menu-default">` shell
 * (see OrgSys/Views/Shared/_Layout.cshtml) — the Dore CSS/menu logic keys off that id and its
 * classes, so this component's host element stands in for `<body>` here.
 */
@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, SidebarComponent, FooterComponent, ToastContainerComponent, ConfirmDialogComponent],
  templateUrl: './main-layout.component.html',
})
export class MainLayoutComponent {
  @HostBinding('id') readonly id = 'app-container';
  @HostBinding('class') get hostClass(): string {
    return this.layout.appContainerClass();
  }
  @HostBinding('attr.dir') readonly dir = 'ltr';

  constructor(readonly layout: LayoutService) {}
}
