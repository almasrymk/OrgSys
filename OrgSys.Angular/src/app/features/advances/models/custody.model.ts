export enum CustodyStatus {
  Draft = 0,
  Approved = 10,
  Issued = 20,
  PartiallySettled = 30,
  Settled = 40,
  Closed = 50,
  Cancelled = 60,
}

export interface CustodyHandover {
  id: number;
  fromHolderId: number;
  toHolderId: number;
  transferDate: string;
  transferredAmount: number;
  reason: string | null;
  approvedByUserId: number;
}

export interface Custody {
  id: number;
  code: string | null;
  holderId: number;
  purpose: string;
  dueDate: string | null;
  currencyId: number;
  rate: number;
  issuedAmount: number;
  settledAmount: number;
  returnedAmount: number;
  outstandingAmount: number;
  lifecycleStatus: CustodyStatus;
  issueDate: string | null;
  issuingFinancialTransactionId: number | null;
  returnFinancialTransactionId: number | null;
  notes: string | null;
  date: string;
  branchId: number | null;
  handovers: CustodyHandover[];
}

export interface CreateCustodyRequest {
  holderId: number;
  purpose: string;
  currencyId: number;
  rate: number;
  issuedAmount: number;
  dueDate: string | null;
  createUserId: number;
  createDate: string;
  branchId: number | null;
  notes: string | null;
  code: string | null;
}

export interface IssueCustodyRequest {
  id: number;
  financialAccountId: number;
  counterAccountId: number;
  issueDate: string;
  createUserId: number;
  branchId: number | null;
  shiftId: number | null;
}

export interface ReturnCustodyRequest {
  id: number;
  amount: number;
  financialAccountId: number;
  counterAccountId: number;
  returnDate: string;
  createUserId: number;
  branchId: number | null;
  shiftId: number | null;
}

export const CUSTODY_STATUS_LABEL: Record<CustodyStatus, string> = {
  [CustodyStatus.Draft]: 'Draft',
  [CustodyStatus.Approved]: 'Approved',
  [CustodyStatus.Issued]: 'Issued',
  [CustodyStatus.PartiallySettled]: 'Partially settled',
  [CustodyStatus.Settled]: 'Settled',
  [CustodyStatus.Closed]: 'Closed',
  [CustodyStatus.Cancelled]: 'Cancelled',
};
