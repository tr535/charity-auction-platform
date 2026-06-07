import { Component, inject, OnInit, signal } from '@angular/core';
import { AsyncPipe, CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Observable, tap } from 'rxjs';

import { GiftService } from '../../../services/gift-service';
import { AuthService } from '../../../services/auth.service';

import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';
import { TooltipModule } from 'primeng/tooltip';
import { BadgeModule } from 'primeng/badge';
import { MessageService } from 'primeng/api';

import { CartComponent } from '../cart/cart.component';

@Component({
  selector: 'app-gifts',
  standalone: true,
  imports: [
    AsyncPipe, CommonModule, ButtonModule, ToastModule,
    TooltipModule, BadgeModule, RouterLink, CartComponent
  ],
  providers: [MessageService],
  templateUrl: './gifts.html',
  styleUrl: './gifts.scss',
})
export class Gifts implements OnInit {
  private giftService = inject(GiftService);
  private messageService = inject(MessageService);
  public authService = inject(AuthService);
  
  public isCartVisible = false;
  public cartCount = signal(0);

  gifts$: Observable<any[]> = this.giftService.getGifts().pipe(
    tap(data => console.log('Gifts loaded:', data))
  );

  ngOnInit() {
    this.refreshCartCount();
  }

  refreshCartCount() {
    const user = this.authService.currentUser();
    if (user?.id) {
      // הקריאה לסל נמצאת עכשיו ב-GiftService
      this.giftService.getUserCart(user.id).subscribe({
        next: (items) => this.cartCount.set(items.length),
        error: (err) => console.error('Error refreshing cart', err)
      });
    }
  }

  toggleCart() {
    this.isCartVisible = !this.isCartVisible;
  }

  addToCart(gift: any) {
    if (gift['winnerName'] || gift['isRaffled']) {
      this.messageService.add({ severity: 'error', summary: 'המכירה נסגרה', detail: 'המתנה כבר הוגרלה.' });
      return;
    }

    const user = this.authService.currentUser();
    if (!user?.id) {
      this.messageService.add({ severity: 'warn', summary: 'כמעט שם...', detail: 'יש להתחבר למערכת' });
      return;
    }

    this.giftService.addPurchase({ userId: user.id, giftId: gift['id'] }).subscribe({
      next: () => {
        this.cartCount.update(v => v + 1); 
        this.messageService.add({ 
          severity: 'success', 
          summary: 'בחירה מצוינת!', 
          detail: `הכרטיס ל-${gift['nameGift']} מחכה לך בסל`,
          life: 3000
        });
        this.isCartVisible = true; 
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'שגיאה', detail: 'חלה שגיאה בשרת.' });
      }
    });
  }
}