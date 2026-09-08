import { BaseEntity } from '../../../../shared/models/base-entity.model';

/** Mirrors Application.DTOs.AccountDto (chart of accounts, tree via BaseModel.ParentId). */
export interface Account extends BaseEntity {
  name: string | null;
  debit: number;
  credit: number;
  accountTypeId: number;
  accountTypeName: string | null;
  parentName: string | null;
  isPostable: boolean;
}

/** Mirrors Domain.Entities.AccountType (dropdown-only, no dedicated MVC screen — see inventory §1). */
export interface AccountType extends BaseEntity {
  name: string | null;
  debitOrCredit: number;
}

/** Mirrors CreateAccountCommand/UpdateAccountCommand (both AccountDto subclasses). */
export interface SaveAccountRequest {
  id?: number;
  name: string;
  code: string;
  parentId: number;
  accountTypeId: number;
  debit: number;
  credit: number;
  isPostable: boolean;
}
