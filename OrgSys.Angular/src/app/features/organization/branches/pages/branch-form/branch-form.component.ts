import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { BranchService } from '../../services/branch.service';

/** Replaces Areas/Setting/Views/Branch/Save.cshtml. */
@Component({
  selector: 'app-branch-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './branch-form.component.html',
})
export class BranchFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly branchService = inject(BranchService);
  private readonly toast = inject(ToastService);
  private readonly router = inject(Router);

  readonly id = signal<number | null>(null);
  readonly saving = signal(false);

  readonly form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(150)]],
  });

  constructor(route: ActivatedRoute) {
    const idParam = route.snapshot.paramMap.get('id');
    if (idParam) {
      const id = Number(idParam);
      this.id.set(id);
      this.branchService.getById(id).subscribe((result) => {
        const branch = result.response;
        if (!branch) return;
        this.form.patchValue({ name: branch.name ?? '' });
      });
    }
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving.set(true);
    const { name } = this.form.getRawValue();
    const id = this.id() ?? undefined;

    const request$ = id ? this.branchService.update({ id, name }) : this.branchService.create({ name });

    request$.subscribe({
      next: (result) => {
        this.saving.set(false);
        if (isApiSuccess(result)) {
          this.toast.success('Branch saved.');
          this.router.navigateByUrl('/organization/branches');
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
