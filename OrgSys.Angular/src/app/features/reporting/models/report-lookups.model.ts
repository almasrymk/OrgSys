import { BaseEntity } from '../../../shared/models/base-entity.model';

export interface Classification extends BaseEntity {
  name: string | null;
}

export interface CashBox extends BaseEntity {
  name: string | null;
}
