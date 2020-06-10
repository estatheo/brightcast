import { Component } from '@angular/core';
import { NbWindowRef, NbToastrService } from '@nebular/theme';
import { CampaignService } from '../../../../@core/apis/campaign.service';
import { FormBuilder, Validators } from '@angular/forms';
import { ContactList } from '../../../_models/contactList';
import { Router } from '@angular/router';
@Component({
    template: `
    <form class="form" [formGroup]="form">
    <div class="form-group">
      <label for="name" class="label">Name</label>
      <input nbInput fullWidth id="name" type="text" value="" formControlName="name">
      </div>
    
      <label for="file" class="label">Media file</label>
      <input type="file" nbInput fullWidth id="file">
    
      <label class="text-label" for="message">Message</label>
      <textarea nbInput fullWidth id="message" formControlName="message"></textarea>

      <div class="form-group">
      <label for="selective_input" class="label">Contact list</label>
      <nb-select selected="1" id="selective_input" fullWidth *ngFor="let list of contactListList" formControlName="contactListId"> 
          <nb-option value="{{list.id}}">{{list.name}}</nb-option>
      </nb-select>
    </div>
    <button type="submit" style="margin-top: 10px" nbButton status="primary" class="button" (click)="onSubmit()">Save</button>
    </form>
  `,
    styleUrls: ['campaign-new.component.scss'],
})
export class CampaignNewComponent {

    contactListList: ContactList[];
    form;
    event;
    constructor(private router: Router, private formBuilder: FormBuilder, public windowRef: NbWindowRef, private campaignService: CampaignService, private toastrService: NbToastrService) {
       this.form = this.formBuilder.group({
         name: ['', Validators.required],
         message: ['', Validators.required],
         contactListId: ['', Validators.required],
       });
    }

    onSubmit() {
      this.campaignService.NewCampaign({
        name: this.form.controls.name.value,
        message: this.form.controls.message.value,
        contactListId: parseInt(this.form.controls.contactListId.value),
        fileUrl: ''
      }).subscribe(() => {
        this.toastrService.success("🚀 The campaign is on the way!", "Success!");
        this.campaignService.refreshData();
        this.router.navigateByUrl('/',{skipLocationChange: true}).then(() => {
          this.router.navigate(['/pages/main/campaign']);
          this.close();
        });
      }, error => {
        this.toastrService.danger(error, "There was an error on our side😢");
      });
    }

    close() {
        this.windowRef.close();
    }


}
