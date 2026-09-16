import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormArray, FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { Currency } from '../../../../master-data/currencies/models/currency.model';
import { CurrencyService } from '../../../../master-data/currencies/services/currency.service';
import { Unit } from '../../../models/unit.model';
import { UnitService } from '../../../data-access/unit.service';
import { Product } from '../../../products/models/product.model';
import { ProductService } from '../../../products/services/product.service';
import { PriceListService } from '../../services/price-list.service';

@Component({
  selector: 'app-price-list-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './price-list-form.component.html',
})
export class PriceListFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly priceListService = inject(PriceListService);
  private readonly currencyService = inject(CurrencyService);
  private readonly productService = inject(ProductService);
  private readonly unitService = inject(UnitService);
  private readonly toast = inject(ToastService);
  private readonly router = inject(Router);

  readonly id = signal<number | null>(null);
  readonly saving = signal(false);
  readonly currencies = signal<Currency[]>([]);
  readonly products = signal<Product[]>([]);
  readonly units = signal<Unit[]>([]);

  readonly form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(150)]],
    currencyId: [0],
    validFrom: [''],
    validTo: [''],
    isDefault: [false],
    isActive: [true],
    entries: this.fb.array<ReturnType<typeof this.buildEntry>>([]),
  });

  get entries(): FormArray {
    return this.form.controls.entries;
  }

  constructor(route: ActivatedRoute) {
    this.currencyService.getList({ pageSize: 500 }).subscribe((r) => this.currencies.set(r.response ?? []));
    this.productService.getList({ pageSize: 5000 }).subscribe((r) => this.products.set(r.response ?? []));
    this.unitService.getList({ pageSize: 500 }).subscribe((r) => this.units.set(r.response ?? []));

    const idParam = route.snapshot.paramMap.get('id');
    if (idParam) {
      const id = Number(idParam);
      this.id.set(id);
      this.priceListService.getById(id).subscribe((result) => {
        const priceList = result.response;
        if (!priceList) return;
        this.form.patchValue({
          name: priceList.name ?? '',
          currencyId: priceList.currencyId ?? 0,
          validFrom: priceList.validFrom ? priceList.validFrom.substring(0, 10) : '',
          validTo: priceList.validTo ? priceList.validTo.substring(0, 10) : '',
          isDefault: priceList.isDefault,
          isActive: priceList.isActive,
        });
        this.entries.clear();
        for (const entry of priceList.entries ?? []) {
          this.entries.push(this.buildEntry(entry.id, entry.productId, entry.unitId, entry.price, entry.minQuantity, entry.validFrom, entry.validTo, entry.isActive));
        }
      });
    }
  }

  private buildEntry(
    id = 0,
    productId = 0,
    unitId: number | null = null,
    price = 0,
    minQuantity: number | null = null,
    validFrom: string | null = null,
    validTo: string | null = null,
    isActive = true,
  ) {
    return this.fb.nonNullable.group({
      id: [id],
      productId: [productId, [Validators.required, Validators.min(1)]],
      unitId: [unitId],
      price: [price, [Validators.required, Validators.min(0)]],
      minQuantity: [minQuantity],
      validFrom: [validFrom ? validFrom.substring(0, 10) : ''],
      validTo: [validTo ? validTo.substring(0, 10) : ''],
      isActive: [isActive],
    });
  }

  addEntry(): void {
    this.entries.push(this.buildEntry());
  }

  removeEntry(index: number): void {
    this.entries.removeAt(index);
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving.set(true);
    const value = this.form.getRawValue();
    const id = this.id() ?? undefined;
    const payload = {
      id,
      name: value.name,
      currencyId: value.currencyId || null,
      validFrom: value.validFrom || null,
      validTo: value.validTo || null,
      isDefault: value.isDefault,
      isActive: value.isActive,
      entries: value.entries.map((entry) => ({
        id: entry.id || undefined,
        productId: entry.productId,
        unitId: entry.unitId || null,
        price: Number(entry.price) || 0,
        minQuantity: entry.minQuantity == null ? null : Number(entry.minQuantity),
        validFrom: entry.validFrom || null,
        validTo: entry.validTo || null,
        isActive: entry.isActive,
      })),
    };

    const request$ = id ? this.priceListService.update(payload) : this.priceListService.create(payload);

    request$.subscribe({
      next: (result) => {
        this.saving.set(false);
        if (isApiSuccess(result)) {
          this.toast.success('Price list saved.');
          this.router.navigateByUrl('/catalog/price-lists');
        } else {
          this.toast.error(result.errors?.[0]?.messageError ?? 'Save failed.');
        }
      },
      error: () => {
        this.saving.set(false);
        this.toast.error('Save failed.');
      },
    });
  }
}
