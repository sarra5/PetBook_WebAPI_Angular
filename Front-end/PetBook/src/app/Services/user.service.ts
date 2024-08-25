import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserDetails } from '../Models/UserDetails';
import { UserUpdateDetails } from '../Models/user-update-details';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseurl="https://localhost:7066/api/User"

  constructor(public http: HttpClient) { }



  getUserById(id: number) {
    const params = new HttpParams().set('id', id);
    return this.http.get<UserDetails>(`${this.baseurl}/id`, {params});
  }

  
  getuserinfo(id: number) {
    return this.http.get<UserDetails>(this.baseurl + "/"+id).pipe(
    );
  }


  updateUser(id: number, user: UserUpdateDetails) {
    const formData = new FormData();

formData.append('Id', user.userID ? user.userID.toString() : '');
  formData.append('Name', user.name || '');
  formData.append('UserName', user.userName || '');
  formData.append('Email', user.email || '');
  formData.append('Password', user.password || '');
  formData.append('Age', user.age ? user.age.toString() : '');
  formData.append('Sex', user.sex || '');
  formData.append('Location', user.location || '');
  formData.append('Phone', user.phone || '');
  formData.append('RoleID', user.roleID ? user.roleID.toString() : '');

  if (user.photo) {
    formData.append('Photo', user.photo);
  }
  console.log(user)
    return this.http.put(`${this.baseurl}/id?id=${id}`, formData);
  }

  updateUserPhoto(id: number, photo: File) {
    const formData = new FormData();
    formData.append('Photo', photo);
    return this.http.put(`${this.baseurl}/id?id=${id}`, formData);
  }
}

