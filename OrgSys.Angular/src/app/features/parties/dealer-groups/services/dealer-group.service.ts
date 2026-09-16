import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../../../core/services/base-api.service';
import { DealerGroup, DealerType, SaveDealerGroupRequest } from '../models/dealer-group.model';

@Injectable({ providedIn: 'root' })
export class DealerGroupService extends BaseApiService<DealerGroup, SaveDealerGroupRequest, SaveDealerGroupRequest> {
  protected readonly entityRoute = 'DealerGroup';

  constructor(http: HttpClient) {
    super(http);
  }

  /** `/DealerGroup/GetMax` returns a bare number, not the Result<T> envelope. */
  getMaxCodeNumber(dealerType: DealerType): Observable<number> {
    const params = new HttpParams().set('ParentId', 0).set('TypeId', dealerType);
    return this.http.get<number>(`${this.baseUrl}/GetMax`, { params });
  }
}
