import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from '../../../../core/services/base-api.service';
import { FiscalYear, SaveFiscalYearRequest } from '../models/fiscal-year.model';

@Injectable({ providedIn: 'root' })
export class FiscalYearService extends BaseApiService<FiscalYear, SaveFiscalYearRequest, SaveFiscalYearRequest> {
  protected readonly entityRoute = 'FiscalYear';

  constructor(http: HttpClient) {
    super(http);
  }
}
