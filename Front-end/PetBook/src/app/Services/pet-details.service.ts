import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PetDetails } from '../Models/pet-details';
import { Observable, catchError, throwError } from 'rxjs';
import { UserPetDetails } from '../Models/user-pet-details';
import { RequestForBreed } from '../Models/request-for-breed';

@Injectable({
  providedIn: 'root'
})
export class PetDetailsService {

  constructor(private http: HttpClient) { }
  baseurl = "https://localhost:7066";

  getPetDetails(petId: number): Observable<PetDetails> {
    const url = `${this.baseurl}/api/Pet/${petId}`;
  
    console.log('getPetDetails: URL', url);
  
    return this.http.get<PetDetails>(url).pipe(
      catchError(error => {
        console.error('Error in getPetDetails:', error);
        return throwError(error);
      })
    );
  }
  getuserDetails(userId: number): Observable<UserPetDetails> {
    console.log('getuserDetails: userId', userId);
    return this.http.get<UserPetDetails>(`${this.baseurl}/api/User/id`, { params: { id: userId.toString() } }).pipe(
      catchError(error => {
        console.error('Error in getuserDetails:', error);
        return throwError(error);
      })
    );
  }
  pairPets(petId: number, selectedPetId: number, userId: number): Observable<boolean> {
    return this.http.post<boolean>(`${this.baseurl}/api/Pet/${petId}/pair`, { selectedPetId, userId }).pipe(
      catchError(error => {
        console.error('Error in pairPets:', error);
        return throwError(error);
      })
    );
  }
  getUserPets(userId: number): Observable<PetDetails[]> {
    return this.http.get<PetDetails[]>(`${this.baseurl}/api/User/${userId}/Pets`).pipe(
      catchError(error => {
        console.error('Error in getUserPets:', error);
        return throwError(error);
      })
    );
  }
  
  
}

