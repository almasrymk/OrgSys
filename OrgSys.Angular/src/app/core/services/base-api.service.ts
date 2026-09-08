import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResult, ApiResultCollection, ApiResultOf, ApiResultPagination } from '../models/api-result.model';
import { SearchParams } from '../../shared/models/base-entity.model';

/**
 * Encodes the API's shared convention (see API/Controllers/BaseController.cs and
 * OrgSys.App's MainController<TDto,TCreate,TUpdate>): every entity is reachable at
 * `/{entityRoute}/{GetById|GetList|Search|Create|Update|Delete|DeleteList}`.
 *
 * Feature services extend this instead of calling HttpClient directly, so a component
 * never talks to the API on its own (see financial-form -> financial.service -> API).
 */
export abstract class BaseApiService<TDto, TCreate, TUpdate> {
  protected abstract readonly entityRoute: string;

  constructor(protected readonly http: HttpClient) {}

  protected get baseUrl(): string {
    return `${environment.apiUrl}/${this.entityRoute}`;
  }

  getById(id: number): Observable<ApiResultOf<TDto>> {
    const params = new HttpParams().set('Id', id);
    return this.http.get<ApiResultOf<TDto>>(`${this.baseUrl}/GetById`, { params });
  }

  getList(search: Partial<SearchParams> = {}): Observable<ApiResultCollection<TDto>> {
    return this.http.get<ApiResultCollection<TDto>>(`${this.baseUrl}/GetList`, {
      params: this.toSearchParams(search),
    });
  }

  search(search: Partial<SearchParams> = {}): Observable<ApiResultPagination<TDto>> {
    return this.http.get<ApiResultPagination<TDto>>(`${this.baseUrl}/Search`, {
      params: this.toSearchParams(search),
    });
  }

  create(payload: TCreate): Observable<ApiResult> {
    return this.http.post<ApiResult>(`${this.baseUrl}/Create`, payload);
  }

  update(payload: TUpdate): Observable<ApiResult> {
    return this.http.put<ApiResult>(`${this.baseUrl}/Update`, payload);
  }

  delete(id: number): Observable<ApiResult> {
    const params = new HttpParams().set('Id', id);
    return this.http.delete<ApiResult>(`${this.baseUrl}/Delete`, { params });
  }

  deleteList(ids: number[]): Observable<ApiResult> {
    let params = new HttpParams();
    ids.forEach((id) => (params = params.append('Ids', id)));
    return this.http.delete<ApiResult>(`${this.baseUrl}/DeleteList`, { params });
  }

  private toSearchParams(search: Partial<SearchParams>): HttpParams {
    return new HttpParams()
      .set('KeySearch', search.keySearch ?? '')
      .set('ParentId', search.parentId ?? 0)
      .set('TypeId', search.typeId ?? 0)
      .set('Page', search.page ?? 1)
      .set('PageSize', search.pageSize ?? 20);
  }
}
