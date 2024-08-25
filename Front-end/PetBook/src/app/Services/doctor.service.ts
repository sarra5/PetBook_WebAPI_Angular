import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Doctor } from '../Models/doctor';
import { Observable } from 'rxjs';
import { DoctorUser } from '../Models/doctor-user';

@Injectable({
  providedIn: 'root'
})
export class DoctorService {

  url='https://localhost:7066/api/Doctor';
  constructor(public http: HttpClient) { }

  updateDoctor(cdoctorData: Doctor): Observable<Doctor> {
    console.log(cdoctorData);
    return this.http.put<Doctor>(this.url, cdoctorData);
  }

  addUserAndDoctor(data: DoctorUser, clinicId: number): Observable<any> {
    const formData = new FormData();
    formData.append('name', data.name);
    formData.append('email', data.email);
    formData.append('password', data.password);
    formData.append('phone', data.phone);
    formData.append('userName', data.userName);
    formData.append('location', data.location);
    if(data.age !== null){
      formData.append('age', data.age.toString());
    }
    formData.append('sex', data.sex);
    formData.append('degree', data.degree);
    formData.append('hiringDate', data.hiringDate.toString());

    if (data.photo) {
      formData.append('photo', data.photo);
    }

    const url = `${this.url}/adduserfirstthenadddoctor?ClinicId=${clinicId}`;
    return this.http.post<any>(url, formData);
  }
}
