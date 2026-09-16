import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from '../../../core/services/base-api.service';
import { Unit } from '../models/unit.model';

@Injectable({ providedIn: 'root' })
export class UnitService extends BaseApiService<Unit, never, never> {
  protected readonly entityRoute = 'Unit';
  constructor(http: HttpClient) {
    super(http);
  }
}
