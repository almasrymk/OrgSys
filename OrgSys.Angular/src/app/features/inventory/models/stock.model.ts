import { BaseEntity } from '../../../shared/models/base-entity.model';

/** Mirrors Inventory StockDto — lookup fields used by warehouse/movement pickers. */
export interface Stock extends BaseEntity {
  name: string | null;
  branchId: number;
  branchName: string | null;
}
