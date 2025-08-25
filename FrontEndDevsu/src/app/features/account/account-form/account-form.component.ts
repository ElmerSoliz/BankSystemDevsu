import { Component, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Account } from 'src/app/core/models/account.model';
import { AccountType } from 'src/app/core/models/enums.model';
import { AccountService } from 'src/app/core/services/account.service';

@Component({
  selector: 'app-account-form',
  templateUrl: './account-form.component.html',
  styleUrls: ['./account-form.component.scss']
})
export class AccountFormComponent implements OnChanges {
  @Input() account?: Account;
  @Output() save = new EventEmitter<Account>();

  form: FormGroup;

  accountTypes = [
    { value: 1, label: 'Corriente' },
    { value: 2, label: 'Ahorro' }
  ];

  constructor(private fb: FormBuilder, private accountService: AccountService) {
    this.form = this.fb.group({
      accountNumber: ['', Validators.required],
      accountType: [AccountType.Corriente, Validators.required],
      initialBalance: [0, [Validators.required, Validators.min(0)]],
      clientId: ['', Validators.required],
      isActive: [true]
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
    this.accountService.update(this.account.id!, payload).subscribe(() => {
      this.save.emit();
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
    this.accountService.create(payload).subscribe(() => {
      this.save.emit();
      this.form.reset({
        accountType: AccountType.Corriente,
        initialBalance: 0,
        clientId: '',
        isActive: true
      });
    });
  }
}

}
