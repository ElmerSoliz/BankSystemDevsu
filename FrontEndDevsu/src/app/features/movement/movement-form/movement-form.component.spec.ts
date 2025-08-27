import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MovementFormComponent } from './movement-form.component';

describe('MovementFormComponent', () => {
  let component: MovementFormComponent;
  let fixture: ComponentFixture<MovementFormComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MovementFormComponent]
    });
    fixture = TestBed.createComponent(MovementFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
