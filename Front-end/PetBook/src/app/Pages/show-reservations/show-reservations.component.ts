import { Component, OnInit } from '@angular/core';
import { AccountServiceService } from '../../Services/account-service.service';
import { ClinicReservationsService } from '../../Services/clinic-reservations.service';
import { ClinlicVccineReservationService } from '../../Services/clinlic-vccine-reservation.service';
import { UserPetInfoServiceService } from '../../Services/user-pet-info-service.service';
import { UserPetInfo } from '../../Models/user-pet-info';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { VaccineReservation } from '../../Models/vaccine-reservation';
import { ClinicReservation } from '../../Models/clinic-reservation';
import { ClinicService } from '../../Services/clinic.service';
import { ClinicPhones } from '../../Models/clinic-phones';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-show-reservations',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './show-reservations.component.html',
  styleUrl: './show-reservations.component.css'
})
export class ShowReservationsComponent implements OnInit  {
  constructor(public AccountService: AccountServiceService,
              public clinicReservationService: ClinicReservationsService,
              public vaccineReservationService: ClinlicVccineReservationService,
              public userPetInfoService: UserPetInfoServiceService,
              public ClinicService: ClinicService
  ){}

  userID: number = Number(this.AccountService.r.id);
  userPetList: UserPetInfo[] = [];
  reservationClinic: ClinicReservation[]=[];
  reservationVacc: VaccineReservation[]=[];

  ngOnInit(): void {
    this.fetchUserPets();
  }

  fetchUserPets(): void {
    this.userPetInfoService.getPetByUserId(this.userID).subscribe({
      next: (UserPetInfoData) => {
        this.userPetList = UserPetInfoData;
        this.fetchReservations();
      }
    });
  }

  fetchReservations(): void {
    this.userPetList.forEach(pet => {
      this.clinicReservationService.getClinicReservationByPetId(pet.petID).subscribe({
        next: (clinicReservations) => {
          clinicReservations.forEach(clinicRes => {
            this.reservationClinic.push(clinicRes);
            this.fetchClinicPhones(clinicRes.clinicID, 'clinic');
          });
        }
      });

      this.vaccineReservationService.getVaccineReservationByPetId(pet.petID).subscribe({
        next: (vaccineReservations) => {
          vaccineReservations.forEach(vaccineRes => {
            this.reservationVacc.push(vaccineRes);
            this.fetchClinicPhones(vaccineRes.clinicID, 'vaccine');
          });
        }
      });
    });
  }

  fetchClinicPhones(clinicID: number, type: 'clinic' | 'vaccine'): void {
    this.ClinicService.getClinicsPhoneNumbers(clinicID).subscribe({
      next: (phonesList) => {
        if (type === 'clinic') {
          this.reservationClinic.forEach(clinic => {
            if (clinic.clinicID === clinicID) {
              clinic.clinicPhones = phonesList;
            }
          });
        } else if (type === 'vaccine') {
          this.reservationVacc.forEach(vaccine => {
            if (vaccine.clinicID === clinicID) {
              vaccine.Phones = phonesList;
            }
          });
        }
      }
    });
  }
  
  DeleteClinicReservation( PetID:number,clinicID:number){
    Swal.fire({
      title: 'Are you sure?',
      text: 'Do you want to delete this pet?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, delete it!',
      cancelButtonText: 'No, keep it'
    }).then((result) => {
      if (result.isConfirmed) {
        this.reservationClinic = this.reservationClinic.filter(reservation => !(reservation.petID === PetID && reservation.clinicID === clinicID));
        this.clinicReservationService.deleteClinicReservation(PetID,clinicID).subscribe({
          next: (d) => {
            console.log(d); 
            Swal.fire('Delete!', 'The pet has been deleted.', 'success'); 
          }
        });
      }
    });
  }

  DeleteVaccReservation(PetID:number,clinicID:number, VaccID: number){
    Swal.fire({
      title: 'Are you sure?',
      text: 'Do you want to delete this pet?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, delete it!',
      cancelButtonText: 'No, keep it'
    }).then((result) => {
      if (result.isConfirmed) {
        this.reservationVacc = this.reservationVacc.filter(reservation => !(reservation.petID === PetID && reservation.clinicID === clinicID && reservation.vaccineID===VaccID));
        this.vaccineReservationService.DeleteVaccineDialogComponent(VaccID,clinicID,PetID).subscribe({
          next: (d) => {
            console.log(d); 
            Swal.fire('Delete!', 'The pet has been deleted.', 'success'); 
          }
        });
      }
    });
  }
}
