import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ClientService } from 'src/app/core/services/client.service';
import { Client } from 'src/app/core/models/client.model';

@Component({
  selector: 'app-client-form',
  templateUrl: './client-form.component.html',
  styleUrls: ['./client-form.component.scss']
})
export class ClientFormComponent implements OnInit {
  @Input() client?: Client;
  @Output() saved = new EventEmitter<void>();
  form!: FormGroup;

  genders = [
    { value: 0, label: 'Otro' },
    { value: 1, label: 'Masculino' },
    { value: 2, label: 'Femenino' }
  ];

  constructor(private fb: FormBuilder, private clientService: ClientService) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      name: [this.client?.name || '', Validators.required],
      gender: [this.client?.gender ?? 0, Validators.required],
      age: [this.client?.age || 0, [Validators.required, Validators.min(0)]],
      identification: [this.client?.identification || '', Validators.required],
      address: [this.client?.address || '', Validators.required],
      phone: [this.client?.phone || '', Validators.required],
      password: ['', this.client ? [] : Validators.required],
      isActive: [this.client?.isActive ?? true]
    });
  }

  onSubmit(): void {
    if (this.form.invalid) return;

    const payload: any = {
      name: this.form.value.name,
      gender: Number(this.form.value.gender),
      age: Number(this.form.value.age),
      identification: this.form.value.identification,
      address: this.form.value.address,
      phone: this.form.value.phone
    };

    if (!this.client) {
      payload.password = this.form.value.password;
    }

    if (this.client) {
      payload.isActive = this.form.value.isActive;
      this.clientService.update(this.client.id!, payload).subscribe(() => this.saved.emit());
    } else {
      this.clientService.create(payload).subscribe(() => this.saved.emit());
    }
  }
}
