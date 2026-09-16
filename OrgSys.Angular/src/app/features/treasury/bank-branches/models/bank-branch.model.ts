import { BaseEntity } from '../../../../shared/models/base-entity.model';

/** Mirrors Application.DTOs.BankBranchDto. */
export interface BankBranch extends BaseEntity {
  name: string | null;
  bankId: number;
  countryId: number;
  cityId: number;
  districtId: number;
  bankName: string | null;
  countryName: string | null;
  cityName: string | null;
  districtName: string | null;
}

/** Mirrors CreateBankBranchCommand/UpdateBankBranchCommand (both BankBranchDto subclasses). */
export interface SaveBankBranchRequest {
  id?: number;
  name: string;
  bankId: number;
  countryId: number;
  cityId: number;
  districtId: number;
}
