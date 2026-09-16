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
import { Company } from '../../../companies/models/company.model';
import { CompanyService } from '../../../companies/services/company.service';
import { OrganizationSettingsService } from '../../services/organization-settings.service';

@Component({
  selector: 'app-organization-settings-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './organization-settings-form.component.html',
})
export class OrganizationSettingsFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly settingsService = inject(OrganizationSettingsService);
  private readonly companyService = inject(CompanyService);
  private readonly currencyService = inject(CurrencyService);
  private readonly countryService = inject(CountryService);
  private readonly toast = inject(ToastService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  readonly saving = signal(false);
  readonly companies = signal<Company[]>([]);
  readonly currencies = signal<Currency[]>([]);
  readonly countries = signal<Country[]>([]);

  readonly form = this.fb.nonNullable.group({
    companyId: [0, [Validators.required, Validators.min(1)]],
    defaultCurrencyId: [0],
    defaultCountryId: [0],
    defaultTimeZone: [''],
    fiscalYearStartMonth: [1, [Validators.min(1), Validators.max(12)]],
    fiscalYearStartDay: [1, [Validators.min(1), Validators.max(31)]],
  });

  constructor() {
    this.companyService.getList({ pageSize: 500 }).subscribe((r) => this.companies.set(r.response ?? []));
    this.currencyService.getList({ pageSize: 500 }).subscribe((r) => this.currencies.set(r.response ?? []));
    this.countryService.getList({ pageSize: 500 }).subscribe((r) => this.countries.set(r.response ?? []));

    const companyId = Number(this.route.snapshot.queryParamMap.get('companyId') ?? 0);
    if (companyId) {
      this.form.controls.companyId.setValue(companyId);
      this.loadSettings(companyId);
    }

    this.form.controls.companyId.valueChanges.subscribe((id) => {
      if (id) this.loadSettings(id);
    });
  }

  private loadSettings(companyId: number): void {
    this.settingsService.getByCompanyId(companyId).subscribe((result) => {
      const settings = result.response;
      if (!settings) return;
      this.form.patchValue({
        companyId: settings.companyId,
        defaultCurrencyId: settings.defaultCurrencyId ?? 0,
        defaultCountryId: settings.defaultCountryId ?? 0,
        defaultTimeZone: settings.defaultTimeZone ?? '',
        fiscalYearStartMonth: settings.fiscalYearStartMonth ?? 1,
        fiscalYearStartDay: settings.fiscalYearStartDay ?? 1,
      });
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving.set(true);
    const value = this.form.getRawValue();
    this.settingsService
      .update({
        companyId: value.companyId,
        defaultCurrencyId: value.defaultCurrencyId || null,
        defaultCountryId: value.defaultCountryId || null,
        defaultTimeZone: value.defaultTimeZone || null,
        fiscalYearStartMonth: value.fiscalYearStartMonth,
        fiscalYearStartDay: value.fiscalYearStartDay,
      })
      .subscribe({
        next: (result) => {
          this.saving.set(false);
          if (isApiSuccess(result)) {
            this.toast.success('Settings saved.');
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
