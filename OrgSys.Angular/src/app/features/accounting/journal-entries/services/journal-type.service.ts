import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from '../../../../core/services/base-api.service';
import { JournalType } from '../models/journal.model';

/** Read-only in Angular too — /JournalType is a CoreController (no Create/Update/Delete). */
@Injectable({ providedIn: 'root' })
export class JournalTypeService extends BaseApiService<JournalType, never, never> {
  protected readonly entityRoute = 'JournalType';

  constructor(http: HttpClient) {
    super(http);
  }
}
