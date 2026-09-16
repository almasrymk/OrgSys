import { BaseEntity } from '../../../../shared/models/base-entity.model';

export interface PartyContact extends BaseEntity {
  dealerId: number;
  name: string | null;
  jobTitle: string | null;
  department: string | null;
  email: string | null;
  phone: string | null;
  mobile: string | null;
  isPrimary: boolean;
  isActive: boolean;
}

export interface SavePartyContactRequest {
  id?: number;
  dealerId: number;
  name: string;
  jobTitle?: string | null;
  department?: string | null;
  email?: string | null;
  phone?: string | null;
  mobile?: string | null;
  isPrimary: boolean;
  isActive: boolean;
}
