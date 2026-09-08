import { Status } from '../../../../shared/models/status.enum';

/**
 * Mirrors Application.DTOs.FinancialTransferDto — a self-contained DTO (not a BaseModel/MovementDto
 * subclass), same shape for both read and the Post request. A posted Transfer creates TWO linked
 * `Financial` legs sharing one Journal (see PostFinancialTransferCommandHandler) — Reverse flips
 * both legs and the shared Journal in one call (see ReverseFinancialTransferCommandHandler).
 */
export interface FinancialTransfer {
  id: number;
  fromFinancialAccountId: number;
  toFinancialAccountId: number;
  amount: number;
  currencyId: number;
  exchangeRate: number;
  transactionDate: string;
  description: string | null;
  createUserId: number;
  branchId: number | null;
  shiftId: number | null;
  fromFinancialAccountName: string | null;
  toFinancialAccountName: string | null;
  status: Status;
  journalId: number | null;
}

export interface PostFinancialTransferRequest {
  fromFinancialAccountId: number;
  toFinancialAccountId: number;
  amount: number;
  currencyId: number;
  exchangeRate: number;
  transactionDate: string;
  description: string | null;
  createUserId: number;
  branchId: number | null;
  shiftId: number | null;
}
