import { Component, OnInit } from '@angular/core';
import { AccountServiceService } from '../../../Services/account-service.service';
import { RouterLink, RouterOutlet } from '@angular/router';
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterOutlet, RouterLink,CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent implements OnInit{

  //added by salma ///////////
  user: { UserName: string, Name: string, id: string, RoleId: string } | null = null;

constructor(private account : AccountServiceService){}
id : string= "";
role:string|undefined=""
Name:string|undefined=""
Logout(){
  this.account.logout();
  this.id="";
}

ngOnInit(): void {
  this.account.r$.subscribe(user => {
    this.user = user;
  });

  // JavaScript for toggling mobile menu
  document.getElementById('mobile-menu-toggle')?.addEventListener('click', () => {
    document.getElementById('mobile-menu')?.classList.toggle('hidden');
  });
  this.role=this.user?.RoleId.toString();
  this.Name=this.user?.Name;  
  }
}