import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResult } from '../../../core/models/api-result.model';
import { BaseApiService } from '../../../core/services/base-api.service';
import { Inventory, SaveInventoryRequest } from '../models/inventory.model';

@Injectable({ providedIn: 'root' })
export class InventoryService extends BaseApiService<Inventory, SaveInventoryRequest, SaveInventoryRequest> {
  protected readonly entityRoute = 'Inventory';

  constructor(http: HttpClient) {
    super(http);
  }

  /** `/Inventory/GetMax` returns a bare number, not the Result<T> envelope. TypeId is always 0 here. */
  getMaxCodeNumber(): Observable<number> {
    const params = new HttpParams().set('ParentId', 0).set('TypeId', 0);
    return this.http.get<number>(`${this.baseUrl}/GetMax`, { params });
  }

  cancel(id: number): Observable<ApiResult> {
    return this.http.put<ApiResult>(`${this.baseUrl}/Cancel`, null, { params: new HttpParams().set('Id', id) });
  }

  redo(id: number): Observable<ApiResult> {
    return this.http.put<ApiResult>(`${this.baseUrl}/Redo`, null, { params: new HttpParams().set('Id', id) });
  }

  /**
   * Materializes AdjustmentIn/AdjustmentOut Transactions (TypeId 5/6) from each line's DiffQuantity —
   * the manual fallback for when the `Inventory`/`AutoCreateAdjustment` Preference isn't set to "1"
   * (same shape as Transaction's `createReceived`, see transaction.service.ts).
   */
  createAdjustment(inventoryId: number): Observable<ApiResult> {
    return this.http.post<ApiResult>(`${this.baseUrl}/CreateAdjustment`, null, { params: new HttpParams().set('InventoryId', inventoryId) });
  }
}
