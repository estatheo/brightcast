import { NgModule } from '@angular/core';
import { NbMenuModule, NbAlertModule, NbInputModule, NbButtonModule, NbCheckboxModule, NbCardModule } from '@nebular/theme';

import { ThemeModule } from '../@theme/theme.module';
import { PagesComponent } from './pages.component';
import { DashboardModule } from './dashboard/dashboard.module';
import { ECommerceModule } from './e-commerce/e-commerce.module';
import { PagesRoutingModule } from './pages-routing.module';
import { MiscellaneousModule } from './miscellaneous/miscellaneous.module';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NbAuthModule } from '@nebular/auth';
import { LogoutComponent } from './logout/logout.component';
import { DashboardService } from '../@core/apis/dashboard.service';

@NgModule({
  imports: [
    PagesRoutingModule,
    ThemeModule,
    NbMenuModule,
    DashboardModule,
    ECommerceModule,
    MiscellaneousModule,
    CommonModule,
    FormsModule,
    RouterModule,
    NbAlertModule,
    NbInputModule,
    NbButtonModule,
    NbCheckboxModule,
    NbCardModule,
    NbAuthModule,
  ],
  declarations: [
    PagesComponent,
    LogoutComponent,
  ],
  providers: [
    DashboardService,
  ],
})
export class PagesModule {
}
