import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { ApiResultCollection } from '../../../../core/models/api-result.model';
import { BaseApiService } from '../../../../core/services/base-api.service';
import { District, SaveDistrictRequest } from '../models/district.model';

@Injectable({ providedIn: 'root' })
export class DistrictService extends BaseApiService<District, SaveDistrictRequest, SaveDistrictRequest> {
  protected readonly entityRoute = 'District';

  constructor(http: HttpClient) {
    super(http);
  }

  /** Replaces the cascading City -> District picker, same shape as City.getByCountry. */
  getByCity(cityId: number, keySearch = ''): Observable<ApiResultCollection<District>> {
    const params = new HttpParams()
      .set('KeySearch', keySearch)
      .set('CityId', cityId)
      .set('ParentId', 0)
      .set('TypeId', 0)
      .set('Page', 1)
      .set('PageSize', 500);

    return this.http.get<ApiResultCollection<District>>(`${environment.apiUrl}/District/GetListByCityId`, { params });
  }
}
