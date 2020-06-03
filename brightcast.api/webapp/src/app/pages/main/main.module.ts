import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MainRoutingModule } from './main-routing.module';
import { SettingsComponent } from './settings/settings.component';
import { MainComponent } from './main/main.component';
import { CustomerListComponent } from './customer-list/customer-list.component';
import { NgxDataTableModule } from "angular-9-datatable";
import {
  NbAccordionModule,
  NbButtonModule,
  NbCardModule,
  NbListModule,
  NbRouteTabsetModule,
  NbStepperModule,
  NbTabsetModule, NbUserModule, NbInputModule, NbSelectModule, NbIconModule, NbLayoutModule, NbWindowModule,
} from '@nebular/theme';

import { ThemeModule } from '../../@theme/theme.module';
import { CampaignComponent } from './campaign/campaign.component';
import { FormsModule } from '@angular/forms';
import { CampaignFormComponent } from './campaign/campaign-form/campaign-form.component';
import { WindowFormComponent } from '../modal-overlays/window/window-form/window-form.component';
import { CampaignService } from './campaign/campaign-form/campaign-form.service';
import { OnboardingComponent } from './onboarding/onboarding.component';
import { CustomerFormComponent } from './customer-list/customer-form/customer-form.component';
import { CustomerService } from './customer-list/customer-form/customer.service';

@NgModule({
  declarations: [SettingsComponent, MainComponent, CustomerListComponent, CampaignComponent, CampaignFormComponent, OnboardingComponent, CustomerFormComponent],
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
    NgxDataTableModule,
    MainRoutingModule
  ],
  entryComponents: [
    WindowFormComponent,

  ],
  providers: [
    CampaignService,
    CustomerService
  ]
})
export class MainModule { }
