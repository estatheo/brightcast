import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { ResetComponent } from './reset/reset.component';
import { NbAuthComponent } from '@nebular/auth';
import { VerifyComponent } from './verify/verify.component';
import { NewPasswordComponent } from './new-password/new-password.component';
import { CommunicationPageComponent } from './communication-page/communication-page.component';
import { TermsComponent } from './terms/terms.component';


const routes: Routes = [
  {
    path: '', component: NbAuthComponent,    // first one has to auth component with router-outlet inside of it
    children: [
      {
        path: '', component: LoginComponent,
      },
      {
        path: 'login', component: LoginComponent,
      },
      {
        path: 'register', component: RegisterComponent,
      },
      {
        path: 'terms', component: TermsComponent,
      },
      {
        path: 'verify/:id', component: VerifyComponent,
      },
      {
        path: 'reset-password', component: ResetComponent,
      },
      {
        path: 'new-password/:id', component: NewPasswordComponent,
      },
      {
        path: ':route/confirm', component: CommunicationPageComponent,
      },
    ],
  },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AuthRoutingModule { }
