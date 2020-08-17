import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SettingsComponent } from './settings/settings.component';
import { MainComponent } from './main/main.component';
import { CustomerListComponent } from './customer-list/customer-list.component';
import { CampaignComponent } from './campaign/campaign.component';
import { ContactComponent } from './contact/contact.component';
import { ChatComponent } from './chat/chat.component';


const routes: Routes = [
  {
    path: '', component: MainComponent,
    children: [
      {
        path: 'settings', component: SettingsComponent,
      },
      {
        path: 'customer-list', component: CustomerListComponent,
      },
      {
        path: 'campaign', component: CampaignComponent,
      },
      {
        path: 'customer-list/:id/contacts', component: ContactComponent,
      },
      {
        path: 'chat', component: ChatComponent,
      },
      {
        path: 'campaign/chat/:campaignId/:contactId', component: ChatComponent,
      },
    ],
  },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MainRoutingModule { }
