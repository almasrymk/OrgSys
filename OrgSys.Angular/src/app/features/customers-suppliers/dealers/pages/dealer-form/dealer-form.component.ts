import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { Account } from '../../../../accounting/accounts/models/account.model';
import { AccountService } from '../../../../accounting/accounts/services/account.service';
import { City } from '../../../../administration/cities/models/city.model';
import { CityService } from '../../../../administration/cities/services/city.service';
import { Country } from '../../../../administration/countries/models/country.model';
import { CountryService } from '../../../../administration/countries/services/country.service';
import { District } from '../../../../administration/districts/models/district.model';
import { DistrictService } from '../../../../administration/districts/services/district.service';
import { DealerGroup } from '../../../dealer-groups/models/dealer-group.model';
import { DealerGroupService } from '../../../dealer-groups/services/dealer-group.service';
import { DealerType } from '../../models/dealer.model';
import { DealerService } from '../../services/dealer.service';

/** Replaces Areas/Setting/Views/Dealer/Save.cshtml. */
@Component({
  selector: 'app-dealer-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './dealer-form.component.html',
})
export class DealerFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly toast = inject(ToastService);
  private readonly service = inject(DealerService);
  private readonly dealerGroupService = inject(DealerGroupService);
  private readonly countryService = inject(CountryService);
  private readonly cityService = inject(CityService);
  private readonly districtService = inject(DistrictService);
  private readonly accountService = inject(AccountService);

  readonly dealerType = this.route.snapshot.data['dealerType'] as DealerType;
  readonly title = this.route.snapshot.data['title'] as string;
  readonly basePath = this.route.snapshot.data['basePath'] as string;

  readonly id = signal<number | null>(null);
  readonly code = signal<string | null>(null);
  readonly saving = signal(false);

  readonly dealerGroups = signal<DealerGroup[]>([]);
  readonly countries = signal<Country[]>([]);
  readonly cities = signal<City[]>([]);
  readonly districts = signal<District[]>([]);
  readonly glAccounts = signal<Account[]>([]);
  readonly loadingCities = signal(false);
  readonly loadingDistricts = signal(false);

  readonly form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(150)]],
    dealerGroupId: [0, [Validators.required, Validators.min(1)]],
    accountId: [0, [Validators.required, Validators.min(1)]],
    phone: [''],
    email: ['', Validators.email],
    address: [''],
    countryId: [0],
    cityId: [0],
    districtId: [0],
  });

  constructor() {
    this.dealerGroupService.getList({ typeId: this.dealerType, pageSize: 500 }).subscribe((r) => this.dealerGroups.set(r.response ?? []));
    this.countryService.getList({ pageSize: 500 }).subscribe((r) => this.countries.set(r.response ?? []));
    this.accountService.getList({ pageSize: 5000 }).subscribe((r) => this.glAccounts.set((r.response ?? []).filter((a) => a.isPostable)));

    this.form.controls.countryId.valueChanges.subscribe((countryId) => {
      this.form.controls.cityId.setValue(0);
      this.districts.set([]);
      this.loadCitiesForCountry(countryId);
    });
    this.form.controls.cityId.valueChanges.subscribe((cityId) => {
      this.form.controls.districtId.setValue(0);
      this.loadDistrictsForCity(cityId);
    });

    const editId = this.route.snapshot.paramMap.get('id');
    if (editId) {
      const id = Number(editId);
      this.id.set(id);
      this.service.getById(id).subscribe((result) => {
        const dealer = result.response;
        if (!dealer) return;

        this.code.set(dealer.code);
        if (dealer.countryId) this.loadCitiesForCountry(dealer.countryId);
        if (dealer.cityId) this.loadDistrictsForCity(dealer.cityId);

        this.form.patchValue({
          name: dealer.name ?? '',
          dealerGroupId: dealer.dealerGroupId ?? 0,
          accountId: dealer.accountId ?? 0,
          phone: dealer.phone ?? '',
          email: dealer.email ?? '',
          address: dealer.address ?? '',
          countryId: dealer.countryId ?? 0,
          cityId: dealer.cityId ?? 0,
          districtId: dealer.districtId ?? 0,
        });
      });
    } else {
      this.service.getMaxCodeNumber(this.dealerType).subscribe((max) => this.code.set(String((max || 0) + 1)));
    }
  }

  private loadCitiesForCountry(countryId: number): void {
    if (!countryId) {
      this.cities.set([]);
      return;
    }
    this.loadingCities.set(true);
    this.cityService.getByCountry(countryId).subscribe({
      next: (r) => {
        this.loadingCities.set(false);
        this.cities.set(r.response ?? []);
      },
      error: () => {
        this.loadingCities.set(false);
        this.cities.set([]);
      },
    });
  }

  private loadDistrictsForCity(cityId: number): void {
    if (!cityId) {
      this.districts.set([]);
      return;
    }
    this.loadingDistricts.set(true);
    this.districtService.getByCity(cityId).subscribe({
      next: (r) => {
        this.loadingDistricts.set(false);
        this.districts.set(r.response ?? []);
      },
      error: () => {
        this.loadingDistricts.set(false);
        this.districts.set([]);
      },
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving.set(true);
    const value = this.form.getRawValue();
    const id = this.id() ?? undefined;
    const codeNumber = Number(this.code() ?? 0);

    const payload = {
      id,
      name: value.name,
      typeId: this.dealerType,
      code: this.code() ?? String(codeNumber),
      codeNumber,
      dealerGroupId: value.dealerGroupId,
      accountId: value.accountId,
      phone: value.phone || null,
      email: value.email || null,
      address: value.address || null,
      countryId: value.countryId || null,
      cityId: value.cityId || null,
      districtId: value.districtId || null,
    };

    const request$ = id ? this.service.update(payload) : this.service.create(payload);

    request$.subscribe({
      next: (result) => {
        this.saving.set(false);
        if (isApiSuccess(result)) {
          this.toast.success('Saved.');
          this.router.navigateByUrl(this.basePath);
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
