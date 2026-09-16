import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from '../../../../core/services/base-api.service';
import { Currency, SaveCurrencyRequest } from '../models/currency.model';

@Injectable({ providedIn: 'root' })
export class CurrencyService extends BaseApiService<Currency, SaveCurrencyRequest, SaveCurrencyRequest> {
  protected readonly entityRoute = 'Currency';

  constructor(http: HttpClient) {
    super(http);
  }
}
