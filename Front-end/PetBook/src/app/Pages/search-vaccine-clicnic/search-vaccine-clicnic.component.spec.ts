import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchVaccineClicnicComponent } from './search-vaccine-clicnic.component';

describe('SearchVaccineClicnicComponent', () => {
  let component: SearchVaccineClicnicComponent;
  let fixture: ComponentFixture<SearchVaccineClicnicComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SearchVaccineClicnicComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SearchVaccineClicnicComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
