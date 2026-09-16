import { Status } from '../../../../shared/models/status.enum';

/**
 * Mirrors the subset of Application.DTOs.FinancialDto that matters for FinancialTypeId=1
 * (OpeningBalance). Unlike every other FinancialType, this one IS created via the generic
 * Create/Update — see PostFinancialOpeningBalanceCommandHandler and financial-transaction-list's
 * docstring for why the other 10 types can't use Create/Update at all.
 */
export interface OpeningBalance {
  id: number;
  code: string | null;
  date: string;
  posted: boolean;
  status: Status;
  createUserId: number;
  createDate: string;
  financialAccountId: number;
  financialAccountName: string | null;
  amount: number;
  currencyId: number;
  currencyName: string | null;
  rate: number;
  notes: string | null;
}

/**
 * Mirrors CreateFinancialCommand/UpdateFinancialCommand (both FinancialDto subclasses) for the
 * OpeningBalance case only. `direction`/`paymentTypeId`/`referenceType` are fixed, not user-editable
 * — MVC's SaveOpeningBalanceDraft locks them the same way (Direction=In, PaymentTypeId=1/"Cash" since
 * Financial.PaymentTypeId is a required FK with nothing on this screen to set it from,
 * ReferenceType=Other). `code`/`codeNumber` are deliberately omitted — unlike Journal/FinancialAccount,
 * OrgSys.App's own FinancialController.SaveOpeningBalanceDraft never calls GetMax for these either.
 */
export interface SaveOpeningBalanceRequest {
  id?: number;
  financialAccountId: number;
  financialTypeId: 1;
  typeId: 1;
  direction: 1;
  referenceType: 0;
  paymentTypeId: 1;
  date: string;
  amount: number;
  amountByDefaultCurrency: number;
  currencyId: number;
  rate: number;
  notes: string | null;
  createUserId?: number;
  createDate?: string;
  modifyUserId?: number;
  modifyDate?: string;
}
