import { HttpClient } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResultOf } from '../models/api-result.model';
import { AuthUser, LoginRequest, LoginResponse } from './auth.model';
import { TokenStorageService } from './token-storage.service';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly tokenStorage = inject(TokenStorageService);
  private readonly router = inject(Router);

  private readonly currentUserSignal = signal<AuthUser | null>(this.tokenStorage.getUser());

  readonly currentUser = this.currentUserSignal.asReadonly();
  readonly isAuthenticated = computed(() => !!this.currentUserSignal());

  login(request: LoginRequest): Observable<ApiResultOf<LoginResponse>> {
    return this.http
      .post<ApiResultOf<LoginResponse>>(`${environment.apiUrl}/api/Auth/login`, request)
      .pipe(
        tap((result) => {
          if (result.response) {
            this.tokenStorage.setSession(result.response.token, result.response.user);
            this.currentUserSignal.set(result.response.user);
          }
        }),
      );
  }

  logout(): void {
    this.tokenStorage.clear();
    this.currentUserSignal.set(null);
    this.router.navigateByUrl('/login');
  }

  getToken(): string | null {
    return this.tokenStorage.getToken();
  }
}
