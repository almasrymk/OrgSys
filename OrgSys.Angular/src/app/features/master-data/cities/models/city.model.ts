import { BaseEntity } from '../../../../shared/models/base-entity.model';

/** Mirrors Application.DTOs.CityDto. */
export interface City extends BaseEntity {
  name: string | null;
  countryId: number | null;
  countryName: string | null;
}

/** Mirrors CreateCityCommand(Name, CountryId). */
export interface CreateCityRequest {
  name: string;
  countryId: number;
}

/** Mirrors UpdateCityCommand(Id, CountryId, Name). */
export interface UpdateCityRequest {
  id: number;
  name: string;
  countryId: number;
}
