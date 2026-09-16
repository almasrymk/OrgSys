import { BaseEntity } from '../../../../shared/models/base-entity.model';

/** Mirrors Application.DTOs.BranchDto. */
export interface Branch extends BaseEntity {
  name: string | null;
}

/** Mirrors CreateBranchCommand(Name) / UpdateBranchCommand(Id, Name). */
export interface SaveBranchRequest {
  id?: number;
  name: string;
}
