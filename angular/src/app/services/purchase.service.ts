import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Purchase } from '../models/purchase-model';

@Injectable({
  providedIn: 'root',
})
export class PurchaseService {
  private httpClient: HttpClient = inject(HttpClient);
  
  private readonly purchaseApiUrl: string = 'http://localhost:5122/api/Purchase';
  private readonly raffleApiUrl: string = 'http://localhost:5122/api/Raffle';

  /** שליפת כל הרכישות במערכת - Admin */
  getAllPurchases(): Observable<Purchase[]> {
    return this.httpClient.get<any>(this.purchaseApiUrl).pipe(
      map(res => res && res.data ? res.data : [])
    );
  }

  /** שליפת סך הכנסות */
  getTotalIncome(): Observable<number> {
    return this.httpClient.get<any>(`${this.raffleApiUrl}/total-income`).pipe(
      map(res => res && res.data !== undefined ? res.data : (typeof res === 'number' ? res : 0))
    );
  }

  /** שליפת דוח זוכים סופי */
  getWinnersReport(): Observable<any[]> {
    return this.httpClient.get<any>(`${this.raffleApiUrl}/winners-report`).pipe(
      map(res => res && res.data ? res.data : [])
    );
  }

  /** הרצת הגרלה */
  runRaffle(giftId: number): Observable<any> {
    return this.httpClient.post<any>(`${this.raffleApiUrl}/draw/${giftId}`, {});
  }

  /** מחיקת רכישה */
  deletePurchase(purchaseId: number): Observable<any> {
    return this.httpClient.delete<any>(`${this.purchaseApiUrl}/${purchaseId}`);
  }
}