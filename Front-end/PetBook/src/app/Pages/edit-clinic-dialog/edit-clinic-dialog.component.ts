import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-edit-clinic-dialog',
  standalone: true,
  imports: [FormsModule,CommonModule],
  templateUrl: './edit-clinic-dialog.component.html',
  styleUrl: './edit-clinic-dialog.component.css'
})
export class EditClinicDialogComponent {
  clinic: any;

  constructor(
    public dialogRef: MatDialogRef<EditClinicDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.clinic = { ...data.clinic }; // Clone the clinic data to avoid modifying original
  }

  onSubmit() {
    this.dialogRef.close(this.clinic); // Pass updated clinic data back to parent component
    console.log(this.clinic);
  }

  closeDialog() {
    this.dialogRef.close();
  }
}