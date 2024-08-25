import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PetDetails } from '../Models/pet-details';

@Injectable({
  providedIn: 'root'
})
export class BreedSearchService {
  private url: string = 'https://localhost:7066/Resources/';
  private baseApiUrl: string = 'https://localhost:7066/api/Pet/';

  constructor(private http: HttpClient) { }


  getPetsReadyForBreeding(ownerId: number, pageNumber: number, pageSize: number,Type:string, Sex:string, Search:string ) {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString())
      .set('type',Type)
      .set('sex',Sex)
      .set('search',Search);
  
    return this.http.get<{ data: PetDetails[], allData: PetDetails[], totalItems: number }>(`${this.baseApiUrl}SearchPetsReadyForBreeding`, { params })
      .pipe(
        map(pets => {
          const filteredPets = pets.data.filter(pet => pet.userID !== ownerId).map(pet => {
            
            pet.photo = this.url + pet.photo;
            return pet;
          });
          return {
            Data: filteredPets,
            allData: pets.allData,
            totalItems: pets.totalItems
          };
        })
      );
  }
  
  //  searchBreedNameOfPetsReadyForBreeding(ownerId: number, breedName: string): Observable<PetDetails[]> {
  //   const encodedSearchQuery = encodeURIComponent(breedName);
  //   const searchUrl = `${this.baseApiUrl}SearchBreedNameOfPetsReadyForBreeding?name=${encodedSearchQuery}`;
  //   return this.http.get<PetDetails[]>(searchUrl).pipe(
  //     map(pets => pets.filter(pet => pet.userID !== ownerId).map(pet => {
  //       pet.photo = this.url + pet.photo;
  //       return pet;
  //     }))
  //   );
  // }
  GetBreedNames(){
    return this.http.get<string[]>(`${this.baseApiUrl}SearchBreedNameOfPetsReadyForBreeding`)
  }


  }