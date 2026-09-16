import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from '../../../core/services/base-api.service';
import { InvoiceType } from '../models/invoice-lookups.model';

@Injectable({ providedIn: 'root' })
export class InvoiceTypeService extends BaseApiService<InvoiceType, never, never> {
  protected readonly entityRoute = 'InvoiceType';
  constructor(http: HttpClient) {
    super(http);
  }
}
