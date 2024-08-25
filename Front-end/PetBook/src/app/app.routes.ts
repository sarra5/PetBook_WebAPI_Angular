import { Routes } from '@angular/router';
import { UserLoginComponent } from './Pages/user-login/user-login.component';
import { UserSignUpComponent } from './Pages/user-sign-up/user-sign-up.component';
import { PetRegisterComponent } from './Pages/pet-register/pet-register.component';
import { noNavigateToLoginPageIfTokenGuard } from './Guard/no-navigate-to-login-page-if-token.guard';
import { MyRequestComponent } from './Pages/my-request/my-request.component';
import { PendingRequestComponent } from './Pages/pending-request/pending-request.component';
import { UserDetailsComponent } from './Pages/User/user-details/user-details.component';
import { UpdateUserDetailsComponent } from './Pages/User/update-user-details/update-user-details.component';
import { LandingPageComponent } from './Pages/LandingPage/landing-page/landing-page.component';
import { PetDetailsComponent } from './Pages/PetInfo/pet-detailss/pet-details.component';
import { BreedSearchComponent } from './Pages/breed-search/breed-search.component';
import { ClinicComponent } from './Pages/clinic/clinic.component';
import { UserProfilePetInfoComponent } from './Pages/userPetInfo/user-pet-info/userProfile-pet-info.component';
import { noNavigateWithoutLoginGuard } from './Guard/no-navigate-without-login.guard';
import { SearchVaccineComponent } from './Pages/search-vaccine/search-vaccine.component';
import { SearchVaccineClicnicComponent } from './Pages/search-vaccine-clicnic/search-vaccine-clicnic.component';
import { UserPetInfoEditComponent } from './Pages/user-pet-info-edit/user-pet-info-edit.component';
import { ReservationForVaccineComponent } from './Pages/reservation-for-vaccine/reservation-for-vaccine.component';
import { ClinicSearchComponent } from './Pages/clinic-search/clinic-search.component';
import { SecretaryVaccineComponent } from './Pages/vaccine_Secretary/secretary-vaccine/secretary-vaccine.component';
import { SecretaryComponent } from './Pages/secretary/secretary.component';
import { SecrteryclinicComponent } from './Pages/secrteryclinic/secrteryclinic.component';
import { UserProfileMainComponent } from './Pages/user-profile-main/user-profile-main.component';
import { ShowReservationsComponent } from './Pages/show-reservations/show-reservations.component';
import { navigateIfClientGuard } from './Guard/navigate-if-client.guard';
import { navigateIfSecretaryGuard } from './Guard/navigate-if-secretary.guard';
import { navigateIfNotSecretaryGuard } from './Guard/navigate-if-not-secretary.guard';

export const routes: Routes = [
    {path: "Login", component:UserLoginComponent, title:"Login", canActivate: [noNavigateToLoginPageIfTokenGuard]},
    {path: "PetRegister", component:PetRegisterComponent, title:"Pet Register", canActivate:[noNavigateWithoutLoginGuard, navigateIfClientGuard]},
    {path: "UserSignUp", component:UserSignUpComponent, title:"User Sign-Up", canActivate: [noNavigateToLoginPageIfTokenGuard]},
    
    {path: "Profile", component:UserProfileMainComponent, title:"Profile", children:[
        {path: "", redirectTo: "Account", pathMatch: "full"},
        {path: "Account", component:UserDetailsComponent, title:"Account", canActivate:[noNavigateWithoutLoginGuard, navigateIfClientGuard]},
        {path:"pendingRequest", component:PendingRequestComponent , title:"pending Request", canActivate:[noNavigateWithoutLoginGuard, navigateIfClientGuard]},
        {path: "MyRequest", component:MyRequestComponent, title:"MY Request", canActivate:[noNavigateWithoutLoginGuard, navigateIfClientGuard]},
        {path: "userPetInfo",component: UserProfilePetInfoComponent, title:"Pet Information", canActivate:[noNavigateWithoutLoginGuard, navigateIfClientGuard]},
        {path: "Reservations",component: ShowReservationsComponent , title:"Reservatios", canActivate:[noNavigateWithoutLoginGuard, navigateIfClientGuard]},
    ], canActivate:[noNavigateWithoutLoginGuard, navigateIfClientGuard]},

    {path: "UpdateUser", component:UpdateUserDetailsComponent, title:"Edit", canActivate:[noNavigateWithoutLoginGuard, navigateIfClientGuard]},
    {path: "BreedSearch", component:BreedSearchComponent, title:"Search Breed", canActivate:[navigateIfClientGuard]},
    {path: 'Pet/details/:id', component: PetDetailsComponent,title:"Pet Details", canActivate:[noNavigateWithoutLoginGuard, navigateIfClientGuard] },
    { path: 'Pet/details/:id/:DoNotShowButton', component: PetDetailsComponent,title:"Pet Details", canActivate:[noNavigateWithoutLoginGuard, navigateIfClientGuard]},
    {path:"Vaccine",component:SearchVaccineComponent,title:"Vaccine", canActivate:[navigateIfClientGuard]},
    {path: 'search-vaccine-clinic/:VaccineId', component: SearchVaccineClicnicComponent, canActivate:[navigateIfClientGuard] },
    {path:"userPetEdit/:id",component:UserPetInfoEditComponent, title: "Edit Pet Information", canActivate:[noNavigateWithoutLoginGuard, navigateIfClientGuard]},
    {path: 'Clinic/:clinicId', component: ClinicComponent,title:"Clinic Details", canActivate:[noNavigateWithoutLoginGuard, navigateIfClientGuard] },
    {path:"ReservationVaccine/:clinicId/:VaccineId",component:ReservationForVaccineComponent,title:"Reservation Vaccine", canActivate:[noNavigateWithoutLoginGuard, navigateIfClientGuard]},
    {path:"clinics",component:ClinicSearchComponent,title:"Clinics", canActivate:[navigateIfClientGuard]},
    {path:"Secretary",component:SecretaryComponent,title:"Secretary", canActivate:[noNavigateWithoutLoginGuard, navigateIfSecretaryGuard]},
    {path:"",component:LandingPageComponent,title:"PetBook", canActivate:[navigateIfNotSecretaryGuard]},
    {path: '**', redirectTo: '/'}
];

