import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from '../../../../core/services/base-api.service';
import { Role, SaveRoleRequest } from '../models/role.model';

@Injectable({ providedIn: 'root' })
export class RoleService extends BaseApiService<Role, SaveRoleRequest, SaveRoleRequest> {
  protected readonly entityRoute = 'Role';

  constructor(http: HttpClient) {
    super(http);
  }
}
