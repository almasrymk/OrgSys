import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from '../../../../core/services/base-api.service';
import { Brand, SaveBrandRequest } from '../models/brand.model';

@Injectable({ providedIn: 'root' })
export class BrandService extends BaseApiService<Brand, SaveBrandRequest, SaveBrandRequest> {
  protected readonly entityRoute = 'Brand';

  constructor(http: HttpClient) {
    super(http);
  }
}
