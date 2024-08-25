import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Secretary } from '../Models/secretary';
import { catchError, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SecretaryServiceService {

  constructor(public http: HttpClient) { }

  url='https://localhost:7066/api/Secretary';
  getallSendingReq(id: number) {
    return this.http.get<Secretary>(this.url+"/getallinfoaboutThisClinicBySID/" + id).pipe(
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

}
