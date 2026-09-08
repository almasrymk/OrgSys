import { BaseEntity } from '../../../shared/models/base-entity.model';

/** Mirrors Application.DTOs.InventoryProductDto (extends InventoryProduct extends BaseModel). */
export interface InventoryProduct {
  id: number;
  inventoryId: number;
  rowNumber: number;
  productId: number;
  productName: string | null;
  unitId: number;
  unitName: string | null;
  calcBalance: number;
  actualBalance: number;
  diffQuantity: number;
  notes: string | null;
}

/**
 * Mirrors Application.DTOs.InventoryDto (extends Inventory extends MovementModel). Unlike
 * Financial/Invoice/Transaction, Inventory has no TypeId-driven screen family in MVC — a single
 * "/Transactions/Inventory" list+form pair, TypeId always 0 (see InitialData.cs Preference rows
 * scoped to Reference="Inventory", TypeId=0).
 */
export interface Inventory extends BaseEntity {
  date: string;
  stockId: number | null;
  stockName: string | null;
  notes: string | null;
  closed: boolean;
  hasAdjustment: boolean;
  adjustmentInTransactionId: number | null;
  adjustmentInTransactionCode: string | null;
  adjustmentOutTransactionId: number | null;
  adjustmentOutTransactionCode: string | null;
  createUserId: number;
  createDate: string;
  inventoryProductList: InventoryProduct[];
}

export interface SaveInventoryProductRequest {
  id?: number;
  inventoryId?: number;
  rowNumber: number;
  productId: number;
  unitId: number;
  calcBalance: number;
  actualBalance: number;
  diffQuantity: number;
  notes: string | null;
}

/**
 * Mirrors CreateInventoryCommand/UpdateInventoryCommand (both InventoryDto subclasses). Create AND
 * Update map `inventoryProductList` directly (MappingProfile.InventoryMappingProfile), no split
 * create-then-update dance — and unlike Journal/Invoice/Transaction, UpdateCommandHandler.SaveDetials
 * DOES set `InventoryId` on every line itself, so the client does not need to set it defensively
 * (harmless to send anyway — see transaction.model.ts's standing rule for the general case).
 */
export interface SaveInventoryRequest {
  id?: number;
  code: string;
  codeNumber: number;
  date: string;
  stockId: number | null;
  notes: string | null;
  inventoryProductList: SaveInventoryProductRequest[];
  createUserId?: number;
  createDate?: string;
  modifyUserId?: number;
  modifyDate?: string;
}
