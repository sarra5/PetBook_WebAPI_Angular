
import { Component, OnInit } from '@angular/core';
import { VaccineClinicsService } from '../../Services/vaccine-clinics.service';
import { VaccineClinic } from '../../Models/vaccine-clinic';
import { switchMap } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { ClinlicLocationService } from '../../Services/clinlic-location.service';
import { VaccineClinicLocation } from '../../Models/vaccine-clinic-location';
import { CommonModule, NgFor, NgForOf, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  standalone: true,
  selector: 'app-search-vaccine-clicnic',
  imports: [FormsModule, NgFor, NgIf],
  templateUrl: './search-vaccine-clicnic.component.html',
  styleUrls: ['./search-vaccine-clicnic.component.css']
})
export class SearchVaccineClicnicComponent implements OnInit {
  location: string = "abu qier";
  VaccineId: number | null = null;
  vaccineClinic: VaccineClinic[] = [];
  vaccineClinicLocation: VaccineClinicLocation[] = [];
  clinicsName: string[] = [];
  searchQuery: string = '';
  ClinicSuggestions: string[] = [];
  Flag: boolean = false;
  noResults: boolean = false;
  MainData: VaccineClinicLocation[] = [];
  numbofpages:number=0;

  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 5;
  paginatedData: VaccineClinicLocation[] = [];

  constructor(
    private route: ActivatedRoute,
    private vaccineClinicService: VaccineClinicsService,
    private clinicLocation: ClinlicLocationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.paramMap.pipe(
      switchMap(params => {
        const id = +params.get('VaccineId')!; // Non-null assertion
        this.VaccineId = id;
        console.log(this.VaccineId);
        return this.vaccineClinicService.GatAllClincsHasThisVaccine(id);
      })
    ).subscribe(
      data => {
        this.vaccineClinic = data;
        console.log(this.vaccineClinic);
        this.vaccineClinic.forEach(element => {
          this.clinicsName.push(element.name);
          this.getClinckwithLocatiion(element.clinicID);
        });
        console.log(this.clinicsName);
      },
      error => {
        console.error('An error occurred:', error);
      }
    );
  }

  getClinckwithLocatiion(id: number): void {
    this.clinicLocation.GatClincsWithLocations(id).subscribe(
      data2 => {
        this.vaccineClinicLocation.push(...data2); // Use spread operator to push array elements
        this.vaccineClinicLocation.forEach(element => {
          const item = this.vaccineClinic.find(c => c.clinicID == element.clinicID && c.vaccineID == this.VaccineId);
          if (item) {
            element.price = item.price;
            element.Quantity = item.quantity;
          }
        });
        this.numbofpages=Math.ceil(this.vaccineClinicLocation.length/5);
        console.log(this.vaccineClinicLocation);
        this.MainData = this.vaccineClinicLocation;
        this.updatePaginatedData();
      },
      error => {
        console.error('An error occurred while fetching clinic locations:', error);
      }
    );
  }

  updatePaginatedData(): void {
    const start = (this.currentPage - 1) * this.itemsPerPage;
    const end = start + this.itemsPerPage;
    this.paginatedData = this.vaccineClinicLocation.slice(start, end);
  }

  BackToVaccines() {
    this.searchQuery = "";
    this.vaccineClinicLocation = this.MainData;
    this.Flag = false;
    window.scrollTo({ top: 0, behavior: 'smooth' });
    this.updatePaginatedData();
  }

  getStars(rate: number): number[] {
    return Array(rate).fill(0).map((x, i) => i);
  }

  GoToClinic(clinicId: number): void {
    this.router.navigate([`/ReservationVaccine/${clinicId}/${this.VaccineId}`]);
  }

  hideSuggestions() {
    this.ClinicSuggestions = [];
  }

  getClinicSuggestions(input: string): string[] {
    return this.clinicsName.filter(clinic => clinic.toLowerCase().includes(input.toLowerCase()));
  }

  onInputChange() {
    this.ClinicSuggestions = this.getClinicSuggestions(this.searchQuery);
    this.noResults = this.ClinicSuggestions.length === 0;
  }

  selectClinic(clinic: string) {
    this.searchQuery = clinic;
    this.ClinicSuggestions = [];
    this.searchBar();
  }

  searchBar() {
    if (this.searchQuery.trim() !== '') {
      this.vaccineClinicLocation = this.MainData;
      this.vaccineClinicLocation = this.vaccineClinicLocation.filter(clinic => clinic.name.toLowerCase() === this.searchQuery.toLowerCase());
      this.Flag = true;
    } else {
      this.vaccineClinicLocation = this.MainData;
      this.Flag = false;
      window.scrollTo({ top: 0, behavior: 'smooth' });
      this.hideSuggestions();
    }
    this.updatePaginatedData();
  }

  nextPage(): void {
    if ((this.currentPage * this.itemsPerPage) < this.vaccineClinicLocation.length) {
      this.currentPage++;
      this.updatePaginatedData();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updatePaginatedData();
    }
  }

  trackByFn(index: number, item: VaccineClinicLocation): number {
    return item.clinicID;
  }
  
}

