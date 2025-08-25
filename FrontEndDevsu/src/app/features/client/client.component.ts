import { Component } from '@angular/core';
import { Client } from 'src/app/core/models/client.model';
import { ClientService } from 'src/app/core/services/client.service';

@Component({
  selector: 'app-client',
  templateUrl: './client.component.html',
  styleUrls: ['./client.component.scss']
})
export class ClientComponent {
  clients: Client[] = [];
  selectedClient?: Client;
  showForm = false;
  loading = true;
  error: string | null = null;

  constructor(private clientService: ClientService) {}

  ngOnInit(): void {
    this.loadClients();
  }

  loadClients() {
    this.loading = true;
    this.clientService.getAll().subscribe({
      next: data => {
        this.clients = data;
        this.loading = false;
      },
      error: err => {
        this.error = 'Error loading clients';
        console.error(err);
        this.loading = false;
      }
    });
  }

  create() {
    this.selectedClient = undefined;
    this.showForm = true;
  }

  edit(client: Client) {
    this.selectedClient = { ...client };
    this.showForm = true;
  }

  formSaved() {
    this.showForm = false;
    this.loadClients();
  }

  deleteClient(id: string) {
    if (!confirm('¿Seguro que deseas eliminar este cliente?')) return;
    this.clientService.delete(id).subscribe(() => this.loadClients());
  }
}
