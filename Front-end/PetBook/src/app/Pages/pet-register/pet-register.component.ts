import { Component, OnInit } from '@angular/core';
import { InputSectionComponent } from '../../Components/input-section/input-section.component';
import { AddPet } from '../../Models/add-pet';
import { AddBreedToPet } from '../../Models/add-breed-to-pet';
import { Breed } from '../../Models/breed';
import { AddPetService } from '../../Services/add-pet.service';
import { FormsModule } from '@angular/forms';
import { AccountServiceService } from '../../Services/account-service.service';
import { CommonModule } from '@angular/common';
import { UserPetInfoServiceService } from '../../Services/user-pet-info-service.service';
import { lastValueFrom, switchMap, tap } from 'rxjs';
import { Router } from '@angular/router';
import { SignalRServiceService } from '../../Services/signal-rservice.service';

@Component({
  selector: 'app-pet-register',
  standalone: true,
  imports: [InputSectionComponent, FormsModule, CommonModule],
  templateUrl: './pet-register.component.html',
  styleUrl: './pet-register.component.css'
})
export class PetRegisterComponent implements OnInit {
  constructor(public addPet:AddPetService, public account:AccountServiceService, 
    public userPetInfo:UserPetInfoServiceService, private router:Router){}

  ngOnInit(): void {
    this.addPet.getBreed().subscribe({
      next: (response) => {
        this.breedsList = response
      },
      error:(err) =>{
        console.log(err)
      }
    })
    this.Pet.userID = +this.account.r.id
  }

  Pet:AddPet={
    name:"",
    photo: null,
    idNoteBookImage: null,
    ageInMonth: null,
    sex: "",
    userID:1,
    readyForBreeding: null,
    type: "",
    other: null,
  }

  breedPet:AddBreedToPet={
    petID:0,
    breedID:0,
  }

  breedsList:Breed[]=[]

  validationErrorsForPet: { [key in keyof AddPet]?: boolean | string | null } = {};
  validationErrorsForBreedPet: { [key in keyof AddBreedToPet]?: boolean | string | null } = {};

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.Pet.photo = file;
      this.validationErrorsForPet.photo = false;

      // Update the image preview
      const reader = new FileReader();
      reader.onload = (e: any) => {
        const photoPreview = document.getElementById('photoPreview') as HTMLImageElement;
        photoPreview.src = e.target.result;
      };
      reader.readAsDataURL(file);
    }
  }

  triggerFileInput() {
    const fileInput = document.getElementById('fileInput') as HTMLInputElement;
    fileInput.click();
  }

  onFileSelectedBookImage(event:any){
    this.Pet.idNoteBookImage = event.target.files[0];
    this.validationErrorsForPet.idNoteBookImage = false;
  }


  isFormValid(): boolean {
    let isValid = true;

    for (const key in this.Pet) {
      if (this.Pet.hasOwnProperty(key)) {
        const fieldPet = key as keyof AddPet;
        if (!this.Pet[fieldPet]) {
          this.validationErrorsForPet[fieldPet] = true;
          isValid = false;
        } else {
          this.validationErrorsForPet[fieldPet] = false;
        }
      }
    }
    
    for (const key in this.breedPet) {
      if (this.breedPet.hasOwnProperty(key)) {
        const fieldBreedPet = key as keyof AddBreedToPet;
        if (!this.breedPet[fieldBreedPet] && fieldBreedPet != "petID") {
          this.validationErrorsForBreedPet[fieldBreedPet] = true;
          isValid = false;
        } else {
          this.validationErrorsForBreedPet[fieldBreedPet] = false;
        }
      }
    }

    return isValid;
  }

  async SignUp() {
    this.breedPet.breedID = +this.breedPet.breedID
    if(this.isFormValid()){
      try {
        // Step 1: Add the pet
        const addPetResponse = await lastValueFrom(this.addPet.AddPet(this.Pet));
        console.log('Pet added successfully:', addPetResponse);
    
        // Step 2: Get pet information by user ID
        const petInfoResponse = await lastValueFrom(this.userPetInfo.getPetByUserId(this.Pet.userID)) || [];
    
        petInfoResponse.forEach(element => {
          if (element.name === this.Pet.name && element.ageInMonth === this.Pet.ageInMonth && element.other === this.Pet.other &&
              element.type === this.Pet.type && element.sex === this.Pet.sex) {
            this.breedPet.petID = element.petID;
          }
        });
    
        // Step 3: Add pet breed
        const addPetBreedResponse = await lastValueFrom(this.addPet.AddPetBreed(this.breedPet));
        console.log('Pet breed added successfully:', addPetBreedResponse);

        console.log('Pet readyForBreeding value:', this.Pet.readyForBreeding); 

        this.router.navigateByUrl("/Profile/userPetInfo")
    
        // Success message or any further actions after successful signup
      } catch (err: any) {
        if (err.error && err.error.errors) {
          // Log and display validation errors
          console.log('Validation errors:', err.error.errors);
          // Handle displaying errors to the user, e.g., show them in a dialog or form
        } else {
          // Handle other types of errors
          console.error('An error occurred:', err);
          // Display a generic error message to the user
        }
      }
    }
  }

  onInputValueChangeForPet(event: { field: keyof AddPet, value: any }) {
    const { field, value } = event;
    if (field in this.Pet) {
      (this.Pet as any)[field] = value;
      if (value) {
        this.validationErrorsForPet[field] = false;
      }
    }
  }
  
  onInputValueChangeForBreedPet(event: { field: keyof AddBreedToPet, value: any }) {
    const { field, value } = event;
    if (field in this.breedPet) {
      (this.breedPet as any)[field] = value;
      if (value) {
        this.validationErrorsForBreedPet[field] = false;
      }
    }
  }

  onSexChange(event: any) {
    const selectedValue = event.target.value;
    // Assuming you have validation logic, update validationErrors object accordingly
    this.validationErrorsForPet.sex = selectedValue ? null : 'Gender is required.';
  }
  
  onTypeChange(event: any) {
    const selectedValue = event.target.value;
    // Assuming you have validation logic, update validationErrors object accordingly
    this.validationErrorsForPet.type = selectedValue ? null : 'Type is required.';
  }
  
  onTBreedChange(event: any) {
    const selectedValue = event.target.value;
    // Assuming you have validation logic, update validationErrors object accordingly
    this.validationErrorsForBreedPet.breedID = selectedValue ? null : 'Breed is required.';
  }
  
  onReadyForBreedChange(event: any) {
    const selectedValue = event.target.value;
    // Assuming you have validation logic, update validationErrors object accordingly
    this.validationErrorsForPet.readyForBreeding = selectedValue ? null : 'Is Ready for breeding is required.';
  }

  Cancel(){
    this.router.navigateByUrl("Profile/userPetInfo");
  }
}
