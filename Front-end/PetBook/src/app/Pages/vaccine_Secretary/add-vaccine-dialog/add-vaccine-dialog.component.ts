import { Component, Inject, NgModule } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { VaccineCliniccAdd } from '../../../Models/vaccine-clinicc-add';
import { CommonModule, NgFor, NgIf } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-add-vaccine-dialog',
  standalone: true,
  imports: [FormsModule,ReactiveFormsModule,CommonModule,],
  templateUrl: './add-vaccine-dialog.component.html',
  styleUrl: './add-vaccine-dialog.component.css'
})
export class AddVaccineDialogComponent {
  addVaccineForm: FormGroup;

  constructor(
    public dialogRef: MatDialogRef<AddVaccineDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { clinicId: number,vaccine: VaccineCliniccAdd },
    private fb: FormBuilder,
    private route: ActivatedRoute
  ) {
    this.addVaccineForm = this.fb.group({
  name: ['', Validators.required],
  description: '',
  clinicId: data.clinicId,
  price: ['', Validators.required],
  quantity: ['', Validators.required]
});
 console.log("id here:"+data.clinicId)

  }
  


  onNoClick(): void {
    this.dialogRef.close();
  }

  onSaveClick(): void {
    if (this.addVaccineForm.valid) {
      this.dialogRef.close(this.addVaccineForm.value);
    } else {
      // Mark all fields as touched to display validation errors
      this.addVaccineForm.markAllAsTouched();
    }
  }
}
