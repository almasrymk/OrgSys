import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { FormArray, FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../../core/auth/auth.service';
import { isApiSuccess } from '../../../../core/models/api-result.model';
import { PageHeaderComponent } from '../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../shared/components/toast/toast.service';
import { Product, Stock, Unit } from '../../../invoices/models/invoice-lookups.model';
import { ProductService, StockService, UnitService } from '../../../invoices/services/invoice-lookups.service';
import { InventoryService } from '../../services/inventory.service';

/**
 * Replaces Areas/Transactions/Views/Inventory/Save.cshtml. No TypeId dimension (see
 * inventory-list.component.ts). "Load warehouse products" mirrors the MVC page's `LoadProducts()`:
 * it replaces every line with the full product list for the chosen Stock, pre-filled with each
 * product's current on-hand `balance` as `calcBalance` (via `Product/GetAllByBalance`) and
 * `actualBalance` defaulted to 0 — the counter then edits `actualBalance` per line, `diffQuantity`
 * is always derived (`actualBalance - calcBalance`), matching Save.cshtml's client-side JS exactly.
 * Lines can also be added/edited manually. Update's `SaveDetials` sets `InventoryId` on every line
 * itself server-side (unlike Journal/Invoice/Transaction) but it's set here too defensively.
 */
@Component({
  selector: 'app-inventory-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './inventory-form.component.html',
})
export class InventoryFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly auth = inject(AuthService);
  private readonly toast = inject(ToastService);
  private readonly service = inject(InventoryService);
  private readonly stockService = inject(StockService);
  private readonly unitService = inject(UnitService);
  private readonly productService = inject(ProductService);

  readonly id = signal<number | null>(null);
  readonly code = signal<string | null>(null);
  readonly saving = signal(false);
  readonly loadingProducts = signal(false);

  readonly stocks = signal<Stock[]>([]);
  readonly units = signal<Unit[]>([]);
  readonly products = signal<Product[]>([]);

  private originalCreateUserId: number | null = null;
  private originalCreateDate: string | null = null;

  readonly form = this.fb.nonNullable.group({
    date: [new Date().toISOString().slice(0, 10), Validators.required],
    stockId: [0, [Validators.required, Validators.min(1)]],
    notes: [''],
    inventoryProductList: this.fb.array<ReturnType<typeof this.buildLine>>([]),
  });

  readonly lines = this.form.controls.inventoryProductList;

  private readonly linesVersion = signal(0);
  readonly diffCount = computed(() => {
    this.linesVersion();
    return this.lines.getRawValue().filter((l) => Number(l.diffQuantity) !== 0).length;
  });

  constructor() {
    this.stockService.getList({ pageSize: 500 }).subscribe((r) => this.stocks.set(r.response ?? []));
    this.unitService.getList({ pageSize: 500 }).subscribe((r) => this.units.set(r.response ?? []));
    this.productService.getList({ pageSize: 5000 }).subscribe((r) => this.products.set(r.response ?? []));

    const editId = this.route.snapshot.paramMap.get('id');
    if (editId) {
      const id = Number(editId);
      this.id.set(id);
      this.service.getById(id).subscribe((result) => {
        const inventory = result.response;
        if (!inventory) return;

        this.code.set(inventory.code);
        this.originalCreateUserId = inventory.createUserId;
        this.originalCreateDate = inventory.createDate;

        this.form.patchValue({
          date: inventory.date?.slice(0, 10) ?? '',
          stockId: inventory.stockId ?? 0,
          notes: inventory.notes ?? '',
        });

        for (const line of inventory.inventoryProductList ?? []) {
          this.lines.push(
            this.buildLine(line.id, line.rowNumber, line.productId, line.unitId, line.calcBalance, line.actualBalance, line.notes),
          );
        }
        this.linesVersion.update((v) => v + 1);
      });
    } else {
      this.service.getMaxCodeNumber().subscribe((max) => this.code.set(String((max || 0) + 1)));
    }
  }

  private buildLine(
    id?: number,
    rowNumber = 1,
    productId = 0,
    unitId = 0,
    calcBalance = 0,
    actualBalance = 0,
    notes: string | null = '',
  ) {
    return this.fb.nonNullable.group({
      id: [id],
      rowNumber: [rowNumber],
      productId: [productId, [Validators.required, Validators.min(1)]],
      unitId: [unitId, [Validators.required, Validators.min(1)]],
      calcBalance: [calcBalance],
      actualBalance: [actualBalance],
      diffQuantity: [actualBalance - calcBalance],
      notes: [notes ?? ''],
    });
  }

  addLine(): void {
    this.lines.push(this.buildLine(undefined, this.lines.length + 1));
    this.linesVersion.update((v) => v + 1);
  }

  removeLine(index: number): void {
    this.lines.removeAt(index);
    this.linesVersion.update((v) => v + 1);
  }

  loadWarehouseProducts(): void {
    const stockId = this.form.controls.stockId.value;
    const date = this.form.controls.date.value;
    if (!stockId) {
      this.toast.error('Select a warehouse first.');
      return;
    }

    this.loadingProducts.set(true);
    this.productService.getAllByBalance(stockId, date).subscribe({
      next: (result) => {
        this.loadingProducts.set(false);
        const products = result.response ?? [];
        this.lines.clear();
        products.forEach((p, i) => {
          const balance = p.balance ?? 0;
          const defaultUnitId = p.productUnits?.find((u) => u.defaultUnit)?.unitId ?? p.productUnits?.[0]?.unitId ?? this.units()[0]?.id ?? 0;
          this.lines.push(this.buildLine(undefined, i + 1, p.id, defaultUnitId, balance, 0, null));
        });
        this.linesVersion.update((v) => v + 1);
        this.toast.success(`Loaded ${products.length} product(s).`);
      },
      error: () => {
        this.loadingProducts.set(false);
        this.toast.error('Unable to load warehouse products.');
      },
    });
  }

  recomputeLine(index: number): void {
    const line = this.lines.at(index);
    const v = line.getRawValue();
    const diffQuantity = (Number(v.actualBalance) || 0) - (Number(v.calcBalance) || 0);
    line.patchValue({ diffQuantity }, { emitEvent: false });
    this.linesVersion.update((v) => v + 1);
  }

  submit(): void {
    if (this.form.invalid || this.lines.length === 0) {
      this.form.markAllAsTouched();
      if (this.lines.length === 0) this.toast.error('Add at least one product line.');
      return;
    }

    this.saving.set(true);
    const value = this.form.getRawValue();
    const id = this.id() ?? undefined;
    const currentUserId = this.auth.currentUser()?.id;
    const nowIso = new Date().toISOString();

    const codeNumber = Number(this.code() ?? 0);

    const payload = {
      ...(id ? { id } : {}),
      code: this.code() ?? String(codeNumber),
      codeNumber,
      date: value.date,
      stockId: value.stockId || null,
      notes: value.notes || null,
      inventoryProductList: value.inventoryProductList.map((l, i) => ({
        id: l.id ?? undefined,
        inventoryId: id,
        rowNumber: i + 1,
        productId: l.productId,
        unitId: l.unitId,
        calcBalance: Number(l.calcBalance) || 0,
        actualBalance: Number(l.actualBalance) || 0,
        diffQuantity: Number(l.diffQuantity) || 0,
        notes: l.notes || null,
      })),
      createUserId: id ? (this.originalCreateUserId ?? undefined) : currentUserId,
      createDate: id ? (this.originalCreateDate ?? undefined) : nowIso,
      ...(id ? { modifyUserId: currentUserId, modifyDate: nowIso } : {}),
    };

    const request$ = id ? this.service.update(payload) : this.service.create(payload);

    request$.subscribe({
      next: (result) => {
        this.saving.set(false);
        if (isApiSuccess(result)) {
          this.toast.success('Inventory count saved.');
          this.router.navigate(['/inventory']);
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
