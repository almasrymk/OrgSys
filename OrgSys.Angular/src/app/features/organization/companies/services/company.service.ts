import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from '../../../../core/services/base-api.service';
import { Company, SaveCompanyRequest } from '../models/company.model';

@Injectable({ providedIn: 'root' })
export class CompanyService extends BaseApiService<Company, SaveCompanyRequest, SaveCompanyRequest> {
  protected readonly entityRoute = 'Company';

  constructor(http: HttpClient) {
    super(http);
  }
}
