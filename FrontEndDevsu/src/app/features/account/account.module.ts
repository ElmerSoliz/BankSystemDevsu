import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { AccountComponent } from './account.component';
import { AccountFormComponent } from './account-form/account-form.component';
import { AccountRoutingModule } from './account-routing.module';
import { MovementModule } from '../movement/movement.module';

@NgModule({
  declarations: [
    AccountComponent,
    AccountFormComponent
  ],
  imports: [
    CommonModule,
    AccountRoutingModule,
    ReactiveFormsModule,
    MovementModule
  ]
})
export class AccountModule {}
