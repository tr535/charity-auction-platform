import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideHttpClient, withInterceptors } from '@angular/common/http'; // הוספנו את withInterceptors

import { routes } from './app.routes';
import { jwtInterceptor } from './interceptors/jwt.interceptor'; // ודאי שהנתיב תואם למיקום הקובץ שיצרת

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    // עדכון ה-HttpClient כך שישתמש ב-Interceptor להזרקת ה-Token
    provideHttpClient(
      withInterceptors([jwtInterceptor])
    ),
    provideAnimations() 
  ]
};