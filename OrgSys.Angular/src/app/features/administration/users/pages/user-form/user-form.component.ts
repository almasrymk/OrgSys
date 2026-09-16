import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { Branch } from '../../../../organization/branches/models/branch.model';
import { BranchService } from '../../../../organization/branches/services/branch.service';
import { Role } from '../../../roles/models/role.model';
import { RoleService } from '../../../roles/services/role.service';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-user-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './user-form.component.html',
})
export class UserFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly userService = inject(UserService);
  private readonly roleService = inject(RoleService);
  private readonly branchService = inject(BranchService);
  private readonly toast = inject(ToastService);
  private readonly router = inject(Router);

  readonly id = signal<number | null>(null);
  readonly saving = signal(false);
  readonly roles = signal<Role[]>([]);
  readonly branches = signal<Branch[]>([]);

  readonly form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(150)]],
    userName: ['', [Validators.required, Validators.maxLength(150)]],
    password: [''],
    roleId: [0, [Validators.required, Validators.min(1)]],
    branchId: [0],
  });

  constructor(route: ActivatedRoute) {
    this.roleService.getList({ pageSize: 500 }).subscribe((r) => this.roles.set(r.response ?? []));
    this.branchService.getList({ pageSize: 500 }).subscribe((r) => this.branches.set(r.response ?? []));

    const idParam = route.snapshot.paramMap.get('id');
    if (idParam) {
      const id = Number(idParam);
      this.id.set(id);
      this.userService.getById(id).subscribe((result) => {
        const user = result.response;
        if (!user) return;
        this.form.patchValue({
          name: user.name ?? '',
          userName: user.userName ?? '',
          password: '',
          roleId: user.roleId ?? 0,
          branchId: user.branchId ?? 0,
        });
      });
    } else {
      this.form.controls.password.addValidators([Validators.required, Validators.minLength(8)]);
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
    const payload = {
      id,
      name: value.name,
      userName: value.userName,
      password: value.password || null,
      roleId: value.roleId,
      branchId: value.branchId || null,
    };

    const request$ = id ? this.userService.update(payload) : this.userService.create(payload);

    request$.subscribe({
      next: (result) => {
        this.saving.set(false);
        if (isApiSuccess(result)) {
          this.toast.success('User saved.');
          this.router.navigateByUrl('/administration/users');
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
