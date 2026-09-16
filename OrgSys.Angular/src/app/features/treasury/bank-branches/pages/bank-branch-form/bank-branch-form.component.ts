import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { City } from '../../../../master-data/cities/models/city.model';
import { CityService } from '../../../../master-data/cities/services/city.service';
import { Country } from '../../../../master-data/countries/models/country.model';
import { CountryService } from '../../../../master-data/countries/services/country.service';
import { District } from '../../../../master-data/districts/models/district.model';
import { DistrictService } from '../../../../master-data/districts/services/district.service';
import { Bank } from '../../../banks/models/bank.model';
import { BankService } from '../../../banks/services/bank.service';
import { BankBranchService } from '../../services/bank-branch.service';

/** Replaces Areas/Setting/Views/BankBranch/Save.cshtml — cascading Country -> City -> District pickers, same shape as Dealer's. */
@Component({
  selector: 'app-bank-branch-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './bank-branch-form.component.html',
})
export class BankBranchFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly bankBranchService = inject(BankBranchService);
  private readonly bankService = inject(BankService);
  private readonly countryService = inject(CountryService);
  private readonly cityService = inject(CityService);
  private readonly districtService = inject(DistrictService);
  private readonly toast = inject(ToastService);
  private readonly router = inject(Router);

  readonly id = signal<number | null>(null);
  readonly saving = signal(false);
  readonly banks = signal<Bank[]>([]);
  readonly countries = signal<Country[]>([]);
  readonly cities = signal<City[]>([]);
  readonly districts = signal<District[]>([]);
  readonly loadingCities = signal(false);
  readonly loadingDistricts = signal(false);

  readonly form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(150)]],
    bankId: [0, [Validators.required, Validators.min(1)]],
    countryId: [0, [Validators.required, Validators.min(1)]],
    cityId: [0, [Validators.required, Validators.min(1)]],
    districtId: [0, [Validators.required, Validators.min(1)]],
  });

  constructor(route: ActivatedRoute) {
    this.bankService.getList({ pageSize: 500 }).subscribe((r) => this.banks.set(r.response ?? []));
    this.countryService.getList({ pageSize: 500 }).subscribe((r) => this.countries.set(r.response ?? []));

    this.form.controls.countryId.valueChanges.subscribe((countryId) => {
      this.form.controls.cityId.setValue(0);
      this.districts.set([]);
      this.loadCitiesForCountry(countryId);
    });
    this.form.controls.cityId.valueChanges.subscribe((cityId) => {
      this.form.controls.districtId.setValue(0);
      this.loadDistrictsForCity(cityId);
    });

    const idParam = route.snapshot.paramMap.get('id');
    if (idParam) {
      const id = Number(idParam);
      this.id.set(id);
      this.bankBranchService.getById(id).subscribe((result) => {
        const bankBranch = result.response;
        if (!bankBranch) return;

        if (bankBranch.countryId) this.loadCitiesForCountry(bankBranch.countryId);
        if (bankBranch.cityId) this.loadDistrictsForCity(bankBranch.cityId);

        this.form.patchValue({
          name: bankBranch.name ?? '',
          bankId: bankBranch.bankId ?? 0,
          countryId: bankBranch.countryId ?? 0,
          cityId: bankBranch.cityId ?? 0,
          districtId: bankBranch.districtId ?? 0,
        });
      });
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
    const { name, bankId, countryId, cityId, districtId } = this.form.getRawValue();
    const id = this.id() ?? undefined;
    const payload = { id, name, bankId, countryId, cityId, districtId };

    const request$ = id ? this.bankBranchService.update(payload) : this.bankBranchService.create(payload);

    request$.subscribe({
      next: (result) => {
        this.saving.set(false);
        if (isApiSuccess(result)) {
          this.toast.success('Bank branch saved.');
          this.router.navigateByUrl('/treasury/bank-branches');
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
