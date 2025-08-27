import { Component } from '@angular/core';
import { Account } from 'src/app/core/models/account.model';
import { AccountService } from 'src/app/core/services/account.service';
import { Movement } from 'src/app/core/models/movement.model';
import { MovementService } from 'src/app/core/services/movement.service';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss']
})
export class AccountComponent {
  accounts: Account[] = [];
  selectedAccount?: Account;
  showForm = false;
  loading = true;
  error: string | null = null;

  movementsByAccount: { [accountId: string]: Movement[] } = {};

  accountTypeMap: { [key: number]: string } = {
    1: 'Corriente',
    2: 'Ahorro'
  };

  constructor(
    private accountService: AccountService,
    private movementService: MovementService
  ) {}

  ngOnInit(): void {
    this.loadAccounts();
  }

  toggleForm() {
    this.showForm = !this.showForm;
    this.selectedAccount = undefined;
  }

  loadAccounts() {
    this.loading = true;
    this.accountService.getAll().subscribe({
      next: (data) => {
        this.accounts = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error cargando cuentas';
        console.error(err);
        this.loading = false;
      }
    });
  }

  edit(acc: Account) {
    this.selectedAccount = acc;
    this.showForm = true;
  }

  formSaved() {
    this.showForm = false;
    this.loadAccounts();
  }

  deleteAccount(id: string) {
    if (!confirm('¿Seguro que deseas eliminar esta cuenta?')) return;
    this.accountService.delete(id).subscribe({
      next: () => this.loadAccounts(),
      error: (err) => console.error(err)
    });
  }

  updateAccount(acc: Account) {
    if (!this.selectedAccount) return;

    this.accountService.update(this.selectedAccount.id, acc).subscribe({
      next: () => {
        this.loadAccounts();
        this.selectedAccount = undefined;
        this.showForm = false;
      },
      error: (err) => console.error(err)
    });
  }

  toggleMovements(account: Account) {
    const accountId = account.id;
    if (this.movementsByAccount[accountId]) {
      delete this.movementsByAccount[accountId];
      return;
    }
    this.movementService.listByAccount(accountId).subscribe({
      next: (movements) => {
        this.movementsByAccount[accountId] = movements;
      },
      error: (err) => console.error(err)
    });
  }
}
