import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from '../../../../core/services/base-api.service';
import { AccountType } from '../models/account.model';

/** Read-only in Angular too — AccountType has no dedicated MVC management screen either (dropdown-only). */
@Injectable({ providedIn: 'root' })
export class AccountTypeService extends BaseApiService<AccountType, never, never> {
  protected readonly entityRoute = 'AccountType';

  constructor(http: HttpClient) {
    super(http);
  }
}
