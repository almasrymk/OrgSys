import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from '../../../../core/services/base-api.service';
import { Account, SaveAccountRequest } from '../models/account.model';

@Injectable({ providedIn: 'root' })
export class AccountService extends BaseApiService<Account, SaveAccountRequest, SaveAccountRequest> {
  protected readonly entityRoute = 'Account';

  constructor(http: HttpClient) {
    super(http);
  }
}
