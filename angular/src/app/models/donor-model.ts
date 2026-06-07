import { Gift } from './gift-model';

export class Donor {
    id: number = 0;
    firstName: string = '';
    familyName: string = '';
    email: string = '';
    phone?: string;
    gifts?: Gift[];
    
    // שדות תואמי שרת (PascalCase) למניעת שורות ריקות
    FirstName?: string; 
    FamilyName?: string;
    Email?: string;
    Phone?: string;
    Gifts?: Gift[];
}