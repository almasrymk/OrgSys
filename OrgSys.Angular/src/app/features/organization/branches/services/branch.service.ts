import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from '../../../../core/services/base-api.service';
import { Branch, SaveBranchRequest } from '../models/branch.model';

@Injectable({ providedIn: 'root' })
export class BranchService extends BaseApiService<Branch, SaveBranchRequest, SaveBranchRequest> {
  protected readonly entityRoute = 'Branch';

  constructor(http: HttpClient) {
    super(http);
  }
}
