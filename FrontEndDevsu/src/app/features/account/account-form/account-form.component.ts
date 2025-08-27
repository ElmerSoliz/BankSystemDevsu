import { Component, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Account } from 'src/app/core/models/account.model';
import { Client } from 'src/app/core/models/client.model';
import { AccountType } from 'src/app/core/models/enums.model';
import { AccountService } from 'src/app/core/services/account.service';
import { ClientService } from 'src/app/core/services/client.service';

@Component({
  selector: 'app-account-form',
  templateUrl: './account-form.component.html',
  styleUrls: ['./account-form.component.scss']
})
export class AccountFormComponent implements OnChanges {
  @Input() account?: Account;
  @Output() save = new EventEmitter<Account>();

  clients: Client[] = [];
  error: string | null = null;
  form: FormGroup;

  accountTypes = [
    { value: 1, label: 'Corriente' },
    { value: 2, label: 'Ahorro' }
  ];

  constructor(private fb: FormBuilder, private accountService: AccountService, private clientService: ClientService) {
    this.form = this.fb.group({
      accountNumber: ['', Validators.required],
      accountType: [AccountType.Corriente, Validators.required],
      initialBalance: [0, [Validators.required, Validators.min(0)]],
      clientId: ['', Validators.required],
      isActive: [true]
    });
    this.loadClients();
  }

  loadClients() {
    this.clientService.getAll().subscribe({
      next: data => {
        this.clients = data;
      },
      error: err => {
        this.error = 'Error loading clients';
        console.error(err);
      }
    });
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['account'] && this.account) {
      this.form.patchValue({
        accountNumber: this.account.accountNumber,
        accountType: Number(this.account.accountType),
        initialBalance: this.account.initialBalance,
        clientId: this.account.clientId,
        isActive: this.account.isActive
      });

      this.form.get('accountNumber')?.disable();
      this.form.get('accountType')?.disable();
      this.form.get('initialBalance')?.disable();
      this.form.get('clientId')?.disable();

    } else {
      this.form.enable();
    }
  }

  submit() {
    if (this.form.invalid) return;

    if (this.account) {
      const payload = {
        isActive: this.form.get('isActive')?.value
      };
      this.accountService.update(this.account.id!, payload).subscribe({ 
        next: (value) => {
          this.save.emit();
        },
        error: (err) => {
          console.error('Error al editar', err);
          if (err.error && err.error.errors) {
            let messages: string[] = [];
            for (const key in err.error.errors) {
              if (err.error.errors.hasOwnProperty(key)) {
                messages = messages.concat(err.error.errors[key]);
              }
            }
            alert(messages.join('\n'));
          } else {
            alert('Error inesperado al actualizar la cuenta');
          }
        }
      });
    } else {
      const payload = {
        accountNumber: this.form.get('accountNumber')?.value,
        accountType: Number(this.form.get('accountType')?.value),
        initialBalance: this.form.get('initialBalance')?.value,
        currentBalance: this.form.get('initialBalance')?.value,
        clientId: this.form.get('clientId')?.value,
        isActive: true
      };
      this.accountService.create(payload).subscribe({ 
        next: (created) => {
          this.save.emit();
          this.form.reset({
            accountType: AccountType.Corriente,
            initialBalance: 0,
            clientId: '',
            isActive: true
          })
        },
        error: (err) => {
          console.error('Error creando cuenta', err);
          if (err.error && err.error.errors) {
            let messages: string[] = [];
            for (const key in err.error.errors) {
              if (err.error.errors.hasOwnProperty(key)) {
                messages = messages.concat(err.error.errors[key]);
              }
            }
            alert(messages.join('\n'));
          } else {
            alert('Error inesperado al crear cuenta');
          }
        }
      });
    }
  }
}
