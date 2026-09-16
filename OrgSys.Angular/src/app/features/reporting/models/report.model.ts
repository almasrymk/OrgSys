/** Mirrors Application.Report.ProductStatment — backs both Stock Movement and Product Movement (one query, see WarehouseReportController.Movement). */
export interface MovementRow {
  id: number;
  referenceId: number | null;
  productCode: string | null;
  transactionCode: string | null;
  typeId: number | null;
  typeName: string | null;
  date: string;
  productId: number;
  productName: string | null;
  stockId: number;
  stockName: string | null;
  quantity: number;
  productImgPath: string | null;
  classificationId: number;
  classificationName: string | null;
}

/** Mirrors Application.Report.StockBalance — backs both Stock Balance and Product Balance (one query, see WarehouseReportController.Balance). */
export interface BalanceRow {
  stockId: number;
  stockName: string | null;
  balance: number;
  classificationId: number;
  classificationName: string | null;
  productId: number;
  productName: string | null;
}

/** Mirrors Application.Report.DealerBalance — backs both Clients Balance (dealerTypeId=1) and Suppliers Balance (dealerTypeId=2). */
export interface DealerBalanceRow {
  dealerId: number;
  dealerName: string | null;
  dealerImgPath: string | null;
  openningBalance: number;
  balance: number;
  totalInvoice: number;
  totalReturnInvoice: number;
  totalCreditInvoice: number;
  totalPaidInvoice: number;
}

/** Mirrors Application.Report.DealerStatment — backs both Clients Statement and Suppliers Statement. */
export interface DealerStatementRow {
  id: number;
  referenceId: number | null;
  code: string | null;
  typeId: number | null;
  openningBalance: number;
  type: number;
  typeName: string | null;
  date: string;
  dealerId: number;
  dealerName: string | null;
  dealerImgPath: string | null;
  amount: number;
  inOut: number;
  balance: number;
}

/** Mirrors Application.Report.SafeStatment — backs Safe Movement. */
export interface SafeMovementRow {
  id: number;
  referenceId: number | null;
  code: string | null;
  amount: number;
  typeId: number | null;
  typeName: string | null;
  date: string;
  safeId: number;
  safeName: string | null;
  dealerId: number;
  dealerName: string | null;
  currencyId: number;
  currencyName: string | null;
}

/** Mirrors Application.Report.SafeBalance — backs Safe Balance. */
export interface SafeBalanceRow {
  id: number;
  safeId: number;
  safeName: string | null;
  balance: number;
}

/** Mirrors Application.Report.SalesBalance — a daily Sales-invoice summary, not a per-dealer balance. */
export interface SalesBalanceRow {
  date: string;
  inAmount: number;
  outAmount: number;
  net: number;
}

export interface ReportPage<T> {
  rows: T[];
  page: number;
  pageCount: number;
}
