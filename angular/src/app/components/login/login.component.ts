import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; // הלב של הוואלידציות
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';

// PrimeNG
import { CardModule } from 'primeng/card';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { ButtonModule } from 'primeng/button';
import { MessageModule } from 'primeng/message';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule, 
    RouterModule, 
    CardModule, 
    InputTextModule, 
    PasswordModule, 
    ButtonModule, 
    MessageModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  credentials = { Email: '', Password: '' };
  errorMessage = signal<string | null>(null);

  onLogin() {
    this.errorMessage.set(null); 
    this.authService.login(this.credentials).subscribe({
      next: (response: any) => {
        const userRole = response.role || response.Role;
        // בדיקה גמישה למנהל
        const isAdmin = userRole === 'Admin' || userRole === 'Manager' || userRole === 1;

        if (isAdmin) {
          this.router.navigate(['/admin/add-gift']); 
        } else {
          this.router.navigate(['/gifts']);
        }
      },
      error: (err: any) => {
        console.error('Login error:', err);
        this.errorMessage.set('התחברות נכשלה. ודאי שהפרטים נכונים.');
      }
    });
  }
}