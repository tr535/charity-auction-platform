import { Component, inject, signal, Input, Output, EventEmitter, OnInit, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../../services/auth.service';
import { SidebarModule } from 'primeng/sidebar';
import { ButtonModule } from 'primeng/button';

interface GroupedCartItem {
  giftId: number;
  giftName: string;
  price: number;
  count: number;
  purchaseIds: number[];
}

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, SidebarModule, ButtonModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.scss'
})
export class CartComponent implements OnInit {
  private http = inject(HttpClient);
  public authService = inject(AuthService);

  private _visible: boolean = false;
  
  @Input() 
  get visible(): boolean { return this._visible; }
  set visible(value: boolean) {
    this._visible = value;
    if (value && this.authService.isLoggedIn()) {
      this.loadCart();
    }
  }

  @Output() visibleChange = new EventEmitter<boolean>();

  groupedItems = signal<GroupedCartItem[]>([]);
  totalAmount = signal<number>(0);
  
  // חישוב כמות פריטים כוללת בשביל התצוגה
  totalItemsCount = computed(() => {
    return this.groupedItems().reduce((acc, item) => acc + item.count, 0);
  });

  ngOnInit() {
    if (this.authService.isLoggedIn()) {
      this.loadCart();
    }
  }

  loadCart() {
    const userId = this.authService.currentUser()?.id;
    if (!userId) return;

    this.http.get<any>(`http://localhost:5122/api/Purchase/cart/${userId}`).subscribe({
      next: (response) => {
        const rawItems = response.data || [];
        this.groupItems(rawItems);
        this.calculateTotal(rawItems);
      },
      error: (err) => console.error('שגיאה בטעינת הסל:', err)
    });
  }

  groupItems(items: any[]) {
    const groups: { [key: number]: GroupedCartItem } = {};

    items.forEach(item => {
      const gId = item.giftId;
      if (!groups[gId]) {
        groups[gId] = {
          giftId: gId,
          giftName: item.gift?.nameGift || 'מתנה ללא שם',
          price: item.gift?.priceTicket || 0,
          count: 0,
          purchaseIds: []
        };
      }
      groups[gId].count++;
      groups[gId].purchaseIds.push(item.id);
    });

    this.groupedItems.set(Object.values(groups));
  }

  calculateTotal(items: any[]) {
    const total = items.reduce((sum, item) => sum + (item.gift?.priceTicket || 0), 0);
    this.totalAmount.set(total);
  }

  incrementItem(giftId: number) {
    const userId = this.authService.currentUser()?.id;
    this.http.post(`http://localhost:5122/api/Purchase/add`, { userId, giftId }).subscribe({
      next: () => this.loadCart()
    });
  }

  decrementItem(purchaseIds: number[]) {
    if (purchaseIds.length === 0) return;
    const idToDelete = purchaseIds[purchaseIds.length - 1];
    this.http.delete(`http://localhost:5122/api/Purchase/${idToDelete}`).subscribe({
      next: () => this.loadCart()
    });
  }

  checkout() {
    const userId = this.authService.currentUser()?.id;
    if (!userId) return;

    this.http.post(`http://localhost:5122/api/Purchase/confirm/${userId}`, {}).subscribe({
      next: () => {
        this.groupedItems.set([]);
        this.totalAmount.set(0);
        this.close();
      }
    });
  }

  close() {
    this.visibleChange.emit(false);
  }
}