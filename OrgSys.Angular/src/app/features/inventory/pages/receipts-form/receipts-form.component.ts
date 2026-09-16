import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { FormArray, FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../../core/auth/auth.service';
import { isApiSuccess } from '../../../../core/models/api-result.model';
import { PageHeaderComponent } from '../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../shared/components/toast/toast.service';
import { Dealer } from '../../../customers-suppliers/dealers/models/dealer.model';
import { DealerService } from '../../../customers-suppliers/dealers/services/dealer.service';
import { Product, Stock, Unit } from '../../../invoices/models/invoice-lookups.model';
import { ProductService, StockService, UnitService } from '../../../invoices/services/invoice-lookups.service';
import { InventoryReceiptService } from '../../services/inventory-receipt.service';

/** New screen (brief §9) — Draft creation only; Post/Cancel happen from receipts-list. Mirrors
 * transactions/pages/transaction-form's FormArray line-editing pattern. */
@Component({
  selector: 'app-receipts-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './receipts-form.component.html',
})
export class ReceiptsFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);
  private readonly auth = inject(AuthService);
  private readonly toast = inject(ToastService);
  private readonly service = inject(InventoryReceiptService);
  private readonly dealerService = inject(DealerService);
  private readonly stockService = inject(StockService);
  private readonly unitService = inject(UnitService);
  private readonly productService = inject(ProductService);

  readonly saving = signal(false);

  readonly dealers = signal<Dealer[]>([]);
  readonly stocks = signal<Stock[]>([]);
  readonly units = signal<Unit[]>([]);
  readonly products = signal<Product[]>([]);

  readonly form = this.fb.nonNullable.group({
    date: [new Date().toISOString().slice(0, 10), Validators.required],
    stockId: [0, [Validators.required, Validators.min(1)]],
    dealerId: [0],
    notes: [''],
    lines: this.fb.array<ReturnType<typeof this.buildLine>>([]),
  });

  readonly lines = this.form.controls.lines;

  private readonly linesVersion = signal(0);
  readonly linesTotal = computed(() => {
    this.linesVersion();
    return this.lines.getRawValue().reduce((sum, l) => sum + (Number(l.quantity) || 0) * (Number(l.unitCost) || 0), 0);
  });

  constructor() {
    this.dealerService.getList({ pageSize: 2000 }).subscribe((r) => this.dealers.set(r.response ?? []));
    this.stockService.getList({ pageSize: 500 }).subscribe((r) => this.stocks.set(r.response ?? []));
    this.unitService.getList({ pageSize: 500 }).subscribe((r) => this.units.set(r.response ?? []));
    this.productService.getList({ pageSize: 5000 }).subscribe((r) => this.products.set(r.response ?? []));
    this.addLine();
  }

  private buildLine(productId = 0, unitId = 0, quantity = 1, unitCost = 0) {
    return this.fb.nonNullable.group({
      productId: [productId, [Validators.required, Validators.min(1)]],
      unitId: [unitId, [Validators.required, Validators.min(1)]],
      quantity: [quantity, [Validators.required, Validators.min(0.001)]],
      unitCost: [unitCost, [Validators.required, Validators.min(0)]],
      notes: [''],
    });
  }

  addLine(): void {
    this.lines.push(this.buildLine());
    this.linesVersion.update((v) => v + 1);
  }

  removeLine(index: number): void {
    this.lines.removeAt(index);
    this.linesVersion.update((v) => v + 1);
  }

  onProductChange(index: number, productId: number): void {
    const product = this.products().find((p) => p.id === productId);
    if (product) this.lines.at(index).patchValue({ unitCost: product.price ?? 0 });
    this.linesVersion.update((v) => v + 1);
  }

  recomputeLine(): void {
    this.linesVersion.update((v) => v + 1);
  }

  submit(): void {
    if (this.form.invalid || this.lines.length === 0) {
      this.form.markAllAsTouched();
      if (this.lines.length === 0) this.toast.error('Add at least one line.');
      return;
    }

    this.saving.set(true);
    const value = this.form.getRawValue();
    const currentUserId = this.auth.currentUser()?.id ?? 0;

    this.service
      .create({
        stockId: value.stockId,
        locationId: null,
        dealerId: value.dealerId || null,
        date: value.date,
        createUserId: currentUserId,
        branchId: null,
        notes: value.notes || null,
        lines: value.lines.map((l) => ({
          productId: l.productId,
          unitId: l.unitId,
          quantity: Number(l.quantity) || 0,
          unitCost: Number(l.unitCost) || 0,
          batchId: null,
          notes: l.notes || null,
        })),
      })
      .subscribe({
        next: (result) => {
          this.saving.set(false);
          if (isApiSuccess(result)) {
            this.toast.success('Receipt created as Draft.');
            this.router.navigate(['/warehouse/receipts']);
          } else {
            this.toast.error(result.errors?.[0]?.messageError ?? 'Could not create the receipt.');
          }
        },
        error: () => {
          this.saving.set(false);
          this.toast.error('Could not create the receipt.');
        },
      });
  }
}
