import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { AuthService } from '../../core/auth/auth.service';
import { LayoutService } from '../layout.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './header.component.html',
})
export class HeaderComponent {
  readonly defaultAvatar = 'assets/dore/img/User.png';

  constructor(
    readonly auth: AuthService,
    readonly layout: LayoutService,
  ) {}

  logout(): void {
    this.auth.logout();
  }
}
