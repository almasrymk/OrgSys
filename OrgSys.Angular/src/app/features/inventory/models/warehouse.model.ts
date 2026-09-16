import { BaseEntity } from '../../../shared/models/base-entity.model';

/**
 * Models for Inventory warehouse locations, receipts, reservations and balances.
 * Stock-count documents live in inventory.model.ts; stock movements live in transaction.model.ts.
 */

// ----- Warehouse Location -----

export interface WarehouseLocation extends BaseEntity {
  stockId: number;
  stockName: string | null;
  name: string | null;
  parentLocationId: number | null;
  locationType: string | null;
  isReceivable: boolean;
  isPickable: boolean;
  isActive: boolean;
}

export interface CreateWarehouseLocationRequest {
  stockId: number;
  code: string;
  name: string | null;
  parentLocationId: number | null;
  locationType: string | null;
  isReceivable: boolean;
  isPickable: boolean;
}

// ----- Stock Adjustment Reason -----

export interface StockAdjustmentReason extends BaseEntity {
  name: string | null;
  isActive: boolean;
}

// ----- Balance / Stock Card -----

export interface StockBalance {
  productId: number;
  stockId: number;
  locationId: number | null;
  batchId: number | null;
  quantityOnHand: number;
  quantityReserved: number;
  quantityAvailable: number;
  averageCost: number;
  inventoryValue: number;
}

export interface StockCardLine {
  date: string;
  reference: string | null;
  movementTypeId: number;
  documentNumber: string | null;
  receiptQuantity: number;
  issueQuantity: number;
  runningBalance: number;
  unitCost: number;
  transactionValue: number;
  runningValue: number;
}

export interface InventoryAvailability {
  onHand: number;
  reserved: number;
  available: number;
  canFulfill: boolean;
}

// ----- Reservations -----

/** Mirrors Inventory.Domain.Enums.ReservationStatus. */
export enum ReservationStatus {
  Active = 0,
  Released = 10,
  Fulfilled = 20,
  Expired = 30,
  Cancelled = 40,
}

/** Mirrors Inventory.Domain.SourceDocumentType. */
export enum SourceDocumentType {
  Invoice = 1,
  StockCount = 2,
  InventoryReceipt = 3,
  InventoryIssue = 4,
  StockTransfer = 5,
  StockAdjustment = 6,
  Reversal = 7,
  SalesOrder = 8,
}

export interface StockReservation {
  id: number;
  productId: number;
  stockId: number;
  locationId: number | null;
  batchId: number | null;
  serialId: number | null;
  quantity: number;
  sourceType: SourceDocumentType;
  sourceId: number;
  sourceLineId: number | null;
  reservationStatus: ReservationStatus;
  expiresAt: string | null;
  createDate: string;
}

export interface ReserveStockRequest {
  productId: number;
  stockId: number;
  locationId: number | null;
  batchId: number | null;
  serialId: number | null;
  quantity: number;
  sourceType: SourceDocumentType;
  sourceId: number;
  sourceLineId: number | null;
  createUserId: number;
  expiresAt: string | null;
}

// ----- Batch / Serial -----

/** Mirrors Inventory.Domain.Enums.BatchStatus. */
export enum BatchStatus {
  Active = 0,
  Expired = 10,
  Quarantine = 20,
  Depleted = 30,
}

/** Mirrors Inventory.Domain.Enums.SerialStatus. */
export enum SerialStatus {
  Available = 0,
  Reserved = 10,
  Issued = 20,
  Returned = 30,
  Damaged = 40,
  Lost = 50,
  Quarantine = 60,
}

export interface BatchExpiry {
  id: number;
  productId: number;
  batchNumber: string;
  expiryDate: string | null;
  daysUntilExpiry: number | null;
  isExpired: boolean;
}

export interface SerialHistory {
  id: number;
  productId: number;
  serialNumber: string;
  serialStatus: SerialStatus;
  currentStockId: number | null;
  currentLocationId: number | null;
  receivedDate: string;
  issuedDate: string | null;
}

// ----- Documents: Receipt / Issue / Transfer / Adjustment -----

/** Mirrors Inventory.Domain.Enums.DocumentStatus. */
export enum DocumentStatus {
  Draft = 0,
  Confirmed = 10,
  Posted = 20,
  Cancelled = 30,
}

/** Mirrors Inventory.Domain.Enums.StockTransferStatus. */
export enum StockTransferStatus {
  Draft = 0,
  Confirmed = 10,
  Shipped = 20,
  Received = 30,
  Completed = 40,
  Cancelled = 50,
}

/** Mirrors Inventory.Domain.Enums.MovementDirection. */
export enum MovementDirection {
  In = 0,
  Out = 1,
}

export interface InventoryReceiptLineInput {
  productId: number;
  unitId: number;
  quantity: number;
  unitCost: number;
  batchId: number | null;
  notes: string | null;
}

export interface CreateInventoryReceiptRequest {
  stockId: number;
  locationId: number | null;
  dealerId: number | null;
  date: string;
  createUserId: number;
  branchId: number | null;
  notes: string | null;
  lines: InventoryReceiptLineInput[];
}

export interface InventoryReceiptLine {
  id: number;
  rowNumber: number;
  productId: number;
  unitId: number;
  quantity: number;
  unitCost: number;
  batchId: number | null;
  notes: string | null;
}

export interface InventoryReceipt {
  id: number;
  code: string | null;
  stockId: number;
  locationId: number | null;
  dealerId: number | null;
  date: string;
  lifecycleStatus: DocumentStatus;
  notes: string | null;
  lines: InventoryReceiptLine[];
}

export interface InventoryIssueLineInput {
  productId: number;
  unitId: number;
  quantity: number;
  batchId: number | null;
  serialId: number | null;
  notes: string | null;
}

export interface CreateInventoryIssueRequest {
  stockId: number;
  locationId: number | null;
  dealerId: number | null;
  date: string;
  createUserId: number;
  branchId: number | null;
  notes: string | null;
  lines: InventoryIssueLineInput[];
}

export interface StockTransferLineInput {
  productId: number;
  unitId: number;
  quantity: number;
  batchId: number | null;
  notes: string | null;
}

export interface CreateStockTransferRequest {
  fromStockId: number;
  toStockId: number;
  date: string;
  createUserId: number;
  branchId: number | null;
  notes: string | null;
  lines: StockTransferLineInput[];
}

export interface StockAdjustmentLineInput {
  productId: number;
  unitId: number;
  direction: MovementDirection;
  quantity: number;
  batchId: number | null;
  notes: string | null;
}

export interface CreateStockAdjustmentRequest {
  stockId: number;
  reasonId: number;
  date: string;
  createUserId: number;
  branchId: number | null;
  notes: string | null;
  lines: StockAdjustmentLineInput[];
}
