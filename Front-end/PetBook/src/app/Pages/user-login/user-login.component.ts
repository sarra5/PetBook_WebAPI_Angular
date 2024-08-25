import { Component } from '@angular/core';
import { InputSectionComponent } from '../../Components/input-section/input-section.component';
import { AccountServiceService } from '../../Services/account-service.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-user-login',
  standalone: true,
  imports: [InputSectionComponent, RouterLink],
  templateUrl: './user-login.component.html',
  styleUrl: './user-login.component.css'
})
export class UserLoginComponent {
  email:string = ""
  password:string = ""

  constructor(public accountService:AccountServiceService, private route: ActivatedRoute, private router: Router){  }

  ngOnInit(){
    this.route.queryParams.subscribe(params => {
      this.email = params['email'] || '';
      this.password = params['password'] || '';
    });
  }

  Login(){
    this.accountService.Login(this.email, this.password)
  }
  
  Cancel(){
    this.router.navigateByUrl("");
  }
}
