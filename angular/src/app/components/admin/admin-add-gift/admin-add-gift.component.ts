import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { GiftService } from '../../../services/gift-service';
import { DonorService } from '../../../services/donor.service';
import { Gift } from '../../../models/gift-model';
import { Donor } from '../../../models/donor-model';

import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-admin-add-gift',
  standalone: true,
  imports: [CommonModule, FormsModule, TableModule, ButtonModule, DialogModule, InputTextModule, DropdownModule, ToastModule],
  providers: [MessageService],
  templateUrl: './admin-add-gift.component.html',
  styleUrl: './admin-add-gift.component.scss'
})
export class AdminAddGiftComponent implements OnInit {
  private giftService = inject(GiftService);
  private donorService = inject(DonorService);
  private messageService = inject(MessageService);

  gifts = signal<Gift[]>([]);
  donors = signal<Donor[]>([]);
  giftDialog = signal<boolean>(false);
  gift = signal<Partial<Gift>>({});
  loading = signal<boolean>(true);

  ngOnInit() { this.loadData(); }

  loadData() {
    this.loading.set(true);
    this.giftService.getGifts().subscribe({
      next: (data) => {
        this.gifts.set(data.map(g => ({ ...g, nameGift: g.nameGift?.trim() })));
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });

    this.donorService.getDonors().subscribe({
      next: (res: any) => this.donors.set(Array.isArray(res) ? res : (res.data || []))
    });
  }

  openNew() {
    this.gift.set({ id: 0, priceTicket: 10, isRaffled: false });
    this.giftDialog.set(true);
  }

  editGift(selectedGift: Gift) {
    this.gift.set({ ...selectedGift });
    this.giftDialog.set(true);
  }

  saveGift() {
    const giftData = this.gift() as Gift;
    if (giftData.nameGift && giftData.donorId) {
      const op = giftData.id ? this.giftService.updateGift(giftData) : this.giftService.addGift(giftData);
      op.subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'בוצע', detail: 'נשמר בהצלחה' });
          this.loadData();
          this.giftDialog.set(false);
        }
      });
    }
  }

  deleteGift(id: number) {
    if (confirm('למחוק מתנה זו?')) {
      this.giftService.deleteGift(id).subscribe({
        next: () => {
          this.gifts.update(prev => prev.filter(g => g.id !== id));
          this.messageService.add({ severity: 'success', summary: 'נמחק', detail: 'הוסר' });
        }
      });
    }
  }
}