import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SecrteryPhonesComponent } from './secrtery-phones.component';

describe('SecrteryPhonesComponent', () => {
  let component: SecrteryPhonesComponent;
  let fixture: ComponentFixture<SecrteryPhonesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SecrteryPhonesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SecrteryPhonesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
