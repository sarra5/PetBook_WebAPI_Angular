import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SecrteryDoctorComponent } from './secrtery-doctor.component';

describe('SecrteryDoctorComponent', () => {
  let component: SecrteryDoctorComponent;
  let fixture: ComponentFixture<SecrteryDoctorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SecrteryDoctorComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SecrteryDoctorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
