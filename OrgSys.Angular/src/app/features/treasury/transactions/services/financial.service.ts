import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { ApiResult } from '../../../../core/models/api-result.model';
import { BaseApiService } from '../../../../core/services/base-api.service';
import { Financial, PostFinancialTransactionRequest } from '../models/financial.model';

/**
 * `never` for TCreate/TUpdate: there is no generic Create/Update for non-OpeningBalance rows (see
 * PostFinancialTransactionRequest's docstring) — use postTransaction() instead. OpeningBalance's
 * Draft-then-Post flow is out of scope for this pass.
 */
@Injectable({ providedIn: 'root' })
export class FinancialService extends BaseApiService<Financial, never, never> {
  protected readonly entityRoute = 'Financial';

  constructor(http: HttpClient) {
    super(http);
  }

  postTransaction(transaction: PostFinancialTransactionRequest): Observable<ApiResult> {
    return this.http.post<ApiResult>(`${environment.apiUrl}/Financial/Transactions/Post`, transaction);
  }

  cancel(id: number): Observable<ApiResult> {
    return this.http.put<ApiResult>(`${this.baseUrl}/Cancel`, null, { params: new HttpParams().set('Id', id) });
  }

  redo(id: number): Observable<ApiResult> {
    return this.http.put<ApiResult>(`${this.baseUrl}/Redo`, null, { params: new HttpParams().set('Id', id) });
  }

  reverse(id: number): Observable<ApiResult> {
    return this.http.put<ApiResult>(`${this.baseUrl}/Reverse`, null, { params: new HttpParams().set('id', id) });
  }
}
