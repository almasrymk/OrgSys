import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { FormArray, FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../../core/auth/auth.service';
import { isApiSuccess } from '../../../../core/models/api-result.model';
import { PageHeaderComponent } from '../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../shared/components/toast/toast.service';
import { Dealer } from '../../../parties';
import { DealerService } from '../../../parties';
import { Product, ProductService, Unit, UnitService } from '../../../catalog';
import { Stock } from '../../models/stock.model';
import { StockService } from '../../services/stock.service';
import { inventoryMovementPath, TransactionType, TransactionTypeId } from '../../models/transaction.model';
import { TransactionService } from '../../services/transaction.service';
import { TransactionTypeService } from '../../services/transaction-type.service';

/**
 * Replaces Areas/Transactions/Views/Transaction/{Save,Products}.cshtml. One form, parameterized by
 * TransactionType (route :typeId) — same pattern as Invoice. `toStockId` only shown/required for
 * Transfer (3); the "Received" (4) leg is auto-generated server-side, never edited directly. Dealer
 * is optional (not every transaction type involves one). Update is blocked server-side for
 * inventory-adjustment-sourced or invoice-linked transactions — Save then surfaces the API error.
 */
@Component({
  selector: 'app-transaction-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './transaction-form.component.html',
})
export class TransactionFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly auth = inject(AuthService);
  private readonly toast = inject(ToastService);
  private readonly service = inject(TransactionService);
  private readonly transactionTypeService = inject(TransactionTypeService);
  private readonly dealerService = inject(DealerService);
  private readonly stockService = inject(StockService);
  private readonly unitService = inject(UnitService);
  private readonly productService = inject(ProductService);

  readonly TransactionTypeId = TransactionTypeId;
  readonly typeId = Number(this.route.snapshot.data['typeId'] ?? this.route.snapshot.paramMap.get('typeId'));
  readonly listPath = inventoryMovementPath(this.typeId);
  readonly transactionType = signal<TransactionType | null>(null);
  readonly id = signal<number | null>(null);
  readonly code = signal<string | null>(null);
  readonly saving = signal(false);

  readonly dealers = signal<Dealer[]>([]);
  readonly stocks = signal<Stock[]>([]);
  readonly units = signal<Unit[]>([]);
  readonly products = signal<Product[]>([]);

  private originalCreateUserId: number | null = null;
  private originalCreateDate: string | null = null;

  readonly form = this.fb.nonNullable.group({
    date: [new Date().toISOString().slice(0, 10), Validators.required],
    dealerId: [0],
    stockId: [0, [Validators.required, Validators.min(1)]],
    toStockId: [0],
    notes: [''],
    transactionProductList: this.fb.array<ReturnType<typeof this.buildLine>>([]),
  });

  readonly lines = this.form.controls.transactionProductList;

  private readonly linesVersion = signal(0);
  readonly linesTotal = computed(() => {
    this.linesVersion();
    return this.lines.getRawValue().reduce((sum, l) => sum + (Number(l.total) || 0), 0);
  });

  constructor() {
    this.transactionTypeService.getById(this.typeId).subscribe((r) => this.transactionType.set(r.response));

    this.dealerService.getList({ pageSize: 2000 }).subscribe((r) => this.dealers.set(r.response ?? []));
    this.stockService.getList({ pageSize: 500 }).subscribe((r) => this.stocks.set(r.response ?? []));
    this.unitService.getList({ pageSize: 500 }).subscribe((r) => this.units.set(r.response ?? []));
    this.productService.getList({ pageSize: 5000 }).subscribe((r) => this.products.set(r.response ?? []));

    const editId = this.route.snapshot.paramMap.get('id');
    if (editId) {
      const id = Number(editId);
      this.id.set(id);
      this.service.getById(id).subscribe((result) => {
        const transaction = result.response;
        if (!transaction) return;

        this.code.set(transaction.code);
        this.originalCreateUserId = transaction.createUserId;
        this.originalCreateDate = transaction.createDate;

        this.form.patchValue({
          date: transaction.date?.slice(0, 10) ?? '',
          dealerId: transaction.dealerId ?? 0,
          stockId: transaction.stockId ?? 0,
          toStockId: transaction.toStockId ?? 0,
          notes: transaction.notes ?? '',
        });

        for (const line of transaction.transactionProductList ?? []) {
          this.lines.push(this.buildLine(line.id, line.rowNumber, line.productId, line.unitId, line.stockId, line.quantity, line.cost, line.notes));
        }
        this.linesVersion.update((v) => v + 1);
      });
    } else {
      this.service.getMaxCodeNumber(this.typeId).subscribe((max) => this.code.set(String((max || 0) + 1)));
      this.addLine();
    }
  }

  private buildLine(
    id?: number,
    rowNumber = 1,
    productId = 0,
    unitId = 0,
    stockId: number | null = null,
    quantity = 1,
    cost = 0,
    notes: string | null = '',
  ) {
    return this.fb.nonNullable.group({
      id: [id],
      rowNumber: [rowNumber],
      productId: [productId, [Validators.required, Validators.min(1)]],
      unitId: [unitId, [Validators.required, Validators.min(1)]],
      stockId: [stockId],
      quantity: [quantity, [Validators.required, Validators.min(0.0001)]],
      cost: [cost, [Validators.required, Validators.min(0)]],
      total: [quantity * cost],
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

  onProductChange(index: number, productId: number): void {
    const product = this.products().find((p) => p.id === productId);
    if (product) this.lines.at(index).patchValue({ cost: product.price });
    this.recomputeLine(index);
  }

  recomputeLine(index: number): void {
    const line = this.lines.at(index);
    const v = line.getRawValue();
    const total = (Number(v.quantity) || 0) * (Number(v.cost) || 0);
    line.patchValue({ total }, { emitEvent: false });
    this.linesVersion.update((v) => v + 1);
  }

  submit(): void {
    const requiresToStock = this.typeId === TransactionTypeId.Transfer;
    if (this.form.invalid || this.lines.length === 0 || (requiresToStock && !this.form.getRawValue().toStockId)) {
      this.form.markAllAsTouched();
      if (this.lines.length === 0) this.toast.error('Add at least one line.');
      if (requiresToStock && !this.form.getRawValue().toStockId) this.toast.error('A destination warehouse is required.');
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
      typeId: this.typeId,
      code: this.code() ?? String(codeNumber),
      codeNumber,
      date: value.date,
      dealerId: value.dealerId || null,
      stockId: value.stockId || null,
      toStockId: requiresToStock ? value.toStockId || null : null,
      total: this.linesTotal(),
      notes: value.notes || null,
      transactionProductList: value.transactionProductList.map((l, i) => ({
        id: l.id ?? undefined,
        transactionId: id,
        rowNumber: i + 1,
        productId: l.productId,
        unitId: l.unitId,
        stockId: l.stockId || value.stockId || null,
        quantity: Number(l.quantity) || 0,
        cost: Number(l.cost) || 0,
        total: Number(l.total) || 0,
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
          this.toast.success('Transaction saved.');
          this.router.navigateByUrl(this.listPath);
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
