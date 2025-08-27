import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MovementComponent } from './movement.component';
import { MovementFormComponent } from './movement-form/movement-form.component';
import { MovementRoutingModule } from './movement-routing.module';

@NgModule({
  declarations: [
    MovementComponent,
    MovementFormComponent
  ],
  imports: [
    CommonModule,
    MovementRoutingModule,
    ReactiveFormsModule
  ],
  exports: [
    MovementComponent
  ]
})
export class MovementModule {}
