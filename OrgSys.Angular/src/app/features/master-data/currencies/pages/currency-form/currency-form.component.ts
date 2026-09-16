import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { CurrencyService } from '../../services/currency.service';

/** Replaces Areas/Setting/Views/Currency/Save.cshtml. */
@Component({
  selector: 'app-currency-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './currency-form.component.html',
})
export class CurrencyFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly currencyService = inject(CurrencyService);
  private readonly toast = inject(ToastService);
  private readonly router = inject(Router);

  readonly id = signal<number | null>(null);
  readonly saving = signal(false);

  readonly form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(150)]],
    rate: [1, [Validators.required, Validators.min(0)]],
    isDefault: [false],
  });

  constructor(route: ActivatedRoute) {
    const idParam = route.snapshot.paramMap.get('id');
    if (idParam) {
      const id = Number(idParam);
      this.id.set(id);
      this.currencyService.getById(id).subscribe((result) => {
        if (result.response) {
          this.form.patchValue({
            name: result.response.name ?? '',
            rate: result.response.rate,
            isDefault: result.response.isDefault,
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
    const { name, rate, isDefault } = this.form.getRawValue();
    const id = this.id() ?? undefined;

    const request$ = id
      ? this.currencyService.update({ id, name, rate, isDefault })
      : this.currencyService.create({ name, rate, isDefault });

    request$.subscribe({
      next: (result) => {
        this.saving.set(false);
        if (isApiSuccess(result)) {
          this.toast.success('Currency saved.');
          this.router.navigateByUrl('/administration/currencies');
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
