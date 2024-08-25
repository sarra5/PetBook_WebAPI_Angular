import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClinicSearchComponent } from './clinic-search.component';

describe('ClinicSearchComponent', () => {
  let component: ClinicSearchComponent;
  let fixture: ComponentFixture<ClinicSearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ClinicSearchComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ClinicSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
