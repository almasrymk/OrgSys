import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product, SaveProductRequest } from '../models/product.model';
import { BaseApiService } from '../../../../core/services/base-api.service';

@Injectable({ providedIn: 'root' })
export class ProductService extends BaseApiService<Product, SaveProductRequest, SaveProductRequest> {
  protected readonly entityRoute = 'Product';

  constructor(http: HttpClient) {
    super(http);
  }

  /**
   * `/Product/GetMax` returns a bare number, not the Result<T> envelope. Note: this endpoint was a
   * real backend bug until fixed alongside this feature — API/Controllers/Org/Setting/ProductController.cs
   * previously wired its GetMax action to Account's GetMaxAccountQuery (a copy-paste artifact), so it
   * silently returned the max CodeNumber from the Account table, not Product. Fixed by adding a proper
   * GetMaxProductQuery (Application/Commands/Org/Setting/Product/Queries/GetMaxQueryHandler.cs).
   */
  getMaxCodeNumber(): Observable<number> {
    const params = new HttpParams().set('ParentId', 0).set('TypeId', 0);
    return this.http.get<number>(`${this.baseUrl}/GetMax`, { params });
  }
}
