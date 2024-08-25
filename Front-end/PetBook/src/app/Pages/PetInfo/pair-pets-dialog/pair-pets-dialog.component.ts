import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-pair-pets-dialog',
  standalone: true,
  imports: [CommonModule ],
  template:   `
  <div class="p-5">
    <p mat-dialog-title class="text-center text-3xl mb-4 text-[#051829] font-semibold">Select a Pet to Pair</p>
    <div class="background-image  rounded-lg shadow-lg overflow-y-scroll">
      <div mat-dialog-content class="w-full">
        <div *ngFor="let pet of data.pets" class="pet-item cursor-pointer mb-2 hover:bg-[#f0f0f0] flex justify-around items-center" (click)="selectPet(pet.petID)">
          <img [src]="pet.photo" alt="Pet" class="w-12 h-12 rounded-full mr-2 object-cover">
          <span class="text-lg text-orange">{{ pet.name }}</span>
        </div>
      </div>
    </div>
    <div mat-dialog-actions class="flex justify-center mt-5">
      <button mat-button class="bg-[#fe7936] px-4 py-2 rounded-lg font-semibold text-white hover:bg-gray-400" (click)="onNoClick()">Cancel</button>
    </div>
  </div>
`,
styles: [`
  .background-image {
            background-image: url('/assets/Images/PetInfoBG.png'); 
            background-size: cover;
            background-position: center;
            background-repeat: no-repeat;
            height: 30vh; 
            width: 100%;
        }
`]
})
export class PairPetsDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<PairPetsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  onNoClick(): void {
    this.dialogRef.close();
  }

  selectPet(petId: number): void {
    this.dialogRef.close(petId);
  }
}
