import { Gift } from './gift-model'; // ודאי שהנתיב נכון למודל המתנה שלך

export enum PurchaseStatus {
    Draft = 0,
    Confirmed = 1,
    Winner = 2
}

export class Purchase {
    id: number = 0;
    userId!: number;
    giftId!: number;
    totalPrice: number = 0;
    
    // השדה שמאפשר לגשת ל-item.gift.nameGift
    gift?: Gift; 

    // שדות אקסטרה שמגיעים מה-DTO לנוחות
    giftName?: string;
    giftPrice?: number;
    userName?: string;
    
    purchaseDate: Date = new Date();
    purchaseStatus: PurchaseStatus = PurchaseStatus.Draft;
}