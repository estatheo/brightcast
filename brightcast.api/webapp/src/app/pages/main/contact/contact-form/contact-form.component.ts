import { Component } from '@angular/core';
import { NbWindowRef, NbToastrService } from '@nebular/theme';
import { Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import { ContactService } from '../../../../@core/apis/contact.service';


@Component({
    template:
    `<form class="form" [formGroup]="form">
    <div class="form-group">
        <label for="name" class="label">Contact First Name</label>
        <input nbInput fullWidth id="firstName" type="text" value="" formControlName="firstName">
    </div>
    <div class="form-group">
        <label for="name" class="label">Contact Last Name</label>
        <input nbInput fullWidth id="lastName" type="text" value="" formControlName="lastName">
    </div>
    <div class="form-group">
        <label for="name" class="label">Contact Phone Number</label>
        <input nbInput fullWidth id="phone" type="tel" value="" formControlName="phone">
    </div>
    <div class="form-group">
        <label for="name" class="label">Contact Email</label>
        <input nbInput fullWidth id="email" type="email" value="" formControlName="email">
    </div>
    <div class="form-group">
        <label for="name" class="label">Contact Subscribed</label>
        <br/>
        <nb-checkbox id="subscribed" value="true" formControlName="subscribed"></nb-checkbox>
    </div>
    <button [nbSpinner]="loading" nbSpinnerStatus="success" type="submit" style="margin-top: 10px" nbButton status="primary" (click)="onSubmit()">Save</button>
    </form>`,
    styleUrls: ['contact-form.component.scss'],
})
export class ContactFormComponent {
    contactListId;
    event;
    form;
    loading = false;
    constructor(
      private router: Router,
      private formBuilder: FormBuilder,
      public windowRef: NbWindowRef,
      private toastrService: NbToastrService,
      private contactService: ContactService) {
        this.form = this.formBuilder.group({
          firstName: ['', Validators.required],
          lastName: ['', Validators.required],
          phone: ['', Validators.required],
          email: ['', Validators.required],
          subscribed: ['', Validators.required],
          });
    }

    onSubmit() {
      this.loading = true;

      this.contactService.NewContact({
        firstName: this.form.controls.firstName.value,
        lastName: this.form.controls.lastName.value,
        phone: this.form.controls.phone.value,
        email: this.form.controls.email.value,
        subscribed: this.form.controls.subscribed.value,
        contactListId: parseInt(this.contactListId, 10),
      }).subscribe(() => {
        this.toastrService.success('🚀 The Contact has been added!', 'Success!');
        this.contactService.refreshData();
        this.router.navigateByUrl('/', {skipLocationChange: true}).then(() => {
          this.router.navigate([`/pages/main/customer-list/${this.contactListId}/contacts`]);
          this.close();
        });
      }, error => {
        this.loading = false;
        this.toastrService.danger(error, 'There was an error on our side😢');
      });

      setTimeout(() => {
        this.loading = false;
      }, 7000);
    }

    close() {
        this.windowRef.close();
    }
}
