import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RequestBreed } from '../Models/request-breed';
import { catchError } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';
import { PetDetails } from '../Models/pet-details';

@Injectable({
  providedIn: 'root'
})
export class MyRequestService {

  constructor(public http: HttpClient) { }

  baseUrl = 'https://localhost:7066/api/RequestBreed/UserSenderID/';
  url = 'https://localhost:7066/api/RequestBreed/';
  pendingUrl='https://localhost:7066/api/RequestBreed/UserReceiverID/';
  updateUrl='https://localhost:7066/api/RequestBreed';

  // petUrl='https://localhost:7066/api/Pet/id?id=';
  petUrl='https://localhost:7066/api/Pet/';
  
  



  getallSendingReq(id: number) {
    return this.http.get<RequestBreed[]>(this.baseUrl + id).pipe(
      catchError(this.handleError)
    );
  }

  DeleteReq(SId: number, RId: number) {
    const deleteUrl = `${this.url}${SId}/${RId}`;
    console.log('DELETE request URL:', deleteUrl);
    return this.http.delete(deleteUrl, { responseType: 'text' }).pipe(
      catchError(this.handleError)
    );
  }

  DeleteALLReq(Id: number) {
    const deleteUrl = `${this.url}deletePetFromRequestsAndPendingR/${Id}`;
    console.log('DELETE request URL:', deleteUrl);
    return this.http.delete(deleteUrl, { responseType: 'text' }).pipe(
      catchError(this.handleError)
    );
  }

  getallPendingReq(id: number) {
    return this.http.get<RequestBreed[]>(this.pendingUrl + id).pipe(
      catchError(this.handleError)
    );
  }

  makeThisPetBeNotReadyForBreeding(id: number): Observable<any> {
    return this.http.get<any>(`${this.url}Turnthispettobenotavailable/${id}`).pipe(
      catchError(this.handleError)
    );
  }


  CheckIfThisPetOndate(id: number) {
    return this.http.get<PetDetails>(`${this.petUrl}${id}`).pipe(
      catchError(this.handleError)
    );
    // return this.http.get<PetDetails>(this.petUrl+id).pipe(
    //   catchError(this.handleError)
    // );
  }

    updateRequestBreed(petIDSender: number, petIDReceiver: number, pair: boolean): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'accept': '*/*'
    });

    const body = {
      petIDSender: petIDSender,
      petIDReceiver: petIDReceiver,
      pair: pair
    };

    console.log('PUT request URL:', this.updateUrl);
    console.log('Request Body:', body);

    return this.http.put<any>(this.updateUrl, body, { headers: headers }).pipe(
      catchError(this.handleError)
    );
  }

  makeThisPetBeReadyForBreeding(id: number): Observable<any> {
    return this.http.get<any>(`${this.url}Turnthispettobeavailable/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  deletePair(id: number){
    return this.http.delete<any>(`${this.url}deletePiar/${id}`)
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
