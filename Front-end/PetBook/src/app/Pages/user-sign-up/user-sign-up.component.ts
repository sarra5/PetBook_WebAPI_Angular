import { Component, SimpleChanges } from '@angular/core';
import { InputSectionComponent } from '../../Components/input-section/input-section.component';
import { UserClient } from '../../Models/user-client';
import { AccountServiceService } from '../../Services/account-service.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-user-sign-up',
  standalone: true,
  imports: [InputSectionComponent, FormsModule, CommonModule],
  templateUrl: './user-sign-up.component.html',
  styleUrl: './user-sign-up.component.css'
})
export class UserSignUpComponent {
  user: UserClient = new UserClient('', '', '', '', '', '', null, '', null, 2);

  // Validation flags
  validationErrors: { [key in keyof UserClient]?: boolean | string | null } = {};

  constructor(public accountService:AccountServiceService, public router:Router, private snackBar: MatSnackBar){  }

  isFormValid(): boolean {
    let isValid = true;

    for (const key in this.user) {
      if (this.user.hasOwnProperty(key)) {
        const field = key as keyof UserClient;
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

  SignUp() {
    if (this.isFormValid()) {
      this.accountService.SignUp(this.user).subscribe({
        next: (response) => {
          this.router.navigate(['/Login'], { queryParams: { email: this.user.email, password: this.user.password } });
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

  onInputValueChange(event: { field: keyof UserClient, value: any }) {
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
      this.validationErrors.photo = false;

      // Update the image preview
      const reader = new FileReader();
      reader.onload = (e: any) => {
        const photoPreview = document.getElementById('photoPreview') as HTMLImageElement;
        photoPreview.src = e.target.result;
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
    this.router.navigateByUrl("");
  }
}
