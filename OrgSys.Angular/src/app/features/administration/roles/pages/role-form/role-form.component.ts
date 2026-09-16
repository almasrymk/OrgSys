import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { PermissionTreeNode } from '../../models/role.model';
import { RoleService } from '../../services/role.service';

@Component({
  selector: 'app-role-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './role-form.component.html',
})
export class RoleFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly roleService = inject(RoleService);
  private readonly toast = inject(ToastService);
  private readonly router = inject(Router);

  readonly id = signal<number | null>(null);
  readonly saving = signal(false);
  readonly permissions = signal<PermissionTreeNode[]>([]);

  readonly form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
  });

  constructor(route: ActivatedRoute) {
    const idParam = route.snapshot.paramMap.get('id');
    const id = idParam ? Number(idParam) : 0;
    if (idParam) this.id.set(id);

    this.roleService.getById(id || 0).subscribe((result) => {
      const role = result.response;
      if (!role) return;
      if (id) this.form.patchValue({ name: role.name ?? '' });
      this.permissions.set(role.permissionsTree ?? []);
    });
  }

  toggle(node: PermissionTreeNode, checked: boolean): void {
    this.permissions.update((list) => list.map((item) => (item.id === node.id ? { ...item, select: checked } : item)));
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving.set(true);
    const { name } = this.form.getRawValue();
    const id = this.id() ?? undefined;
    const permissionList = this.permissions()
      .filter((p) => p.select)
      .map((p) => ({ permissionId: p.id }));

    const request$ = id
      ? this.roleService.update({ id, name, permissionList })
      : this.roleService.create({ name, permissionList });

    request$.subscribe({
      next: (result) => {
        this.saving.set(false);
        if (isApiSuccess(result)) {
          this.toast.success('Role saved.');
          this.router.navigateByUrl('/administration/roles');
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
