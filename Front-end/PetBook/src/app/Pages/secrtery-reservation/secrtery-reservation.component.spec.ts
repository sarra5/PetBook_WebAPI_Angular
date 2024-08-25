import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SecrteryReservationComponent } from './secrtery-reservation.component';

describe('SecrteryReservationComponent', () => {
  let component: SecrteryReservationComponent;
  let fixture: ComponentFixture<SecrteryReservationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SecrteryReservationComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SecrteryReservationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
