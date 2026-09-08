import { Status } from './status.enum';

/** Mirrors Domain.Entities.BaseModel — the fields every OrgSys entity DTO carries. */
export interface BaseEntity {
  id: number;
  codeNumber: number;
  code: string | null;
  maskText: string | null;
  parentId: number;
  typeId: number;
  hide: boolean;
  imgPath: string | null;
  status: Status;
}

export interface SearchParams {
  keySearch?: string;
  parentId?: number;
  typeId?: number;
  page: number;
  pageSize: number;
}
