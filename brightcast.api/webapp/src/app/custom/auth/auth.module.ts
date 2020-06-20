import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AuthRoutingModule } from './auth-routing.module';
import { LoginComponent } from './login/login.component';
import { NbAuthModule } from '@nebular/auth';
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
} from '@nebular/theme';
import { RegisterComponent } from './register/register.component';
import { ResetComponent } from './reset/reset.component';
import { ThemeModule } from '../../@theme/theme.module';
import { VerifyComponent } from './verify/verify.component';
import { NewPasswordComponent } from './new-password/new-password.component';
import { CommunicationPageComponent } from './communication-page/communication-page.component';


@NgModule({
  declarations: [LoginComponent, RegisterComponent, ResetComponent, VerifyComponent, NewPasswordComponent, CommunicationPageComponent],
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
    AuthRoutingModule,
    NbTabsetModule,
    NbSelectModule,
    NbRouteTabsetModule,
    NbStepperModule,

    NbAuthModule,

  ]
})
export class AuthModule { }
