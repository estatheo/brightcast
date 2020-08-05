import { Component, OnInit } from '@angular/core';
import { NbWindowRef, NbToastrService } from '@nebular/theme';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Contact } from '../../../_models/contact';
import { ContactService } from '../../../../@core/apis/contact.service';


@Component({
    template: `<form class="form" [formGroup]="form">
    <div class="form-group">
        <label for="name" class="label">Contact First Name</label>
        <input nbInput fullWidth id="firstName" type="text" value="{{contactElement.firstName}}" formControlName="firstName">
    </div>
    <div class="form-group">
        <label for="name" class="label">Contact Last Name</label>
        <input nbInput fullWidth id="lastName" type="text" value="{{contactElement.lastName}}" formControlName="lastName">
    </div>
    <div class="form-group">
        <label for="name" class="label">Contact Phone Number</label>
        <input nbInput fullWidth id="phone" type="tel" value="{{contactElement.phone}}" formControlName="phone">
    </div>
    <div class="form-group">
        <label for="name" class="label">Contact Email</label>
        <input nbInput fullWidth id="email" type="email" value="{{contactElement.email}}" formControlName="email">
    </div>
    <div class="form-group">
        <label for="name" class="label">Contact Subscribed</label>
        <br/>
        <nb-checkbox id="subscribed" value="{{contactElement.subscribed}}" formControlName="subscribed"></nb-checkbox>
    </div>
    <button type="submit" style="margin-top: 10px" nbButton status="primary" (click)="onSubmit()">Save</button>
</form>`,
    styleUrls: ['contact-edit.component.scss'],
})
export class ContactEditComponent implements OnInit {
    contactElement: Contact;
    form;
    constructor(
      private router: Router,
      private formBuilder: FormBuilder,
      public windowRef: NbWindowRef,
      private contactService: ContactService,
      private toastrService: NbToastrService) {
        this.form = this.formBuilder.group({
          firstName: ['', Validators.required],
          lastName: ['', Validators.required],
          phone: ['', Validators.required],
          email: ['', Validators.required],
          subscribed: ['', Validators.required],
        });
    }

    ngOnInit() {
      this.form.patchValue(this.contactElement);
    }

    close() {
        this.windowRef.close();
    }

    onSubmit() {
        this.contactService.Update({
          id: this.contactElement.id,
          firstName: this.form.controls.firstName.value,
          lastName: this.form.controls.lastName.value,
          phone: this.form.controls.phone.value,
          email: this.form.controls.email.value,
          subscribed: this.form.controls.subscribed.value,
          contactListId: this.contactElement.contactListId,

        }).subscribe(() => {
            this.toastrService.success('🚀 The Contact List has been updated!', 'Success!');
            this.contactService.refreshData();
            this.router.navigateByUrl('/', {skipLocationChange: true}).then(() => {
              this.router.navigate([`/pages/main/customer-list/${this.contactElement.contactListId}/contacts`]);
              this.close();
            });
          }, error => {
            this.toastrService.danger(error, 'There was an error on our side😢');
          });
      }
}
