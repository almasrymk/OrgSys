import { BaseEntity } from '../../../../shared/models/base-entity.model';

export interface User extends BaseEntity {
  name: string | null;
  userName: string | null;
  password: string | null;
  mustResetPassword: boolean;
  roleId: number;
  branchId: number | null;
  loginUserId: number;
  roleName: string | null;
  branchName: string | null;
}

export interface SaveUserRequest {
  id?: number;
  name: string;
  userName: string;
  password?: string | null;
  newPassword?: string | null;
  confirmPassword?: string | null;
  roleId: number;
  branchId?: number | null;
}
