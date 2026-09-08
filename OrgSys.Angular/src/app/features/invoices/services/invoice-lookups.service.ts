import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResultCollection } from '../../../core/models/api-result.model';
import { BaseApiService } from '../../../core/services/base-api.service';
import { InvoiceType, PaymentType, Product, Stock, Unit } from '../models/invoice-lookups.model';

@Injectable({ providedIn: 'root' })
export class PaymentTypeService extends BaseApiService<PaymentType, never, never> {
  protected readonly entityRoute = 'PaymentType';
  constructor(http: HttpClient) {
    super(http);
  }
}

@Injectable({ providedIn: 'root' })
export class StockService extends BaseApiService<Stock, never, never> {
  protected readonly entityRoute = 'Stock';
  constructor(http: HttpClient) {
    super(http);
  }
}

@Injectable({ providedIn: 'root' })
export class UnitService extends BaseApiService<Unit, never, never> {
  protected readonly entityRoute = 'Unit';
  constructor(http: HttpClient) {
    super(http);
  }
}

@Injectable({ providedIn: 'root' })
export class ProductService extends BaseApiService<Product, never, never> {
  protected readonly entityRoute = 'Product';
  constructor(http: HttpClient) {
    super(http);
  }

  /** Mirrors MVC's `/Setting/Product/LoadProductsByStock` proxy — every product with its on-hand `balance` in `stockId` as of `date`. */
  getAllByBalance(stockId: number, date: string): Observable<ApiResultCollection<Product>> {
    const params = new HttpParams().set('StockId', stockId).set('date', date);
    return this.http.get<ApiResultCollection<Product>>(`${this.baseUrl}/GetAllByBalance`, { params });
  }
}

@Injectable({ providedIn: 'root' })
export class InvoiceTypeService extends BaseApiService<InvoiceType, never, never> {
  protected readonly entityRoute = 'InvoiceType';
  constructor(http: HttpClient) {
    super(http);
  }
}
