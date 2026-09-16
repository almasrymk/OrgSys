import { BaseEntity } from '../../../../shared/models/base-entity.model';

export interface PermissionTreeNode {
  id: number;
  parentId: number;
  key: string | null;
  value: string | null;
  select: boolean;
}

export interface RolePermission {
  id?: number;
  roleId?: number;
  permissionId: number;
}

export interface Role extends BaseEntity {
  name: string | null;
  permissionList: RolePermission[] | null;
  permissionsTree: PermissionTreeNode[] | null;
}

export interface SaveRoleRequest {
  id?: number;
  name: string;
  permissionList: RolePermission[];
}
