import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { PermissionService } from '../auth/permission.service';

/**
 * Route-level equivalent of Razor's per-menu-item `User.IsAllowed(...)` gating.
 * Usage in a route: `canActivate: [permissionGuard('Countries.All')]`
 */
export function permissionGuard(keys: string): CanActivateFn {
  return () => {
    const permissionService = inject(PermissionService);
    const router = inject(Router);

    if (permissionService.can(keys)) return true;

    router.navigateByUrl('/');
    return false;
  };
}
