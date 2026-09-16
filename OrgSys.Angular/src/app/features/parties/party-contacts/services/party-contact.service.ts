import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { ApiResult, ApiResultCollection, ApiResultOf } from '../../../../core/models/api-result.model';
import { PartyContact, SavePartyContactRequest } from '../models/party-contact.model';

@Injectable({ providedIn: 'root' })
export class PartyContactService {
  private readonly baseUrl = `${environment.apiUrl}/PartyContact`;

  constructor(private readonly http: HttpClient) {}

  getById(id: number): Observable<ApiResultOf<PartyContact>> {
    const params = new HttpParams().set('id', id);
    return this.http.get<ApiResultOf<PartyContact>>(`${this.baseUrl}/GetById`, { params });
  }

  getListByDealer(dealerId: number, keySearch = '', page = 1, pageSize = 50): Observable<ApiResultCollection<PartyContact>> {
    const params = new HttpParams()
      .set('dealerId', dealerId)
      .set('keySearch', keySearch)
      .set('page', page)
      .set('pageSize', pageSize);
    return this.http.get<ApiResultCollection<PartyContact>>(`${this.baseUrl}/GetListByDealer`, { params });
  }

  create(payload: SavePartyContactRequest): Observable<ApiResult> {
    return this.http.post<ApiResult>(`${this.baseUrl}/Create`, payload);
  }

  update(payload: SavePartyContactRequest): Observable<ApiResult> {
    return this.http.put<ApiResult>(`${this.baseUrl}/Update`, payload);
  }

  delete(id: number): Observable<ApiResult> {
    const params = new HttpParams().set('id', id);
    return this.http.delete<ApiResult>(`${this.baseUrl}/Delete`, { params });
  }
}
