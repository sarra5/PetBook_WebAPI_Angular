import { Component, OnInit } from '@angular/core';
import { Clinic } from '../../Models/clinic';
import { Doctor } from '../../Models/doctor';
import { Reservation } from '../../Models/reservation';
import { ClinicInfo } from '../../Models/clinic-info';
import { ActivatedRoute } from '@angular/router';
import { ClinicService } from '../../Services/clinic.service';
import { AccountServiceService } from '../../Services/account-service.service';
import { switchMap } from 'rxjs/operators';
import { CommonModule, NgFor } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ClinlicVccineReservationService } from '../../Services/clinlic-vccine-reservation.service';
import { PetDetailsService } from '../../Services/pet-details.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { PetDetails } from '../../Models/pet-details';
import { PairPetsDialogComponent } from '../PetInfo/pair-pets-dialog/pair-pets-dialog.component';
import { ClinlicVccineService } from '../../Services/clinlic-vccine.service';
import { ClinicDoctorService } from '../../Services/clinic-doctor.service';
import { ClinicDoctor } from '../../Models/clinic-doctor';

@Component({
  selector: 'app-reservation-for-vaccine',
  standalone: true,
  imports: [NgFor, CommonModule, FormsModule],
  templateUrl: './reservation-for-vaccine.component.html',
  styleUrls: ['./reservation-for-vaccine.component.css']
})
export class ReservationForVaccineComponent implements OnInit {
choosePet() {
  this.openPairPopup();
}
  clinic: Clinic = new Clinic(0, "", 0, "");
  clinicID: number | null = null;
  VaccineID: number | undefined;
  petId: number | null = null;
  url:string='https://localhost:7066/Resources/'
  userPets: PetDetails[] = [];
  appointment: Reservation = new Reservation(1, 1, new Date(2020, 0, 6));
  clinicName: string = 'Default Clinic Name';
  clinicInfo: ClinicInfo | null = null;
  stars: number[] = [1, 2, 3, 4, 5];
  userId:number | null = null;
  ClinicDoctors: ClinicDoctor[] = []

  constructor(
    private route: ActivatedRoute,
    private VaccineclinicService: ClinlicVccineReservationService,
    private clinicService: ClinicService,
    private petDetailsService: PetDetailsService,
    public Account:AccountServiceService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog,
    private clicnicVaccineSer :ClinlicVccineService,
    private clinicDoctor: ClinicDoctorService
  ) { }

  ngOnInit(): void {
    // Subscribe to route params and fetch clinic details
    this.userId= parseInt(this.Account.r.id);
    this.loadUserPets();
    this.route.params.pipe(
      switchMap(params => {
        this.clinicID = +params['clinicId'];
        this.VaccineID = +params['VaccineId'];

        return this.clinicService.getClinicbyId(this.clinicID);

      })
    ).subscribe(clinic => {
      this.clinic = clinic;
      this.getDoctors(clinic.clinicID);
      this.searchClinicByName(clinic.name);
    });
  }

  // Fetch doctors for the clinic
  getDoctors(clinicId: number): void {
    this.clinicDoctor.GetDoctorsByClinicID(clinicId).subscribe(data => {
      this.ClinicDoctors = data.map((doctor: any) => {
        doctor.photo = this.url + doctor.photo;
        return doctor;
      });
      console.log(this.ClinicDoctors);
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

  // Book an appointment for the vaccine
  bookAppointment(): void {
    if (this.petId !== null && this.clinicID !== null && this.VaccineID !== undefined&& this.appointment.date !== undefined) {

      console.log(this.appointment.date);
      console.log(this.clinic.clinicID);
      this.VaccineclinicService.addReservationForVaccine(this.petId, this.clinicID, this.VaccineID, this.appointment.date)
        .subscribe(
          response => {
            alert('Appointment booked successfully');
          },
          error => {
            alert('Failed to book appointment');
          }
        );
        this.clicnicVaccineSer.decreaseNumberOfVaccine(this.VaccineID,this.clinicID)
    } else {
      alert('Please ensure all necessary information is provided and choose pet');
    }
  }

  // Search for clinic information by name
  searchClinicByName(name: string): void {
    this.clinicService.getInfoClinicbyname(name).subscribe(
      data => {
        console.log('Clinic Info:', data);
        if (Array.isArray(data) && data.length > 0) {
          this.clinicInfo = data[0];
        } else {
          this.clinicInfo = null;
        }
      },
      error => {
        console.error('Error fetching clinic info:', error);
      }
    );
  }
}
