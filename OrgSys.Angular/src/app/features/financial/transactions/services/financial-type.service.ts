import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from '../../../../core/services/base-api.service';
import { FinancialType } from '../models/financial.model';

/** Lookup-only — no dedicated admin screen in MVC either (drives the Financial menu + this form). */
@Injectable({ providedIn: 'root' })
export class FinancialTypeService extends BaseApiService<FinancialType, never, never> {
  protected readonly entityRoute = 'FinancialType';

  constructor(http: HttpClient) {
    super(http);
  }
}
