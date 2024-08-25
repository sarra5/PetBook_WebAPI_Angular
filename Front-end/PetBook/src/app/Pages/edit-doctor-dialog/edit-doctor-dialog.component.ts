import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Doctor } from '../../Models/doctor';

@Component({
  selector: 'app-edit-doctor-dialog',
  standalone: true,
  imports: [FormsModule,CommonModule],
  templateUrl: './edit-doctor-dialog.component.html',
  styleUrl: './edit-doctor-dialog.component.css'
})
export class EditDoctorDialogComponent {
  doctor: Doctor;

  constructor(
    public dialogRef: MatDialogRef<EditDoctorDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.doctor = { ...data.doctor }; // Clone the clinic data to avoid modifying original
    console.log(this.doctor)
  }

  onSubmit() {
    this.dialogRef.close(this.doctor); // Pass updated clinic data back to parent component
    console.log(this.doctor);
  }

  closeDialog() {
    this.dialogRef.close();
  }
}