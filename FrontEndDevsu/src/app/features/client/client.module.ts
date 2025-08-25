import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { ClientComponent } from './client.component';
import { ClientFormComponent } from './client-form/client-form.component';
import { ClientRoutingModule } from './client-routing.module';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';

@NgModule({
  declarations: [
    ClientComponent, 
    ClientFormComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ClientRoutingModule,
    MatButtonModule,
    MatInputModule,
    MatSelectModule
  ]
})
export class ClientModule {}
