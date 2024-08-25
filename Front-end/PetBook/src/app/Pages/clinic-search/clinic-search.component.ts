
import { Component, OnInit } from '@angular/core';
import { ClinicService } from '../../Services/clinic.service';
import { FormsModule } from '@angular/forms';
import { ClinicLocation } from '../../Models/clinic_location';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import Swal from 'sweetalert2';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-clinic-search',
  standalone: true,
  imports: [FormsModule,CommonModule, RouterLink],
  templateUrl: './clinic-search.component.html',
  styleUrl: './clinic-search.component.css'
})
export class ClinicSearchComponent implements OnInit{
  Clinics:ClinicLocation[]=[];
  MainData:ClinicLocation[]=[];
  clinicSearch : string = ""
  ClinicsByName : ClinicLocation[] = [];
  ClinicPhone : any =[];
  AllClinics: ClinicLocation[] =[];
  Flag:boolean=false;
  ClinicNames:string[]=[];
  countIsZero=false;
  //for pagination
  pageNumber: number = 1;
  pageSize: number = 9;
  totalPages: number = 0;

  //for auto correct
  clinicSuggestions: string[] = [];
  noResults: boolean = false;
  clinicsName: string[] = [];

  constructor(private clinicService:ClinicService){}
  ngOnInit(): void {
    this.getAllClinics()
  }
  getAllClinics() {
    this.clinicService.getAllClinics(this.pageNumber, this.pageSize).subscribe({
      next: (data) => {
        this.Clinics = data.data;
        this.MainData = data.allData;
       // this.MainData = this.Clinics;
        this.Flag=false;
        this.clinicSearch=""
        this.countIsZero= this.Clinics.length===0;
        if(this.countIsZero){
          this.totalPages=this.pageSize
        }
        //for pagination
        this.AllClinics=data.data;
        this.totalPages = data.totalItems;
        window.scrollTo({ top: 0, behavior: 'smooth' });

        //for autoCorrect
        this.getClinicsName();
      }
    });
  }

  searchClinics() {
    if (this.clinicSearch.trim() !== '') {
      this.pageNumber=1;
      this.Clinics = this.MainData;
      this.Clinics = this.Clinics.filter(clinic => clinic.name.toLowerCase() === this.clinicSearch.toLowerCase());
      this.ClinicsByName=[];
        this.Flag=true;
        this.totalPages=this.Clinics.length;
        this.countIsZero=this.Clinics.length===0;
        if(this.countIsZero){
          this.totalPages=this.pageSize
        }
    } else {
      this.getAllClinics();
      this.Flag = false;
      window.scrollTo({ top: 0, behavior: 'smooth' });
      this.hideSuggestions();
    }
  }

  BackToClinics(){
    this.ClinicsByName=[];
    this.getAllClinics()

  }


async getClinicPhones(id: number) {
    const phoneNumbers = await firstValueFrom(this.clinicService.getClinicsPhoneNumbers(id));
    if (phoneNumbers) {
      this.ClinicPhone = [];
      phoneNumbers.forEach(ph => this.ClinicPhone.push(ph.phoneNumber));
      const result = await Swal.fire({
        title: 'Clinic Phone Numbers',
        html: this.ClinicPhone.join('<br>'),
        icon: 'info',
        confirmButtonText: 'OK'
      });

      if (result.isConfirmed) {
        this.ClinicPhone = [];
      }
    } else {
      console.error('No phone numbers found');
    }
}

  getStars(rate: number): number[] {
    return Array(rate).fill(0).map((x, i) => i);
  }
  //for pagination
  nextPage(): void {
    if (this.pageNumber < this.totalitems) {
      this.pageNumber++;
      this. getAllClinics();
    }
  }

  prevPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this. getAllClinics();
    }
  }
  get totalitems(): number {
    return Math.ceil(this.totalPages / this.pageSize);
  }

  //for auot correct
  hideSuggestions() {
    this.clinicSuggestions = [];
   }

   getClinicSuggestions(input: string): string[] {
    return this.ClinicNames.filter(clinic => clinic.toLowerCase().includes(input.toLowerCase()));
  }

  onInputChange() {
    this.clinicSuggestions = this.getClinicSuggestions(this.clinicSearch);
    this.noResults = this.clinicSuggestions.length === 0;
  }

  getClinicsName() {
    this.ClinicNames = [];  // Make sure ClinicNames is reset
    this.MainData.forEach(element => {
      this.ClinicNames.push(element.name);
    });
    this.ClinicNames = [...new Set(this.ClinicNames)]; // Remove duplicates using Set
  }
  selectClinic(clinic: string) {
    this.clinicSearch = clinic;
    this.clinicSuggestions = [];
    this.searchClinics();
  }
}


