import { Component, Input } from '@angular/core';
import { Doctor } from '../../Models/doctor';
import { ClinicService } from '../../Services/clinic.service';
import { MatDialog } from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';
import { CommonModule, NgFor, NgIf } from '@angular/common';
import { EditDoctorDialogComponent } from '../edit-doctor-dialog/edit-doctor-dialog.component';
import { DoctorService } from '../../Services/doctor.service';
import { AddNewDoctorComponent } from '../add-new-doctor/add-new-doctor.component';
import { DoctorUser } from '../../Models/doctor-user';
import { ClinicDoctorService } from '../../Services/clinic-doctor.service';

@Component({
  selector: 'app-secrtery-doctor',
  standalone: true,
  imports: [FormsModule,CommonModule,NgFor,NgIf],
  templateUrl: './secrtery-doctor.component.html',
  styleUrl: './secrtery-doctor.component.css'
})
export class SecrteryDoctorComponent {


  @Input() id: number | undefined;
  doctor: Doctor[] =[];

  constructor(private clinicService: ClinicService,private dialog: MatDialog ,private doctorService:DoctorService,private doctorClinic: ClinicDoctorService) { }

  ngOnInit(): void {
    console.log(this.id);
    if (this.id) {
      this.clinicService.getDoctors(this.id).subscribe(data => {
        this.doctor = data;
      });
    }
  }

  openEditDialog(doctor1:Doctor) {
    const dialogRef = this.dialog.open(EditDoctorDialogComponent, {
      width: '400px',
      data: { doctor: doctor1 }
    });

    dialogRef.afterClosed().subscribe((updateddoctor: Doctor) => {
      if (updateddoctor) {
        console.log(updateddoctor);
        this.doctorService.updateDoctor(updateddoctor).subscribe(data => {
          const index = this.doctor.findIndex(d => d.doctorID === updateddoctor.doctorID);
          if (index !== -1) {
            this.doctor[index] = updateddoctor;
          }
        }, error => {
          alert('Email and User_Name Should be Unique' );
        });
      }
    });
  }

  openAddDialog(): void {
    const dialogRef = this.dialog.open(AddNewDoctorComponent, {
      width: '400px',
      data: {  }
    });

    dialogRef.afterClosed().subscribe((newDoctor: DoctorUser) => {
      if(newDoctor&&this.id){
        this.doctorService.addUserAndDoctor(newDoctor,this.id).subscribe(data => {
          if(this.id)
          this.clinicService.getDoctors(this.id).subscribe(data => {
            this.doctor = [];
            this.doctor = data;
          });

        }, error => {
          alert(' Email and User_Name Should be Unique' );
        });
      }
    });
  }

  deleteDoctor(Did: number) {
    if(this.id)
     this.doctorClinic.deleteClinicDoctor(this.id,Did).subscribe(data => {
      this.doctor = this.doctor.filter(d => d.doctorID !== Did);
      console.log(this.doctor);
    });
  }

}
