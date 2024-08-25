import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReservationForVaccineComponent } from './reservation-for-vaccine.component';

describe('ReservationForVaccineComponent', () => {
  let component: ReservationForVaccineComponent;
  let fixture: ComponentFixture<ReservationForVaccineComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReservationForVaccineComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ReservationForVaccineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
