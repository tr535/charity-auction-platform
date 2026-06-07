import { Routes } from '@angular/router';
import { Gifts } from './components/client/gifts/gifts'; 
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { AdminAddGiftComponent } from './components/admin/admin-add-gift/admin-add-gift.component';
import { DonorManagement } from './components/admin/donor-management/donor-management.component'; 
import { AdminPurchaseComponent } from './components/admin/admin-purchase/admin-purchase.component'; // ייבוא הקומפוננטה החדשה
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: 'gifts', component: Gifts },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  // נתיבי ניהול - מוגנים ע"י ה-Guard
  { 
    path: 'admin/add-gift', 
    component: AdminAddGiftComponent, 
    canActivate: [authGuard] 
  },
  { 
    path: 'admin/donors', 
    component: DonorManagement, 
    canActivate: [authGuard] 
  },
  { 
    // הנתיב החדש עבור ניהול רכישות והגרלה
    path: 'admin/purchase', 
    component: AdminPurchaseComponent, 
    canActivate: [authGuard] 
  },

  // ניתוב ברירת מחדל
  { path: '', redirectTo: 'gifts', pathMatch: 'full' },
  { path: '**', redirectTo: 'gifts' }
];