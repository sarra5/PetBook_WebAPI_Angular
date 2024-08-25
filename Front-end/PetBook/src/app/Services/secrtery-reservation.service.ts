import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SecrteryReservation } from '../Models/secrtery-reservation';
import { SecreteryReservationVaccine } from '../Models/secretery-reservation-vaccine';

@Injectable({
  providedIn: 'root'
})
export class SecrteryReservationService {

  url='https://localhost:7066/api/Reservation/clinicIncludeUserInfo/'
  url2='https://localhost:7066/api/ReservationForVaccine/reservation_For_VaccineRepository/'
  constructor(public http: HttpClient) { }

  getallReservationByCID(id: number) {
    return this.http.get<SecrteryReservation[]>(this.url + id).pipe(
    );
  }

  getallReservationVaccineByCID(id: number) {
    return this.http.get<SecreteryReservationVaccine[]>(this.url2 + id).pipe(
    );
  }


}
