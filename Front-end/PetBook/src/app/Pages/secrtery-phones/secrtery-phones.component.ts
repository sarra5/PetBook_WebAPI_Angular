import { Component, Input } from '@angular/core';
import { ClinicPhones } from '../../Models/clinic-phones';
import { ClinicPhonesService } from '../../Services/clinic-phones.service';
import { MatDialog } from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-secrtery-phones',
  standalone: true,
  imports: [FormsModule,CommonModule],
  templateUrl: './secrtery-phones.component.html',
  styleUrl: './secrtery-phones.component.css'
})
export class SecrteryPhonesComponent {
  @Input() id: number | undefined;
  phones: ClinicPhones[] = [];
  showAddLocation: boolean = false;
  newLocation: string = '';

  constructor(private clinicPhServ: ClinicPhonesService, private dialog: MatDialog) { }

  ngOnInit(): void {
    if (this.id) {
      this.clinicPhServ.getallLocationByClinicId(this.id).subscribe(data => {
        this.phones = data;
      });
    }
  }

  toggleAddLocation() {
    this.showAddLocation = true;
  }

  cancelAddLocation() {
    this.showAddLocation = false;
    this.newLocation = ''; // Reset newLocation input
  }

  saveLocation() {
    if (this.id&&this.newLocation) {
      console.log(this.id,this.newLocation);
      this.clinicPhServ.addLocation(this.id, this.newLocation).subscribe((data) => {
        if (this.id) {
          this.clinicPhServ.getallLocationByClinicId(this.id).subscribe(data => {
            this.phones = data;
          });
        }       
        this.showAddLocation = false;
        this.newLocation = ''; // Reset newLocation input
      }, error => {
        console.error('Error :', error);
      });

    }
  }

  delete(ph: string) {
    if (this.id) {
      this.clinicPhServ.deleteClinicLocation(this.id, ph).subscribe(data => {
        this.phones = this.phones.filter(l => l.phoneNumber !== ph);
      });
    }
  }
}
