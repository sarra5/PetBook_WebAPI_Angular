import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ClinicReservation } from '../Models/clinic-reservation';

@Injectable({
  providedIn: 'root'
})
export class ClinicReservationsService {
  constructor(public http: HttpClient) { }
  url= "https://localhost:7066/api/Reservation"
  

  getClinicReservationByPetId(id :number) {
    return this.http.get<ClinicReservation[]>(`${this.url}/GetReservationForClinicbypetID/${id}`)
  }

  deleteClinicReservation(PetID:number,ClinicID:number){
    return this.http.delete<any>(`${this.url}/${PetID}/${ClinicID}`)
  }
}
