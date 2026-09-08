import { Directive, Input, TemplateRef, ViewContainerRef, effect } from '@angular/core';
import { PermissionService } from './permission.service';

/**
 * Structural directive replacing Razor's `@if (User.IsAllowed("Key1,Key2"))`.
 * Usage: <button *appHasPermission="'Financial.Post'">Post</button>
 */
@Directive({
  selector: '[appHasPermission]',
  standalone: true,
})
export class HasPermissionDirective {
  private hasView = false;

  @Input() set appHasPermission(keys: string) {
    this.keys = keys;
    this.render();
  }

  private keys = '';

  constructor(
    private readonly templateRef: TemplateRef<unknown>,
    private readonly viewContainer: ViewContainerRef,
    private readonly permissionService: PermissionService,
  ) {
    effect(() => this.render());
  }

  private render(): void {
    const allowed = this.keys ? this.permissionService.can(this.keys) : true;

    if (allowed && !this.hasView) {
      this.viewContainer.createEmbeddedView(this.templateRef);
      this.hasView = true;
    } else if (!allowed && this.hasView) {
      this.viewContainer.clear();
      this.hasView = false;
    }
  }
}
