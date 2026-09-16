import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from '../../../../core/services/base-api.service';
import { SaveUserRequest, User } from '../models/user.model';

@Injectable({ providedIn: 'root' })
export class UserService extends BaseApiService<User, SaveUserRequest, SaveUserRequest> {
  protected readonly entityRoute = 'User';

  constructor(http: HttpClient) {
    super(http);
  }
}
