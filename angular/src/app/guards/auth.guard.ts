import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const isLoggedIn = authService.isLoggedIn();
  const isAdmin = authService.isAdmin();

  // לוג לצורך ניפוי שגיאות (Debug) - תוכלי לראות ב-F12 למה זה נחסם
  console.log(`Guard Check - LoggedIn: ${isLoggedIn}, Admin: ${isAdmin}`);

  if (isLoggedIn && isAdmin) {
    return true; // הכל תקין, כנס לממשק הניהול
  }

  if (!isLoggedIn) {
    console.warn('הגישה נדחתה: משתמש אינו מחובר');
    router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
  } else {
    console.warn('הגישה נדחתה: המשתמש מחובר אך אינו מנהל');
    router.navigate(['/gifts']); // שלח אותו חזרה למתנות
  }

  return false;
};