import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ApiResult, ApiResultCollection, ApiResultOf } from '../../../core/models/api-result.model';
import {
  CreateCustodyRequest,
  Custody,
  CustodyStatus,
  IssueCustodyRequest,
  ReturnCustodyRequest,
} from '../models/custody.model';

@Injectable({ providedIn: 'root' })
export class CustodyService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/Custody`;

  create(payload: CreateCustodyRequest): Observable<ApiResultOf<number>> {
    return this.http.post<ApiResultOf<number>>(this.baseUrl, payload);
  }

  getById(id: number): Observable<ApiResultOf<Custody>> {
    return this.http.get<ApiResultOf<Custody>>(`${this.baseUrl}/${id}`);
  }

  getList(holderId: number | null, status: CustodyStatus | null): Observable<ApiResultCollection<Custody>> {
    let params = new HttpParams();
    if (holderId != null) params = params.set('holderId', holderId);
    if (status != null) params = params.set('status', status);
    return this.http.get<ApiResultCollection<Custody>>(this.baseUrl, { params });
  }

  approve(id: number): Observable<ApiResult> {
    return this.http.put<ApiResult>(`${this.baseUrl}/${id}/Approve`, null);
  }

  issue(id: number, payload: IssueCustodyRequest): Observable<ApiResult> {
    return this.http.put<ApiResult>(`${this.baseUrl}/${id}/Issue`, payload);
  }

  settle(id: number, amount: number): Observable<ApiResult> {
    return this.http.put<ApiResult>(`${this.baseUrl}/${id}/Settle`, { id, amount });
  }

  returnAmount(id: number, payload: ReturnCustodyRequest): Observable<ApiResult> {
    return this.http.put<ApiResult>(`${this.baseUrl}/${id}/Return`, payload);
  }

  close(id: number): Observable<ApiResult> {
    return this.http.put<ApiResult>(`${this.baseUrl}/${id}/Close`, null);
  }

  cancel(id: number): Observable<ApiResult> {
    return this.http.put<ApiResult>(`${this.baseUrl}/${id}/Cancel`, null);
  }

  transfer(id: number, toHolderId: number, reason: string, approvedByUserId: number, transferDate: string): Observable<ApiResult> {
    return this.http.put<ApiResult>(`${this.baseUrl}/${id}/Transfer`, {
      id,
      toHolderId,
      reason,
      approvedByUserId,
      transferDate,
    });
  }
}
