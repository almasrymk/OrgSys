import { BaseEntity } from '../../../../shared/models/base-entity.model';

export interface CustomerProfile extends BaseEntity {
  dealerId: number;
  accountId: number | null;
  creditLimit: number | null;
  isCreditAllowed: boolean;
  isOnHold: boolean;
}

export interface AssignCustomerRoleRequest {
  dealerId: number;
  accountId?: number | null;
  autoCreateReceivableAccount?: boolean | null;
  creditLimit?: number | null;
}
