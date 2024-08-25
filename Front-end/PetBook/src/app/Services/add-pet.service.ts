import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AddPet } from '../Models/add-pet';
import { AddBreedToPet } from '../Models/add-breed-to-pet';
import { Breed } from '../Models/breed';
import { PetBreed } from '../Models/pet-breed';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AddPetService {
  private baseurl="https://localhost:7066/api"

  constructor(public http: HttpClient) { }

  AddPet(pet: AddPet) {
    
    const formData = new FormData();
    formData.append('name', pet.name);
    if(pet.ageInMonth !== null){
      formData.append('ageInMonth', pet.ageInMonth.toString());
    }
    formData.append('sex', pet.sex);
    formData.append('userID', pet.userID.toString());
    if(pet.readyForBreeding !== null){
      formData.append('readyForBreeding', pet.readyForBreeding.toString());
    }
    formData.append('type', pet.type);
    if(pet.other !== null){
      formData.append('other', pet.other);
    }
    
    if (pet.photo && pet.idNoteBookImage) {
      formData.append('photo', pet.photo);
      formData.append('idNoteBookImage', pet.idNoteBookImage);
      }
    
    return this.http.post(`${this.baseurl}/Pet`, formData);
  }

  AddPetBreed(petBreed: AddBreedToPet){
    const formData = new FormData();
    formData.append('petID', petBreed.petID.toString());
    formData.append('breedID', petBreed.breedID.toString());
    return this.http.post(`${this.baseurl}/PetBreed`, formData);
  }

  getBreed(){
    return this.http.get<Breed[]>(`${this.baseurl}/Breed`)
  }

  getPetBreed(id:number):Observable<PetBreed>{
    const url= `${this.baseurl}/PetBreed/getByPetId/${id}`
    return this.http.get<PetBreed>(url);
  }
}
