import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SecrteryReservationVaccineComponent } from './secrtery-reservation-vaccine.component';

describe('SecrteryReservationVaccineComponent', () => {
  let component: SecrteryReservationVaccineComponent;
  let fixture: ComponentFixture<SecrteryReservationVaccineComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SecrteryReservationVaccineComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SecrteryReservationVaccineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
