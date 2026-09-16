import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ApiResult, ApiResultCollection, ApiResultOf } from '../../../core/models/api-result.model';
import { ReservationStatus, ReserveStockRequest, StockReservation } from '../models/warehouse.model';

@Injectable({ providedIn: 'root' })
export class StockReservationService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/StockReservations`;

  reserve(payload: ReserveStockRequest): Observable<ApiResultOf<number>> {
    return this.http.post<ApiResultOf<number>>(this.baseUrl, payload);
  }

  release(id: number): Observable<ApiResult> {
    return this.http.put<ApiResult>(`${this.baseUrl}/${id}/Release`, null);
  }

  fulfill(id: number): Observable<ApiResult> {
    return this.http.put<ApiResult>(`${this.baseUrl}/${id}/Fulfill`, null);
  }

  getList(productId: number | null, stockId: number | null, status: ReservationStatus | null): Observable<ApiResultCollection<StockReservation>> {
    let params = new HttpParams();
    if (productId != null) params = params.set('productId', productId);
    if (stockId != null) params = params.set('stockId', stockId);
    if (status != null) params = params.set('status', status);
    return this.http.get<ApiResultCollection<StockReservation>>(this.baseUrl, { params });
  }
}
