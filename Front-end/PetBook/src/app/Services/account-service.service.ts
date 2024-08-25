import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { MatSnackBar } from '@angular/material/snack-bar';
import { UserClient } from '../Models/user-client';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountServiceService {
  //////////////////////////////added by salma
  private rSubject = new BehaviorSubject<{ UserName: string, Name: string, id: string, RoleId: string } | null>(null);
  r$ = this.rSubject.asObservable();
  isAuthenticated = !!localStorage.getItem("token"); // Check if token exists
  ///////////////////////////////////////////////////
  // r ==> to get the claim results in this variable
  r: { UserName:string, Name:string, id:string, RoleId:string } = { UserName:"", Name:"", id:"", RoleId:"" };
  constructor(public http:HttpClient, private router: Router, private snackBar: MatSnackBar) 
  { 
    this.CheckToken()
  }
////commented by salma
  // isAuthenticated=false;
  baseUrl="https://localhost:7066/api/Account";
  
  private CheckToken(): void {
    const token = localStorage.getItem("token");
    if (token) {
      this.isAuthenticated = true;
      this.r = jwtDecode(token);
      // console.log(this.r.UserName);
      // console.log(this.r.Name);
      // console.log(this.r.id);
      // console.log(this.r.RoleId);
      //added by salma//////////////
      this.rSubject.next(jwtDecode(token)); // Update rSubject with user data
    } else {
      this.isAuthenticated = false;
      this.rSubject.next(null);
    }
  }

  Login(email: string, password: string) {
    const params = new HttpParams().set('email', email).set('password', password);
    
    this.http.get(`${this.baseUrl}?email=${email}&password=${password}`, { params, responseType: 'text' })
    .subscribe(d => {
      this.isAuthenticated = true;
      localStorage.setItem("token", d);
      try {
        this.r = jwtDecode(d);
        console.log(this.r);
        /////added by salma///////////
        this.rSubject.next(this.r);
        ////////////////////////////////
        if(this.r.RoleId=="2")
        this.router.navigateByUrl("");
      else 
      if(this.r.RoleId=="3")
        this.router.navigateByUrl("Secretary");
      } catch (error) {
        console.error('Failed to decode token:', error);
      }
    },
      (error: HttpErrorResponse) => {
        if (error.status === 401) {
          // Show a snackbar for invalid email or password
          this.snackBar.open('Invalid email or password', 'Close', {
            duration: 5000, // Duration in milliseconds
            verticalPosition: 'top' // Position of the snackbar
          });
        } else if(email == "" && password != ""){
            this.snackBar.open('Email can not be empty', 'Close', {
              duration: 5000, // Duration in milliseconds
              verticalPosition: 'top' // Position of the snackbar
            });
        }
        else if(password == "" && email != ""){
          this.snackBar.open('Password can not be empty', 'Close', {
            duration: 5000, // Duration in milliseconds
            verticalPosition: 'top' // Position of the snackbar
          });
        }
        else{
          this.snackBar.open('Please enter the data', 'Close', {
            duration: 5000, // Duration in milliseconds
            verticalPosition: 'top' // Position of the snackbar
          });
        }
      }
    );
  }
 
  logout(){
    this.isAuthenticated=false;
    localStorage.removeItem("token");
    //added by salma////////////////
    this.rSubject.next(null);
  }

  SignUp(user:UserClient){
    const formData = new FormData();
    formData.append('name', user.name);
    formData.append('email', user.email);
    formData.append('password', user.password);
    formData.append('phone', user.phone);
    formData.append('userName', user.userName);
    formData.append('location', user.location);
    if(user.age !== null){
      formData.append('age', user.age.toString());
    }
    formData.append('sex', user.sex);
    formData.append('roleID', user.roleID.toString());

    if (user.photo) {
      formData.append('photo', user.photo);
    }

    return this.http.post(`${this.baseUrl}/Register`, formData);
  }
}
 
 