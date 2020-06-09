import { Component } from '@angular/core';
import { NbWindowRef, NbToastrService } from '@nebular/theme';
import { CampaignService } from '../../../../@core/apis/campaign.service';
import { FormBuilder, Validators } from '@angular/forms';
import { CampaignElement } from '../../../_models/campaignElement';
import { Router } from '@angular/router';
@Component({
    template: `
    <form class="form" [formGroup]="form">
      <div class="form-group">
        <label for="name" class="label">Name</label>
        <input nbInput fullWidth id="name" type="text" value="{{campaign.name}}" formControlName="name">
      </div>    
      <label for="file" class="label">Media file</label>
      <input type="file" nbInput fullWidth id="file">    
      <label class="text-label" for="message">Message</label>
      <textarea nbInput fullWidth id="message" formControlName="message">{{campaign.message}}</textarea>      
      <button type="submit" style="margin-top: 10px" nbButton status="primary" class="button" (click)="onSubmit()">Save</button>
    </form>
  `,
    styleUrls: ['campaign-form.component.scss'],
})
export class CampaignFormComponent {

    campaign: CampaignElement;
    form;
    constructor(private router: Router, private toastrService: NbToastrService, private formBuilder: FormBuilder, public windowRef: NbWindowRef, private campaignService: CampaignService) {
      this.form = this.formBuilder.group({
        name: ['', Validators.required],
        message: ['', Validators.required],
        contactListId: ['', Validators.required],
      });
    }

    onSubmit() {
      this.campaignService.Update({
        id: this.campaign.id,
        name: this.form.controls.name.value,
        message: this.form.controls.message.value,
        contactListId: parseInt(this.form.controls.contactListId.value),
        fileUrl: ''
      }).subscribe(() => {
        this.toastrService.success("🚀 The campaign has been updated!", "Success!");
        this.campaignService.refreshData();
        this.router.navigateByUrl('/',{skipLocationChange: true}).then(() => {
          this.router.navigate(['/pages/main/campaign'])
        });
      }, error => {
        this.toastrService.danger(error, "There was an error on our side😢");
      });
    }

    close() {
        this.windowRef.close();
    }
}
