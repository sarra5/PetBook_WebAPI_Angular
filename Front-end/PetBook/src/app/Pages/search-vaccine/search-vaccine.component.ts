import { Component, OnInit } from '@angular/core';
import { VaccineService } from '../../Services/vaccine.service';
import { Vaccine } from '../../Models/vaccine';
import { FormsModule } from '@angular/forms';
import { NgFor, NgForOf, NgIf } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-search-vaccine',
  standalone: true,
  imports: [FormsModule, NgFor, NgIf, NgForOf],
  templateUrl: './search-vaccine.component.html',
  styleUrl: './search-vaccine.component.css'
})
export class SearchVaccineComponent implements OnInit {

  
  vaccine :Vaccine[]=[]
  noResults: boolean = false;
  VaccineSuggestions: string[] = [];
  searchQuery: string = '';
  /////here
  Flag:boolean=false;
  /////for pagination
  pageNumber: number = 1;
  pageSize: number = 9;
  totalPages: number = 0;


  constructor(public vaccineService :VaccineService ,private router: Router
  ){}
  ngOnInit(): void {
    this.fetchVaccine();
  }

  fetchVaccine(){
   this.vaccineService.GatAllVaccine(this.pageNumber, this.pageSize).subscribe(data=>{
  //  this.vaccine=data;
  //here
   this.vaccine=data.data;
   this.totalPages=data.totalItems;
   console.log(this.vaccine);
   this.Flag=false;
   //here
   window.scrollTo({ top: 0, behavior: 'smooth' });
   })
  }

  hideSuggestions() {
    this.VaccineSuggestions = [];
   }

 onInputChange() {
      this.VaccineSuggestions = this.vaccineService.getVaccineSuggestions(this.searchQuery);
      this.noResults = this.VaccineSuggestions.length === 0;
 }

 searchBar() {
  if (this.searchQuery.trim() !== '') {
    this.pageNumber=1;
    const encodedSearchQuery = encodeURIComponent(this.searchQuery);
      this.vaccineService.GatVaccineByName(this.searchQuery).subscribe(
        data => {
          this.vaccine = [];
          this.vaccine.push(data);
          this.noResults = this.vaccine.length === 0;
          this.Flag=true;
          this.totalPages=this.vaccine.length;

        },
        error => {
          console.error('Error fetching pets:', error);
          this.noResults = true;
        }
      );
  } else {
    this.fetchVaccine();
    this.hideSuggestions();
  }
}

selectBreed(vacc: string) {
  this.searchQuery = vacc;
  this.VaccineSuggestions = [];
  this.searchBar();
}

GoToClinic(id: number): void {
  this.router.navigate(['search-vaccine-clinic', id]);
}
 BackToVaccines(){
  this.searchQuery = "";
  this.fetchVaccine()
 }


 //for pagination
 nextPage(): void {
  if (this.pageNumber < this.totalitems) {
    this.pageNumber++;
    this.fetchVaccine();
  }
}

prevPage(): void {
  if (this.pageNumber > 1) {
    this.pageNumber--;
    this. fetchVaccine();
  }
}
get totalitems(): number {
  return Math.ceil(this.totalPages / this.pageSize);
}
}
