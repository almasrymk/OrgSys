import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { DealerType } from '../../models/dealer-group.model';
import { DealerGroupService } from '../../services/dealer-group.service';

/** Replaces Areas/Setting/Views/DealerGroup/Save.cshtml. */
@Component({
  selector: 'app-dealer-group-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './dealer-group-form.component.html',
})
export class DealerGroupFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly toast = inject(ToastService);
  private readonly service = inject(DealerGroupService);

  readonly dealerType = this.route.snapshot.data['dealerType'] as DealerType;
  readonly title = this.route.snapshot.data['title'] as string;
  readonly basePath = this.route.snapshot.data['basePath'] as string;

  readonly id = signal<number | null>(null);
  readonly code = signal<string | null>(null);
  readonly saving = signal(false);

  readonly form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(150)]],
  });

  constructor() {
    const editId = this.route.snapshot.paramMap.get('id');
    if (editId) {
      const id = Number(editId);
      this.id.set(id);
      this.service.getById(id).subscribe((result) => {
        if (result.response) {
          this.form.patchValue({ name: result.response.name ?? '' });
          this.code.set(result.response.code);
        }
      });
    } else {
      this.service.getMaxCodeNumber(this.dealerType).subscribe((max) => this.code.set(String((max || 0) + 1)));
    }
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving.set(true);
    const name = this.form.getRawValue().name;
    const id = this.id() ?? undefined;
    const codeNumber = Number(this.code() ?? 0);

    const payload = { id, name, typeId: this.dealerType, code: this.code() ?? String(codeNumber), codeNumber };
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
