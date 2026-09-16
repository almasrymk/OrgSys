import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ApiResultPagination } from '../../../core/models/api-result.model';
import {
  BalanceRow,
  DealerBalanceRow,
  DealerStatementRow,
  MovementRow,
  ReportPage,
  SafeBalanceRow,
  SafeMovementRow,
  SalesBalanceRow,
} from '../models/report.model';

function toPage<T>(result: ApiResultPagination<T>): ReportPage<T> {
  return { rows: result.response ?? [], page: result.page || 1, pageCount: result.pageCount || 1 };
}

@Injectable({ providedIn: 'root' })
export class WarehouseReportService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/WarehouseReport`;

  movement(params: { fromDate: string; toDate: string; stockId: number; productId: number; page: number; pageSize: number }): Observable<ReportPage<MovementRow>> {
    const httpParams = new HttpParams()
      .set('FromDate', params.fromDate)
      .set('ToDate', params.toDate)
      .set('StockId', params.stockId)
      .set('ProductId', params.productId)
      .set('Page', params.page)
      .set('PageSize', params.pageSize);
    return this.http.get<ApiResultPagination<MovementRow>>(`${this.baseUrl}/Movement`, { params: httpParams }).pipe(map(toPage));
  }

  balance(params: { toDate: string; stockId: number; productId: number; classificationId: number; sortByStock: boolean; page: number; pageSize: number }): Observable<ReportPage<BalanceRow>> {
    const httpParams = new HttpParams()
      .set('ToDate', params.toDate)
      .set('StockId', params.stockId)
      .set('ProductId', params.productId)
      .set('ClassificationId', params.classificationId)
      .set('SortByStock', params.sortByStock)
      .set('Page', params.page)
      .set('PageSize', params.pageSize);
    return this.http.get<ApiResultPagination<BalanceRow>>(`${this.baseUrl}/Balance`, { params: httpParams }).pipe(map(toPage));
  }
}

@Injectable({ providedIn: 'root' })
export class DealerReportService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/DealerReport`;

  balance(params: { dealerTypeId: number; toDate: string; dealerId: number; page: number; pageSize: number }): Observable<ReportPage<DealerBalanceRow>> {
    const httpParams = new HttpParams()
      .set('DealerTypeId', params.dealerTypeId)
      .set('ToDate', params.toDate)
      .set('DealerId', params.dealerId)
      .set('ShiftId', 0)
      .set('BranchId', 0)
      .set('UserId', 0)
      .set('Page', params.page)
      .set('PageSize', params.pageSize);
    return this.http.get<ApiResultPagination<DealerBalanceRow>>(`${this.baseUrl}/Balance`, { params: httpParams }).pipe(map(toPage));
  }

  statement(params: { dealerTypeId: number; fromDate: string; toDate: string; dealerId: number; page: number; pageSize: number }): Observable<ReportPage<DealerStatementRow>> {
    const httpParams = new HttpParams()
      .set('DealerTypeId', params.dealerTypeId)
      .set('FromDate', params.fromDate)
      .set('ToDate', params.toDate)
      .set('DealerId', params.dealerId)
      .set('ShiftId', 0)
      .set('BranchId', 0)
      .set('UserId', 0)
      .set('Page', params.page)
      .set('PageSize', params.pageSize);
    return this.http.get<ApiResultPagination<DealerStatementRow>>(`${this.baseUrl}/Statement`, { params: httpParams }).pipe(map(toPage));
  }
}

@Injectable({ providedIn: 'root' })
export class FinancialReportService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/FinancialReport`;

  safeMovement(params: { fromDate: string; toDate: string; dealerId: number; cashBoxId: number; page: number; pageSize: number }): Observable<ReportPage<SafeMovementRow>> {
    const httpParams = new HttpParams()
      .set('FromDate', params.fromDate)
      .set('ToDate', params.toDate)
      .set('DealerId', params.dealerId)
      .set('CashBoxId', params.cashBoxId)
      .set('Page', params.page)
      .set('PageSize', params.pageSize);
    return this.http.get<ApiResultPagination<SafeMovementRow>>(`${this.baseUrl}/SafeMovement`, { params: httpParams }).pipe(map(toPage));
  }

  safeBalance(params: { toDate: string; cashBoxId: number; page: number; pageSize: number }): Observable<ReportPage<SafeBalanceRow>> {
    const httpParams = new HttpParams()
      .set('ToDate', params.toDate)
      .set('CashBoxId', params.cashBoxId)
      .set('UserId', 0)
      .set('ShiftId', 0)
      .set('Page', params.page)
      .set('PageSize', params.pageSize);
    return this.http.get<ApiResultPagination<SafeBalanceRow>>(`${this.baseUrl}/SafeBalance`, { params: httpParams }).pipe(map(toPage));
  }
}

@Injectable({ providedIn: 'root' })
export class SalesReportService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/SalesReport`;

  balance(params: { fromDate: string; toDate: string; page: number; pageSize: number }): Observable<ReportPage<SalesBalanceRow>> {
    const httpParams = new HttpParams()
      .set('FromDate', params.fromDate)
      .set('ToDate', params.toDate)
      .set('Page', params.page)
      .set('PageSize', params.pageSize);
    return this.http.get<ApiResultPagination<SalesBalanceRow>>(`${this.baseUrl}/Balance`, { params: httpParams }).pipe(map(toPage));
  }
}
