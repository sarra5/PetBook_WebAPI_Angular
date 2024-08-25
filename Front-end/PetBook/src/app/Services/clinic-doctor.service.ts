import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ClinicDoctor } from '../Models/clinic-doctor';

@Injectable({
  providedIn: 'root'
})
export class ClinicDoctorService {

  url='https://localhost:7066/api/ClinicDoctor';
  constructor(private http: HttpClient) { }

  deleteClinicDoctor(clid: number, dcid: number): Observable<void> {
    const url = `${this.url}/${clid}/${dcid}`;
    return this.http.delete<void>(url);
  }

  GetDoctorsByClinicID(cid:number){
    const url = `${this.url}/${cid}`;
    console.log(url)
    return this.http.get<ClinicDoctor[]>(url);
  }
}
