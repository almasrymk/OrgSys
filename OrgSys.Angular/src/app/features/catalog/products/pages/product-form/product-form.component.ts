import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormArray, FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { Dealer } from '../../../../parties';
import { DealerService } from '../../../../parties';
import { Unit } from '../../../models/unit.model';
import { UnitService } from '../../../data-access/unit.service';
import { Classification } from '../../../models/classification.model';
import { ClassificationService } from '../../../data-access/classification.service';
import { ProductService } from '../../services/product.service';

/**
 * Replaces Areas/Setting/Views/Product/Save.cshtml, trimmed to core fields — ProductRecipes
 * (bill-of-materials) and ProductPropertyElements (variant properties) are their own sub-features,
 * not built this pass. `ProductUnits` (multi-unit + conversion rate) IS built, since every
 * Invoice/Transaction/Inventory line-item unit picker depends on it existing per product.
 */
@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './product-form.component.html',
})
export class ProductFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly toast = inject(ToastService);
  private readonly service = inject(ProductService);
  private readonly classificationService = inject(ClassificationService);
  private readonly dealerService = inject(DealerService);
  private readonly unitService = inject(UnitService);

  readonly id = signal<number | null>(null);
  readonly code = signal<string | null>(null);
  readonly saving = signal(false);

  readonly classifications = signal<Classification[]>([]);
  readonly dealers = signal<Dealer[]>([]);
  readonly units = signal<Unit[]>([]);

  readonly form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.minLength(2)]],
    nickname: [''],
    barcode: [''],
    description: [''],
    price: [0, [Validators.required, Validators.min(0)]],
    cost: [0, [Validators.required, Validators.min(0)]],
    classificationId: [0, [Validators.required, Validators.min(1)]],
    dealerId: [0],
    productUnits: this.fb.array<ReturnType<typeof this.buildUnit>>([]),
  });

  readonly productUnits = this.form.controls.productUnits;

  constructor() {
    this.classificationService.getList({ pageSize: 500 }).subscribe((r) => this.classifications.set(r.response ?? []));
    this.dealerService.getList({ pageSize: 2000 }).subscribe((r) => this.dealers.set(r.response ?? []));
    this.unitService.getList({ pageSize: 500 }).subscribe((r) => this.units.set(r.response ?? []));

    const editId = this.route.snapshot.paramMap.get('id');
    if (editId) {
      const id = Number(editId);
      this.id.set(id);
      this.service.getById(id).subscribe((result) => {
        const product = result.response;
        if (!product) return;

        this.code.set(product.code);
        this.form.patchValue({
          name: product.name ?? '',
          nickname: product.nickname ?? '',
          barcode: product.barcode ?? '',
          description: product.description ?? '',
          price: product.price,
          cost: product.cost,
          classificationId: product.classificationId,
          dealerId: product.dealerId ?? 0,
        });

        for (const unit of product.productUnits ?? []) {
          this.productUnits.push(this.buildUnit(unit.id, unit.unitId, unit.rate, unit.defaultUnit));
        }
      });
    } else {
      this.service.getMaxCodeNumber().subscribe((max) => this.code.set(String((max || 0) + 1)));
      this.addUnit();
    }
  }

  private buildUnit(id?: number, unitId = 0, rate = 1, defaultUnit = false) {
    return this.fb.nonNullable.group({
      id: [id],
      unitId: [unitId, [Validators.required, Validators.min(1)]],
      rate: [rate, [Validators.required, Validators.min(0.0001)]],
      defaultUnit: [defaultUnit],
    });
  }

  addUnit(): void {
    this.productUnits.push(this.buildUnit(undefined, 0, 1, this.productUnits.length === 0));
  }

  removeUnit(index: number): void {
    this.productUnits.removeAt(index);
  }

  setDefaultUnit(index: number): void {
    this.productUnits.controls.forEach((control, i) => control.patchValue({ defaultUnit: i === index }));
  }

  submit(): void {
    if (this.form.invalid || this.productUnits.length === 0) {
      this.form.markAllAsTouched();
      if (this.productUnits.length === 0) this.toast.error('Add at least one unit.');
      return;
    }

    this.saving.set(true);
    const value = this.form.getRawValue();
    const id = this.id() ?? undefined;
    const codeNumber = Number(this.code() ?? 0);

    const payload = {
      ...(id ? { id } : {}),
      code: this.code() ?? String(codeNumber),
      codeNumber,
      name: value.name,
      nickname: value.nickname || null,
      barcode: value.barcode || null,
      description: value.description || null,
      price: Number(value.price) || 0,
      cost: Number(value.cost) || 0,
      classificationId: value.classificationId,
      dealerId: value.dealerId || null,
      productUnits: value.productUnits.map((u) => ({
        id: u.id ?? undefined,
        productId: id,
        unitId: u.unitId,
        rate: Number(u.rate) || 0,
        defaultUnit: u.defaultUnit,
      })),
    };

    const request$ = id ? this.service.update(payload) : this.service.create(payload);

    request$.subscribe({
      next: (result) => {
        this.saving.set(false);
        if (isApiSuccess(result)) {
          this.toast.success('Product saved.');
          this.router.navigateByUrl('/catalog/products');
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
