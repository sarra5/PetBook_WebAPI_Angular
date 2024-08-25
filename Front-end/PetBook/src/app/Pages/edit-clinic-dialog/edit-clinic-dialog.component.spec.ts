import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditClinicDialogComponent } from './edit-clinic-dialog.component';

describe('EditClinicDialogComponent', () => {
  let component: EditClinicDialogComponent;
  let fixture: ComponentFixture<EditClinicDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditClinicDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EditClinicDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
