import { Component, OnInit } from '@angular/core';
import { SecretaryServiceService } from '../../Services/secretary-service.service';
import { ActivatedRoute, Router, RouterLink, RouterOutlet } from '@angular/router';
import { AccountServiceService } from '../../Services/account-service.service';
import { Secretary } from '../../Models/secretary';
import { UserService } from '../../Services/user.service';
import { UserDetails } from '../../Models/UserDetails';
import { SecrteryclinicComponent } from '../secrteryclinic/secrteryclinic.component';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { SecrteryDoctorComponent } from '../secrtery-doctor/secrtery-doctor.component';
import { SecrteryLocationComponent } from '../secrtery-location/secrtery-location.component';
import { SecrteryPhonesComponent } from '../secrtery-phones/secrtery-phones.component';
import { SecretaryVaccineComponent } from '../vaccine_Secretary/secretary-vaccine/secretary-vaccine.component';
import { SecrteryReservationComponent } from '../secrtery-reservation/secrtery-reservation.component';
import { SecrteryReservationVaccineComponent } from '../secrtery-reservation-vaccine/secrtery-reservation-vaccine.component';

@Component({
  selector: 'app-secretary',
  standalone: true,
  imports: [RouterOutlet, RouterLink , SecrteryclinicComponent,FormsModule,CommonModule,SecrteryDoctorComponent,SecrteryLocationComponent,SecrteryPhonesComponent,SecretaryVaccineComponent,SecrteryReservationComponent,SecrteryReservationVaccineComponent] ,
  templateUrl: './secretary.component.html',
  styleUrl: './secretary.component.css'
})
export class SecretaryComponent implements OnInit {

   Secretary:number | undefined ;
   clinicId:number | undefined ;
   SecrteryclinicInfo :Secretary | undefined;
   private imageUrlBase: string = 'https://localhost:7066/Resources/';
   User:UserDetails=new UserDetails(0,"","","","","","",0,"","",0);
   selectedTab: string = 'clinic';

  constructor(
    public secrService: SecretaryServiceService,
    public activateRoute: ActivatedRoute,
    public AccountService:AccountServiceService ,
    public userService:UserService,
    private router: Router
  ) {}
  ngOnInit(): void {
    this.Secretary= parseInt(this.AccountService.r.id);
    this.secrService.getallSendingReq(this.Secretary).subscribe(data => {
      this.SecrteryclinicInfo = data;
      this.Secretary=data.secretaryID;
      this.clinicId=data.clinicID;
      this.loadUserData(data.secretaryID);
    });
  }

  loadUserData(userId: number): void {
    this.userService.getUserById(userId).subscribe(user => {
      user.photo = this.imageUrlBase + user.photo;
      this.User = user;
    });
  }

  selectTab(tab: string | null): void {
    if (tab) {
      this.selectedTab = tab;
    }
  }
  }

