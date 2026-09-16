import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from '../../../../core/services/base-api.service';
import { Bank, BankBranch } from '../models/bank-lookup.model';

@Injectable({ providedIn: 'root' })
export class BankService extends BaseApiService<Bank, never, never> {
  protected readonly entityRoute = 'Bank';

  constructor(http: HttpClient) {
    super(http);
  }
}

/** No GetListByBankId endpoint exists (unlike City/District) — fetch once and filter by bankId client-side. */
@Injectable({ providedIn: 'root' })
export class BankBranchService extends BaseApiService<BankBranch, never, never> {
  protected readonly entityRoute = 'BankBranch';

  constructor(http: HttpClient) {
    super(http);
  }
}
