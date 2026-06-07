import { Component, signal, inject } from '@angular/core'; // הוספנו inject
import { RouterOutlet, RouterLink } from '@angular/router'; // הוספנו RouterLink לניווט
import { CommonModule } from '@angular/common'; // חשוב עבור ngIf
import { AuthService } from './services/auth.service'; // ודאי שהנתיב נכון
import { ButtonModule } from 'primeng/button'; // אם את משתמשת בפתור של PrimeNG

@Component({
  selector: 'app-root',
  standalone: true,
  // הוספנו CommonModule, RouterLink ו-ButtonModule
  imports: [RouterOutlet, CommonModule, RouterLink, ButtonModule], 
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('mechira-sinit');
  // הזרקת ה-Service כדי להשתמש בו ב-HTML
  public authService = inject(AuthService); 
}