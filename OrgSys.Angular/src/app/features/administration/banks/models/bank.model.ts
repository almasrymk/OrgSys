import { BaseEntity } from '../../../../shared/models/base-entity.model';

/** Mirrors Application.DTOs.BankDto. */
export interface Bank extends BaseEntity {
  name: string | null;
  countryId: number | null;
}

/** Mirrors CreateBankCommand/UpdateBankCommand (both BankDto subclasses). */
export interface SaveBankRequest {
  id?: number;
  name: string;
  countryId: number | null;
}
