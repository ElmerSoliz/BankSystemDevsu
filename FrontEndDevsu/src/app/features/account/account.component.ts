import { Component } from '@angular/core';
import { Account } from 'src/app/core/models/account.model';
import { AccountService } from 'src/app/core/services/account.service';

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

  constructor(private accountService: AccountService) {}

  ngOnInit(): void {
    this.loadAccounts();
  }

  toggleForm() {
    this.showForm = !this.showForm;
    this.selectedAccount = undefined;
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

  loadAccounts() {
    this.loading = true;
    this.accountService.getAll().subscribe({
      next: (data) => {
        this.accounts = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error loading accounts';
        console.error(err);
        this.loading = false;
      }
    });
  }

  createAccount(acc: Account) {
    this.accountService.create(acc).subscribe({
      next: (created) => {
        this.accounts.push(created);
        this.showForm = false;
      },
      error: (err) => console.error(err)
    });
  }

  edit(acc: Account) {
    this.selectedAccount = acc;
  }

  deleteAccount(id: string) {
    if (!confirm('¿Seguro que deseas eliminar esta cuenta?')) return;
    this.accountService.delete(id).subscribe({
      next: () => this.loadAccounts(),
      error: (err) => console.error(err)
    });
  }
}
