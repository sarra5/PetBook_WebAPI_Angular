import { Component, Inject } from '@angular/core';
import { FormsModule, NgModel } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-edit-vaccine-dialog',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './edit-vaccine-dialog.component.html',
  styleUrl: './edit-vaccine-dialog.component.css'
})
export class EditVaccineDialogComponent {
  editedVaccine: any; 

  constructor(
    public dialogRef: MatDialogRef<EditVaccineDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
   
    this.editedVaccine = { ...data.vaccine };
  }

  ngOnInit(): void {
  }

  submitForm(): void {
    
    this.dialogRef.close(this.editedVaccine); }

  closeDialog(): void {
    this.dialogRef.close(); 
}
}