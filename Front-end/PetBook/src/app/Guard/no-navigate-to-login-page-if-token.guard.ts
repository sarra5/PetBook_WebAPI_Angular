import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

export const noNavigateToLoginPageIfTokenGuard: CanActivateFn = (route, state) => {
  let t = localStorage.getItem("token")
  const dialog = inject(MatDialog);
  const router = inject(Router);
  const snackBar = inject(MatSnackBar);

  if(t != null){
    snackBar.open('You are already logged in', 'Close', {
      duration: 5000,
      verticalPosition: 'top'
    });
    router.navigateByUrl('');
    return false;
  }
  return true
};
