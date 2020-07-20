import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NbAuthComponent } from '@nebular/auth';
import { ContactSignupComponent } from './contact-signup.component';


const routes: Routes = [
  {
    path: '', component: NbAuthComponent,    // first one has to auth component with router-outlet inside of it
    children: [
      {
        path: '', component: ContactSignupComponent,
      },
    ],
  },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ContactSignupRoutingModule { }
