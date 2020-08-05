import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NbAuthComponent } from '@nebular/auth';
import { OnboardingComponent } from './onboarding.component';


const routes: Routes = [
  {
    path: '', component: NbAuthComponent,    // first one has to auth component with router-outlet inside of it
    children: [
      {
        path: '', component: OnboardingComponent,
      },
    ],
  },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class OnboardingRoutingModule { }
