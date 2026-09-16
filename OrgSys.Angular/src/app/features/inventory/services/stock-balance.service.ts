import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ApiResultCollection, ApiResultOf } from '../../../core/models/api-result.model';
import { InventoryAvailability, StockBalance, StockCardLine } from '../models/warehouse.model';

/** `/InventoryBalances` — the Stock Balance / Stock Card / Availability read models. */
@Injectable({ providedIn: 'root' })
export class StockBalanceService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/InventoryBalances`;

  getBalance(productId: number, stockId: number, locationId: number | null, batchId: number | null): Observable<ApiResultOf<StockBalance>> {
    let params = new HttpParams().set('productId', productId).set('stockId', stockId);
    if (locationId != null) params = params.set('locationId', locationId);
    if (batchId != null) params = params.set('batchId', batchId);
    return this.http.get<ApiResultOf<StockBalance>>(this.baseUrl, { params });
  }

  getList(stockId: number | null, productId: number | null): Observable<ApiResultCollection<StockBalance>> {
    let params = new HttpParams();
    if (stockId != null) params = params.set('stockId', stockId);
    if (productId != null) params = params.set('productId', productId);
    return this.http.get<ApiResultCollection<StockBalance>>(`${this.baseUrl}/List`, { params });
  }

  getStockCard(productId: number, stockId: number | null, dateFrom: string | null, dateTo: string | null): Observable<ApiResultCollection<StockCardLine>> {
    let params = new HttpParams().set('productId', productId);
    if (stockId != null) params = params.set('stockId', stockId);
    if (dateFrom) params = params.set('dateFrom', dateFrom);
    if (dateTo) params = params.set('dateTo', dateTo);
    return this.http.get<ApiResultCollection<StockCardLine>>(`${this.baseUrl}/StockCard`, { params });
  }

  getAvailability(productId: number, stockId: number, locationId: number | null, batchId: number | null, requestedQuantity: number): Observable<ApiResultOf<InventoryAvailability>> {
    let params = new HttpParams().set('productId', productId).set('stockId', stockId).set('requestedQuantity', requestedQuantity);
    if (locationId != null) params = params.set('locationId', locationId);
    if (batchId != null) params = params.set('batchId', batchId);
    return this.http.get<ApiResultOf<InventoryAvailability>>(`${this.baseUrl}/Availability`, { params });
  }
}
