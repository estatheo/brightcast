import { Component, OnInit } from '@angular/core';
import { NbToastrService } from '@nebular/theme';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ContactService } from '../../@core/apis/contact.service';

@Component({
  selector: 'ngx-register',
  templateUrl: './contact-signup.component.html',
  styleUrls: ['./contact-signup.component.scss'],
})
export class ContactSignupComponent implements OnInit {

  form: FormGroup;
  loading = false;
  submitted = false;
  contactListId: string;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private toastrService: NbToastrService,
    private contactService: ContactService,
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(p => this.contactListId = p['id'] );
    this.toastrService.info('', this.contactListId);
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
      });
    }, error => {
      this.toastrService.danger(error, 'There was an error on our side😢');
    });
  }
}
