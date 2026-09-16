import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { ApiResult, ApiResultOf } from '../../../../core/models/api-result.model';
import { AssignSupplierRoleRequest, SupplierProfile } from '../models/supplier-profile.model';

@Injectable({ providedIn: 'root' })
export class SupplierProfileService {
  private readonly baseUrl = `${environment.apiUrl}/SupplierProfile`;

  constructor(private readonly http: HttpClient) {}

  getByDealerId(dealerId: number): Observable<ApiResultOf<SupplierProfile>> {
    const params = new HttpParams().set('dealerId', dealerId);
    return this.http.get<ApiResultOf<SupplierProfile>>(`${this.baseUrl}/GetByDealerId`, { params });
  }

  assign(payload: AssignSupplierRoleRequest): Observable<ApiResult> {
    return this.http.post<ApiResult>(`${this.baseUrl}/Assign`, payload);
  }
}
