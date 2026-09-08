import { BaseEntity } from '../../../../shared/models/base-entity.model';
import { DealerType } from '../../dealer-groups/models/dealer-group.model';

export { DealerType };

/** Mirrors Application.DTOs.DealerDto. */
export interface Dealer extends BaseEntity {
  name: string | null;
  phone: string | null;
  email: string | null;
  address: string | null;
  dealerGroupId: number | null;
  dealerGroupName: string | null;
  countryId: number | null;
  countryName: string | null;
  cityId: number | null;
  cityName: string | null;
  districtId: number | null;
  districtName: string | null;
  accountId: number | null;
  accountCode: string | null;
  accountName: string | null;
}

/**
 * Mirrors CreateDealerCommand/UpdateDealerCommand (both DealerDto subclasses). `accountId` is
 * required here — an existing GL account must be picked; the auto-create-sub-account flags
 * (`AutoCreateReceivableAccount`/`AutoCreatePayableAccount` on the real DTO) are intentionally not
 * exposed in this pass (see DealerReceivableAccountProvisioning/DealerPayableAccountProvisioning —
 * without one of these two, the API requires an explicit accountId anyway).
 */
export interface SaveDealerRequest {
  id?: number;
  name: string;
  typeId: DealerType;
  code: string;
  codeNumber: number;
  phone: string | null;
  email: string | null;
  address: string | null;
  dealerGroupId: number;
  countryId: number | null;
  cityId: number | null;
  districtId: number | null;
  accountId: number;
}
