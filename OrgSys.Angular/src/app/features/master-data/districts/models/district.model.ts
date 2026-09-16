import { BaseEntity } from '../../../../shared/models/base-entity.model';

/** Mirrors Application.DTOs.DistrictDto. */
export interface District extends BaseEntity {
  name: string | null;
  countryId: number | null;
  cityId: number | null;
  countryName: string | null;
  cityName: string | null;
}

/**
 * Mirrors CreateDistrictCommand/UpdateDistrictCommand — both are plain `DistrictDto` subclasses
 * (see Application/Commands/Org/Setting/District/Commands), so one shape covers create and update.
 */
export interface SaveDistrictRequest {
  id?: number;
  name: string;
  countryId: number;
  cityId: number;
}
