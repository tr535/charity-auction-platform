import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PurchaseService } from '../../../services/purchase.service';
import { Purchase, PurchaseStatus } from '../../../models/purchase-model';
import emailjs from '@emailjs/browser'; 
import * as XLSX from 'xlsx';

import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { TagModule } from 'primeng/tag';
import { InputTextModule } from 'primeng/inputtext';
import { DialogModule } from 'primeng/dialog';

@Component({
  selector: 'app-admin-purchase',
  standalone: true,
  imports: [CommonModule, TableModule, ButtonModule, ToastModule, TagModule, InputTextModule, DialogModule],
  providers: [MessageService],
  templateUrl: './admin-purchase.component.html',
  styleUrl: './admin-purchase.component.scss'
})
export class AdminPurchaseComponent implements OnInit {
  private purchaseService = inject(PurchaseService);
  private messageService = inject(MessageService);

  purchases: Purchase[] = [];
  winnersReport: any[] = [];
  totalIncome: number = 0;
  loading: boolean = true;
  isProcessing: boolean = false;
  displayWinnersDialog: boolean = false;

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.loading = true;
    this.purchaseService.getAllPurchases().subscribe({
      next: (data) => {
        // עיבוד נתונים כדי לוודא ששמות המתנות והמחירים קיימים
        this.purchases = data.map(p => ({
          ...p,
          giftName: p.giftName?.trim() || p.gift?.nameGift?.trim() || 'מתנה לא ידועה'
        }));
        this.calculateTotalIncome(); // חישוב מקומי כגיבוי לשרת
        this.loading = false;
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'שגיאה', detail: 'טעינת הנתונים נכשלה' });
        this.loading = false;
      }
    });

    // טעינת הכנסות מהשרת
    this.purchaseService.getTotalIncome().subscribe({
      next: (res: any) => {
        this.totalIncome = (typeof res === 'number') ? res : (res.data || 0);
      }
    });
  }

  // פונקציית עזר לחישוב הכנסות אם השרת לא מחזיר ערך
  calculateTotalIncome() {
    if (this.totalIncome === 0) {
        this.totalIncome = this.purchases.reduce((sum, p) => sum + (p.totalPrice || p.gift?.priceTicket || 0), 0);
    }
  }

  viewWinnersReport() {
    this.purchaseService.getWinnersReport().subscribe({
      next: (data: any) => {
        this.winnersReport = data?.data || data || [];
        this.displayWinnersDialog = true;
      },
      error: () => this.messageService.add({ severity: 'error', summary: 'שגיאה', detail: 'לא ניתן לטעון דוח זוכים' })
    });
  }

  exportToExcel() {
    if (this.winnersReport.length === 0) return;
    const worksheet = XLSX.utils.json_to_sheet(this.winnersReport);
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, "Winners");
    XLSX.writeFile(workbook, "Luxe_Winners_2026.xlsx");
  }

  runRaffle(giftId: number, giftName: string) {
    if (this.isProcessing) return;
    this.isProcessing = true;

    this.purchaseService.runRaffle(giftId).subscribe({
      next: (res: any) => {
        const winnerData = res.data || res;
        this.messageService.add({ 
            severity: 'success', 
            summary: `הגרלת ${giftName} הושלמה!`, 
            detail: `הזוכה המאושר: ${winnerData?.winnerName || 'נבחר זוכה'}` 
        });
        
        if (winnerData?.winnerEmail) this.sendEmail(winnerData, giftName);
        this.loadData();
        this.isProcessing = false;
      },
      error: () => {
        this.messageService.add({ severity: 'warn', summary: 'שגיאה', detail: 'לא ניתן לבצע הגרלה למתנה זו' });
        this.isProcessing = false;
      }
    });
  }

  sendEmail(winner: any, giftName: string) {
    const params = { winner_name: winner.winnerName, gift_name: giftName, to_email: winner.winnerEmail };
    emailjs.send('service_rdir9sz', 'template_4iybpb8', params, '852_AZJ_wjwPVF_wa')
      .catch(err => console.error('Email Error:', err));
  }

  getStatusLabel(status: number): string {
    switch(status) {
        case 1: return 'מאושר';
        case 2: return 'זוכה';
        default: return 'טיוטה';
    }
  }

  isGiftDrawn(giftId: number): boolean {
    return this.purchases.some(p => p.giftId === giftId && p.purchaseStatus === 2);
  }
}