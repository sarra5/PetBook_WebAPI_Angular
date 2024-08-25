import { Component, OnInit, TemplateRef } from '@angular/core';
import { UserPetInfoServiceService } from '../../../Services/user-pet-info-service.service';
import { UserPetInfo } from '../../../Models/user-pet-info';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AccountServiceService } from '../../../Services/account-service.service';
import { CommonModule } from '@angular/common';
import { MyRequestService } from '../../../Services/my-request.service';
import Swal from 'sweetalert2';
import { Dialog, DialogRef } from '@angular/cdk/dialog';


@Component({
  selector: 'app-userProfile-pet-info',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './userProfile-pet-info.component.html',
  styleUrls: ['./userProfile-pet-info.component.css']
})
export class UserProfilePetInfoComponent implements OnInit {
  userPetList: UserPetInfo[] = [];
  userPetInfoSub: Subscription | null = null;
  userID: number = Number(this.account.r.id);
  petInfo: UserPetInfo | null = null;
  url: string = 'https://localhost:7066/Resources/';
  //here
  myDialogRef: DialogRef<any> | null = null;

  constructor(
    public userpetInfoService: UserPetInfoServiceService,
    public account: AccountServiceService,
    public router: Router,
    public requestForBreedService: MyRequestService,
    //here
    private dialog: Dialog
  ) {}

  ngOnInit(): void {
    this.Upload();
  }
//here
  openDialog(template: TemplateRef<unknown>) {
    // you can pass additional params, choose the size and much more
    this.myDialogRef = this.dialog.open(template);
 }

  Upload(): void {
    this.userpetInfoService.getPetByUserId(this.userID).subscribe({
      next: (UserPetInfoData) => {
        this.userPetList = UserPetInfoData;
        

        this.userPetList.forEach(element => {
          element.photo = this.url + element.photo;
          element.idNoteBookImage = this.url + element.idNoteBookImage;
          element.isReadyForBreeding = element.readyForBreeding;  // Initialize the property

          this.userpetInfoService.isPaired(element.petID).subscribe({
            next: (d) => {
              if (d.petIDSender == element.petID) {
                element.pairWith = d.receiverPetName;
                element.PairedWithPetID = d.petIDReceiver
              } else {
                element.pairWith = d.senderPetName;
                element.PairedWithPetID = d.petIDSender
              }
            },
            error: (error) => {
              element.pairWith = "I'm not paired";
            }
          });
        });
      }
    });
  }


  navigateToAdd() {
    this.router.navigateByUrl("PetRegister");
  }

  toggleBreedingStatus(pet: UserPetInfo) {
    if (pet.isReadyForBreeding) {
      // Handle the case when the pet is already ready for breeding
      this.requestForBreedService.makeThisPetBeNotReadyForBreeding(pet.petID).subscribe({
        next: (d) => {
          Swal.fire("Your Pet Is Not Ready For Breeding");
          this.requestForBreedService.DeleteALLReq(pet.petID).subscribe({
            next:(d)=>{console.log(d)}
          });
        }
      });
    } else {
      // Handle the case when the pet is not ready for breeding
      this.requestForBreedService.makeThisPetBeReadyForBreeding(pet.petID).subscribe({
        next: (d) => {
          console.log(d);
          Swal.fire("Your Pet Is Ready For Breeding");
        }
      });
    }
    pet.isReadyForBreeding = !pet.isReadyForBreeding;  // Toggle the breeding status
  }



UnPair(id: number) {
  Swal.fire({
    title: 'Are you sure?',
    text: 'Do you want to unpair this pet?',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText: 'Yes, unpair it!',
    cancelButtonText: 'No, keep it paired'
  }).then((result) => {
    if (result.isConfirmed) {
      this.requestForBreedService.deletePair(id).subscribe({
        next: (d) => {
          console.log(d);
        }
      }); 
      Swal.fire('Unpaired!', 'The pet has been unpaired.', 'success');
      this.Upload();
      }
  });
}

GoToPairedPetDetails(pairedPetId:number){
  this.router.navigateByUrl(`/Pet/details/${pairedPetId}/false`)
}

deletePet(id:number){
  Swal.fire({
    title: 'Are you sure?',
    text: 'Do you want to delete this pet?',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText: 'Yes, delete it!',
    cancelButtonText: 'No, keep it'
  }).then((result) => {
    if (result.isConfirmed) {
      this.userpetInfoService.deletePet(id).subscribe({
        next: (d) => {
          console.log(d); 
          this.Upload();
        }
      });
      Swal.fire('Delete!', 'The pet has been deleted.', 'success');
      
    }
   
  });
}

  navigateToEdit(id: number) {
    this.router.navigateByUrl(`userPetEdit/${id}`);
  }
}
