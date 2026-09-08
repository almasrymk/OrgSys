import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, switchMap } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { ApiResult, isApiSuccess } from '../../../../core/models/api-result.model';
import { BaseApiService } from '../../../../core/services/base-api.service';
import { Journal, SaveJournalRequest } from '../models/journal.model';

@Injectable({ providedIn: 'root' })
export class JournalService extends BaseApiService<Journal, SaveJournalRequest, SaveJournalRequest> {
  protected readonly entityRoute = 'Journal';

  constructor(http: HttpClient) {
    super(http);
  }

  /** `/Journal/GetMax` returns a bare number (the highest CodeNumber), not the Result<T> envelope. */
  getMaxCodeNumber(): Observable<number> {
    const params = new HttpParams().set('ParentId', 0).set('TypeId', 0);
    return this.http.get<number>(`${this.baseUrl}/GetMax`, { params });
  }

  /**
   * Creates a journal header (Create ignores `journalItems`, see journal.model.ts), looks the new
   * row up by its unique `code` (Create's Result carries no Id), then Updates it with the lines —
   * replicating OrgSys.App's AutoSave-then-search-by-Code flow in one call instead of on every field blur.
   */
  createWithLines(payload: SaveJournalRequest): Observable<ApiResult> {
    const { journalItems, ...header } = payload;

    return this.create({ ...header, journalItems: [] }).pipe(
      switchMap((createResult) => {
        if (!isApiSuccess(createResult)) return [createResult];

        return this.search({ keySearch: payload.code, page: 1, pageSize: 1 }).pipe(
          switchMap((searchResult) => {
            const createdId = searchResult.response?.[0]?.id;
            if (!createdId) return [{ statusCode: 500, errors: [{ messageError: 'Could not locate the created journal.', key: '' }] } as ApiResult];

            return this.update({
              ...payload,
              id: createdId,
              journalItems: journalItems.map((line) => ({ ...line, journalId: createdId })),
            });
          }),
        );
      }),
    );
  }

  post(id: number): Observable<ApiResult> {
    return this.http.put<ApiResult>(`${this.baseUrl}/Post`, null, { params: new HttpParams().set('Id', id) });
  }

  cancel(id: number): Observable<ApiResult> {
    return this.http.put<ApiResult>(`${this.baseUrl}/Cancel`, null, { params: new HttpParams().set('Id', id) });
  }

  redo(id: number): Observable<ApiResult> {
    return this.http.put<ApiResult>(`${this.baseUrl}/Redo`, null, { params: new HttpParams().set('Id', id) });
  }

  reverse(id: number): Observable<ApiResult> {
    return this.http.put<ApiResult>(`${this.baseUrl}/Reverse`, null, { params: new HttpParams().set('Id', id) });
  }
}
