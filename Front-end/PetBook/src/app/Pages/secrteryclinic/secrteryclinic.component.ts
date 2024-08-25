import { Component, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ClinicService } from '../../Services/clinic.service';
import { Clinic } from '../../Models/clinic';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { EditClinicDialogComponent } from '../edit-clinic-dialog/edit-clinic-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-secrteryclinic',
  standalone: true,
  imports: [FormsModule,CommonModule],
  templateUrl: './secrteryclinic.component.html',
  styleUrl: './secrteryclinic.component.css'
})
export class SecrteryclinicComponent {
  @Input() id: number | undefined;
  clinic: Clinic | undefined;

  constructor(private clinicService: ClinicService,private dialog: MatDialog) { }

  ngOnInit(): void {
    if (this.id) {
      this.clinicService.getClinicbyId(this.id).subscribe(data => {
        this.clinic = data;
      });
    }
  }

  openEditDialog() {
    const dialogRef = this.dialog.open(EditClinicDialogComponent, {
      width: '400px',
      data: { clinic: this.clinic }
    });

    dialogRef.afterClosed().subscribe((updatedClinic: any) => {
      if (updatedClinic) {
        console.log(updatedClinic);
        this.clinicService.updateClinic(updatedClinic).subscribe(data => {
          this.clinic = updatedClinic;
        }, error => {
          console.error('Error updating clinic:', error);
        });
      }
    });
  }
}