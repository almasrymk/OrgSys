import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from '../../../../core/services/base-api.service';
import { Bank, SaveBankRequest } from '../models/bank.model';

@Injectable({ providedIn: 'root' })
export class BankService extends BaseApiService<Bank, SaveBankRequest, SaveBankRequest> {
  protected readonly entityRoute = 'Bank';

  constructor(http: HttpClient) {
    super(http);
  }
}
