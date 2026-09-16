import { BaseEntity } from '../../../shared/models/base-entity.model';

/** Lookup-only models — none of these have a dedicated Angular admin screen yet (see AccountType/JournalType). */

export interface PaymentType extends BaseEntity {
  name: string | null;
}

export interface Stock extends BaseEntity {
  name: string | null;
  branchId: number;
  branchName: string | null;
}

export interface Unit extends BaseEntity {
  name: string | null;
}

/**
 * Mirrors Application.DTOs.ProductDto, trimmed to what a line-item picker needs. Product has real
 * multi-unit complexity (ProductUnits) not modeled here — every line uses the plain Unit list
 * (see invoice.model.ts), not a per-product filtered one; a fuller Product feature (with its own
 * ProductUnit sub-editor) is a separate, not-yet-built piece of work.
 */
/** Mirrors Application.DTOs.ProductUnitDto — only populated by `ProductService.getAllByBalance()`. */
export interface ProductUnit {
  unitId: number;
  unitName: string | null;
  rate: number;
  defaultUnit: boolean;
}

export interface Product extends BaseEntity {
  name: string | null;
  price: number;
  /** Both only populated by `ProductService.getAllByBalance()` (current on-hand quantity in a given Stock as of a date, and its unit list). */
  balance?: number;
  productUnits?: ProductUnit[];
}

/** Mirrors Application.DTOs.InvoiceTypeDto (Domain.Entities.InvoiceType). */
export interface InvoiceType extends BaseEntity {
  name: string | null;
  inOut: number;
  icon: string | null;
  group: string | null;
}
