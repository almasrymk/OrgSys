import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { ApiResult, ApiResultCollection, ApiResultOf } from '../../../../core/models/api-result.model';
import { PartyAddress, SavePartyAddressRequest } from '../models/party-address.model';

@Injectable({ providedIn: 'root' })
export class PartyAddressService {
  private readonly baseUrl = `${environment.apiUrl}/PartyAddress`;

  constructor(private readonly http: HttpClient) {}

  getById(id: number): Observable<ApiResultOf<PartyAddress>> {
    const params = new HttpParams().set('id', id);
    return this.http.get<ApiResultOf<PartyAddress>>(`${this.baseUrl}/GetById`, { params });
  }

  getListByDealer(dealerId: number, page = 1, pageSize = 50): Observable<ApiResultCollection<PartyAddress>> {
    const params = new HttpParams().set('dealerId', dealerId).set('page', page).set('pageSize', pageSize);
    return this.http.get<ApiResultCollection<PartyAddress>>(`${this.baseUrl}/GetListByDealer`, { params });
  }

  create(payload: SavePartyAddressRequest): Observable<ApiResult> {
    return this.http.post<ApiResult>(`${this.baseUrl}/Create`, payload);
  }

  update(payload: SavePartyAddressRequest): Observable<ApiResult> {
    return this.http.put<ApiResult>(`${this.baseUrl}/Update`, payload);
  }

  delete(id: number): Observable<ApiResult> {
    const params = new HttpParams().set('id', id);
    return this.http.delete<ApiResult>(`${this.baseUrl}/Delete`, { params });
  }
}
