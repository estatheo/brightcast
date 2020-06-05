import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import { AccountService } from '../../pages/_services';
import { NbToastrService } from '@nebular/theme';

@Component({
  selector: 'ngx-onboarding',
  templateUrl: './onboarding.component.html',
  styleUrls: ['./onboarding.component.scss']
})
export class OnboardingComponent implements OnInit {

  model;
  form1: any;
  form2: any;
  form3: any;
  form4: any;
  constructor(private router: Router, private formBuilder: FormBuilder, private accountService: AccountService, private toastrService: NbToastrService) { }

  ngOnInit(): void {
    this.form1 = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      picture: ['', Validators.required],
      phone: ['', Validators.required],
      role: ['', Validators.required]
    });
    this.form2 = this.formBuilder.group({
      name: ['', Validators.required],
      logo: ['', Validators.required],
      email: ['', Validators.required],
      website: ['', Validators.required],
      address: ['', Validators.required],
      category: ['', Validators.required]
    });
    this.form3 = this.formBuilder.group({
      contactListName: ['', Validators.required],
      contactList: ['', Validators.required]
    });
    this.form4 = this.formBuilder.group({
      name: ['', Validators.required],
      message: ['', Validators.required],
      file: ['', Validators.required]
    });
  }

  onSubmit() {
    this.accountService.onboarding({
      address: this.form2.controls.address.value,
      category: this.form2.controls.category.value,
      email:this.form2.controls.email.value,
      name: this.form2.controls.name.value,
      website: this.form2.controls.website.value,
      membership: "free"  }, {
        name: 'user',
        scope: ['user', this.form1.controls.role.value],
      }, {
        Default: true,
        Phone: this.form1.controls.phone.value,
        firstName: this.form1.controls.firstName.value,
        lastName: this.form1.controls.lastName.value,
        pictureUrl: 'test'
      }, {
        name: this.form4.controls.name.value,
        message: this.form4.controls.message.value,
        fileUrl: 'test'
      }, {
        name: this.form3.controls.contactListName.value,
        fileUrl: 'test'
      }).subscribe(result => {this.router.navigate(['pages/dashboard']);}, error => {
        this.toastrService.danger(error, "There was an error on our side😢");
      })
  }

  loadCampaigns() {
    this.router.navigate(['pages/dashboard']);
  }
}
