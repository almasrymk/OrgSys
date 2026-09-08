import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { ApiResultCollection } from '../../../../core/models/api-result.model';
import { BaseApiService } from '../../../../core/services/base-api.service';
import { City, CreateCityRequest, UpdateCityRequest } from '../models/city.model';

@Injectable({ providedIn: 'root' })
export class CityService extends BaseApiService<City, CreateCityRequest, UpdateCityRequest> {
  protected readonly entityRoute = 'City';

  constructor(http: HttpClient) {
    super(http);
  }

  /** Replaces the cascading Country -> City picker (Areas/Setting/Views/BankBranch/Save.cshtml pattern). */
  getByCountry(countryId: number, keySearch = ''): Observable<ApiResultCollection<City>> {
    const params = new HttpParams()
      .set('KeySearch', keySearch)
      .set('CountryId', countryId)
      .set('ParentId', 0)
      .set('TypeId', 0)
      .set('Page', 1)
      .set('PageSize', 500);

    return this.http.get<ApiResultCollection<City>>(`${environment.apiUrl}/City/GetListByCountryId`, { params });
  }
}
