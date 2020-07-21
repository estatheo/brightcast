import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import {
  NbAlertModule,
  NbButtonModule,
  NbCheckboxModule,
  NbInputModule,
  NbCardModule,
  NbTabsetModule,
  NbSelectModule,
  NbRouteTabsetModule,
  NbStepperModule,
  NbToastrModule,
  NbSpinnerModule,
} from '@nebular/theme';
import { ThemeModule } from '../../@theme/theme.module';
import { ContactSignupComponent } from './contact-signup.component';
import { ContactSignupRoutingModule } from './contact-signup-routing.module';
import { ContactService } from '../../@core/apis/contact.service';
import { ContactListService } from '../../@core/apis/contactList.service';


@NgModule({
  declarations: [ ContactSignupComponent ],
  imports: [
    CommonModule,
    ThemeModule,
    ReactiveFormsModule,
    FormsModule,
    RouterModule,
    NbAlertModule,
    NbInputModule,
    NbButtonModule,
    NbCheckboxModule,
    NbCardModule,
    NbTabsetModule,
    NbSelectModule,
    NbRouteTabsetModule,
    NbStepperModule,
    ContactSignupRoutingModule,
    NbToastrModule,
    NbSpinnerModule,
  ],
  providers: [
    ContactService,
    ContactListService,
  ],
})
export class ContactSignupModule { }
