import { BaseEntity } from '../../../shared/models/base-entity.model';

/** Mirrors CommercialDocuments InvoiceTypeDto. */
export interface InvoiceType extends BaseEntity {
  name: string | null;
  inOut: number;
  icon: string | null;
  group: string | null;
}

/** Seeded InvoiceType Ids (CommercialDocuments invoice type rows). */
export const INVOICE_TYPE_PATHS: Record<number, string> = {
  1: 'sales-invoices',
  2: 'purchase-invoices',
  3: 'sales-returns',
  4: 'purchase-returns',
};

export function commercialDocumentPath(typeId: number): string {
  return `/commercial-documents/${INVOICE_TYPE_PATHS[typeId] ?? String(typeId)}`;
}
