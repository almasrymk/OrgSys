import { BaseEntity } from '../../../../shared/models/base-entity.model';

export enum AttributeDataType {
  Text = 0,
  Number = 1,
  Boolean = 2,
  Date = 3,
  Selection = 4,
  MultiSelection = 5,
}

export interface Property extends BaseEntity {
  name: string | null;
  dataType: AttributeDataType;
}

export interface SavePropertyRequest {
  id?: number;
  name: string;
  dataType: AttributeDataType;
}
