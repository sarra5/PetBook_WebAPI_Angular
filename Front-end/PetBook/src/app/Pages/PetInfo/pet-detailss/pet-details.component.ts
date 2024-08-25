import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PetDetailsService } from '../../../Services/pet-details.service';
import { PetDetails } from '../../../Models/pet-details';
import { UserPetDetails } from '../../../Models/user-pet-details';
import { AccountServiceService } from '../../../Services/account-service.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { PairPetsDialogComponent } from '../pair-pets-dialog/pair-pets-dialog.component';

@Component({
  selector: 'app-pet-details',
  standalone: true,
  imports: [],
  templateUrl:'./pet-details.component.html',
  styleUrl: './pet-details.component.css'
})
export class PetDetailsComponent {
    pet: PetDetails | undefined;
    owner: UserPetDetails | undefined;
    url:string='https://localhost:7066/Resources/'
    userPets: PetDetails[] = [];
    selectedPetId: number | undefined;
    IsButtonShown:boolean = true

    constructor(
      private route: ActivatedRoute,
      private petDetailsService: PetDetailsService,
      public Account:AccountServiceService   ,
      private snackBar: MatSnackBar,
      private dialog: MatDialog,
     ) {}
  
    ngOnInit(): void {

      this.route.params.subscribe(params => {
        const petId = +params['id'];
        console.log('ngOnInit: petId', petId);
        this.loadPetDetails(petId);
        const userId = Number(this.Account.r.id)
        this.loadUserPets(userId);

        if (params['DoNotShowButton']) {
          this.IsButtonShown = params['DoNotShowButton'];
        }
      });
    }
  
    loadPetDetails(petId: number) {
      console.log('loadPetDetails: petId', petId);
      this.petDetailsService.getPetDetails(petId).subscribe(
        (pet: PetDetails) => {
          pet.photo=this.url+pet.photo
          pet.idNoteBookImage=this.url+pet.idNoteBookImage
          this.pet = pet;
          
          this.loadOwnerDetails(pet.userID);
        },
        error => {
          console.error('Error fetching pet details:', error);
        }
      );
    }
  
    loadOwnerDetails(userId: number) {
      console.log('loadOwnerDetails: userId', userId);
      this.petDetailsService.getuserDetails(userId).subscribe(
        (user: UserPetDetails) => {
          user.photo=this.url+user.photo
          this.owner = user;
        },
        error => {
          console.error('Error fetching user details:', error);
        }
      );
    }

    loadUserPets(userId: number): void {
      this.petDetailsService.getUserPets(userId).subscribe(
        pets => {
          this.userPets = pets.map(pet => ({
            ...pet,
            photo: this.url + pet.photo 
          }));
        },
        error => {
          console.error('Error fetching user pets:', error);
          this.snackBar.open('Failed to fetch user pets', 'Close', {
            duration: 3000,
          });
        }
      );
    }

    userPetsWithTrueValues:PetDetails[] = []

    openPairPopup(): void {
      this.userPetsWithTrueValues = []
      this.userPets.forEach(element => {
        if(element.readyForBreeding == true){
          this.userPetsWithTrueValues.push(element)
        }
      });

      const dialogRef = this.dialog.open(PairPetsDialogComponent, {
        width: '400px',
        data: { pets: this.userPetsWithTrueValues }
      });
  
      dialogRef.afterClosed().subscribe(selectedPetId => {
        if (selectedPetId) {
          this.pairPets(selectedPetId);
        }
      });
    }

      
      pairPets(selectedPetId: number): void {
        const petId = this.pet?.petID || 0;
        const userId = Number(this.Account.r.id);
    
        this.petDetailsService.pairPets(petId, selectedPetId, userId).subscribe(
          response => {
            if (response) {
              this.openSnackBar('Pet paired successfully', 'Close');
            } else {
              this.openSnackBar('The pet is not available for breeding', 'Close');
            }
          },
          error => {
            console.error('Pairing failed', error);
            this.openSnackBar('Pairing failed.', 'Close');
          }
        );
      }
    
      openSnackBar(message: string, action: string): void {
        this.snackBar.open(message, action, {
          duration: 9000,
          horizontalPosition: 'center',
          verticalPosition: 'top'
        });
      }
    
    
  }