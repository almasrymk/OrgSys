import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResult } from '../../../../core/models/api-result.model';
import { BaseApiService } from '../../../../core/services/base-api.service';
import { OpeningBalance, SaveOpeningBalanceRequest } from '../models/opening-balance.model';

@Injectable({ providedIn: 'root' })
export class OpeningBalanceService extends BaseApiService<OpeningBalance, SaveOpeningBalanceRequest, SaveOpeningBalanceRequest> {
  protected readonly entityRoute = 'Financial';

  constructor(http: HttpClient) {
    super(http);
  }

  /** PUT /Financial/Post?id=&userId= — PostFinancialOpeningBalanceCommand, distinct from Transactions/Post. */
  post(id: number, userId: number): Observable<ApiResult> {
    const params = new HttpParams().set('id', id).set('userId', userId);
    return this.http.put<ApiResult>(`${this.baseUrl}/Post`, null, { params });
  }

  reverse(id: number): Observable<ApiResult> {
    return this.http.put<ApiResult>(`${this.baseUrl}/Reverse`, null, { params: new HttpParams().set('id', id) });
  }
}
