import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ClinlicVccineReservationService {

  reservationUrl='https://localhost:7066/api/ReservationForVaccine';
  constructor(public http: HttpClient) { }


  addReservationForVaccine(PetID: number, ClinicID: number, VaccineID: number, Date: Date): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'accept': '*/*'
    });

    const body = {
        PetID,
        ClinicID,
        VaccineID,
        Date
    };

    console.log('POST request URL:', this.reservationUrl);
    console.log('Request Body:', body);

    return this.http.post<any>(this.reservationUrl, body).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred.
      console.error('An error occurred:', error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      console.error(`Backend returned code ${error.status}, ` + `body was: ${error.error}`);
    }
    // Return an observable with a user-facing error message.
    return throwError('Something bad happened; please try again later.');
  }

  getVaccineReservationByPetId(id:number){
    return this.http.get<any[]>(`${this.reservationUrl}/GetReservationforVaccinebypetID/${id}`)
  }


  DeleteVaccineDialogComponent(VaccID:number,clinicID:number,PetID:number){
    return this.http.delete<any>(`${this.reservationUrl}/${VaccID}/${clinicID}/${PetID}`)
  }
}
