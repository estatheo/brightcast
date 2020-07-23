import { Component, OnInit } from '@angular/core';
import { NbWindowRef, NbToastrService } from '@nebular/theme';
import { CampaignService } from '../../../../@core/apis/campaign.service';
import { FormBuilder, Validators } from '@angular/forms';
import { ContactList } from '../../../_models/contactList';
import { Router } from '@angular/router';
import { AccountService } from '../../../_services';
@Component({
    template: `
    <form class="form" [formGroup]="form">
      <div class="form-group">
        <label for="name" class="label">Name</label>
        <input nbInput fullWidth id="name" type="text" value="" formControlName="name">
      </div>
      <label for="file" class="label">Media file</label>
      <input #image type="file" multiple accept="image/x-png,image/gif,image/jpeg" (change)="uploadImage(image.files)" nbInput fullWidth id="file">
      <label class="text-label" for="message">Message</label>
      <textarea nbInput fullWidth id="message" formControlName="message"></textarea>
      <div class="form-group" *ngIf="contactListList != null && contactListList !== undefined && contactListList.length">
        <label for="selective_input" class="label">Contact list</label>
        <nb-select id="selective_input" fullWidth formControlName="contactListId">
          <nb-option *ngFor="let list of contactListList" [value]="list.id">{{list.name}}</nb-option>
        </nb-select>
      </div>
      <button [nbSpinner]="loading" nbSpinnerStatus="success" type="submit" style="margin-top: 10px" nbButton status="primary" class="button" (click)="onSubmit()">Save</button>
    </form>
  `,
    styleUrls: ['campaign-new.component.scss'],
})
export class CampaignNewComponent implements OnInit {
    loading = false;
    image: FormData;
    contactListList: ContactList[];
    form;
    event;
    constructor(
      private router: Router,
      private formBuilder: FormBuilder,
      public windowRef: NbWindowRef,
      private accountService: AccountService,
      private campaignService: CampaignService,
      private toastrService: NbToastrService) {
    }

    ngOnInit() {
      this.form = this.formBuilder.group({
        name: ['', Validators.required],
        message: ['', Validators.required],
        contactListId: [
          this.contactListList != null &&
          this.contactListList !== undefined &&
          this.contactListList.length ? this.contactListList[0].id : 0],
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
      this.loading = true;

      if (this.image != null || this.image !== undefined) {
        this.accountService.uploadImage(this.image).subscribe(im => {
          this.campaignService.NewCampaign({
            name: this.form.controls.name.value,
            message: this.form.controls.message.value,
            contactListIds: [parseInt(this.form.controls.contactListId.value, 10)],
            status: 0,
            fileUrl: im['name'],
          }).subscribe(() => {
            this.toastrService.success('🚀 The campaign is on the way!', 'Success!');
            this.campaignService.refreshData();
            this.router.navigateByUrl('/', {skipLocationChange: true}).then(() => {
              this.router.navigate(['/pages/main/campaign']);
              this.close();
            });
            this.loading = false;
          }, error => {
            this.toastrService.danger(error, 'There was an error on our side😢');
            this.loading = false;
          });
        });
      } else {
        this.campaignService.NewCampaign({
          name: this.form.controls.name.value,
          message: this.form.controls.message.value,
          contactListIds: [parseInt(this.form.controls.contactListId.value, 10)],
          status: 0,
          fileUrl: '',
        }).subscribe(() => {
          this.toastrService.success('🚀 The campaign has been created on the way!', 'Success!');
          this.campaignService.refreshData();
          this.router.navigateByUrl('/', {skipLocationChange: true}).then(() => {
            this.router.navigate(['/pages/main/campaign']);
            this.close();
          });
          this.loading = false;
        }, error => {
          this.toastrService.danger(error, 'There was an error on our side😢');
          this.loading = false;
        });
      }
    }

    close() {
        this.windowRef.close();
    }


}
