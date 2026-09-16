import { BaseEntity } from '../../../shared/models/base-entity.model';

/** Mirrors MasterData PaymentTypeDto. */
export interface PaymentType extends BaseEntity {
  name: string | null;
}
