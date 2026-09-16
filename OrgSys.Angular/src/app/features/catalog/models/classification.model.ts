import { BaseEntity } from '../../../shared/models/base-entity.model';

/** Mirrors Catalog.Application ClassificationDto. */
export interface Classification extends BaseEntity {
  name: string | null;
}
