import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from '../../../../core/services/base-api.service';
import { BaseEntity } from '../../../../shared/models/base-entity.model';

interface Branch extends BaseEntity {
  name: string | null;
}

/** Lookup-only, like AccountType/JournalType — Branch has no dedicated Angular admin screen yet. */
@Injectable({ providedIn: 'root' })
export class BranchService extends BaseApiService<Branch, never, never> {
  protected readonly entityRoute = 'Branch';

  constructor(http: HttpClient) {
    super(http);
  }
}
