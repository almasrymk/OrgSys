import { BaseEntity } from '../../../../shared/models/base-entity.model';

export interface OrganizationSettings extends BaseEntity {
  companyId: number;
  defaultCurrencyId: number | null;
  defaultCountryId: number | null;
  defaultTimeZone: string | null;
  fiscalYearStartMonth: number | null;
  fiscalYearStartDay: number | null;
}

export interface UpdateOrganizationSettingsRequest {
  companyId: number;
  defaultCurrencyId?: number | null;
  defaultCountryId?: number | null;
  defaultTimeZone?: string | null;
  fiscalYearStartMonth?: number | null;
  fiscalYearStartDay?: number | null;
}
