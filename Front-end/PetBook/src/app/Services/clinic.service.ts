import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Clinic } from '../Models/clinic';
import { Doctor } from '../Models/doctor';
import { Reservation } from '../Models/reservation';
import { ClinicPhones } from '../Models/clinic-phones';
import { ClinicLocation } from '../Models/clinic_location';

@Injectable({
  providedIn: 'root'
})
export class ClinicService {
  
  private apiUrl = 'https://localhost:7066/api/Clinic'; 
  private apiUrlPhoneNumber = 'https://localhost:7066/api/ClinicPhone/clinic'; 
  private apiUrl2 = 'https://localhost:7066/'; 

  constructor(private http: HttpClient) { }

  getClinicbyId(id: number): Observable<Clinic> {
    return this.http.get<Clinic>(`${this.apiUrl2}api/Clinic/id?id=${id}`);
  }

  getInfoClinicbyname(clinicName: string): Observable<any> {
    return this.http.get(`${this.apiUrl2}api/ClinicLocation/search/${clinicName}`);
  }

  getDoctors(clinicId: number): Observable<Doctor[]> {
    console.log(this.apiUrl2+"api/Doctor/"+clinicId+"/doctors");
    return this.http.get<Doctor[]>(`${this.apiUrl2}api/Doctor/${clinicId}/doctors`);
  }

  bookAppointment(reservation: Reservation): Observable<any> {
    return this.http.post(`${this.apiUrl2}api/Reservation`, reservation);
  }
  
/////////////////////////////////////////////////////////////////

  getAllClinics(pageNumber: number, pageSize: number): Observable<{ data: ClinicLocation[],allData:ClinicLocation[] ,totalItems: number }>{
    let params = new HttpParams()
    .set('pageNumber', pageNumber.toString())
    .set('pageSize', pageSize.toString());
    return this.http.get<{ data: ClinicLocation[],allData:ClinicLocation[], totalItems: number }>(`${this.apiUrl}/Clinics`, { params });
  }

  getClinicByName(name: string){
    return this.http.get<any>(`${this.apiUrl}/Name?name=${name}`);
  }
  
  getClinicsPhoneNumbers(id:number){
    return this.http.get<ClinicPhones[]>(`${this.apiUrlPhoneNumber}/${id}`)
  }

  updateClinic(clinicData: Clinic): Observable<Clinic> {
    console.log(clinicData);
    return this.http.put<Clinic>(this.apiUrl, clinicData);
  }
}
