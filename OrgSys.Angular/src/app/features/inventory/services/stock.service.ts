import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from '../../../core/services/base-api.service';
import { Stock } from '../models/stock.model';

@Injectable({ providedIn: 'root' })
export class StockService extends BaseApiService<Stock, never, never> {
  protected readonly entityRoute = 'Stock';
  constructor(http: HttpClient) {
    super(http);
  }
}
