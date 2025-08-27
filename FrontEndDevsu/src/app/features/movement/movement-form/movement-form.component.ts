import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MovementService } from 'src/app/core/services/movement.service';
import { Movement } from 'src/app/core/models/movement.model';

@Component({
  selector: 'app-movement-form',
  templateUrl: './movement-form.component.html',
  styleUrls: ['./movement-form.component.scss']
})
export class MovementFormComponent {
  @Input() accountId!: string;
  @Output() saved = new EventEmitter<Movement>();

  form: FormGroup;
  movementType = [
    { value: 1, label: 'Credito' },
    { value: 2, label: 'Debito' }
  ];

  constructor(private fb: FormBuilder, private movementService: MovementService) {
    this.form = this.fb.group({
      movementType: ['', Validators.required],
      amount: [0, Validators.required],
      dateUtc: [new Date().toISOString().substring(0,10)]
    });
  }

  submit() {
    if (this.form.invalid) return;

    let amount = Number(this.form.value.amount);
    const type = Number(this.form.value.movementType);

    if (type === 1 && amount <= 0) amount = Math.abs(amount);
    if (type === 2 && amount >= 0) amount = -Math.abs(amount);

    const payload: Partial<Movement> = {
      accountId: this.accountId,
      movementType: type,
      amount,
      dateUtc: new Date(this.form.value.dateUtc).toISOString()
    };

    this.movementService.create(payload).subscribe({
      next: (movement) => {
        this.saved.emit(movement);
        this.form.reset({
          movementType: '',
          amount: 0,
          dateUtc: new Date().toISOString().substring(0,10)
        });
      },
      error: (err) => {
        console.error('Error creando movimiento', err);
        let messages: string[] = [];
        if (err.error?.errors) {
          for (const key in err.error.errors) {
            if (err.error.errors.hasOwnProperty(key)) {
              messages = messages.concat(err.error.errors[key] as string[]);
            }
          }
        } 
        else if (err.error) {
          if ((err.error as any).detail) {
            messages.push((err.error as any).detail);
          } 
          else if (typeof err.error === 'string') {
            messages.push(err.error);
          } 
          else if (Array.isArray(err.error)) {
            messages = messages.concat(err.error as string[]);
          } 
          else if (typeof err.error === 'object') {
            const vals = Object.values(err.error).flatMap(v => 
              Array.isArray(v) ? v.map(x => String(x)) : [String(v)]
            );
            messages = messages.concat(vals);
          }
        }
        if (messages.length > 0) {
          alert(messages.join('\n'));
        } else {
          alert('Error inesperado al crear movimiento');
        }
      }
    });
  }
}
