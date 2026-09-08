/**
 * Mirrors Domain.Shared.Result / Result<T> / ResultCollection<T> / ResultPagination<T>.
 * The API always answers HTTP 200 — success/failure lives in `statusCode`/`errors` here,
 * never in the HTTP status. Always inspect these, the way OrgSys.App's MainController.Save() does.
 */
export interface ApiError {
  messageError: string;
  key: string;
}

export interface ApiResult {
  statusCode: number;
  errors: ApiError[] | null;
}

export interface ApiResultOf<T> extends ApiResult {
  response: T | null;
}

export interface ApiResultCollection<T> extends ApiResult {
  response: T[];
}

export interface ApiResultPagination<T> extends ApiResult {
  response: T[];
  page: number;
  pageSize: number;
  pageCount: number;
}

export function isApiSuccess(result: ApiResult): boolean {
  return result.statusCode >= 200 && result.statusCode < 300;
}
