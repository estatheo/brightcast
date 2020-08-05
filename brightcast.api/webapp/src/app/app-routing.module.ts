import { ExtraOptions, RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { AuthGuard } from './pages/_helpers';
import { LandingComponent } from './landing/landing.component';

export const routes: Routes = [

  {
    path: 'home', component: LandingComponent,
  },
  {
    path: 'pages',
    loadChildren: () => import('./pages/pages.module').then(m => m.PagesModule), canActivate: [AuthGuard],
  },
  {
    path: 'auth',
    loadChildren: './custom/auth/auth.module#AuthModule',
  },
  {
    path: 'onboarding',
    loadChildren: './custom/onboarding/onboarding.module#OnboardingModule', canActivate: [AuthGuard],
  },
  {
    path: 'signup_contact/:id',
    loadChildren: './custom/contact-signup/contact-signup.module#ContactSignupModule',
  },
  { path: '', redirectTo: 'pages', pathMatch: 'full' },
  { path: '**', redirectTo: 'pages' },
];

const config: ExtraOptions = {
  useHash: false,
};

@NgModule({
  imports: [RouterModule.forRoot(routes, config)],
  exports: [RouterModule],
})
export class AppRoutingModule {
}
