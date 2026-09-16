import { BaseEntity } from '../../../../shared/models/base-entity.model';

/** Mirrors Domain.Enums.FiscalYearStatus. */
export enum FiscalYearStatus {
  Open = 1,
  Closed = 2,
}

/**
 * Mirrors Application.DTOs.FiscalYearDto. `periods` is deliberately omitted here: the API
 * persists FiscalPeriod rows only as a child collection synced from FiscalYear's Update command
 * (see Application/Commands/Org/Setting/FiscalYear/Commands/UpdateCommandHandler.cs `SaveDetials`)
 * and there's no standalone FiscalPeriod CQRS/API surface (see docs/ANGULAR_MIGRATION_INVENTORY.md
 * gap list) — period editing is left for a follow-up feature rather than guessed at here.
 */
export interface FiscalYear extends BaseEntity {
  name: string | null;
  startDate: string;
  endDate: string;
  isCurrent: boolean;
  fiscalYearStatus: FiscalYearStatus;
}

/** Mirrors CreateFiscalYearCommand/UpdateFiscalYearCommand (both FiscalYearDto subclasses). */
export interface SaveFiscalYearRequest {
  id?: number;
  name: string;
  startDate: string;
  endDate: string;
  isCurrent: boolean;
  fiscalYearStatus: FiscalYearStatus;
}
