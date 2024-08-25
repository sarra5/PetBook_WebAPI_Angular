import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { VaccineClinic } from '../Models/vaccine-clinic';
import { Observable, map } from 'rxjs';
import { VaccineCliniccAdd } from '../Models/vaccine-clinicc-add';

@Injectable({
  providedIn: 'root'
})
export class VaccineClinicsService {

  url='https://localhost:7066/api/VaccineClinic';
  constructor(private http: HttpClient) {}

  GatAllClincsHasThisVaccine(id:number){
    return this.http.get<VaccineClinic[]>(`${this.url}/vaccineClinicInclude/${id}`)
  }

  getVaccinesByClinicId(clinicId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.url}/clinicc/${clinicId}`);
  }
  updateVaccine(vaccineDto: any): Observable<any> {
    return this.http.put(`${this.url}/updateVaccine`, vaccineDto);
  }

  deleteVaccine(vaccineId: number, clinicId: number): Observable<any> {
    const url = `${this.url}/${vaccineId}/${clinicId}`;
    return this.http.delete<any>(url);
  }
  addVaccine(vaccineDto: any): Observable<any> {
    return this.http.post(`${this.url}/addVaccine`, vaccineDto);
  }

}
