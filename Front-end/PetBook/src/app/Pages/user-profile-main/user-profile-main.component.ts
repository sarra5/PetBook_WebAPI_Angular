import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-user-profile-main',
  standalone: true,
  imports: [RouterLink, RouterOutlet, RouterLinkActive],
  templateUrl: './user-profile-main.component.html',
  styleUrl: './user-profile-main.component.css'
})
export class UserProfileMainComponent {

}
