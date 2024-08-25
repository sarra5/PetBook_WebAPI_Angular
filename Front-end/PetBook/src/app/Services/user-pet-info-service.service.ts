import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserPetInfo } from '../Models/user-pet-info';
import { AddPet } from '../Models/add-pet';
import { RequestBreed } from '../Models/request-breed';
import { EditPet } from '../Models/edit-pet';
import { PetBreedEdit } from '../Models/pet-breed-edit';

@Injectable({
  providedIn: 'root'
})
export class UserPetInfoServiceService {
  baseURL= "https://localhost:7066/api/Pet";
  requestBreedUrl= "https://localhost:7066/api/RequestBreed/retunIfItPaired";
  URL_PetBreed= "https://localhost:7066/api/PetBreed";

  constructor(public http: HttpClient) {}

    getPetById(id:number){
      const url= `${this.baseURL}/${id}`;
      return this.http.get<EditPet>(url);
    }

    getPetByUserId(id : number):Observable<UserPetInfo[]>
    {
      const url= `${this.baseURL}/GetByUserID/${id}`;
      return this.http.get<UserPetInfo[]>(url);
    }

    isPaired(id:number):Observable<any>
    {
      const url= `${this.requestBreedUrl}/${id}`;
      return this.http.get<any>(url);
    }
    
    editUserPet(editedPet: EditPet, id: number):Observable<AddPet>
    {
      const formData = new FormData();
      formData.append('petID', id.toString());
      formData.append('name', editedPet.name);
      formData.append('ageInMonth', editedPet.ageInMonth.toString());
      formData.append('sex', editedPet.sex);
      formData.append('userID', editedPet.userID.toString());
      formData.append('readyForBreeding', editedPet.readyForBreeding.toString());
      formData.append('type',editedPet.type);
      formData.append('other', editedPet.other);
      
      if (editedPet.photo instanceof File) {
        formData.append('photo', editedPet.photo);
      }
      if (editedPet.idNoteBookImage instanceof File) {
        formData.append('idNoteBookImage', editedPet.idNoteBookImage);
      }
      return this.http.put<AddPet>(this.baseURL,formData);
    }

    EditUserPetBreed(petToEdit:PetBreedEdit){
      return this.http.put(this.URL_PetBreed,petToEdit);
    }

    deletePet(id:number): Observable<any>{
      return this.http.delete<any>(`${this.baseURL}/${id}`);
    }
}
