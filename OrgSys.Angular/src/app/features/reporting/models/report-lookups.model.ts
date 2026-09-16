import { BaseEntity } from '../../../shared/models/base-entity.model';

export interface CashBox extends BaseEntity {
  name: string | null;
}
