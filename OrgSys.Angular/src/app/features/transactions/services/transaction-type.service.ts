import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from '../../../core/services/base-api.service';
import { TransactionType } from '../models/transaction.model';

/** Lookup-only — no dedicated admin screen in MVC either. */
@Injectable({ providedIn: 'root' })
export class TransactionTypeService extends BaseApiService<TransactionType, never, never> {
  protected readonly entityRoute = 'TransactionType';

  constructor(http: HttpClient) {
    super(http);
  }
}
