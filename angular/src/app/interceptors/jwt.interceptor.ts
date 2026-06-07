import { HttpInterceptorFn } from '@angular/common/http';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  // שליפת הטוקן מהאחסון המקומי
  const token = localStorage.getItem('userToken');

  // אם קיים טוקן, משכפלים את הבקשה ומוסיפים את ה-Header המתאים
  if (token) {
    const cloned = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    return next(cloned);
  }

  // אם אין טוקן, ממשיכים בבקשה המקורית
  return next(req);
};