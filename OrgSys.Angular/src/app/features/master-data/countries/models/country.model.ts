import { BaseEntity } from '../../../../shared/models/base-entity.model';

/** Mirrors Application.DTOs.CountryDto (Domain.Entities.Country + BaseModel). */
export interface Country extends BaseEntity {
  name: string | null;
}

/** Mirrors Application.Commands.Org.Setting.Country.Commands.CreateCountryCommand. */
export interface CreateCountryRequest {
  name: string;
}

/** Mirrors Application.Commands.Org.Setting.Country.Commands.UpdateCountryCommand. */
export interface UpdateCountryRequest {
  id: number;
  name: string;
}
