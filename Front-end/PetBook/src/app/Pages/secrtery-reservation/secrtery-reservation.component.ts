import { Component, Input } from '@angular/core';
import { SecrteryReservation } from '../../Models/secrtery-reservation';
import { SecrteryReservationService } from '../../Services/secrtery-reservation.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-secrtery-reservation',
  standalone: true,
  imports: [FormsModule,CommonModule],
  templateUrl: './secrtery-reservation.component.html',
  styleUrl: './secrtery-reservation.component.css'
})
export class SecrteryReservationComponent {
  @Input() id: number | undefined;
  reservations:SecrteryReservation[]=[]

  constructor(private reservService: SecrteryReservationService) { }

  ngOnInit(): void {
    if (this.id) {
      this.reservService.getallReservationByCID(this.id).subscribe(data => {
        this.reservations = data;
      });
    }
  }
}
