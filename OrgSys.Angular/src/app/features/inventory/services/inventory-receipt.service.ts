import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ApiResult, ApiResultCollection, ApiResultOf } from '../../../core/models/api-result.model';
import { CreateInventoryReceiptRequest, DocumentStatus, InventoryReceipt } from '../models/warehouse.model';

@Injectable({ providedIn: 'root' })
export class InventoryReceiptService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/InventoryReceipts`;

  create(payload: CreateInventoryReceiptRequest): Observable<ApiResultOf<number>> {
    return this.http.post<ApiResultOf<number>>(this.baseUrl, payload);
  }

  getById(id: number): Observable<ApiResultOf<InventoryReceipt>> {
    return this.http.get<ApiResultOf<InventoryReceipt>>(`${this.baseUrl}/${id}`);
  }

  getList(stockId: number | null, status: DocumentStatus | null): Observable<ApiResultCollection<InventoryReceipt>> {
    let params = new HttpParams();
    if (stockId != null) params = params.set('stockId', stockId);
    if (status != null) params = params.set('status', status);
    return this.http.get<ApiResultCollection<InventoryReceipt>>(this.baseUrl, { params });
  }

  confirm(id: number): Observable<ApiResult> {
    return this.http.put<ApiResult>(`${this.baseUrl}/${id}/Confirm`, null);
  }

  post(id: number): Observable<ApiResult> {
    return this.http.put<ApiResult>(`${this.baseUrl}/${id}/Post`, null);
  }

  cancel(id: number): Observable<ApiResult> {
    return this.http.put<ApiResult>(`${this.baseUrl}/${id}/Cancel`, null);
  }
}
