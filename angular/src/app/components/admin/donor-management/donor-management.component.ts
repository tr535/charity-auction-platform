import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DonorService } from '../../../services/donor.service'; 
import { Donor } from '../../../models/donor-model'; 
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';
import { DialogModule } from 'primeng/dialog'; 
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-donor-management',
  standalone: true,
  imports: [CommonModule, FormsModule, TableModule, ButtonModule, InputTextModule, ToastModule, DialogModule],
  providers: [MessageService],
  templateUrl: './donor-management.component.html',
  styleUrl: './donor-management.component.scss'
})
export class DonorManagement implements OnInit {
  private donorService = inject(DonorService);
  private messageService = inject(MessageService);

  donors = signal<Donor[]>([]); 
  loading = signal<boolean>(true);
  donorDialog = signal<boolean>(false); 
  currentDonor = signal<Partial<Donor>>({}); 

  ngOnInit() {
    this.loadDonors();
  }

  loadDonors() {
    this.loading.set(true);
    this.donorService.getDonors().subscribe({
      next: (response: any) => {
        // וידוא שהנתונים הם מערך
        const data = Array.isArray(response) ? response : (response.data || []);
        this.donors.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'שגיאה', detail: 'טעינת הנתונים נכשלה' });
        this.loading.set(false);
      }
    });
  }

  openNew() {
    this.currentDonor.set({ id: 0, firstName: '', familyName: '', email: '', phone: '' });
    this.donorDialog.set(true); 
  }

  editDonor(donor: Donor) {
    // מיפוי השדות למקרה שהגיעו מהשרת ב-PascalCase
    this.currentDonor.set({ 
      id: donor.id || (donor as any).Id,
      firstName: donor.firstName || (donor as any).FirstName,
      familyName: donor.familyName || (donor as any).FamilyName,
      email: donor.email || (donor as any).Email,
      phone: donor.phone || (donor as any).Phone
    });
    this.donorDialog.set(true); 
  }

  hideDialog() {
    this.donorDialog.set(false);
  }

  saveDonor() {
    const d = this.currentDonor();
    
    // יצירת אובייקט נקי למשלוח שתואם ל-DonorDTO בשרת (PascalCase)
    const payload: any = {
      Id: d.id || 0,
      FirstName: d.firstName,
      FamilyName: d.familyName,
      Email: d.email,
      Phone: d.phone,
      Gifts: [] // הוספת מערך ריק כדי למנוע שגיאת 400 ב-Required
    };

    if (payload.Id > 0) {
      this.donorService.updateDonor(payload).subscribe({
        next: () => {
          this.loadDonors(); // טעינה מחדש לביטחון מלא
          this.messageService.add({ severity: 'success', summary: 'בוצע', detail: 'פרטי התורם עודכנו' });
          this.donorDialog.set(false);
        },
        error: (err) => {
           console.error('Update failed:', err);
           this.messageService.add({ severity: 'error', summary: 'שגיאה', detail: 'העדכון נכשל - בדוק תקינות שדות' });
        }
      });
    } else {
      this.donorService.addDonor(payload).subscribe({
        next: () => {
          this.loadDonors();
          this.messageService.add({ severity: 'success', summary: 'בוצע', detail: 'תורם חדש נוסף למערכת' });
          this.donorDialog.set(false);
        },
        error: (err) => {
          console.error('Add failed:', err);
          this.messageService.add({ severity: 'error', summary: 'שגיאה', detail: 'ההוספה נכשלה - וודא שכל שדות החובה מלאים' });
        }
      });
    }
  }

  deleteDonor(id: number, name: string) {
    if (confirm(`האם למחוק את התורם ${name}?`)) {
      this.donorService.deleteDonor(id).subscribe({
        next: () => {
          this.donors.update(prev => prev.filter(val => (val.id || (val as any).Id) !== id));
          this.messageService.add({ severity: 'success', summary: 'בוצע', detail: 'תורם נמחק' });
        },
        error: () => this.messageService.add({ severity: 'error', summary: 'שגיאה', detail: 'המחיקה נכשלה' })
      });
    }
  }
}