import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchVaccineComponent } from './search-vaccine.component';

describe('SearchVaccineComponent', () => {
  let component: SearchVaccineComponent;
  let fixture: ComponentFixture<SearchVaccineComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SearchVaccineComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SearchVaccineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
