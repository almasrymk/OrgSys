import { Injectable, signal } from '@angular/core';

interface ConfirmState {
  message: string;
  title: string;
  resolve: (confirmed: boolean) => void;
}

/** Replaces OrgSys.App's shared #MsgModelId confirmation modal (custom.js `Delete(...)`). */
@Injectable({ providedIn: 'root' })
export class ConfirmDialogService {
  readonly state = signal<ConfirmState | null>(null);

  confirm(message: string, title = 'Confirm'): Promise<boolean> {
    return new Promise<boolean>((resolve) => {
      this.state.set({ message, title, resolve });
    });
  }

  respond(confirmed: boolean): void {
    this.state()?.resolve(confirmed);
    this.state.set(null);
  }
}
