import { BaseEntity } from '../../../../shared/models/base-entity.model';

export interface Company extends BaseEntity {
  tenantId: number | null;
  legalName: string | null;
  tradeName: string | null;
  taxRegistrationNumber: string | null;
  commercialRegistrationNumber: string | null;
  defaultCurrencyId: number | null;
  countryId: number | null;
  address: string | null;
  phone: string | null;
  email: string | null;
  website: string | null;
}

export interface SaveCompanyRequest {
  id?: number;
  tenantId?: number | null;
  legalName?: string | null;
  tradeName?: string | null;
  taxRegistrationNumber?: string | null;
  commercialRegistrationNumber?: string | null;
  defaultCurrencyId?: number | null;
  countryId?: number | null;
  address?: string | null;
  phone?: string | null;
  email?: string | null;
  website?: string | null;
}
