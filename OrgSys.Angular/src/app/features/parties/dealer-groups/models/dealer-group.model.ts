import { BaseEntity } from '../../../../shared/models/base-entity.model';

/** Mirrors Domain.Enums.DealerType — shared by Dealer and DealerGroup's TypeId. */
export enum DealerType {
  Client = 1,
  Supplier = 2,
}

/** Mirrors Application.DTOs.DealerGroupDto. */
export interface DealerGroup extends BaseEntity {
  name: string | null;
}

/**
 * Mirrors CreateDealerGroupCommand/UpdateDealerGroupCommand (both DealerGroupDto subclasses).
 * `code`/`codeNumber` come from GetMax+1 (same pattern as FinancialAccount — see
 * dealer-group.service.ts and docs/ANGULAR_MIGRATION_INVENTORY.md item on this), `typeId` is the
 * DealerType this group belongs to.
 */
export interface SaveDealerGroupRequest {
  id?: number;
  name: string;
  typeId: DealerType;
  code: string;
  codeNumber: number;
}
