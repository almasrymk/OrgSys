import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from '../../../core/services/base-api.service';
import { CashBox } from '../models/report-lookups.model';

/** Legacy `/CashBox` lookup still used by safe reports. Transactional UI uses FinancialAccount. */
@Injectable({ providedIn: 'root' })
export class CashBoxService extends BaseApiService<CashBox, never, never> {
  protected readonly entityRoute = 'CashBox';
  constructor(http: HttpClient) {
    super(http);
  }
}
