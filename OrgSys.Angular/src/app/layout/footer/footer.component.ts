import { Component } from '@angular/core';

/** Mirrors OrgSys.App's `<footer class="page-footer">` (_Layout.cshtml) — its copyright column
 * is real; the demo Review/Purchase/Docs link list on the right was unused template boilerplate
 * and is not reproduced. */
@Component({
  selector: 'app-footer',
  standalone: true,
  templateUrl: './footer.component.html',
})
export class FooterComponent {
  readonly year = new Date().getFullYear();
}
