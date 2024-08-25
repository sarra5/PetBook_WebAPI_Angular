import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SecrteryLocationComponent } from './secrtery-location.component';

describe('SecrteryLocationComponent', () => {
  let component: SecrteryLocationComponent;
  let fixture: ComponentFixture<SecrteryLocationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SecrteryLocationComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SecrteryLocationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
