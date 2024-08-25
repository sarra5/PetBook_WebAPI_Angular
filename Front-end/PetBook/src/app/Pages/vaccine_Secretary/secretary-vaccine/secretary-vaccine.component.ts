import { Component, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { NgFor } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { VaccineClinicsService } from '../../../Services/vaccine-clinics.service';
import { EditVaccineDialogComponent } from '../edit-vaccine-dialog/edit-vaccine-dialog.component';
import { DeleteVaccineDialogComponent } from '../delete-vaccine-dialog/delete-vaccine-dialog.component';
import { AddVaccineDialogComponent } from '../add-vaccine-dialog/add-vaccine-dialog.component';
import { VaccineCliniccAdd } from '../../../Models/vaccine-clinicc-add';


@Component({
  selector: 'app-secretary-vaccine',
  standalone: true,
  imports: [NgFor],
  templateUrl: './secretary-vaccine.component.html',
  styleUrl: './secretary-vaccine.component.css'
})
export class SecretaryVaccineComponent {
  @Input() ClinicId: number | undefined;
  vaccines: any[] = [];

  constructor(private route: ActivatedRoute, private vaccineClinicsService: VaccineClinicsService,private dialog: MatDialog) { }

  

  ngOnInit(): void {
     if(this.ClinicId)
      this.fetchVaccines(this.ClinicId);

    }
  

  fetchVaccines(clinicId:number): void {
    this.vaccineClinicsService.getVaccinesByClinicId(clinicId).subscribe(data => {
      this.vaccines = data;
    });
  }

  editVaccine(vaccineId: number): void {
    const vaccineToEdit = this.vaccines.find(vaccine => vaccine.vaccineID === vaccineId);

    if (!vaccineToEdit) {
      console.error(`Vaccine with ID ${vaccineId} not found.`);
      return;
    }

    const dialogRef = this.dialog.open(EditVaccineDialogComponent, {
      width: '400px',
      data: { vaccine: vaccineToEdit }
    });

    dialogRef.afterClosed().subscribe(updatedVaccine => {
      if (updatedVaccine) {
        this.vaccineClinicsService.updateVaccine(updatedVaccine).subscribe(response => {
          Object.assign(vaccineToEdit, updatedVaccine);
        }, error => {
          console.error('Error updating vaccine:', error);
        });
      }
    });
  }

  deleteVaccine(vaccineId: number): void {
    const vaccineToDelete = this.vaccines.find(vaccine => vaccine.vaccineID === vaccineId);
  
    if (!vaccineToDelete) {
      console.error(`Vaccine with ID ${vaccineId} not found.`);
      return;
    }
  
    const dialogRef = this.dialog.open(DeleteVaccineDialogComponent, {
      width: '300px',
      data: { vaccine: vaccineToDelete }
    });
  
    dialogRef.afterClosed().subscribe(result => {
      if (result && this.ClinicId) {
        this.vaccineClinicsService.deleteVaccine(vaccineId, this.ClinicId).subscribe(
          (response: any) => {
            this.vaccines = this.vaccines.filter(vaccine => vaccine.vaccineID !== vaccineId);
            this.refreshVaccines();
          },
          (error: any) => {
            console.error('Error deleting vaccine:', error);
          }
        );
      }
    });
  }
  refreshVaccines(): void {
    if (this.ClinicId) {
      this.vaccineClinicsService.getVaccinesByClinicId(this.ClinicId).subscribe(data => {
        this.vaccines = data;
      });
    }
  }
  
  addVaccine(): void {
    const dialogRef = this.dialog.open(AddVaccineDialogComponent, {
      width: '400px',
      data: {clinicId:this.ClinicId,vaccine: new VaccineCliniccAdd(0, '', 0, 0,'') }
      
    });
  

    dialogRef.afterClosed().subscribe(newVaccine => {
      if (newVaccine) {
        this.vaccineClinicsService.addVaccine(newVaccine).subscribe(
          (response: any) => {
            const addedVaccine: VaccineCliniccAdd = response; 
            this.vaccines.push(addedVaccine);
            this.refreshVaccines();
          },
          (error: any) => {
            console.error('Error adding vaccine:', error);
          }
        );
      }
    });
  }
}