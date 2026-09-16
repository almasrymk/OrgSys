import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResultOf } from '../../../../core/models/api-result.model';
import { BaseApiService } from '../../../../core/services/base-api.service';
import { Dealer, DealerType, SaveDealerRequest } from '../models/dealer.model';

@Injectable({ providedIn: 'root' })
export class DealerService extends BaseApiService<Dealer, SaveDealerRequest, SaveDealerRequest> {
  protected readonly entityRoute = 'Dealer';

  constructor(http: HttpClient) {
    super(http);
  }

  /** `/Dealer/GetMax` returns a bare number, not the Result<T> envelope. */
  getMaxCodeNumber(dealerType: DealerType): Observable<number> {
    const params = new HttpParams().set('ParentId', 0).set('TypeId', dealerType);
    return this.http.get<number>(`${this.baseUrl}/GetMax`, { params });
  }

  getBalance(id: number): Observable<ApiResultOf<number>> {
    const params = new HttpParams().set('Id', id);
    return this.http.get<ApiResultOf<number>>(`${this.baseUrl}/Balance`, { params });
  }
}
