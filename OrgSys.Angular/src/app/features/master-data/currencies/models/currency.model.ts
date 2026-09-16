import { BaseEntity } from '../../../../shared/models/base-entity.model';

/** Mirrors Application.DTOs.CurrencyDto. */
export interface Currency extends BaseEntity {
  name: string | null;
  rate: number;
  isDefault: boolean;
}

/** Mirrors CreateCurrencyCommand/UpdateCurrencyCommand (both CurrencyDto subclasses). */
export interface SaveCurrencyRequest {
  id?: number;
  name: string;
  rate: number;
  isDefault: boolean;
}
