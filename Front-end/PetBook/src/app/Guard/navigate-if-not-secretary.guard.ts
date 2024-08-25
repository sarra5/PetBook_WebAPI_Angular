import { CanActivateFn, Router } from '@angular/router';
import { AccountServiceService } from '../Services/account-service.service';
import { inject } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

export const navigateIfNotSecretaryGuard: CanActivateFn = (route, state) => {
  let t = localStorage.getItem("token")
  const router = inject(Router);
  const snackBar = inject(MatSnackBar);
  const accountService = inject(AccountServiceService)
  let id = accountService.r.RoleId
  
  if(t != null && id == "3"){
    snackBar.open('No Permission', 'Close', {
      duration: 5000,
      verticalPosition: 'top'
    });
    router.navigateByUrl('/Secretary');
    return false;
  }

  return true;
};
