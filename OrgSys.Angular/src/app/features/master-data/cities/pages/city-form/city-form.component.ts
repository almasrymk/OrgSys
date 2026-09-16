import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { Country } from '../../../countries/models/country.model';
import { CountryService } from '../../../countries/services/country.service';
import { CityService } from '../../services/city.service';

/** Replaces Areas/Setting/Views/City/Save.cshtml. */
@Component({
  selector: 'app-city-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './city-form.component.html',
})
export class CityFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly cityService = inject(CityService);
  private readonly countryService = inject(CountryService);
  private readonly toast = inject(ToastService);
  private readonly router = inject(Router);

  readonly id = signal<number | null>(null);
  readonly saving = signal(false);
  readonly countries = signal<Country[]>([]);

  readonly form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(150)]],
    countryId: [0, [Validators.required, Validators.min(1)]],
  });

  constructor(route: ActivatedRoute) {
    this.countryService.getList({ pageSize: 500 }).subscribe((result) => this.countries.set(result.response ?? []));

    const idParam = route.snapshot.paramMap.get('id');
    if (idParam) {
      const id = Number(idParam);
      this.id.set(id);
      this.cityService.getById(id).subscribe((result) => {
        if (result.response) {
          this.form.patchValue({
            name: result.response.name ?? '',
            countryId: result.response.countryId ?? 0,
          });
        }
      });
    }
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving.set(true);
    const { name, countryId } = this.form.getRawValue();
    const id = this.id();

    const request$ = id ? this.cityService.update({ id, name, countryId }) : this.cityService.create({ name, countryId });

    request$.subscribe({
      next: (result) => {
        this.saving.set(false);
        if (isApiSuccess(result)) {
          this.toast.success('City saved.');
          this.router.navigateByUrl('/master-data/cities');
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
