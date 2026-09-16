import { BaseEntity } from '../../../shared/models/base-entity.model';

/** Mirrors Application.DTOs.InvoiceProductDto (extends InvoiceProduct extends BaseModel). */
export interface InvoiceProduct {
  id: number;
  invoiceId: number;
  rowNumber: number;
  productId: number;
  productName: string | null;
  unitId: number;
  unitName: string | null;
  stockId: number | null;
  quantity: number;
  price: number;
  total: number;
  discount: number;
  tax: number;
  service: number;
  net: number;
  notes: string | null;
}

/** Mirrors Application.DTOs.InvoiceDto (extends Invoice extends MovementModel). */
export interface Invoice extends BaseEntity {
  date: string;
  posted: boolean;
  dealerId: number;
  dealerName: string | null;
  paymentTypeId: number;
  paymentTypeName: string | null;
  stockId: number | null;
  stockName: string | null;
  currencyId: number;
  currencyName: string | null;
  rate: number;
  total: number;
  discount: number;
  tax: number;
  service: number;
  net: number;
  netByDefaultCurrency: number;
  notes: string | null;
  createUserId: number;
  createDate: string;
  invoiceProductList: InvoiceProduct[];
}

export interface SaveInvoiceProductRequest {
  id?: number;
  invoiceId?: number;
  rowNumber: number;
  productId: number;
  unitId: number;
  stockId: number | null;
  quantity: number;
  price: number;
  total: number;
  discount: number;
  tax: number;
  service: number;
  net: number;
  notes: string | null;
}

/**
 * Mirrors CreateInvoiceCommand/UpdateInvoiceCommand (both InvoiceDto subclasses). DiscountType/
 * TaxType/ServiceType (0/1 on the entity, meaning presumably flat-amount vs percentage) are fixed
 * at 0 (flat amount) here — a percentage mode is not exposed in this pass, kept simple like Journal/
 * Financial's own scope trims. `createUserId`/`createDate` follow the same MovementModel audit-field
 * rule as Journal/Financial (see docs/ANGULAR_MIGRATION_INVENTORY.md) — required, not server-assigned.
 */
export interface SaveInvoiceRequest {
  id?: number;
  typeId: number;
  code: string;
  codeNumber: number;
  date: string;
  dealerId: number;
  paymentTypeId: number;
  stockId: number | null;
  currencyId: number;
  rate: number;
  discount: number;
  discountType: 0;
  tax: number;
  taxType: 0;
  service: number;
  serviceType: 0;
  net: number;
  netByDefaultCurrency: number;
  notes: string | null;
  invoiceProductList: SaveInvoiceProductRequest[];
  createUserId?: number;
  createDate?: string;
  modifyUserId?: number;
  modifyDate?: string;
}
