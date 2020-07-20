import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ContactSignupComponent } from './contact-signup.component';

describe('ContactSignupComponent', () => {
  let component: ContactSignupComponent;
  let fixture: ComponentFixture<ContactSignupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ContactSignupComponent ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ContactSignupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
