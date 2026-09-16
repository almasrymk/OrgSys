import { BaseEntity } from '../../../../shared/models/base-entity.model';

export enum PartyAddressType {
  Billing = 1,
  Shipping = 2,
  Registered = 3,
  Office = 4,
  Other = 5,
}

export interface PartyAddress extends BaseEntity {
  dealerId: number;
  addressType: PartyAddressType;
  line1: string | null;
  line2: string | null;
  countryId: number | null;
  countryName: string | null;
  cityId: number | null;
  cityName: string | null;
  districtId: number | null;
  districtName: string | null;
  postalCode: string | null;
  isPrimary: boolean;
}

export interface SavePartyAddressRequest {
  id?: number;
  dealerId: number;
  addressType: PartyAddressType;
  line1: string;
  line2?: string | null;
  countryId?: number | null;
  cityId?: number | null;
  districtId?: number | null;
  postalCode?: string | null;
  isPrimary: boolean;
}
