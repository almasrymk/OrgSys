import { BaseEntity } from '../../../../shared/models/base-entity.model';

/** Mirrors Domain.Enums.FinancialTransactionType — shares its Id space with the seeded FinancialType table. */
export enum FinancialTransactionType {
  OpeningBalance = 1,
  Receipt = 2,
  Payment = 3,
  TransferIn = 4,
  Deposit = 5,
  Withdrawal = 6,
  Fee = 7,
  Interest = 8,
  Cheque = 9,
  Adjustment = 10,
  TransferOut = 11,
}

export enum FinancialTransactionDirection {
  In = 1,
  Out = 2,
}

/** Only the reference types PostTransactionCommandHandler actually resolves against a linked GL account. */
export enum FinancialReferenceType {
  Other = 0,
  Customer = 1,
  Supplier = 2,
  Expense = 4,
  Income = 5,
}

/** Mirrors Application.DTOs.FinancialTypeDto (Domain.Entities.FinancialType). */
export interface FinancialType extends BaseEntity {
  name: string | null;
  inOut: number; // 1 = In-only, -1 = Out-only, 0 = user chooses (see Direction lock rule)
  icon: string | null;
}

/** Mirrors Application.DTOs.FinancialDto (extends Financial extends MovementModel). */
export interface Financial extends BaseEntity {
  date: string;
  posted: boolean;
  dealerId: number | null;
  dealerName: string | null;
  currencyId: number;
  currencyName: string | null;
  rate: number;
  amount: number;
  financialAccountId: number | null;
  financialAccountName: string | null;
  financialTypeId: number;
  financialTypeName: string | null;
  direction: FinancialTransactionDirection | null;
  referenceType: FinancialReferenceType;
  referenceId: number | null;
  referenceNumber: string | null;
  referenceName: string | null;
  counterAccountId: number;
  counterAccountName: string | null;
  notes: string | null;
  financialTransferId: number | null;
}

/**
 * Mirrors Application.DTOs.PostFinancialTransactionDto — the ONLY way to create a non-OpeningBalance
 * Financial row (there is no generic Create call for these types; POST /Financial/Transactions/Post
 * creates AND posts in one step, immediately booking the linked Journal). OpeningBalance uses a
 * separate Draft-then-Post flow and is intentionally out of scope here — see financial.service.ts.
 */
export interface PostFinancialTransactionRequest {
  financialAccountId: number;
  financialTypeId: FinancialTransactionType;
  direction: FinancialTransactionDirection;
  amount: number;
  currencyId: number;
  exchangeRate: number;
  transactionDate: string;
  referenceType: FinancialReferenceType;
  referenceId: number | null;
  referenceNumber: string | null;
  counterAccountId: number;
  dealerId: number | null;
  description: string | null;
  createUserId: number;
  branchId: number | null;
  shiftId: number | null;
}
