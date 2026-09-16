import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from '../../../core/services/base-api.service';
import { CashBox, Classification } from '../models/report-lookups.model';

@Injectable({ providedIn: 'root' })
export class ClassificationService extends BaseApiService<Classification, never, never> {
  protected readonly entityRoute = 'Classification';
  constructor(http: HttpClient) {
    super(http);
  }
}

@Injectable({ providedIn: 'root' })
export class CashBoxService extends BaseApiService<CashBox, never, never> {
  protected readonly entityRoute = 'CashBox';
  constructor(http: HttpClient) {
    super(http);
  }
}
