import { Component, Input } from '@angular/core';
import { SecreteryReservationVaccine } from '../../Models/secretery-reservation-vaccine';
import { SecrteryReservationService } from '../../Services/secrtery-reservation.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-secrtery-reservation-vaccine',
  standalone: true,
  imports: [FormsModule,CommonModule],
  templateUrl: './secrtery-reservation-vaccine.component.html',
  styleUrl: './secrtery-reservation-vaccine.component.css'
})
export class SecrteryReservationVaccineComponent {
  @Input() id: number | undefined;
  reservations:SecreteryReservationVaccine[]=[]

  constructor(private reservService: SecrteryReservationService) { }

  ngOnInit(): void {
    if (this.id) {
      this.reservService.getallReservationVaccineByCID(this.id).subscribe(data => {
        this.reservations = data;
      });
    }
  }
}
