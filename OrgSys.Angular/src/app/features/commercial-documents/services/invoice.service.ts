import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResult } from '../../../core/models/api-result.model';
import { BaseApiService } from '../../../core/services/base-api.service';
import { Invoice, SaveInvoiceRequest } from '../models/invoice.model';

@Injectable({ providedIn: 'root' })
export class InvoiceService extends BaseApiService<Invoice, SaveInvoiceRequest, SaveInvoiceRequest> {
  protected readonly entityRoute = 'Invoice';

  constructor(http: HttpClient) {
    super(http);
  }

  /** `/Invoice/GetMax` returns a bare number, not the Result<T> envelope. */
  getMaxCodeNumber(typeId: number): Observable<number> {
    const params = new HttpParams().set('ParentId', 0).set('TypeId', typeId);
    return this.http.get<number>(`${this.baseUrl}/GetMax`, { params });
  }

  cancel(id: number): Observable<ApiResult> {
    return this.http.put<ApiResult>(`${this.baseUrl}/Cancel`, null, { params: new HttpParams().set('Id', id) });
  }

  redo(id: number): Observable<ApiResult> {
    return this.http.put<ApiResult>(`${this.baseUrl}/Redo`, null, { params: new HttpParams().set('Id', id) });
  }
}
