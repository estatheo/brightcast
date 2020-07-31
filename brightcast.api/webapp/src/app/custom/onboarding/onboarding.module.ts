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
  NbIconModule,
  NbSpinnerModule,
} from '@nebular/theme';
import { ThemeModule } from '../../@theme/theme.module';
import { OnboardingComponent } from './onboarding.component';
import { OnboardingRoutingModule } from './onboarding-routing.module';
import { NbEvaIconsModule } from '@nebular/eva-icons';


@NgModule({
  declarations: [ OnboardingComponent ],
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
    OnboardingRoutingModule,
    NbToastrModule,
    NbEvaIconsModule,
    NbIconModule,
    NbSpinnerModule,
  ],
})
export class OnboardingModule { }
