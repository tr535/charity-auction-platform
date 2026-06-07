import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map, catchError, of } from 'rxjs';
import { Gift } from '../models/gift-model';
import { Purchase } from '../models/purchase-model';

@Injectable({
  providedIn: 'root',
})
export class GiftService {
  private httpClient: HttpClient = inject(HttpClient);
  
  private readonly giftApiUrl: string = 'http://localhost:5122/api/Gift';
  private readonly purchaseApiUrl: string = 'http://localhost:5122/api/Purchase';

  /** שליפת כל המתנות */
  getGifts(): Observable<Gift[]> {
    return this.httpClient.get<any>(this.giftApiUrl).pipe(
      map(response => (Array.isArray(response) ? response : (response.data || [])) as Gift[]),
      catchError(error => {
        console.error('Error fetching gifts:', error);
        return of([]);
      })
    );
  }

  /** הוספת רכישה חדשה - המשתמש קונה מתנה */
  addPurchase(request: { userId: number; giftId: number }): Observable<Purchase> {
    return this.httpClient.post<Purchase>(`${this.purchaseApiUrl}/add`, request);
  }

  /** בדיקת סל הקניות של המשתמש */
  getUserCart(userId: number): Observable<Purchase[]> {
    return this.httpClient.get<any>(`${this.purchaseApiUrl}/cart/${userId}`).pipe(
      map(res => res && res.data ? res.data : [])
    );
  }

  /** הוספת מתנה חדשה (Admin) */
  addGift(gift: Gift): Observable<Gift> {
    return this.httpClient.post<Gift>(this.giftApiUrl, gift);
  }

  /** עדכון מתנה (Admin) */
  updateGift(updatedGift: Gift): Observable<any> {
    return this.httpClient.put<any>(`${this.giftApiUrl}/${updatedGift.id}`, updatedGift);
  }

  /** מחיקת מתנה (Admin) */
  deleteGift(giftId: number): Observable<any> {
    return this.httpClient.delete<any>(`${this.giftApiUrl}/${giftId}`);
  }
}