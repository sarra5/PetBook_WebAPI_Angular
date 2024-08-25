import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ClinicPhones } from '../Models/clinic-phones';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ClinicPhonesService {
  url='https://localhost:7066/api/ClinicPhone';

  constructor(private http: HttpClient) {}
  getallLocationByClinicId(id:number){
    return this.http.get<ClinicPhones[]>(`${this.url}/clinic/${id}`)
  }

  deleteClinicLocation(clinicId: number, phones: string): Observable<any> {
    const url = `${this.url}/${clinicId}/${(phones)}`;
    return this.http.delete<any>(url, { responseType: 'text' as 'json' }); // Specify responseType as 'text'
  }

  addLocation(clinicId: number, phone: string): Observable<any> {
    console.log(clinicId, phone ,this.url);
    return this.http.post<any>(this.url, { clinicID:clinicId,phoneNumber: phone},{ responseType: 'text' as 'json' });
  }
}