import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { Country } from '../../../../master-data/countries/models/country.model';
import { CountryService } from '../../../../master-data/countries/services/country.service';
import { Currency } from '../../../../master-data/currencies/models/currency.model';
import { CurrencyService } from '../../../../master-data/currencies/services/currency.service';
import { CompanyService } from '../../services/company.service';

@Component({
  selector: 'app-company-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './company-form.component.html',
})
export class CompanyFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly companyService = inject(CompanyService);
  private readonly currencyService = inject(CurrencyService);
  private readonly countryService = inject(CountryService);
  private readonly toast = inject(ToastService);
  private readonly router = inject(Router);

  readonly id = signal<number | null>(null);
  readonly saving = signal(false);
  readonly currencies = signal<Currency[]>([]);
  readonly countries = signal<Country[]>([]);

  readonly form = this.fb.nonNullable.group({
    legalName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(150)]],
    tradeName: ['', [Validators.maxLength(150)]],
    taxRegistrationNumber: ['', [Validators.maxLength(50)]],
    commercialRegistrationNumber: ['', [Validators.maxLength(50)]],
    defaultCurrencyId: [0],
    countryId: [0],
    address: ['', [Validators.maxLength(500)]],
    phone: [''],
    email: ['', Validators.email],
    website: [''],
  });

  constructor(route: ActivatedRoute) {
    this.currencyService.getList({ pageSize: 500 }).subscribe((r) => this.currencies.set(r.response ?? []));
    this.countryService.getList({ pageSize: 500 }).subscribe((r) => this.countries.set(r.response ?? []));

    const idParam = route.snapshot.paramMap.get('id');
    if (idParam) {
      const id = Number(idParam);
      this.id.set(id);
      this.companyService.getById(id).subscribe((result) => {
        const company = result.response;
        if (!company) return;
        this.form.patchValue({
          legalName: company.legalName ?? '',
          tradeName: company.tradeName ?? '',
          taxRegistrationNumber: company.taxRegistrationNumber ?? '',
          commercialRegistrationNumber: company.commercialRegistrationNumber ?? '',
          defaultCurrencyId: company.defaultCurrencyId ?? 0,
          countryId: company.countryId ?? 0,
          address: company.address ?? '',
          phone: company.phone ?? '',
          email: company.email ?? '',
          website: company.website ?? '',
        });
      });
    }
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
      legalName: value.legalName,
      tradeName: value.tradeName || null,
      taxRegistrationNumber: value.taxRegistrationNumber || null,
      commercialRegistrationNumber: value.commercialRegistrationNumber || null,
      defaultCurrencyId: value.defaultCurrencyId || null,
      countryId: value.countryId || null,
      address: value.address || null,
      phone: value.phone || null,
      email: value.email || null,
      website: value.website || null,
    };

    const request$ = id ? this.companyService.update(payload) : this.companyService.create(payload);

    request$.subscribe({
      next: (result) => {
        this.saving.set(false);
        if (isApiSuccess(result)) {
          this.toast.success('Company saved.');
          this.router.navigateByUrl('/organization/companies');
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
