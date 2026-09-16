import { CommonModule } from '@angular/common';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { environment } from '../../../../environments/environment';
import { ApiResultCollection, ApiResultOf, ApiResultPagination } from '../../../core/models/api-result.model';
import { PageHeaderComponent } from '../page-header/page-header.component';
import { ToastService } from '../toast/toast.service';

type ListMode = 'search' | 'getList' | 'root' | 'action';

@Component({
  selector: 'app-crud-search-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, PageHeaderComponent],
  template: `
    <app-page-header [title]="title" />
    <div class="mb-3 d-flex gap-2">
      <input class="form-control" [(ngModel)]="keySearch" (keyup.enter)="load()" placeholder="Search" />
      <button class="btn btn-primary" type="button" (click)="load()">Search</button>
    </div>
    <div *ngIf="loading()" class="text-muted">Loading…</div>
    <table class="table table-sm" *ngIf="!loading()">
      <thead>
        <tr>
          <th>Id</th>
          <th>Code</th>
          <th>Name</th>
        </tr>
      </thead>
      <tbody>
        <tr *ngFor="let row of rows()">
          <td>{{ display(row, ['id', 'invoiceId', 'assetId']) }}</td>
          <td>{{ display(row, ['code', 'number', 'invoiceNumber', 'sourceDocumentNumber', 'serialNumber']) }}</td>
          <td>{{ display(row, ['name', 'legalName', 'status', 'customerId', 'dealerId']) }}</td>
        </tr>
      </tbody>
    </table>
    <p class="text-muted" *ngIf="!loading() && rows().length === 0">No rows.</p>
  `,
})
export class CrudSearchListComponent {
  private readonly http = inject(HttpClient);
  private readonly route = inject(ActivatedRoute);
  private readonly toast = inject(ToastService);

  readonly rows = signal<Record<string, unknown>[]>([]);
  readonly loading = signal(false);
  keySearch = '';
  title = 'List';
  private entityRoute = '';
  private listMode: ListMode = 'search';
  private action = '';
  private extraParams: Record<string, string | number> = {};

  constructor() {
    const data = this.route.snapshot.data;
    this.title = (data['title'] as string) ?? 'List';
    this.entityRoute = (data['entityRoute'] as string) ?? '';
    this.listMode = (data['listMode'] as ListMode) ?? 'search';
    this.action = (data['action'] as string) ?? '';
    this.extraParams = (data['extraParams'] as Record<string, string | number>) ?? {};
    this.load();
  }

  display(row: Record<string, unknown>, keys: string[]): unknown {
    for (const key of keys) {
      if (row[key] != null && row[key] !== '') return row[key];
    }
    return '';
  }

  load(): void {
    if (!this.entityRoute) return;
    this.loading.set(true);
    const url = this.buildUrl();
    this.http
      .get<ApiResultPagination<Record<string, unknown>> | ApiResultCollection<Record<string, unknown>> | ApiResultOf<Record<string, unknown>[]>>(
        url,
        { params: this.buildParams() },
      )
      .subscribe({
        next: (result) => {
          this.loading.set(false);
          const response = result.response;
          this.rows.set(Array.isArray(response) ? response : response ? [response as unknown as Record<string, unknown>] : []);
        },
        error: () => {
          this.loading.set(false);
          this.toast.error(`Could not load ${this.title}.`);
        },
      });
  }

  private buildUrl(): string {
    const base = `${environment.apiUrl}/${this.entityRoute}`;
    switch (this.listMode) {
      case 'getList':
        return `${base}/GetList`;
      case 'root':
        return base;
      case 'action':
        return `${base}/${this.action}`;
      default:
        return `${base}/Search`;
    }
  }

  private buildParams(): HttpParams {
    let params = new HttpParams()
      .set('KeySearch', this.keySearch)
      .set('keySearch', this.keySearch)
      .set('ParentId', 0)
      .set('TypeId', 0)
      .set('Page', 1)
      .set('PageSize', 50);
    for (const [key, value] of Object.entries(this.extraParams)) {
      params = params.set(key, value);
    }
    return params;
  }
}
