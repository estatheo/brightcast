import { Component, OnInit } from '@angular/core';
import { NbToastrService } from '@nebular/theme';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ContactService } from '../../@core/apis/contact.service';
import { ContactListService } from '../../@core/apis/contactList.service';

@Component({
  selector: 'ngx-register',
  templateUrl: './contact-signup.component.html',
  styleUrls: ['./contact-signup.component.scss'],
})
export class ContactSignupComponent implements OnInit {

  form: FormGroup;
  loading = false;
  submitted = false;
  contactListId: number = 0;
  contactListKeyString: string;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private toastrService: NbToastrService,
    private contactService: ContactService,
    private contactListSevice: ContactListService,
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(p => this.contactListKeyString = p['id'] );
    this.contactListSevice.GetContactListId(this.contactListKeyString).subscribe((data: number) => {
      this.contactListId = data;
      if (this.contactListId === 0) {
        this.toastrService.danger('Sorry, your contact list link url is incorrect.', 'Link Error');
      }
    });
    this.form = this.formBuilder.group({
      email: ['', Validators.required],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      phone: ['', Validators.required],
      subscribed: [true, Validators.required],
    });
  }

  onSubmit() {
    this.submitted = true;

    if (this.contactListId === 0) {
      this.toastrService.danger('Sorry, your contact list link url is incorrect.', 'Link Error');
    } else {
      this.contactService.NewContact({
        firstName: this.form.controls.firstName.value,
        lastName: this.form.controls.lastName.value,
        phone: this.form.controls.phone.value,
        email: this.form.controls.email.value,
        subscribed: this.form.controls.subscribed.value,
        contactListId: this.contactListId,
      }).subscribe(() => {
        this.toastrService.success('🚀 The Contact has been added!', 'Success!');
        this.contactService.refreshData();
      }, error => {
        this.toastrService.danger(error, 'There was an error on our side😢');
      });
    }
  }
}
