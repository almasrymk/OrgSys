import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from '../../../../core/services/base-api.service';
import { PriceList, SavePriceListRequest } from '../models/price-list.model';

@Injectable({ providedIn: 'root' })
export class PriceListService extends BaseApiService<PriceList, SavePriceListRequest, SavePriceListRequest> {
  protected readonly entityRoute = 'PriceList';

  constructor(http: HttpClient) {
    super(http);
  }
}
