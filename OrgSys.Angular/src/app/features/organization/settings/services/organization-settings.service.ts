import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { ApiResult, ApiResultOf } from '../../../../core/models/api-result.model';
import { OrganizationSettings, UpdateOrganizationSettingsRequest } from '../models/organization-settings.model';

@Injectable({ providedIn: 'root' })
export class OrganizationSettingsService {
  private readonly baseUrl = `${environment.apiUrl}/OrganizationSettings`;

  constructor(private readonly http: HttpClient) {}

  getByCompanyId(companyId: number): Observable<ApiResultOf<OrganizationSettings>> {
    const params = new HttpParams().set('companyId', companyId);
    return this.http.get<ApiResultOf<OrganizationSettings>>(`${this.baseUrl}/GetByCompanyId`, { params });
  }

  update(payload: UpdateOrganizationSettingsRequest): Observable<ApiResult> {
    return this.http.put<ApiResult>(`${this.baseUrl}/Update`, payload);
  }
}
