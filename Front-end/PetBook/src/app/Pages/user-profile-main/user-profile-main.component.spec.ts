import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserProfileMainComponent } from './user-profile-main.component';

describe('UserProfileMainComponent', () => {
  let component: UserProfileMainComponent;
  let fixture: ComponentFixture<UserProfileMainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserProfileMainComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(UserProfileMainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
