import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ClinicService } from '../../Services/clinic.service';
import { switchMap } from 'rxjs';
import { Clinic } from '../../Models/clinic';
import { Doctor } from '../../Models/doctor';
import { Reservation } from '../../Models/reservation';
import { ClinicInfo } from '../../Models/clinic-info';
import { CommonModule, NgClass, NgFor } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AccountServiceService } from '../../Services/account-service.service';
import { ClinicDoctor } from '../../Models/clinic-doctor';
import { ClinicDoctorService } from '../../Services/clinic-doctor.service';
import { PairPetsDialogComponent } from '../PetInfo/pair-pets-dialog/pair-pets-dialog.component';
import { PetDetailsService } from '../../Services/pet-details.service';
import { PetDetails } from '../../Models/pet-details';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-clinic',
  standalone: true,
  imports: [NgFor,CommonModule,FormsModule],
  templateUrl: './clinic.component.html',
  styleUrl: './clinic.component.css'
})
export class ClinicComponent {
  choosePet() {
    this.openPairPopup();
  }
  clinic: Clinic = new Clinic(0, "", 0, "");
  appointment: Reservation = new Reservation(1, 1, new Date(2020, 0, 6));
  clinicName: string = 'Default Clinic Name';
  clinicInfo: ClinicInfo | null = null;
  stars: number[] = [1, 2, 3, 4, 5];
  ClinicDoctors: ClinicDoctor[] = []

  userPets: PetDetails[] = [];
  petId: number | null = null;
  userId:number | null = null;
  errorMessage: string | null = null;
   url:string='https://localhost:7066/Resources/'

  constructor(private route: ActivatedRoute, private snackBar: MatSnackBar  ,private dialog: MatDialog,private petDetailsService: PetDetailsService,private clinicService: ClinicService, public account:AccountServiceService, private clinicDoctor: ClinicDoctorService) { }

  ngOnInit(): void {
    this.userId= parseInt(this.account.r.id);
    this.loadUserPets();
    this.route.params.pipe(
      switchMap(params => {
        const clinicId = +params['clinicId'];
        return this.clinicService.getClinicbyId(clinicId);
      })
    ).subscribe(clinic => {
      this.clinic = clinic;
      this.getDoctors(clinic.clinicID);
      this.searchClinicByName(clinic.name);
    });
  }
  loadUserPets(): void {
    if(this.userId){
    this.petDetailsService.getUserPets(this.userId).subscribe(
      pets => {
        this.userPets = pets.map(pet => ({
          ...pet,
          photo: this.url + pet.photo
        }));
        console.log(this.userPets)
      },
      error => {
        console.error('Error fetching user pets:', error);
        this.snackBar.open('Failed to fetch user pets', 'Close', {
          duration: 3000,
        });
      }
    );
  }
  }
  getDoctors(clinicId: number): void {
    this.clinicDoctor.GetDoctorsByClinicID(clinicId).subscribe(data => {
      this.ClinicDoctors = data.map((doctor: any) => {
        doctor.photo = this.url + doctor.photo;
        return doctor;
      });
      console.log(this.ClinicDoctors);
    });
  }

  bookAppointment(): void {
    if (this.petId !== null) {
      const reservation = new Reservation(
        this.petId,
        this.clinic.clinicID,
        this.appointment.date
      );

      console.log('Booking appointment with:', reservation);

      this.clinicService.bookAppointment(reservation).subscribe(
        response => {
          console.log('Appointment booked successfully:', response);
          this.errorMessage = null; // Clear any previous error message
          this.snackBar.open('Appointment booked successfully', 'Close', {
            duration: 9000,
          horizontalPosition: 'center',
          verticalPosition: 'top'
          });
        },
        error => {
          console.error('Error booking appointment:', error);
          if (error.status === 400 && error.error === 'Reservation already exists') {
            this.errorMessage = 'Failed to book appointment: Reservation already exists for the selected clinic and pet.';
          } else if (error.status === 500 && error.error.error) {
            this.errorMessage = `Failed to book appointment: ${error.error.error}`;
          } else {
            this.errorMessage = 'Failed to book appointment: An unexpected error occurred.';
          }
        }
      );
    }
  }


  openPairPopup(): void {

    const dialogRef = this.dialog.open(PairPetsDialogComponent, {
      width: '400px',
      data: { pets: this.userPets }
    });

    dialogRef.afterClosed().subscribe(selectedPetId => {
      if (selectedPetId) {
        this.petId=selectedPetId;
      }
    });
  }



  searchClinicByName(name: string): void {
    this.clinicService.getInfoClinicbyname(name).subscribe(data => {
      console.log('Clinic Info:', data);
      if (Array.isArray(data) && data.length > 0) {
        this.clinicInfo = data[0];
      }
      else {
        this.clinicInfo = null;
      }
    }, error => {
      console.error('Error fetching clinic info:', error);
    });
  }
}
