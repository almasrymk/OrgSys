import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from '../../../../core/services/base-api.service';
import { Country, CreateCountryRequest, UpdateCountryRequest } from '../models/country.model';

@Injectable({ providedIn: 'root' })
export class CountryService extends BaseApiService<Country, CreateCountryRequest, UpdateCountryRequest> {
  protected readonly entityRoute = 'Country';

  constructor(http: HttpClient) {
    super(http);
  }
}
