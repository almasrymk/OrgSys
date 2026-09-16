import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResult } from '../../../core/models/api-result.model';
import { BaseApiService } from '../../../core/services/base-api.service';
import { SaveTransactionRequest, Transaction } from '../models/transaction.model';

@Injectable({ providedIn: 'root' })
export class TransactionService extends BaseApiService<Transaction, SaveTransactionRequest, SaveTransactionRequest> {
  protected readonly entityRoute = 'Transaction';

  constructor(http: HttpClient) {
    super(http);
  }

  /** `/Transaction/GetMax` returns a bare number, not the Result<T> envelope. */
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

  /** Manually (re-)creates the "Received" leg of a Transfer — usually auto-created on save, this covers the case where auto-sync was skipped. */
  createReceived(transferId: number): Observable<ApiResult> {
    return this.http.post<ApiResult>(`${this.baseUrl}/CreateReceived`, null, { params: new HttpParams().set('TransferId', transferId) });
  }
}
