import { Component } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { UserService } from '../../../Services/user.service';
import { AccountServiceService } from '../../../Services/account-service.service';
import { UserDetails } from '../../../Models/UserDetails';

@Component({
  selector: 'app-user-details',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './user-details.component.html',
  styleUrl: './user-details.component.css'
})
export class UserDetailsComponent {

User:UserDetails=new UserDetails(0,"","","","","","",0,"","",0);
userid :number=0;
private imageUrlBase: string = 'https://localhost:7066/Resources/';
  constructor(public userService:UserService,public AccountService:AccountServiceService ,public  router:Router){}

  ngOnInit(): void {
    this.userid = parseInt(this.AccountService.r.id)
    this.loadUserData(this.userid);
  }

  nameOfImage:string|null=null

  loadUserData(userId: number): void {
    this.userService.getUserById(userId).subscribe(user => {
      this.nameOfImage = user.photo
      user.photo = this.imageUrlBase + user.photo;
      this.User = user;
      console.log(user);
    });
  }

  onButtonClick(userId: number): void {
    this.loadUserData(userId);

    this.router.navigateByUrl("/Account");
  }
}

