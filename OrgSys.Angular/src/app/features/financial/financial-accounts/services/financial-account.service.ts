import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../../../core/services/base-api.service';
import { FinancialAccount, FinancialAccountType, SaveFinancialAccountRequest } from '../models/financial-account.model';

@Injectable({ providedIn: 'root' })
export class FinancialAccountService extends BaseApiService<FinancialAccount, SaveFinancialAccountRequest, SaveFinancialAccountRequest> {
  protected readonly entityRoute = 'FinancialAccount';

  constructor(http: HttpClient) {
    super(http);
  }

  /** `/FinancialAccount/GetMax` returns a bare number, not the Result<T> envelope (see GetMaxQueryHandler). */
  getMaxCodeNumber(financialAccountType: FinancialAccountType): Observable<number> {
    const params = new HttpParams().set('ParentId', 0).set('TypeId', financialAccountType);
    return this.http.get<number>(`${this.baseUrl}/GetMax`, { params });
  }
}
