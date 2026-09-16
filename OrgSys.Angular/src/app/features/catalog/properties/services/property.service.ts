import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from '../../../../core/services/base-api.service';
import { Property, SavePropertyRequest } from '../models/property.model';

@Injectable({ providedIn: 'root' })
export class PropertyService extends BaseApiService<Property, SavePropertyRequest, SavePropertyRequest> {
  protected readonly entityRoute = 'Property';

  constructor(http: HttpClient) {
    super(http);
  }
}
