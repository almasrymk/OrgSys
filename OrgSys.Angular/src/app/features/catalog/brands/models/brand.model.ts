import { BaseEntity } from '../../../../shared/models/base-entity.model';

export interface Brand extends BaseEntity {
  name: string | null;
  description: string | null;
  isActive: boolean;
}

export interface SaveBrandRequest {
  id?: number;
  name: string;
  description?: string | null;
  isActive: boolean;
}
