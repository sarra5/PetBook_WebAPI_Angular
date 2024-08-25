import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ClinlicVccineService {

  private baseUrl = 'https://localhost:7066/api/VaccineClinic';

  constructor(private http: HttpClient) {}


  


  decreaseNumberOfVaccine(VaccineId: number, ClinicID: number): Observable<any> {
    const url = `${this.baseUrl}/decreasetheNumberOfVaccine/${VaccineId}/${ClinicID}`;
    const headers = new HttpHeaders({
      'accept': '*/*'
    });
    console.log('PUT request URL:', url);
    return this.http.put(url, null, { headers: headers, responseType: 'text' }).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: any) {
    console.error('An error occurred', error);
    return throwError('Something went wrong; please try again later.');
  }
}