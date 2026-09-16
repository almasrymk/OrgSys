import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ApiResult, ApiResultCollection } from '../../../core/models/api-result.model';
import { CreateWarehouseLocationRequest, WarehouseLocation } from '../models/warehouse.model';

/** `/WarehouseLocations` — a plain Create + GetByStock list, not the full
 * GetById/Search/Update/Delete shape (no edit/delete endpoint exists yet on the API side). */
@Injectable({ providedIn: 'root' })
export class WarehouseLocationService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/WarehouseLocations`;

  create(payload: CreateWarehouseLocationRequest): Observable<ApiResult> {
    return this.http.post<ApiResult>(this.baseUrl, payload);
  }

  getByStock(stockId: number): Observable<ApiResultCollection<WarehouseLocation>> {
    const params = new HttpParams().set('stockId', stockId);
    return this.http.get<ApiResultCollection<WarehouseLocation>>(`${this.baseUrl}/ByStock`, { params });
  }
}
