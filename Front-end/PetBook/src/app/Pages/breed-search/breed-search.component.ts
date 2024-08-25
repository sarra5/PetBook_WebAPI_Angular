import { NgFor, NgForOf, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BreedDetails } from '../../Models/breed-details';
import { PetDetails } from '../../Models/pet-details';
import { HttpClient } from '@angular/common/http';
import { AutoCorrectService } from '../../Services/auto-correct.service';
import { SignalRServiceService } from '../../Services/signal-rservice.service';
import { Router,ActivatedRoute } from '@angular/router';
import { AccountServiceService } from '../../Services/account-service.service';
import { BreedSearchService } from '../../Services/breed-search.service';
//import { SetFilterModule } from '@ag-grid-enterprise/set-filter';
//import { AgGridModule } from '@ag-grid-community/angular';
//import { flush } from '@angular/core/testing';

@Component({
  selector: 'app-breed-search',
  standalone: true,
  imports: [FormsModule, NgFor, NgIf, NgForOf],
  templateUrl: './breed-search.component.html',
  styleUrl: './breed-search.component.css'
})
export class BreedSearchComponent implements OnInit {

  pets: PetDetails[] = []; // get list of pets available for breeding
  AllPets:PetDetails[]=[];
  type:string="";
  sex:string="";
  search:string="";
  FlagToBack:Boolean=false;
  breedSuggestions:string[]=[]
  CountIsZero:boolean=false;
  pageNumber: number = 1;
  pageSize: number =9;
  totalPages: number = 0;
  OwnderId:number= parseInt(this.AccountService.r.id);



  url: string = 'https://localhost:7066/Resources/';

  constructor(private http: HttpClient, 
    public autoCorrectService: AutoCorrectService, 
    public signalRService:SignalRServiceService,
     public router:Router,
     public AccountService:AccountServiceService,
     public breedSearchService: BreedSearchService
     ) { }
  ngOnInit() {
    this.signalRService.startConnection()
    this.signalRService.PetWithReadyForBreedTrueListener((pet) => {
      pet.photo = this.url+pet.photo
      this.pets.push(pet)
    })
    this.signalRService.PetWithReadyForBreedFalseListener((pet) => {
      this.pets.forEach(element => {
        if(element.petID == pet.petID){
          this.pets = this.pets.filter(item => item !== element)
        }
      });
    })
    this.hideSuggestions();
    this.fetchPets();
    this.GetSuggesstions()
  }

  fetchPets() {
    this.breedSearchService.getPetsReadyForBreeding(this.OwnderId,this.pageNumber, this.pageSize,this.type,this.sex,this.search).subscribe(
      pets => {
        this.hideSuggestions();
        this.pets = pets.Data;
        this.totalPages = pets.totalItems;
        this.CountIsZero= this.pets.length===0;
        if(this.CountIsZero){
          this.totalPages=this.pageSize
        }
        window.scrollTo({ top: 0, behavior: 'smooth' });
      },
      error => {
        console.error('Error fetching pets:', error);
      }
    );
  }
  CallToFetch(){
    this.pageNumber=1;
    this.FlagToBack=true;
    this.fetchPets()
  }


  hideSuggestions() {
    this.breedSuggestions = [];
  }
  chooseme(id:number){
    this.router.navigateByUrl(`Pet/details/${id}`)
  }

  nextPage(): void {
    if (this.pageNumber < this.totalitems) {
      this.pageNumber++;
      this.fetchPets();
    }
  }

  prevPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.fetchPets();
    }
  }
  get totalitems(): number {
    return Math.ceil(this.totalPages / this.pageSize);
  }
  

  GetSuggesstions(){
    this.breedSearchService.GetBreedNames().subscribe(
      data=>{
        this.breedSuggestions=data
    })
  }

  onInputChange() {
    this.breedSuggestions = this.autoCorrectService.getBreedSuggestions(this.search);
  }

  FunctionTakesSuggAndBindIt(sugg:string){
    this.search=sugg;
    this.pageNumber=1;
    this.FlagToBack=true
    this.fetchPets();
  }
  BackToPets(){
    this.sex=""
    this.type=""
    this.search=""
    this.FlagToBack=false;
    this.fetchPets();
  }

}


