import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { ApiResult, ApiResultOf } from '../../../../core/models/api-result.model';
import { AssignCustomerRoleRequest, CustomerProfile } from '../models/customer-profile.model';

@Injectable({ providedIn: 'root' })
export class CustomerProfileService {
  private readonly baseUrl = `${environment.apiUrl}/CustomerProfile`;

  constructor(private readonly http: HttpClient) {}

  getByDealerId(dealerId: number): Observable<ApiResultOf<CustomerProfile>> {
    const params = new HttpParams().set('dealerId', dealerId);
    return this.http.get<ApiResultOf<CustomerProfile>>(`${this.baseUrl}/GetByDealerId`, { params });
  }

  assign(payload: AssignCustomerRoleRequest): Observable<ApiResult> {
    return this.http.post<ApiResult>(`${this.baseUrl}/Assign`, payload);
  }
}
