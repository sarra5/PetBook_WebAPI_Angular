import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { AddPet } from '../Models/add-pet';
@Injectable({
  providedIn: 'root'
})
export class SignalRServiceService {

  private hubConnection!: signalR.HubConnection;

  constructor() {}

  public startConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
    .withUrl('https://localhost:7066/PetHub')
    .build();

    this.hubConnection
      .start()
      .then(() => console.log('SignalR connection started'))
      .catch(err => console.error('Error starting SignalR connection:', err));
  }

  public PetWithReadyForBreedTrueListener(callback: (pet: any) => void) {
    this.hubConnection.on('PetWithReadyForBreedingTrue', (pet) => {
      callback(pet);
    });
  }
  
  public PetWithReadyForBreedFalseListener(callback: (pet: any) => void) {
    this.hubConnection.on('PetWithReadyForBreedingFalse', (pet) => {
      callback(pet);
    });
  }

}
