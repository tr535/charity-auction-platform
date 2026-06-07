import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminAddGiftComponent } from './admin-add-gift.component';

describe('AdminAddGiftComponent', () => {
  let component: AdminAddGiftComponent;
  let fixture: ComponentFixture<AdminAddGiftComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminAddGiftComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AdminAddGiftComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
