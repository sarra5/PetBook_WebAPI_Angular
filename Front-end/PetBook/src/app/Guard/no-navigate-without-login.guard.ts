import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';


export const noNavigateWithoutLoginGuard: CanActivateFn = (route, state) => {
  let t = localStorage.getItem("token")
  const router = inject(Router);
  const snackBar = inject(MatSnackBar);

  if(t == null){
    snackBar.open('You Have To Login', 'Close', {
      duration: 5000,
      verticalPosition: 'top'
    });
    router.navigateByUrl('/Login');
    return false;
  }
  return true
};