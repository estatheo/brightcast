import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MainRoutingModule } from './main-routing.module';
import { SettingsComponent } from './settings/settings.component';
import { MainComponent } from './main/main.component';
import { CustomerListComponent } from './customer-list/customer-list.component';
import { NgxDataTableModule } from 'angular-9-datatable';
import {
  NbAccordionModule,
  NbButtonModule,
  NbCardModule,
  NbListModule,
  NbRouteTabsetModule,
  NbStepperModule,
  NbTabsetModule,
  NbUserModule,
  NbInputModule,
  NbSelectModule,
  NbIconModule,
  NbLayoutModule,
  NbWindowModule,
  NbCheckboxModule,
  NbTooltipModule,
  NbAlertModule,
  NbSpinnerModule,
  NbChatModule,
  NbMenuModule,
  NbToastrModule,
} from '@nebular/theme';

import { ThemeModule } from '../../@theme/theme.module';
import { CampaignComponent } from './campaign/campaign.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CampaignFormComponent } from './campaign/campaign-form/campaign-form.component';
import { WindowFormComponent } from '../modal-overlays/window/window-form/window-form.component';
import { CampaignService} from '../../@core/apis/campaign.service';
import { CustomerFormComponent } from './customer-list/customer-form/customer-form.component';
import { CampaignNewComponent } from './campaign/campaign-new/campaign-new.component';
import { ContactListService } from '../../@core/apis/contactList.service';
import { CustomerListEditComponent } from './customer-list/customer-list-edit/customer-list-edit.component';
import { ContactComponent } from './contact/contact.component';
import { ContactEditComponent } from './contact/contact-edit/contact-edit.component';
import { ContactFormComponent } from './contact/contact-form/contact-form.component';
import { ContactService } from '../../@core/apis/contact.service';
import { ClipboardModule } from '@angular/cdk/clipboard';
import { ChatComponent } from './chat/chat.component';

@NgModule({
  declarations: [
    SettingsComponent,
    MainComponent,
    CustomerListComponent,
    CustomerListEditComponent,
    CampaignComponent,
    ContactEditComponent,
    ContactFormComponent,
    CampaignFormComponent,
    CustomerFormComponent,
    CampaignNewComponent,
    ContactComponent,
    ChatComponent,
  ],
  imports: [
    CommonModule,
    ThemeModule,
    NbInputModule,
    NbTabsetModule,
    NbSelectModule,
    NbRouteTabsetModule,
    NbStepperModule,
    NbCardModule,
    NbButtonModule,
    NbListModule,
    NbAccordionModule,
    NbUserModule,
    NbIconModule,
    NbLayoutModule,
    NbInputModule,
    NbStepperModule,
    NbWindowModule.forChild(),
    FormsModule,
    ReactiveFormsModule,
    NgxDataTableModule,
    MainRoutingModule,
    NbCheckboxModule,
    NbTooltipModule,
    NbAlertModule,
    NbSpinnerModule,
    ClipboardModule,
    NbChatModule,
    NbMenuModule,
    NbToastrModule,
  ],
  entryComponents: [
    WindowFormComponent,

  ],
  providers: [
    CampaignService,
    ContactListService,
    ContactService,
  ],
})
export class MainModule { }
