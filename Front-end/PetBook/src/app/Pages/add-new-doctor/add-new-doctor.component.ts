import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { DoctorUser } from '../../Models/doctor-user';

@Component({
  selector: 'app-add-new-doctor',
  standalone: true,
  imports: [FormsModule,CommonModule],
  templateUrl: './add-new-doctor.component.html',
  styleUrl: './add-new-doctor.component.css'
})
export class AddNewDoctorComponent {
  DoctorUs = {
    name: '',
    email: '',
    password: '',
    phone: '',
    userName: '',
    location: '',
    age: null,
    sex: '',
    degree: '',
    hiringDate: '',
    photo: ''
  };  doc:any;
  constructor(
    public dialogRef: MatDialogRef<AddNewDoctorComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    }

  onSubmit() {
    this.dialogRef.close(this.DoctorUs); // Pass updated clinic data back to parent component
    console.log(this.DoctorUs);
  }

  closeDialog() {
    this.dialogRef.close();
  }

  triggerFileInput() {
    const fileInput = document.getElementById('fileInput') as HTMLInputElement;
    fileInput.click();
  }
  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.DoctorUs.photo= file;


    }
  }
}