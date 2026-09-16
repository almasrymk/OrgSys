import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { ApiResult, ApiResultCollection, ApiResultOf } from '../../../../core/models/api-result.model';
import { FinancialTransfer, PostFinancialTransferRequest } from '../models/financial-transfer.model';

/** Plain ControllerBase on the API side, not the generic Base<> pattern — no Search/Create/Update/Delete/GetMax. */
@Injectable({ providedIn: 'root' })
export class FinancialTransferService {
  private readonly baseUrl = `${environment.apiUrl}/FinancialTransfer`;

  constructor(private readonly http: HttpClient) {}

  getList(): Observable<ApiResultCollection<FinancialTransfer>> {
    return this.http.get<ApiResultCollection<FinancialTransfer>>(`${this.baseUrl}/GetList`);
  }

  getById(id: number): Observable<ApiResultOf<FinancialTransfer>> {
    return this.http.get<ApiResultOf<FinancialTransfer>>(`${this.baseUrl}/GetById`, { params: new HttpParams().set('id', id) });
  }

  post(transfer: PostFinancialTransferRequest): Observable<ApiResult> {
    return this.http.post<ApiResult>(`${this.baseUrl}/Post`, transfer);
  }

  reverse(id: number): Observable<ApiResult> {
    return this.http.put<ApiResult>(`${this.baseUrl}/Reverse`, null, { params: new HttpParams().set('id', id) });
  }
}
