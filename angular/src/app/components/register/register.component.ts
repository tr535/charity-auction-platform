import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';

// PrimeNG Imports
import { CardModule } from 'primeng/card';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { ButtonModule } from 'primeng/button';
import { MessageModule } from 'primeng/message';
import { ToastModule } from 'primeng/toast';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule, 
    RouterModule, 
    CardModule, 
    InputTextModule, 
    PasswordModule, 
    ButtonModule, 
    MessageModule,
    ToastModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  user = { 
    UserName: '', 
    Email: '', 
    Password: '', 
    Phone: '' 
  };
  
  errorMessage = signal<string | null>(null);

  onRegister() {
    this.errorMessage.set(null);

    // שליחה לשרת רק אם הטופס עבר את הוואלידציות של ה-HTML
    this.authService.register(this.user).subscribe({
      next: (res: any) => {
        // לאחר הרשמה מוצלחת - נעבור להתחברות
        this.router.navigate(['/login']);
      },
      error: (err: any) => {
        // טיפול בשגיאות שחוזרות מהשרת (למשל: מייל כבר קיים)
        if (err.status === 400 && err.error?.errors) {
          const validationErrors = Object.values(err.error.errors).flat();
          this.errorMessage.set(validationErrors[0] as string);
        } else {
          this.errorMessage.set(err.error?.message || 'ההרשמה נכשלה. ייתכן שהנתונים כבר קיימים במערכת');
        }
      }
    });
  }
}