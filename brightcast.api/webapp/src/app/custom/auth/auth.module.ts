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
} from '@nebular/theme';
import { RegisterComponent } from './register/register.component';
import { ResetComponent } from './reset/reset.component';
import { RequestPasswordComponent } from './request-password/request-password.component';


@NgModule({
  declarations: [LoginComponent, RegisterComponent, ResetComponent, RequestPasswordComponent ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    RouterModule,
    NbAlertModule,
    NbInputModule,
    NbButtonModule,
    NbCheckboxModule,
    NbCardModule,
    AuthRoutingModule,

    NbAuthModule,

  ]
})
export class AuthModule { }
