import { Component } from '@angular/core';
import { NbWindowRef, NbToastrService } from '@nebular/theme';
import { CampaignService } from '../../../../@core/apis/campaign.service';
import { FormBuilder, Validators } from '@angular/forms';
import { CampaignElement } from '../../../_models/campaignElement';
import { Router } from '@angular/router';
import { AccountService } from '../../../_services';
@Component({
    template: `
    <form class="form" [formGroup]="form">
      <div class="form-group">
        <label for="name" class="label">Name</label>
        <input nbInput fullWidth id="name" type="text" value="{{campaign.name}}" formControlName="name">
      </div>    
      <label for="file" class="label" >Media file</label>
      <input #image type="file" multiple accept="image/x-png,image/gif,image/jpeg" (change)="uploadImage(image.files)" nbInput fullWidth nbInput fullWidth id="file">    
      <label class="text-label" for="message">Message</label>
      <textarea nbInput fullWidth id="message" formControlName="message">{{campaign.message}}</textarea>      
      <button type="submit" style="margin-top: 10px" nbButton status="primary" class="button" (click)="onSubmit()">Save</button>
    </form>
  `,
    styleUrls: ['campaign-form.component.scss'],
})
export class CampaignFormComponent {

    image: FormData;
    campaign: CampaignElement;
    form;
    constructor(private router: Router, private toastrService: NbToastrService, private accountService: AccountService, private formBuilder: FormBuilder, public windowRef: NbWindowRef, private campaignService: CampaignService) {
      this.form = this.formBuilder.group({
        name: ['', Validators.required],
        message: ['', Validators.required],
        contactListId: ['', Validators.required],
      });
    }

    uploadImage(files) {
      if (files.length === 0) {
        return;
      }  

      this.image = new FormData();

      for (let file of files) {
        this.image.append(file.name, file);
      }
      
    }

    onSubmit() {
      this.accountService.uploadImage(this.image).subscribe(im => {
        this.campaignService.Update({
          id: this.campaign.id,
          name: this.form.controls.name.value,
          message: this.form.controls.message.value,
          contactListIds: [parseInt(this.form.controls.contactListId.value)],
          fileUrl: im['name']
        }).subscribe(() => {
          this.toastrService.success("🚀 The campaign has been updated!", "Success!");
          this.campaignService.refreshData();
          this.router.navigateByUrl('/',{skipLocationChange: true}).then(() => {
            this.router.navigate(['/pages/main/campaign']);
            this.close();
          });
        }, error => {
          this.toastrService.danger(error, "There was an error on our side😢");
        });
      });      
    }

    close() {
        this.windowRef.close();
    }
}
