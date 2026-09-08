import { BaseEntity } from '../../../../shared/models/base-entity.model';

/** Mirrors Application.DTOs.JournalItemDto. */
export interface JournalItem {
  id: number;
  journalId: number;
  accountId: number;
  accountName: string | null;
  debit: number;
  credit: number;
  note: string | null;
}

/** Mirrors Application.DTOs.JournalDto (extends MovementDto extends BaseModel). */
export interface Journal extends BaseEntity {
  date: string;
  posted: boolean;
  review: boolean;
  hasJournal: boolean;
  createUserId: number;
  createDate: string;
  journalTypeId: number;
  journalTypeName: string | null;
  currencyId: number;
  currencyName: string | null;
  rate: number;
  fiscalYearName: string | null;
  fiscalPeriodName: string | null;
  refranceTable: string | null;
  note: string | null;
  journalItems: JournalItem[];
  originalJournalCode: string | null;
  reversalJournalCode: string | null;
}

/** Mirrors Application.DTOs.JournalTypeDto. */
export interface JournalType extends BaseEntity {
  name: string | null;
  isOpeningBlance: boolean;
  icon: string | null;
  group: string | null;
}

/**
 * `journalId` is required by AutoMapper on the server side — UpdateCommandHandler.SaveDetials maps
 * `JournalItemDto` straight to the `JournalItem` entity without ever setting JournalId itself
 * (unlike e.g. FiscalYear's handler, which does set the FK in code), so the client must supply it
 * on every line, matching the journal's own id.
 */
export interface SaveJournalItemRequest {
  id?: number;
  journalId?: number;
  accountId: number;
  debit: number;
  credit: number;
  note: string | null;
}

/**
 * Mirrors CreateJournalCommand/UpdateJournalCommand (both JournalDto subclasses). Note: the API's
 * Create handler does NOT persist `journalItems` (see Application/Commands/.../Journal/Command/
 * CreateCommandHandler.cs — it never calls SaveDetials) — only Update does. A new journal must be
 * created header-only, then immediately updated with its lines once its Id is known
 * (see JournalService.createWithLines, which replicates OrgSys.App's AutoSave-then-search-by-Code flow).
 *
 * `createUserId`/`createDate`/`modifyUserId`/`modifyDate` are NOT server-assigned — OrgSys.App's
 * MainController.FixData() sets them from the authenticated session before every save (Journal has
 * a required FK from CreateUserId to User), so the Angular client must set them too.
 */
export interface SaveJournalRequest {
  id?: number;
  date: string;
  journalTypeId: number;
  currencyId: number;
  rate: number;
  note: string | null;
  code?: string;
  codeNumber?: number;
  journalItems: SaveJournalItemRequest[];
  createUserId?: number;
  createDate?: string;
  modifyUserId?: number;
  modifyDate?: string;
}
