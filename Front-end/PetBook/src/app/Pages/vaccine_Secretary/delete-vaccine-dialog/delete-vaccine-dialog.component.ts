import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-delete-vaccine-dialog',
  standalone: true,
  imports: [],
  templateUrl: './delete-vaccine-dialog.component.html',
  styleUrl: './delete-vaccine-dialog.component.css'
})
export class DeleteVaccineDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DeleteVaccineDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  closeDialog(): void {
    this.dialogRef.close(false); 
  }

  confirmDelete(): void {
    this.dialogRef.close(true); 
  }
}
