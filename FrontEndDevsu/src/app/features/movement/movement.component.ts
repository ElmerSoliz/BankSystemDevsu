import { Component, Input, OnInit } from '@angular/core';
import { MovementService } from 'src/app/core/services/movement.service';
import { Movement } from 'src/app/core/models/movement.model';

@Component({
  selector: 'app-movement',
  templateUrl: './movement.component.html',
  styleUrls: ['./movement.component.scss']
})
export class MovementComponent implements OnInit {
  @Input() accountId!: string;

  movements: Movement[] = [];
  showForm = false;
  loading = false;
  error: string | null = null;

  constructor(private movementService: MovementService) {}

  ngOnInit(): void {
    if (this.accountId) this.loadMovements();
  }

  ngOnChanges(): void {
    if (this.accountId) this.loadMovements();
  }

  toggleForm() {
    this.showForm = !this.showForm;
  }

  loadMovements() {
    this.loading = true;
    this.movementService.listByAccount(this.accountId).subscribe({
      next: (data) => {
        this.movements = data;
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.error = 'Error cargando movimientos';
        this.loading = false;
      }
    });
  }

  onSaved(movement: Movement) {
    this.movements.push(movement);
    this.showForm = false;
  }
}
