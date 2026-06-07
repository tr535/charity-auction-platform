export class Gift {
    id: number = 0;
    nameGift!: string;
    category!: string; 
    
    // מזהה התורם (הקשר ל-DB)
    donorId!: number; 
    
    // שם התורם (לתצוגה בטבלה) - חייב להיות string!
    donorName!: string; 
    
    priceTicket: number = 10;
    
    // סטטוס ההגרלה
    isRaffled: boolean = false; 
}