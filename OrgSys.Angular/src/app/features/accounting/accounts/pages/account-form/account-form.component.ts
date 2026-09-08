import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { buildTree } from '../../../../../shared/components/tree-view/tree-node.model';
import { Account, AccountType } from '../../models/account.model';
import { AccountTypeService } from '../../services/account-type.service';
import { AccountService } from '../../services/account.service';

interface ParentOption {
  id: number;
  label: string;
}

/** Replaces Areas/Setting/Views/Account/Save.cshtml (Parent picker: flattened/indented select instead of the jQuery-UI typeahead). */
@Component({
  selector: 'app-account-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './account-form.component.html',
})
export class AccountFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly accountService = inject(AccountService);
  private readonly accountTypeService = inject(AccountTypeService);
  private readonly toast = inject(ToastService);
  private readonly router = inject(Router);

  readonly id = signal<number | null>(null);
  readonly saving = signal(false);
  readonly accountTypes = signal<AccountType[]>([]);
  readonly parentOptions = signal<ParentOption[]>([]);

  readonly form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(150)]],
    code: ['', Validators.required],
    accountTypeId: [0, [Validators.required, Validators.min(1)]],
    parentId: [0],
    debit: [0],
    credit: [0],
    isPostable: [true],
  });

  constructor(route: ActivatedRoute) {
    this.accountTypeService.getList({ pageSize: 500 }).subscribe((result) => this.accountTypes.set(result.response ?? []));

    const editId = route.snapshot.paramMap.get('id');
    const parentIdParam = route.snapshot.queryParamMap.get('parentId');

    this.accountService.getList({ pageSize: 5000 }).subscribe((result) => {
      const accounts = result.response ?? [];
      const excludeId = editId ? Number(editId) : undefined;
      this.parentOptions.set(this.flattenForPicker(accounts, excludeId));
    });

    if (editId) {
      const id = Number(editId);
      this.id.set(id);
      this.accountService.getById(id).subscribe((result) => {
        const account = result.response;
        if (!account) return;

        this.form.patchValue({
          name: account.name ?? '',
          code: account.code ?? '',
          accountTypeId: account.accountTypeId,
          parentId: account.parentId,
          debit: account.debit,
          credit: account.credit,
          isPostable: account.isPostable,
        });
      });
    } else if (parentIdParam) {
      this.form.patchValue({ parentId: Number(parentIdParam) });
    }
  }

  private flattenForPicker(accounts: Account[], excludeId?: number): ParentOption[] {
    const tree = buildTree(
      accounts,
      (a) => a.id,
      (a) => a.parentId,
      (a) => `${a.code ?? ''} ${a.name ?? ''}`.trim(),
    );

    const options: ParentOption[] = [];

    const visit = (nodes: typeof tree, depth: number) => {
      for (const node of nodes) {
        if (node.id === excludeId) continue; // an account can't be its own parent (or its own descendant)
        options.push({ id: node.id, label: `${'— '.repeat(depth)}${node.label}` });
        visit(node.children, depth + 1);
      }
    };
    visit(tree, 0);

    return options;
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving.set(true);
    const value = this.form.getRawValue();
    const id = this.id() ?? undefined;

    const request$ = id ? this.accountService.update({ id, ...value }) : this.accountService.create(value);

    request$.subscribe({
      next: (result) => {
        this.saving.set(false);
        if (isApiSuccess(result)) {
          this.toast.success('Account saved.');
          this.router.navigateByUrl('/accounting/accounts');
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
