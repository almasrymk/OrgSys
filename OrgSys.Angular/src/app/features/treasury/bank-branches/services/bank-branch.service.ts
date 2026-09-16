import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from '../../../../core/services/base-api.service';
import { BankBranch, SaveBankBranchRequest } from '../models/bank-branch.model';

@Injectable({ providedIn: 'root' })
export class BankBranchService extends BaseApiService<BankBranch, SaveBankBranchRequest, SaveBankBranchRequest> {
  protected readonly entityRoute = 'BankBranch';

  constructor(http: HttpClient) {
    super(http);
  }
}
