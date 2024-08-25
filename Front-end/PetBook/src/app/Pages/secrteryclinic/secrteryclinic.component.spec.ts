import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SecrteryclinicComponent } from './secrteryclinic.component';

describe('SecrteryclinicComponent', () => {
  let component: SecrteryclinicComponent;
  let fixture: ComponentFixture<SecrteryclinicComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SecrteryclinicComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SecrteryclinicComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
