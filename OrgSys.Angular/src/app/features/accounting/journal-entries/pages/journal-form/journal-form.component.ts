import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { FormArray, FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../../../core/auth/auth.service';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { PageHeaderComponent } from '../../../../../shared/components/page-header/page-header.component';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { Status } from '../../../../../shared/models/status.enum';
import { Account } from '../../../accounts/models/account.model';
import { AccountService } from '../../../accounts/services/account.service';
import { Currency } from '../../../../master-data/currencies/models/currency.model';
import { CurrencyService } from '../../../../master-data/currencies/services/currency.service';
import { JournalType } from '../../models/journal.model';
import { JournalService } from '../../services/journal.service';
import { JournalTypeService } from '../../services/journal-type.service';

/**
 * Replaces Areas/Financials/Views/Journal/{Save,Items}.cshtml. Debit/Credit balance is checked
 * here only for UX (a Draft may be saved unbalanced, matching the API — PostJournalCommandHandler
 * is the actual enforcement point). Currency default from Preferences is intentionally not wired
 * here — Preferences is its own not-yet-migrated feature (see migration inventory §6); pick a
 * currency manually for now.
 */
@Component({
  selector: 'app-journal-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, PageHeaderComponent],
  templateUrl: './journal-form.component.html',
})
export class JournalFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly journalService = inject(JournalService);
  private readonly journalTypeService = inject(JournalTypeService);
  private readonly currencyService = inject(CurrencyService);
  private readonly accountService = inject(AccountService);
  private readonly toast = inject(ToastService);
  private readonly router = inject(Router);
  private readonly auth = inject(AuthService);

  readonly Status = Status;

  readonly id = signal<number | null>(null);
  readonly code = signal<string | null>(null);
  readonly saving = signal(false);
  readonly readOnly = signal(false);
  readonly journal = signal<{ posted: boolean; status: Status; refranceTable: string | null } | null>(null);

  /** Set once, on load — see the audit-field note on SaveJournalRequest. */
  private originalCreateUserId: number | null = null;
  private originalCreateDate: string | null = null;

  readonly journalTypes = signal<JournalType[]>([]);
  readonly currencies = signal<Currency[]>([]);
  readonly accounts = signal<Account[]>([]);

  readonly form = this.fb.nonNullable.group({
    date: [new Date().toISOString().slice(0, 10), Validators.required],
    journalTypeId: [0, [Validators.required, Validators.min(1)]],
    currencyId: [0, [Validators.required, Validators.min(1)]],
    rate: [1, [Validators.required, Validators.min(0)]],
    note: [''],
    journalItems: this.fb.array<ReturnType<typeof this.buildLine>>([]),
  });

  readonly lines = this.form.controls.journalItems;

  readonly totalDebit = computed(() => this.lineTotals().debit);
  readonly totalCredit = computed(() => this.lineTotals().credit);
  readonly isBalanced = computed(() => this.totalDebit() === this.totalCredit() && this.totalDebit() > 0);

  private readonly linesVersion = signal(0);
  private readonly lineTotals = computed(() => {
    this.linesVersion(); // touched on every line change to keep this computed reactive
    const values = this.lines.getRawValue();
    return {
      debit: values.reduce((sum, l) => sum + (Number(l.debit) || 0), 0),
      credit: values.reduce((sum, l) => sum + (Number(l.credit) || 0), 0),
    };
  });

  constructor(route: ActivatedRoute) {
    this.journalTypeService.getList({ pageSize: 100 }).subscribe((r) => this.journalTypes.set(r.response ?? []));
    this.currencyService.getList({ pageSize: 500 }).subscribe((r) => this.currencies.set(r.response ?? []));
    this.accountService.getList({ pageSize: 5000 }).subscribe((r) => this.accounts.set((r.response ?? []).filter((a) => a.isPostable)));

    const editId = route.snapshot.paramMap.get('id');
    if (editId) {
      const id = Number(editId);
      this.id.set(id);
      this.journalService.getById(id).subscribe((result) => {
        const j = result.response;
        if (!j) return;

        this.code.set(j.code);
        this.journal.set({ posted: j.posted, status: j.status, refranceTable: j.refranceTable });
        this.originalCreateUserId = j.createUserId;
        this.originalCreateDate = j.createDate;
        this.readOnly.set(!!j.refranceTable || j.posted || j.status === Status.Reversed || j.status === Status.Cancel);

        this.form.patchValue({
          date: j.date?.slice(0, 10) ?? '',
          journalTypeId: j.journalTypeId,
          currencyId: j.currencyId,
          rate: j.rate,
          note: j.note ?? '',
        });

        for (const item of j.journalItems ?? []) {
          this.lines.push(this.buildLine(item.accountId, item.debit, item.credit, item.note, item.id));
        }
        this.linesVersion.update((v) => v + 1);

        if (this.readOnly()) this.form.disable();
      });
    } else {
      this.journalService.getMaxCodeNumber().subscribe((max) => this.code.set(String((max || 0) + 1)));
      this.addLine();
    }
  }

  private buildLine(accountId = 0, debit = 0, credit = 0, note: string | null = '', id?: number) {
    return this.fb.nonNullable.group({
      id: [id],
      accountId: [accountId, [Validators.required, Validators.min(1)]],
      debit: [debit],
      credit: [credit],
      note: [note ?? ''],
    });
  }

  addLine(): void {
    this.lines.push(this.buildLine());
    this.linesVersion.update((v) => v + 1);
  }

  removeLine(index: number): void {
    this.lines.removeAt(index);
    this.linesVersion.update((v) => v + 1);
  }

  onLineAmountChange(): void {
    this.linesVersion.update((v) => v + 1);
  }

  onCurrencyChange(currencyId: number): void {
    const currency = this.currencies().find((c) => c.id === currencyId);
    if (currency) this.form.controls.rate.setValue(currency.rate);
  }

  submit(): void {
    if (this.form.invalid || this.lines.length === 0) {
      this.form.markAllAsTouched();
      if (this.lines.length === 0) this.toast.error('Add at least one line.');
      return;
    }

    this.saving.set(true);
    const value = this.form.getRawValue();
    const id = this.id();
    const currentUserId = this.auth.currentUser()?.id;
    const nowIso = new Date().toISOString();

    const payload = {
      ...(id ? { id } : {}),
      date: value.date,
      journalTypeId: value.journalTypeId,
      currencyId: value.currencyId,
      rate: value.rate,
      note: value.note,
      code: this.code() ?? undefined,
      journalItems: value.journalItems.map((l) => ({
        id: l.id ?? undefined,
        journalId: id ?? undefined,
        accountId: l.accountId,
        debit: Number(l.debit) || 0,
        credit: Number(l.credit) || 0,
        note: l.note,
      })),
      // Journal.CreateUserId is a required FK — OrgSys.App's MainController.FixData() sets these
      // from the session on every save; the Angular client has to do the same (see journal.model.ts).
      createUserId: id ? this.originalCreateUserId ?? undefined : currentUserId,
      createDate: id ? this.originalCreateDate ?? undefined : nowIso,
      ...(id ? { modifyUserId: currentUserId, modifyDate: nowIso } : {}),
    };

    const request$ = id ? this.journalService.update(payload) : this.journalService.createWithLines(payload);

    request$.subscribe({
      next: (result) => {
        this.saving.set(false);
        if (isApiSuccess(result)) {
          this.toast.success('Journal entry saved.');
          this.router.navigateByUrl('/accounting/journal-entries');
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
