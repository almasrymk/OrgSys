import { BaseEntity } from '../../../../shared/models/base-entity.model';

export interface SupplierProfile extends BaseEntity {
  dealerId: number;
  accountId: number | null;
  isApproved: boolean;
  isOnHold: boolean;
}

export interface AssignSupplierRoleRequest {
  dealerId: number;
  accountId?: number | null;
  autoCreatePayableAccount?: boolean | null;
}
