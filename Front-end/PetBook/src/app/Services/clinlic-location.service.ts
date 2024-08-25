import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { VaccineClinicLocation } from '../Models/vaccine-clinic-location';
import { ClinicLocation } from '../Models/clinic_location';
import { Observable } from 'rxjs';
import { clinicLocationonly } from '../Models/clinic-locationonly';

@Injectable({
  providedIn: 'root'
})
export class ClinlicLocationService {

  url='https://localhost:7066/api/ClinicLocation/locationinclude/';
  getUrl='https://localhost:7066/api/ClinicLocation/location/';
  private baseUrl = 'https://localhost:7066/api/ClinicLocation';

  constructor(private http: HttpClient) {}

  GatClincsWithLocations(id:number){
    return this.http.get<VaccineClinicLocation[]>(`${this.url}${id}`)
  }

  getallLocationByClinicId(id:number){
    return this.http.get<clinicLocationonly[]>(`${this.getUrl}${id}`)
  }

  deleteClinicLocation(clinicId: number, location: string): Observable<any> {
    const url = `${this.baseUrl}?ClinicID=${clinicId}&Location=${encodeURIComponent(location)}`;
    return this.http.delete<any>(url, { responseType: 'text' as 'json' }); // Specify responseType as 'text'
  }

  addLocation(clinicId: number, location: string): Observable<any> {
    return this.http.post<any>(this.baseUrl, { clinicID: clinicId, location},{ responseType: 'text' as 'json' });
  }
}
