import { BaseEntity } from '../../../../shared/models/base-entity.model';

/** Mirrors Domain.Enums.FinancialAccountType. */
export enum FinancialAccountType {
  CashBox = 1,
  Bank = 2,
}

/**
 * Mirrors Application.DTOs.FinancialAccountDto (Application/DTOs/OrgDb/UnifiedFinancialDtos.cs) —
 * one entity for both CashBox and Bank, multiplexed by `financialAccountType` (filters via the
 * standard `typeId` search param — GetListFinancialAccountQuery filters on FinancialAccountType,
 * not BaseModel.TypeId, see docs/ANGULAR_MIGRATION_INVENTORY.md).
 */
export interface FinancialAccount extends BaseEntity {
  name: string;
  financialAccountType: FinancialAccountType;
  accountId: number | null;
  accountName: string | null;
  accountCode: string | null;
  currencyId: number | null;
  currencyName: string | null;
  isActive: boolean;
  branchId: number | null;
  keeperUserId: number | null;
  bankId: number | null;
  bankName: string | null;
  bankBranchId: number | null;
  bankBranchDisplayName: string | null;
  accountNumber: string | null;
  iban: string | null;
  swiftCode: string | null;
}

/**
 * `typeId`/`code`/`codeNumber` are BaseModel fields OrgSys.App's FinancialAccountController sets
 * explicitly before Create: `TypeId` is kept equal to `financialAccountType` (GetMax filters by
 * `TypeId`, not `FinancialAccountType`, unlike GetList/Search — see
 * docs/ANGULAR_MIGRATION_INVENTORY.md), and `Code`/`CodeNumber` come from GetMax+1. None of this is
 * server-assigned; the Angular client has to replicate it (see financial-account.service.ts).
 */
export interface SaveFinancialAccountRequest {
  id?: number;
  name: string;
  financialAccountType: FinancialAccountType;
  typeId: FinancialAccountType;
  code: string;
  codeNumber: number;
  accountId: number;
  currencyId: number | null;
  isActive: boolean;
  branchId?: number | null;
  keeperUserId?: number | null;
  bankId?: number | null;
  bankBranchId?: number | null;
  accountNumber?: string | null;
  iban?: string | null;
  swiftCode?: string | null;
}
