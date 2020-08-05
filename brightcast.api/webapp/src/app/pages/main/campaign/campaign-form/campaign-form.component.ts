import { Component, OnInit } from '@angular/core';
import { NbWindowRef, NbToastrService } from '@nebular/theme';
import { CampaignService } from '../../../../@core/apis/campaign.service';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from '../../../_services';
import { ContactList } from '../../../_models/contactList';
@Component({
    template: `
    <form class="form" [formGroup]="form">
      <div class="form-group">
        <label for="name" class="label">Name</label>
        <input nbInput fullWidth id="name" type="text" value="{{campaign.name}}" formControlName="name">
      </div>
      <label for="file" class="label" >Media file</label>
      <input #image type="file" multiple accept="image/x-png,image/gif,image/jpeg" (change)="uploadImage(image.files)" filename="campaign?.fileUrl" nbInput fullWidth id="file">
      <label class="text-label" for="message">Message</label>
      <textarea nbInput fullWidth id="message" formControlName="message">{{campaign.message}}</textarea>
      <div class="form-group">
        <label for="selective_input" class="label">Contact list</label>
        <nb-select id="selective_input" fullWidth formControlName="contactListId">
          <nb-option *ngFor="let list of contactListList" [value]="list.id">{{list.name}}</nb-option>
        </nb-select>
      </div>
      <button type="submit" style="margin-top: 10px" nbButton status="primary" class="button" (click)="onSubmit()">Save</button>
    </form>
  `,
    styleUrls: ['campaign-form.component.scss'],
})
export class CampaignFormComponent implements OnInit {

    image: FormData;
    contactListList: ContactList[];
    campaign: any;
    form;
    constructor(
      private router: Router,
      private toastrService: NbToastrService,
      private accountService: AccountService,
      private formBuilder: FormBuilder,
      public windowRef: NbWindowRef,
      private campaignService: CampaignService) {

    }

    ngOnInit() {
      this.form = this.formBuilder.group({
        name: [this.campaign.name, Validators.required],
        message: [this.campaign.message, Validators.required],
        contactListId: [
          this.campaign.contactListIds != null &&
          this.campaign.contactListIds !== undefined &&
          this.campaign.contactListIds.length ? this.campaign.contactListIds[0] : 0],
      });
    }

    uploadImage(files) {
      if (files.length === 0) {
        return;
      }

      this.image = new FormData();

      for (const file of files) {
        this.image.append(file.name, file);
      }

    }

    onSubmit() {
      if (this.image != null || this.image !== undefined) {
        this.accountService.uploadImage(this.image).subscribe(im => {
          this.campaignService.Update({
            id: this.campaign.id,
            name: this.form.controls.name.value,
            message: this.form.controls.message.value,
            contactListIds: [parseInt(this.form.controls.contactListId.value, 10)],
            fileUrl: im['name'],
          }).subscribe(() => {
            this.toastrService.success('🚀 The campaign has been updated!', 'Success!');
            this.campaignService.refreshData();
            this.router.navigateByUrl('/', {skipLocationChange: true}).then(() => {
              this.router.navigate(['/pages/main/campaign']);
              this.close();
            });
          }, error => {
            this.toastrService.danger(error, 'There was an error on our side😢');
          });
        });
      } else {
        this.campaignService.Update({
          id: this.campaign.id,
          name: this.form.controls.name.value,
          message: this.form.controls.message.value,
          contactListIds: [parseInt(this.form.controls.contactListId.value, 10)],
          fileUrl: '',
        }).subscribe(() => {
          this.toastrService.success('🚀 The campaign has been updated!', 'Success!');
          this.campaignService.refreshData();
          this.router.navigateByUrl('/', {skipLocationChange: true}).then(() => {
            this.router.navigate(['/pages/main/campaign']);
            this.close();
          });
        }, error => {
          this.toastrService.danger(error, 'There was an error on our side😢');
        });
      }
    }

    close() {
        this.windowRef.close();
    }
}
