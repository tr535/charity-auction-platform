import { HttpClient } from '@angular/common/http';
import { Injectable, inject, signal, computed } from '@angular/core';
import { Observable, tap } from 'rxjs';

interface AuthResponse {
  token: string;
  role?: string | number; // יכול להיות טקסט או מספר מהשרת
  userName?: string;
  id?: number;
  [key: string]: any; 
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5122/api/Auth';

  currentUser = signal<AuthResponse | null>(this.getInitialUser());

  private getInitialUser(): AuthResponse | null {
    const token = localStorage.getItem('userToken');
    const userId = localStorage.getItem('userId');
    
    if (!token || !userId || userId === '0' || userId === 'undefined') return null;

    return {
      token: token,
      role: localStorage.getItem('userRole') || 'Customer',
      userName: localStorage.getItem('userName') || 'משתמש',
      id: Number(userId)
    };
  }

  register(userData: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, userData);
  }

  refreshUserFromStorage() {
    const user = this.getInitialUser();
    this.currentUser.set(user);
  }

  login(credentials: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, credentials).pipe(
      tap(response => {
        console.log('Server Raw Response:', response);

        const apiData = response.data || response.Data;

        if (response.success && apiData) {
          const token = apiData.token || apiData.Token;
          const name = apiData.fullName || apiData.FullName || apiData.userName || apiData.UserName;
          const role = apiData.role || apiData.Role;
          const id = apiData.id || apiData.Id; 

          if (id) {
            localStorage.setItem('userToken', token);
            localStorage.setItem('userRole', String(role || 'Customer'));
            localStorage.setItem('userName', name || 'משתמש');
            localStorage.setItem('userId', String(id)); 
            
            this.currentUser.set({
              token: token,
              role: role || 'Customer',
              userName: name || 'משתמש',
              id: Number(id)
            });
            
            console.log('AuthService: Login successful for ID:', id);
          }
        }
      })
    );
  }

  isLoggedIn = computed(() => !!this.currentUser()?.token);
  
  /**
   * התיקון הקריטי: 
   * שימוש ב-String() כדי למנוע קריסה אם ה-role מגיע כמספר (כמו 1) מהשרת.
   */
  isAdmin = computed(() => {
    const user = this.currentUser();
    if (!user || !user.role) return false;

    // הופכים לטקסט ואז לאותיות קטנות - זה מונע את השגיאה image_041871.png
    const roleStr = String(user.role).toLowerCase();
    
    return roleStr === 'admin' || roleStr === 'manager' || roleStr === '1';
  });
  
  getUserName = computed(() => this.currentUser()?.userName || 'אורח');

  logout() {
    localStorage.clear();
    this.currentUser.set(null);
  }
}