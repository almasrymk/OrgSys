import { BaseEntity } from '../../../../shared/models/base-entity.model';

/** Mirrors Application.DTOs.BankDto. No dedicated Angular admin screen yet — lookup-only, like AccountType. */
export interface Bank extends BaseEntity {
  name: string | null;
  countryId: number | null;
}

/** Mirrors Application.DTOs.BankBranchDto. */
export interface BankBranch extends BaseEntity {
  name: string | null;
  bankId: number;
  bankName: string | null;
}
