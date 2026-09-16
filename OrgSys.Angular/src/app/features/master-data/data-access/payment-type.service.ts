import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from '../../../core/services/base-api.service';
import { PaymentType } from '../models/payment-type.model';

@Injectable({ providedIn: 'root' })
export class PaymentTypeService extends BaseApiService<PaymentType, never, never> {
  protected readonly entityRoute = 'PaymentType';
  constructor(http: HttpClient) {
    super(http);
  }
}
