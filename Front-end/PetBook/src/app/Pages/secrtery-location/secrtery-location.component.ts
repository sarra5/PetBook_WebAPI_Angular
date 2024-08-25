import { Component, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ClinlicLocationService } from '../../Services/clinlic-location.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { clinicLocationonly } from '../../Models/clinic-locationonly';

@Component({
  selector: 'app-secrtery-location',
  standalone: true,
  imports: [FormsModule,CommonModule],
  templateUrl: './secrtery-location.component.html',
  styleUrl: './secrtery-location.component.css'
})
export class SecrteryLocationComponent {
  @Input() id: number | undefined;
  Locations: clinicLocationonly[] = [];
  showAddLocation: boolean = false;
  newLocation: string = '';

  constructor(private clinicLcServ: ClinlicLocationService, private dialog: MatDialog) { }

  ngOnInit(): void {
    if (this.id) {
      this.clinicLcServ.getallLocationByClinicId(this.id).subscribe(data => {
        this.Locations = data;
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
      this.clinicLcServ.addLocation(this.id, this.newLocation).subscribe((data) => {
        if (this.id) {
          this.clinicLcServ.getallLocationByClinicId(this.id).subscribe(data => {
            this.Locations = data;
          });
        }       
        this.showAddLocation = false;
        this.newLocation = ''; // Reset newLocation input
      }, error => {
        console.error('Error :', error);
      });

    }
  }

  delete(loc: string) {
    if (this.id) {
      this.clinicLcServ.deleteClinicLocation(this.id, loc).subscribe(data => {
        this.Locations = this.Locations.filter(l => l.location !== loc);
      });
    }
  }
}
