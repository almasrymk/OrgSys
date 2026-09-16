import { BaseEntity } from '../../../shared/models/base-entity.model';

/** Mirrors Domain.Entities.TransactionType (Infrastructure/Seed/InitialData.cs InitialTransactionType). */
export enum TransactionTypeId {
  Addition = 1,
  Issue = 2,
  Transfer = 3,
  Received = 4,
  AdjustmentIn = 5,
  AdjustmentOut = 6,
  OpeningBalance = 7,
  Damaged = 8,
}

/** Business URL segments under /inventory/movements. Received (4) has no standalone screen. */
export const INVENTORY_MOVEMENT_PATHS: Record<number, string> = {
  1: 'addition',
  2: 'issue',
  3: 'transfer',
  5: 'adjustment-in',
  6: 'adjustment-out',
  7: 'opening-balance',
  8: 'damaged',
};

export function inventoryMovementPath(typeId: number): string {
  return `/inventory/movements/${INVENTORY_MOVEMENT_PATHS[typeId] ?? String(typeId)}`;
}

export interface TransactionType extends BaseEntity {
  name: string | null;
  inOut: number;
  icon: string | null;
}

/** Mirrors Application.DTOs.TransactionProductDto (extends TransactionProduct extends BaseModel). */
export interface TransactionProduct {
  id: number;
  transactionId: number;
  rowNumber: number;
  productId: number;
  productName: string | null;
  unitId: number;
  unitName: string | null;
  stockId: number | null;
  quantity: number;
  cost: number;
  total: number;
  notes: string | null;
}

/** Mirrors Application.DTOs.TransactionDto (extends Transaction extends MovementModel). */
export interface Transaction extends BaseEntity {
  date: string;
  dealerId: number | null;
  dealerName: string | null;
  stockId: number | null;
  stockName: string | null;
  toStockId: number | null;
  toStockName: string | null;
  total: number;
  notes: string | null;
  createUserId: number;
  createDate: string;
  transactionProductList: TransactionProduct[];
}

export interface SaveTransactionProductRequest {
  id?: number;
  transactionId?: number;
  rowNumber: number;
  productId: number;
  unitId: number;
  stockId: number | null;
  quantity: number;
  cost: number;
  total: number;
  notes: string | null;
}

/**
 * Mirrors CreateTransactionCommand/UpdateTransactionCommand (both TransactionDto subclasses).
 * `toStockId` only matters for TypeId=Transfer (3) — the destination Stock; the "Received" (4) leg
 * is auto-generated server-side (TransferReceivedIntegration.SyncAsync), never created directly here.
 */
export interface SaveTransactionRequest {
  id?: number;
  typeId: TransactionTypeId;
  code: string;
  codeNumber: number;
  date: string;
  dealerId: number | null;
  stockId: number | null;
  toStockId: number | null;
  total: number;
  notes: string | null;
  transactionProductList: SaveTransactionProductRequest[];
  createUserId?: number;
  createDate?: string;
  modifyUserId?: number;
  modifyDate?: string;
}
