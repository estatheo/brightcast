import { Component, OnInit } from '@angular/core';
import { NbWindowService, NbToastrService } from '@nebular/theme';
import { CampaignFormComponent } from './campaign-form/campaign-form.component';
import { of as observableOf, Observable } from 'rxjs';
import { CampaignService} from '../../../@core/apis/campaign.service';
import { CampaignElement } from '../../_models/campaignElement';
import { CampaignNewComponent } from './campaign-new/campaign-new.component';

@Component({
  selector: 'ngx-campaign',
  templateUrl: './campaign.component.html',
  styleUrls: ['./campaign.component.scss']
})
export class CampaignComponent implements OnInit {

  constructor(private windowService: NbWindowService, private toastrService: NbToastrService, private campaignService: CampaignService, private campaignsService: CampaignService) { }
  data: CampaignElement[];
  ngOnInit(): void {
    this.campaignsService.data.subscribe((data: CampaignElement[]) => {
      this.data = data;
    });
  }
  openModal() {
    this.windowService.open(CampaignNewComponent, { title: 'New Campaign', context: { contactListList: this.data[0].contactListList} });
  }
  
  openModalForEdit(event) {
    let item: CampaignElement = event;
    this.windowService.open(CampaignFormComponent, { title: 'Edit Campaign', context: { campaign: item } });
  }

  delete(id) {
    this.campaignsService.Delete(id).subscribe(() => {
      this.toastrService.primary("❌ The campaign has been deleted!", "Deleted!");
      this.campaignsService.refreshData();
      this.campaignsService.data.subscribe((data: CampaignElement[]) => {
        this.data = data;
      });
    }, error => {
      this.toastrService.warning("⚠ There was an error processing the request!", "Error!");
    })
  }
}
