import { Component, OnInit } from '@angular/core';
import { NbWindowService, NbToastrService } from '@nebular/theme';
import { CampaignFormComponent } from './campaign-form/campaign-form.component';
import { CampaignService} from '../../../@core/apis/campaign.service';
import { CampaignNewComponent } from './campaign-new/campaign-new.component';
import { CampaignData } from '../../_models/campaignData';

@Component({
  selector: 'ngx-campaign',
  templateUrl: './campaign.component.html',
  styleUrls: ['./campaign.component.scss'],
})
export class CampaignComponent implements OnInit {

  constructor(
    private windowService: NbWindowService,
    private toastrService: NbToastrService,
    private campaignsService: CampaignService) { }
  loading = false;
  data: any;
  ngOnInit(): void {
    this.campaignsService.refreshData();
    this.campaignsService.data.subscribe((data: CampaignData) => {
      this.data = data;
    });
  }

  openModal() {
    this.windowService.open(
      CampaignNewComponent,
      { title: 'New Campaign', context: { contactListList: this.data.contactLists} });
  }

  openModalForEdit(event) {
    this.campaignsService.data.subscribe((data: CampaignData) => {
      this.data = data;
      this.windowService.open(
        CampaignFormComponent,
        { title: 'Edit Campaign', context: { contactListList: this.data.contactLists, campaign: event } });
    });

  }

  delete(id) {
    this.campaignsService.Delete(id).subscribe(() => {
      this.toastrService.primary('❌ The campaign has been deleted!', 'Deleted!');
      this.campaignsService.refreshData();
      this.campaignsService.requestData().subscribe((data: CampaignData) => {
        this.data = data;
      });
    }, error => {
      this.toastrService.danger('⚠ There was an error processing the request!', 'Error!');
    });
  }

  send(campaign) {
    this.loading = true;
    this.campaignsService.SendCampaign(campaign).subscribe(result => {
      this.toastrService.primary('🎉 The campaign has been sent!', 'Success!');
      this.campaignsService.refreshData();
      this.campaignsService.data.subscribe((data: CampaignData) => {
        this.data = data;
      });
      this.loading = false;
    }, error => {
      this.toastrService.danger('⚠ There was an error processing the request!', 'Error!');
      this.campaignsService.refreshData();
      this.loading = false;
    });
  }
}
