import { Component, OnInit } from '@angular/core';
import { NbWindowService } from '@nebular/theme';
import { CampaignFormComponent } from './campaign-form/campaign-form.component';
import { CampaignService } from './campaign-form/campaign-form.service';
import { of as observableOf, Observable } from 'rxjs';

@Component({
  selector: 'ngx-campaign',
  templateUrl: './campaign.component.html',
  styleUrls: ['./campaign.component.scss']
})
export class CampaignComponent implements OnInit {

  constructor(private windowService: NbWindowService, private campaignService: CampaignService) { }
  data = [
    {
      name: "initial",
      sent: 100,
      read: 0,
      response: 0,
      status: 1,
    },
    {
      name: "Product Launch",
      sent: 100,
      read: 1,
      response: 0,
      status: 2,
    },
    {
      name: "Marketing",
      sent: 100,
      read: 0,
      response: 0,
      status: 3,
    }
  ];
  ngOnInit(): void {
  }
  openModal() {
    console.log("the modal open button clicked");
    let event = {
      sent: 0,
      read: 0,
    }
    let observable = observableOf(event);
    this.campaignService.setState(observable);
    this.windowService.open(CampaignFormComponent, { title: 'window' });
  }
  openModalForEdit(event) {
    console.log("the event from the Edit: ", event);
    let observable = observableOf(event);
    this.campaignService.setState(observable);
    this.windowService.open(CampaignFormComponent, { title: 'window' });
  }
}
