import { BaseEntity } from '../../../../shared/models/base-entity.model';

export interface PriceListEntry extends BaseEntity {
  priceListId: number;
  productId: number;
  unitId: number | null;
  price: number;
  minQuantity: number | null;
  validFrom: string | null;
  validTo: string | null;
  isActive: boolean;
  productName: string | null;
  unitName: string | null;
}

export interface PriceList extends BaseEntity {
  name: string | null;
  currencyId: number | null;
  validFrom: string | null;
  validTo: string | null;
  isDefault: boolean;
  isActive: boolean;
  entries: PriceListEntry[] | null;
}

export interface SavePriceListRequest {
  id?: number;
  name: string;
  currencyId?: number | null;
  validFrom?: string | null;
  validTo?: string | null;
  isDefault: boolean;
  isActive: boolean;
  entries?: Array<{
    id?: number;
    productId: number;
    unitId: number | null;
    price: number;
    minQuantity: number | null;
    validFrom: string | null;
    validTo: string | null;
    isActive: boolean;
  }>;
}
