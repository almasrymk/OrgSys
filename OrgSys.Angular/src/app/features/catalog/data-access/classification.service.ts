import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from '../../../core/services/base-api.service';
import { Classification } from '../models/classification.model';

@Injectable({ providedIn: 'root' })
export class ClassificationService extends BaseApiService<Classification, never, never> {
  protected readonly entityRoute = 'Classification';
  constructor(http: HttpClient) {
    super(http);
  }
}
