import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { City } from '../../../cities/models/city.model';
import { CityService } from '../../../cities/services/city.service';
import { Country } from '../../../countries/models/country.model';
import { CountryService } from '../../../countries/services/country.service';
import { DistrictService } from '../../services/district.service';

/** Replaces Areas/Setting/Views/District/Save.cshtml — cascading Country -> City pickers. */
@Component({
  selector: 'app-district-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './district-form.component.html',
})
export class DistrictFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly districtService = inject(DistrictService);
  private readonly countryService = inject(CountryService);
  private readonly cityService = inject(CityService);
  private readonly toast = inject(ToastService);
  private readonly router = inject(Router);

  readonly id = signal<number | null>(null);
  readonly saving = signal(false);
  readonly countries = signal<Country[]>([]);
  readonly cities = signal<City[]>([]);
  readonly loadingCities = signal(false);

  readonly form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(150)]],
    countryId: [0, [Validators.required, Validators.min(1)]],
    cityId: [0, [Validators.required, Validators.min(1)]],
  });

  constructor(route: ActivatedRoute) {
    this.countryService.getList({ pageSize: 500 }).subscribe((result) => this.countries.set(result.response ?? []));

    this.form.controls.countryId.valueChanges.subscribe((countryId) => {
      this.form.controls.cityId.setValue(0);
      this.loadCitiesForCountry(countryId);
    });

    const idParam = route.snapshot.paramMap.get('id');
    if (idParam) {
      const id = Number(idParam);
      this.id.set(id);
      this.districtService.getById(id).subscribe((result) => {
        const district = result.response;
        if (!district) return;

        if (district.countryId) this.loadCitiesForCountry(district.countryId);

        this.form.patchValue({
          name: district.name ?? '',
          countryId: district.countryId ?? 0,
          cityId: district.cityId ?? 0,
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
      next: (result) => {
        this.loadingCities.set(false);
        this.cities.set(result.response ?? []);
      },
      error: () => {
        this.loadingCities.set(false);
        this.cities.set([]);
      },
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving.set(true);
    const { name, countryId, cityId } = this.form.getRawValue();
    const id = this.id() ?? undefined;

    const request$ = id
      ? this.districtService.update({ id, name, countryId, cityId })
      : this.districtService.create({ name, countryId, cityId });

    request$.subscribe({
      next: (result) => {
        this.saving.set(false);
        if (isApiSuccess(result)) {
          this.toast.success('District saved.');
          this.router.navigateByUrl('/master-data/districts');
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
