import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../../Services/user.service';
import { AccountServiceService } from '../../../Services/account-service.service';
import { UserDetails } from '../../../Models/UserDetails';
import { UserUpdateDetails } from '../../../Models/user-update-details';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-update-user-details',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './update-user-details.component.html',
  styleUrl: './update-user-details.component.css'
})
export class UpdateUserDetailsComponent {

  user:UserUpdateDetails=new UserUpdateDetails(0,"","","","","","",0,"",null,null,0);

  validationErrors: { [key in keyof UserUpdateDetails]?: boolean | string | null } = {};

  userid:number=parseInt(this.account.r.id);

  private imageUrlBase: string = 'https://localhost:7066/Resources/';

  constructor(public userService:UserService
    ,public activatedRoute:ActivatedRoute,
  public router:Router ,
  public account:AccountServiceService
  , private snackBar: MatSnackBar
  ){}

  ngOnInit(): void {
    this.loadUserData(this.userid)
  }

  loadUserData(userId: number): void {
    this.userService.getUserById(userId).subscribe(user => {
      user.photo = this.imageUrlBase + user.photo;
      this.user = user;
      ///
      this.user.previewPhoto = user.photo;
      if(this.user.photo == "https://localhost:7066/Resources/null"){
        this.updateImagePreview("../../../../assets/Images/null.jpg");
      }
      else{
        this.updateImagePreview(user.photo);
      }
      ///
      console.log(user)
      console.log(this.user.previewPhoto)
      console.log(this.user.photo)
    });
  }

  updateImagePreview(src: string | ArrayBuffer | null) {
    const photoPreview = document.getElementById('photoPreview') as HTMLImageElement;
    photoPreview.src = src as string;
  }

  save(){
    if (this.isFormValid()){
      const updateUser = { ...this.user };
  
      this.userService.updateUser(this.userid,this.user).subscribe({
        next: (response) => {
          this.router.navigateByUrl("/Profile/Account");
        },
        error: (err: HttpErrorResponse) => {
          if (err.status === 409) {
            // Show a snackbar for email already exists
            this.snackBar.open('Email already exists, please use a different email.', 'Close', {
              duration: 5000, // Duration in milliseconds
              verticalPosition: 'top' // Position of the snackbar
            });
          } else if (err.status === 400) {
            // Log the validation errors from the server
            console.error('Validation errors:', err.error.errors);
            this.snackBar.open('Validation errors occurred. Please check your input.', 'Close', {
              duration: 5000, // Duration in milliseconds
              verticalPosition: 'top' // Position of the snackbar
            });
          } else {
            console.log(err);
          }
        }
      });
    }
  }

  isFormValid(): boolean {
    let isValid = true;

    for (const key in this.user) {
      if (this.user.hasOwnProperty(key)) {
        const field = key as keyof UserUpdateDetails;
        if (!this.user[field] && field !== 'age' && field !== "photo") {
          this.validationErrors[field] = true;
          isValid = false;
        } else {
          this.validationErrors[field] = false;
        }
      }
    }

    return isValid;
  }

  onInputValueChange(event: { field: keyof UserUpdateDetails, value: any }) {
    const { field, value } = event;
    if (field in this.user) {
      (this.user as any)[field] = value;
      if (value) {
        this.validationErrors[field] = false;
      }
    }
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.user.photo = file;

      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.user.previewPhoto = e.target.result;
        this.updateImagePreview(e.target.result);
      };
      reader.readAsDataURL(file);
    }
  }

  triggerFileInput() {
    const fileInput = document.getElementById('fileInput') as HTMLInputElement;
    fileInput.click();
  }

  onSexChange(event: any) {
    const selectedValue = event.target.value;
    // Assuming you have validation logic, update validationErrors object accordingly
    this.validationErrors.sex = selectedValue ? null : 'Gender is required.';
  }

  Cancel(){
    this.router.navigateByUrl("/Profile/Account");
  }
}
