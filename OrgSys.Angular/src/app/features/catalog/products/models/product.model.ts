import { BaseEntity } from '../../../../shared/models/base-entity.model';

/** Mirrors Application.DTOs.ProductUnitDto. */
export interface ProductUnit {
  id: number;
  productId: number;
  unitId: number;
  unitName: string | null;
  rate: number;
  defaultUnit: boolean;
}

/**
 * Mirrors Application.DTOs.ProductDto, trimmed to core fields — ProductRecipes (bill-of-materials)
 * and ProductPropertyElements (variant properties) are real sub-features of their own, not built
 * this pass (same "trim to what's needed first" scope call as Invoice's percentage-mode discount).
 */
export interface Product extends BaseEntity {
  name: string | null;
  nickname: string | null;
  barcode: string | null;
  description: string | null;
  price: number;
  cost: number;
  classificationId: number;
  classificationName: string | null;
  dealerId: number | null;
  dealerName: string | null;
  productUnits: ProductUnit[];
  /** Populated only by GET /Product/GetAllByBalance (Inventory query on the Product controller). */
  balance?: number;
}

export interface SaveProductUnitRequest {
  id?: number;
  productId?: number;
  unitId: number;
  rate: number;
  defaultUnit: boolean;
}

/** Mirrors CreateProductCommand/UpdateProductCommand (both ProductDto subclasses). */
export interface SaveProductRequest {
  id?: number;
  code: string;
  codeNumber: number;
  name: string;
  nickname: string | null;
  barcode: string | null;
  description: string | null;
  price: number;
  cost: number;
  classificationId: number;
  dealerId: number | null;
  productUnits: SaveProductUnitRequest[];
}
