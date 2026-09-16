import { BaseEntity } from '../../../shared/models/base-entity.model';

/** Mirrors Catalog.Application UnitDto — lookup fields used by line pickers. */
export interface Unit extends BaseEntity {
  name: string | null;
}
