import { CommonModule } from '@angular/common';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { environment } from '../../../../environments/environment';
import { ApiResultOf } from '../../../core/models/api-result.model';
import { PageHeaderComponent } from '../page-header/page-header.component';
import { ToastService } from '../toast/toast.service';

export interface QueryField {
  name: string;
  label: string;
  type?: 'text' | 'number' | 'date';
  value?: string | number;
}

@Component({
  selector: 'app-query-workspace',
  standalone: true,
  imports: [CommonModule, FormsModule, PageHeaderComponent],
  template: `
    <app-page-header [title]="title" />
    <div class="row g-2 mb-3">
      <div class="col-auto" *ngFor="let field of fields">
        <label class="form-label">{{ field.label }}</label>
        <input class="form-control" [type]="field.type || 'text'" [(ngModel)]="field.value" />
      </div>
      <div class="col-auto align-self-end">
        <button class="btn btn-primary" type="button" (click)="load()">Load</button>
      </div>
    </div>
    <div *ngIf="loading()" class="text-muted">Loading…</div>
    <pre class="bg-light p-3" *ngIf="!loading() && payload()">{{ payload() }}</pre>
  `,
})
export class QueryWorkspaceComponent {
  private readonly http = inject(HttpClient);
  private readonly route = inject(ActivatedRoute);
  private readonly toast = inject(ToastService);

  title = 'Lookup';
  fields: QueryField[] = [];
  readonly loading = signal(false);
  readonly payload = signal('');
  private entityRoute = '';
  private action = '';

  constructor() {
    const data = this.route.snapshot.data;
    this.title = (data['title'] as string) ?? 'Lookup';
    this.entityRoute = (data['entityRoute'] as string) ?? '';
    this.action = (data['action'] as string) ?? '';
    this.fields = ((data['fields'] as QueryField[]) ?? []).map((field) => ({ ...field }));
  }

  load(): void {
    if (!this.entityRoute || !this.action) return;
    this.loading.set(true);
    let params = new HttpParams();
    for (const field of this.fields) {
      if (field.value != null && field.value !== '') {
        params = params.set(field.name, field.value);
      }
    }
    this.http.get<ApiResultOf<unknown>>(`${environment.apiUrl}/${this.entityRoute}/${this.action}`, { params }).subscribe({
      next: (result) => {
        this.loading.set(false);
        this.payload.set(JSON.stringify(result.response ?? result, null, 2));
      },
      error: () => {
        this.loading.set(false);
        this.toast.error(`Could not load ${this.title}.`);
      },
    });
  }
}
