import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Donor } from '../models/donor-model'; // ודאי שהשם והנתיב תואמים לקובץ שלך

@Injectable({
  providedIn: 'root'
})
export class DonorService {
  private http = inject(HttpClient);
  
  // הכתובת המעודכנת של ה-API שלך
  private readonly apiUrl = 'http://localhost:5122/api/Donor'; 

  /**
   * שליפת כל התורמים
   * דרישת פרויקט: "צפייה ברשימת התורמים"
   */
  getDonors(): Observable<Donor[]> {
    return this.http.get<Donor[]>(this.apiUrl);
  }

  /**
   * שליפת תורם בודד לפי ID
   */
  getDonorById(id: number): Observable<Donor> {
    return this.http.get<Donor>(`${this.apiUrl}/${id}`);
  }

  /**
   * הוספת תורם חדש (C# DonorDTO)
   * דרישת פרויקט: "הוספה של תורם"
   */
  addDonor(donor: Donor): Observable<Donor> {
    return this.http.post<Donor>(this.apiUrl, donor);
  }

  /**
   * עדכון תורם קיים
   * דרישת פרויקט: "עדכון של תורם"
   */
  updateDonor(donor: Donor): Observable<any> {
    // בדרך כלל ב-PUT מעבירים את ה-ID בנתיב
    return this.http.put(`${this.apiUrl}/${donor.id}`, donor);
  }

  /**
   * מחיקת תורם
   * דרישת פרויקט: "מחיקה של תורם"
   */
  deleteDonor(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  /**
   * חיפוש וסינון תורמים
   * דרישת פרויקט: "סינון של תורם לפי (שם/ מייל/ מתנה)"
   */
  searchDonors(term: string): Observable<Donor[]> {
    return this.http.get<Donor[]>(`${this.apiUrl}/search?term=${term}`);
  }
}