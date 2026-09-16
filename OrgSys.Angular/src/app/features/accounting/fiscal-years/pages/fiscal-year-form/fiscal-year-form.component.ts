import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { FiscalYearStatus } from '../../models/fiscal-year.model';
import { FiscalYearService } from '../../services/fiscal-year.service';

/** Mirrors CreateFiscalYearCommandValidator's `EndDate must be after StartDate` rule for UX only — the API re-validates. */
function dateRangeValidator(control: AbstractControl): ValidationErrors | null {
  const start = control.get('startDate')?.value;
  const end = control.get('endDate')?.value;
  return start && end && end <= start ? { dateRange: true } : null;
}

/** Replaces Areas/Setting/Views/FiscalYear/Save.cshtml (period editing intentionally deferred, see fiscal-year.model.ts). */
@Component({
  selector: 'app-fiscal-year-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './fiscal-year-form.component.html',
})
export class FiscalYearFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly fiscalYearService = inject(FiscalYearService);
  private readonly toast = inject(ToastService);
  private readonly router = inject(Router);

  readonly id = signal<number | null>(null);
  readonly saving = signal(false);
  readonly FiscalYearStatus = FiscalYearStatus;

  readonly form = this.fb.nonNullable.group(
    {
      name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(150)]],
      startDate: ['', Validators.required],
      endDate: ['', Validators.required],
      isCurrent: [false],
      fiscalYearStatus: [FiscalYearStatus.Open, Validators.required],
    },
    { validators: [dateRangeValidator] },
  );

  constructor(route: ActivatedRoute) {
    const idParam = route.snapshot.paramMap.get('id');
    if (idParam) {
      const id = Number(idParam);
      this.id.set(id);
      this.fiscalYearService.getById(id).subscribe((result) => {
        const fy = result.response;
        if (!fy) return;

        this.form.patchValue({
          name: fy.name ?? '',
          startDate: fy.startDate?.slice(0, 10) ?? '',
          endDate: fy.endDate?.slice(0, 10) ?? '',
          isCurrent: fy.isCurrent,
          fiscalYearStatus: fy.fiscalYearStatus,
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

    const request$ = id ? this.fiscalYearService.update({ id, ...value }) : this.fiscalYearService.create(value);

    request$.subscribe({
      next: (result) => {
        this.saving.set(false);
        if (isApiSuccess(result)) {
          this.toast.success('Fiscal year saved.');
          this.router.navigateByUrl('/administration/fiscal-years');
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
