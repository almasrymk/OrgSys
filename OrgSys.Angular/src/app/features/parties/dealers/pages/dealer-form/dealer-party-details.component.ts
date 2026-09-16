import { CommonModule } from '@angular/common';
import { Component, Input, OnChanges, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { isApiSuccess } from '../../../../../core/models/api-result.model';
import { ConfirmDialogService } from '../../../../../shared/components/confirm-dialog/confirm-dialog.service';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { Country } from '../../../../master-data/countries/models/country.model';
import { CountryService } from '../../../../master-data/countries/services/country.service';
import { CustomerProfile } from '../../../customer-profiles/models/customer-profile.model';
import { CustomerProfileService } from '../../../customer-profiles/services/customer-profile.service';
import { PartyAddress, PartyAddressType } from '../../../party-addresses/models/party-address.model';
import { PartyAddressService } from '../../../party-addresses/services/party-address.service';
import { PartyContact } from '../../../party-contacts/models/party-contact.model';
import { PartyContactService } from '../../../party-contacts/services/party-contact.service';
import { SupplierProfile } from '../../../supplier-profiles/models/supplier-profile.model';
import { SupplierProfileService } from '../../../supplier-profiles/services/supplier-profile.service';
import { DealerType } from '../../models/dealer.model';

@Component({
  selector: 'app-dealer-party-details',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './dealer-party-details.component.html',
})
export class DealerPartyDetailsComponent implements OnChanges {
  @Input({ required: true }) dealerId!: number;
  @Input({ required: true }) dealerType!: DealerType;

  private readonly fb = inject(FormBuilder);
  private readonly toast = inject(ToastService);
  private readonly confirmDialog = inject(ConfirmDialogService);
  private readonly customerProfileService = inject(CustomerProfileService);
  private readonly supplierProfileService = inject(SupplierProfileService);
  private readonly contactService = inject(PartyContactService);
  private readonly addressService = inject(PartyAddressService);
  private readonly countryService = inject(CountryService);

  readonly DealerType = DealerType;
  readonly PartyAddressType = PartyAddressType;
  readonly addressTypes = [
    { value: PartyAddressType.Billing, label: 'Billing' },
    { value: PartyAddressType.Shipping, label: 'Shipping' },
    { value: PartyAddressType.Registered, label: 'Registered' },
    { value: PartyAddressType.Office, label: 'Office' },
    { value: PartyAddressType.Other, label: 'Other' },
  ];

  readonly customerProfile = signal<CustomerProfile | null>(null);
  readonly supplierProfile = signal<SupplierProfile | null>(null);
  readonly contacts = signal<PartyContact[]>([]);
  readonly addresses = signal<PartyAddress[]>([]);
  readonly countries = signal<Country[]>([]);
  readonly assigning = signal(false);
  readonly savingContact = signal(false);
  readonly savingAddress = signal(false);

  readonly creditLimit = this.fb.nonNullable.control<number | null>(null);

  readonly contactForm = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(150)]],
    jobTitle: [''],
    department: [''],
    email: [''],
    phone: [''],
    mobile: [''],
    isPrimary: [false],
    isActive: [true],
  });

  readonly addressForm = this.fb.nonNullable.group({
    addressType: [PartyAddressType.Billing],
    line1: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(500)]],
    line2: [''],
    countryId: [0],
    postalCode: [''],
    isPrimary: [false],
  });

  ngOnChanges(): void {
    if (!this.dealerId) return;
    this.countryService.getList({ pageSize: 500 }).subscribe((r) => this.countries.set(r.response ?? []));
    this.reloadProfiles();
    this.reloadContacts();
    this.reloadAddresses();
  }

  private reloadProfiles(): void {
    this.customerProfileService.getByDealerId(this.dealerId).subscribe({
      next: (r) => this.customerProfile.set(r.response),
      error: () => this.customerProfile.set(null),
    });
    this.supplierProfileService.getByDealerId(this.dealerId).subscribe({
      next: (r) => this.supplierProfile.set(r.response),
      error: () => this.supplierProfile.set(null),
    });
  }

  private reloadContacts(): void {
    this.contactService.getListByDealer(this.dealerId).subscribe({
      next: (r) => this.contacts.set(r.response ?? []),
      error: () => this.contacts.set([]),
    });
  }

  private reloadAddresses(): void {
    this.addressService.getListByDealer(this.dealerId).subscribe({
      next: (r) => this.addresses.set(r.response ?? []),
      error: () => this.addresses.set([]),
    });
  }

  assignCustomer(): void {
    this.assigning.set(true);
    this.customerProfileService
      .assign({
        dealerId: this.dealerId,
        autoCreateReceivableAccount: true,
        creditLimit: this.creditLimit.value,
      })
      .subscribe({
        next: (result) => {
          this.assigning.set(false);
          if (isApiSuccess(result)) {
            this.toast.success('Customer role assigned.');
            this.reloadProfiles();
          } else {
            this.toast.error(result.errors?.[0]?.messageError ?? 'Assign failed.');
          }
        },
        error: () => {
          this.assigning.set(false);
          this.toast.error('Assign failed.');
        },
      });
  }

  assignSupplier(): void {
    this.assigning.set(true);
    this.supplierProfileService
      .assign({
        dealerId: this.dealerId,
        autoCreatePayableAccount: true,
      })
      .subscribe({
        next: (result) => {
          this.assigning.set(false);
          if (isApiSuccess(result)) {
            this.toast.success('Supplier role assigned.');
            this.reloadProfiles();
          } else {
            this.toast.error(result.errors?.[0]?.messageError ?? 'Assign failed.');
          }
        },
        error: () => {
          this.assigning.set(false);
          this.toast.error('Assign failed.');
        },
      });
  }

  addContact(): void {
    if (this.contactForm.invalid) {
      this.contactForm.markAllAsTouched();
      return;
    }

    this.savingContact.set(true);
    const value = this.contactForm.getRawValue();
    this.contactService
      .create({
        dealerId: this.dealerId,
        name: value.name,
        jobTitle: value.jobTitle || null,
        department: value.department || null,
        email: value.email || null,
        phone: value.phone || null,
        mobile: value.mobile || null,
        isPrimary: value.isPrimary,
        isActive: value.isActive,
      })
      .subscribe({
        next: (result) => {
          this.savingContact.set(false);
          if (isApiSuccess(result)) {
            this.toast.success('Contact saved.');
            this.contactForm.reset({ name: '', jobTitle: '', department: '', email: '', phone: '', mobile: '', isPrimary: false, isActive: true });
            this.reloadContacts();
          } else {
            this.toast.error(result.errors?.[0]?.messageError ?? 'Save failed.');
          }
        },
        error: () => {
          this.savingContact.set(false);
          this.toast.error('Save failed.');
        },
      });
  }

  async deleteContact(contact: PartyContact): Promise<void> {
    const confirmed = await this.confirmDialog.confirm(`Delete "${contact.name}"?`, 'Delete contact');
    if (!confirmed) return;
    this.contactService.delete(contact.id).subscribe({
      next: (result) => {
        if (isApiSuccess(result)) {
          this.toast.success('Contact deleted.');
          this.reloadContacts();
        } else {
          this.toast.error(result.errors?.[0]?.messageError ?? 'Delete failed.');
        }
      },
      error: () => this.toast.error('Delete failed.'),
    });
  }

  addAddress(): void {
    if (this.addressForm.invalid) {
      this.addressForm.markAllAsTouched();
      return;
    }

    this.savingAddress.set(true);
    const value = this.addressForm.getRawValue();
    this.addressService
      .create({
        dealerId: this.dealerId,
        addressType: value.addressType,
        line1: value.line1,
        line2: value.line2 || null,
        countryId: value.countryId || null,
        postalCode: value.postalCode || null,
        isPrimary: value.isPrimary,
      })
      .subscribe({
        next: (result) => {
          this.savingAddress.set(false);
          if (isApiSuccess(result)) {
            this.toast.success('Address saved.');
            this.addressForm.reset({
              addressType: PartyAddressType.Billing,
              line1: '',
              line2: '',
              countryId: 0,
              postalCode: '',
              isPrimary: false,
            });
            this.reloadAddresses();
          } else {
            this.toast.error(result.errors?.[0]?.messageError ?? 'Save failed.');
          }
        },
        error: () => {
          this.savingAddress.set(false);
          this.toast.error('Save failed.');
        },
      });
  }

  async deleteAddress(address: PartyAddress): Promise<void> {
    const confirmed = await this.confirmDialog.confirm('Delete this address?', 'Delete address');
    if (!confirmed) return;
    this.addressService.delete(address.id).subscribe({
      next: (result) => {
        if (isApiSuccess(result)) {
          this.toast.success('Address deleted.');
          this.reloadAddresses();
        } else {
          this.toast.error(result.errors?.[0]?.messageError ?? 'Delete failed.');
        }
      },
      error: () => this.toast.error('Delete failed.'),
    });
  }

  addressTypeLabel(type: PartyAddressType): string {
    return this.addressTypes.find((t) => t.value === type)?.label ?? String(type);
  }
}
