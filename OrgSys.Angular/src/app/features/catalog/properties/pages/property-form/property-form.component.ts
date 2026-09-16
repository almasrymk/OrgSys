import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { AttributeDataType } from '../../models/property.model';
import { PropertyService } from '../../services/property.service';

@Component({
  selector: 'app-property-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './property-form.component.html',
})
export class PropertyFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly propertyService = inject(PropertyService);
  private readonly toast = inject(ToastService);
  private readonly router = inject(Router);

  readonly AttributeDataType = AttributeDataType;
  readonly dataTypes = [
    { value: AttributeDataType.Text, label: 'Text' },
    { value: AttributeDataType.Number, label: 'Number' },
    { value: AttributeDataType.Boolean, label: 'Boolean' },
    { value: AttributeDataType.Date, label: 'Date' },
    { value: AttributeDataType.Selection, label: 'Selection' },
    { value: AttributeDataType.MultiSelection, label: 'Multi selection' },
  ];

  readonly id = signal<number | null>(null);
  readonly saving = signal(false);

  readonly form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
    dataType: [AttributeDataType.Selection],
  });

  constructor(route: ActivatedRoute) {
    const idParam = route.snapshot.paramMap.get('id');
    if (idParam) {
      const id = Number(idParam);
      this.id.set(id);
      this.propertyService.getById(id).subscribe((result) => {
        const property = result.response;
        if (!property) return;
        this.form.patchValue({
          name: property.name ?? '',
          dataType: property.dataType ?? AttributeDataType.Selection,
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
    const { name, dataType } = this.form.getRawValue();
    const id = this.id() ?? undefined;
    const request$ = id
      ? this.propertyService.update({ id, name, dataType })
      : this.propertyService.create({ name, dataType });

    request$.subscribe({
      next: (result) => {
        this.saving.set(false);
        if (isApiSuccess(result)) {
          this.toast.success('Property saved.');
          this.router.navigateByUrl('/catalog/properties');
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
