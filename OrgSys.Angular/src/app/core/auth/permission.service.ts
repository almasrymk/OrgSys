import { Injectable, computed } from '@angular/core';
import { AuthService } from './auth.service';

/**
 * Replaces OrgSys.App's `User.IsAllowed("Key1,Key2")` extension method.
 * RoleId === 1 is the superadmin/owner role and is always allowed, matching Extensions.cs.
 */
@Injectable({ providedIn: 'root' })
export class PermissionService {
  private readonly OWNER_ROLE_ID = 1;

  private readonly permissionKeys = computed(
    () => new Set(this.auth.currentUser()?.permissions.map((p) => p.key) ?? []),
  );

  private readonly isOwner = computed(() => this.auth.currentUser()?.roleId === this.OWNER_ROLE_ID);

  constructor(private readonly auth: AuthService) {}

  /** Accepts a single key or a comma-separated list, mirroring `User.IsAllowed("A,B,C")`. */
  can(keys: string): boolean {
    if (this.isOwner()) return true;

    const requested = keys.split(',').map((k) => k.trim()).filter(Boolean);
    const granted = this.permissionKeys();

    return requested.some((key) => granted.has(key));
  }
}
