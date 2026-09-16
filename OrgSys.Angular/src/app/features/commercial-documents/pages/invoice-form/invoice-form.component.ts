import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { FormArray, FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../../core/auth/auth.service';
import { isApiSuccess } from '../../../../core/models/api-result.model';
import { PageHeaderComponent } from '../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../shared/components/toast/toast.service';
import { Currency } from '../../../master-data/currencies/models/currency.model';
import { CurrencyService } from '../../../master-data/currencies/services/currency.service';
import { Dealer, DealerType } from '../../../parties';
import { DealerService } from '../../../parties';
import { commercialDocumentPath, InvoiceType } from '../../models/invoice-lookups.model';
import { InvoiceTypeService } from '../../services/invoice-type.service';
import { PaymentType } from '../../../master-data';
import { PaymentTypeService } from '../../../master-data';
import { Product, ProductService, Unit, UnitService } from '../../../catalog';
import { Stock, StockService } from '../../../inventory';
import { InvoiceService } from '../../services/invoice.service';

/**
 * Replaces Areas/Invoices/Views/Invoice/{Save,Products}.cshtml. One form, parameterized by
 * InvoiceType (route :typeId) — same pattern as Journal/Financial. Dealer picker switches between
 * Customers and Suppliers based on the type's Group ("Sales" -> Client, "Purchases" -> Supplier).
 * Discount/Tax/Service are flat amounts only (percentage mode, DiscountType=1 etc. on the entity,
 * is not exposed — see invoice.model.ts). Per-product unit filtering is not modeled either (Product
 * has its own multi-unit complexity not built this pass) — every line offers the full Unit list.
 */
@Component({
  selector: 'app-invoice-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './invoice-form.component.html',
})
export class InvoiceFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly auth = inject(AuthService);
  private readonly toast = inject(ToastService);
  private readonly service = inject(InvoiceService);
  private readonly invoiceTypeService = inject(InvoiceTypeService);
  private readonly dealerService = inject(DealerService);
  private readonly paymentTypeService = inject(PaymentTypeService);
  private readonly stockService = inject(StockService);
  private readonly unitService = inject(UnitService);
  private readonly productService = inject(ProductService);
  private readonly currencyService = inject(CurrencyService);

  readonly typeId = Number(this.route.snapshot.data['typeId'] ?? this.route.snapshot.paramMap.get('typeId'));
  readonly listPath = commercialDocumentPath(this.typeId);
  readonly invoiceType = signal<InvoiceType | null>(null);
  readonly id = signal<number | null>(null);
  readonly code = signal<string | null>(null);
  readonly saving = signal(false);

  readonly dealers = signal<Dealer[]>([]);
  readonly paymentTypes = signal<PaymentType[]>([]);
  readonly stocks = signal<Stock[]>([]);
  readonly units = signal<Unit[]>([]);
  readonly products = signal<Product[]>([]);
  readonly currencies = signal<Currency[]>([]);

  private originalCreateUserId: number | null = null;
  private originalCreateDate: string | null = null;

  readonly form = this.fb.nonNullable.group({
    date: [new Date().toISOString().slice(0, 10), Validators.required],
    dealerId: [0, [Validators.required, Validators.min(1)]],
    paymentTypeId: [0, [Validators.required, Validators.min(1)]],
    stockId: [0, [Validators.required, Validators.min(1)]],
    currencyId: [0, [Validators.required, Validators.min(1)]],
    rate: [1, [Validators.required, Validators.min(0.0001)]],
    discount: [0],
    tax: [0],
    service: [0],
    notes: [''],
    invoiceProductList: this.fb.array<ReturnType<typeof this.buildLine>>([]),
  });

  readonly lines = this.form.controls.invoiceProductList;

  private readonly linesVersion = signal(0);
  readonly linesTotal = computed(() => {
    this.linesVersion();
    return this.lines.getRawValue().reduce((sum, l) => sum + (Number(l.net) || 0), 0);
  });

  private readonly headerAdjustments = signal({ discount: 0, tax: 0, service: 0 });
  readonly grandTotal = computed(() => {
    const adj = this.headerAdjustments();
    return this.linesTotal() - adj.discount + adj.tax + adj.service;
  });

  constructor() {
    this.invoiceTypeService.getById(this.typeId).subscribe((r) => this.invoiceType.set(r.response));

    const dealerType = this.typeId === 2 || this.typeId === 4 ? DealerType.Supplier : DealerType.Client;
    this.dealerService.getList({ typeId: dealerType, pageSize: 2000 }).subscribe((r) => this.dealers.set(r.response ?? []));

    this.paymentTypeService.getList({ pageSize: 100 }).subscribe((r) => this.paymentTypes.set(r.response ?? []));
    this.stockService.getList({ pageSize: 500 }).subscribe((r) => this.stocks.set(r.response ?? []));
    this.unitService.getList({ pageSize: 500 }).subscribe((r) => this.units.set(r.response ?? []));
    this.productService.getList({ pageSize: 5000 }).subscribe((r) => this.products.set(r.response ?? []));
    this.currencyService.getList({ pageSize: 500 }).subscribe((r) => this.currencies.set(r.response ?? []));

    this.form.valueChanges.subscribe(() => {
      const v = this.form.getRawValue();
      this.headerAdjustments.set({ discount: Number(v.discount) || 0, tax: Number(v.tax) || 0, service: Number(v.service) || 0 });
    });

    const editId = this.route.snapshot.paramMap.get('id');
    if (editId) {
      const id = Number(editId);
      this.id.set(id);
      this.service.getById(id).subscribe((result) => {
        const invoice = result.response;
        if (!invoice) return;

        this.code.set(invoice.code);
        this.originalCreateUserId = invoice.createUserId;
        this.originalCreateDate = invoice.createDate;

        this.form.patchValue({
          date: invoice.date?.slice(0, 10) ?? '',
          dealerId: invoice.dealerId,
          paymentTypeId: invoice.paymentTypeId,
          stockId: invoice.stockId ?? 0,
          currencyId: invoice.currencyId,
          rate: invoice.rate,
          discount: invoice.discount,
          tax: invoice.tax,
          service: invoice.service,
          notes: invoice.notes ?? '',
        });

        for (const line of invoice.invoiceProductList ?? []) {
          this.lines.push(
            this.buildLine(line.id, line.rowNumber, line.productId, line.unitId, line.stockId, line.quantity, line.price, line.discount, line.tax, line.service, line.notes),
          );
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
    price = 0,
    discount = 0,
    tax = 0,
    service = 0,
    notes: string | null = '',
  ) {
    return this.fb.nonNullable.group({
      id: [id],
      rowNumber: [rowNumber],
      productId: [productId, [Validators.required, Validators.min(1)]],
      unitId: [unitId, [Validators.required, Validators.min(1)]],
      stockId: [stockId],
      quantity: [quantity, [Validators.required, Validators.min(0.0001)]],
      price: [price, [Validators.required, Validators.min(0)]],
      discount: [discount],
      tax: [tax],
      service: [service],
      total: [quantity * price],
      net: [quantity * price - discount + tax + service],
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
    if (product) this.lines.at(index).patchValue({ price: product.price });
    this.recomputeLine(index);
  }

  recomputeLine(index: number): void {
    const line = this.lines.at(index);
    const v = line.getRawValue();
    const total = (Number(v.quantity) || 0) * (Number(v.price) || 0);
    const net = total - (Number(v.discount) || 0) + (Number(v.tax) || 0) + (Number(v.service) || 0);
    line.patchValue({ total, net }, { emitEvent: false });
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
      dealerId: value.dealerId,
      paymentTypeId: value.paymentTypeId,
      stockId: value.stockId || null,
      currencyId: value.currencyId,
      rate: value.rate,
      discount: Number(value.discount) || 0,
      discountType: 0 as const,
      tax: Number(value.tax) || 0,
      taxType: 0 as const,
      service: Number(value.service) || 0,
      serviceType: 0 as const,
      net: this.grandTotal(),
      netByDefaultCurrency: this.grandTotal() * (Number(value.rate) || 1),
      notes: value.notes || null,
      invoiceProductList: value.invoiceProductList.map((l, i) => ({
        id: l.id ?? undefined,
        invoiceId: id,
        rowNumber: i + 1,
        productId: l.productId,
        unitId: l.unitId,
        stockId: l.stockId || value.stockId || null,
        quantity: Number(l.quantity) || 0,
        price: Number(l.price) || 0,
        total: Number(l.total) || 0,
        discount: Number(l.discount) || 0,
        tax: Number(l.tax) || 0,
        service: Number(l.service) || 0,
        net: Number(l.net) || 0,
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
          this.toast.success('Invoice saved.');
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
