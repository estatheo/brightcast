import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import { AccountService } from '../../_services';
import { NbToastrService } from '@nebular/theme';
import { Business } from '../../_models/business';
import { User } from '../../_models';
import { UserProfile } from '../../_models/userProfile';

@Component({
  selector: 'ngx-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit {
  btnstuatus = ['primary', 'secondary']
  settingClass = ['personal', 'business'];
  userPicture: FormData;
  businessLogo: FormData;
  form1: any;
  form2: any;
  edit = true;
  edit1 = true;
  business: Business;
  user: UserProfile;
  dataReady = false;
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
    this.accountService.getSettingsData().subscribe(data => {
      this.business = data['business'];
      this.user = data['user'];
      this.form1.patchValue(this.user);
      this.form2.patchValue(this.business);
      this.dataReady = true;
    });
    
  }

  uploadImage(files, isBusiness) {
    if (files.length === 0) {
      return;
    }

    if(isBusiness) {
      this.businessLogo = new FormData();

      for (let file of files) {
        this.businessLogo.append(file.name, file);
      }  
    } else {
      this.userPicture = new FormData();

      for (let file of files) {
        this.userPicture.append(file.name, file);
      }  
    }
    
  }

  toggle() {
    let temp = this.btnstuatus[0];
    this.btnstuatus[0] = this.btnstuatus[1];
    this.btnstuatus[1] = temp;

    temp = this.settingClass[0];
    this.settingClass[0] = this.settingClass[1];
    this.settingClass[1] = temp;
  }

  onSubmitPersonal() {
    this.accountService.uploadImage(this.userPicture).subscribe(up => {
      this.accountService.update(this.user.id, {
        id: this.user.id,
        firstName: this.form1.controls.firstName.value,
        lastName: this.form1.controls.lastName.value,
        pictureUrl: up['name'],
        Phone: this.form1.controls.phone.value,
        Default: this.user.Default
      }).subscribe(() => {
        this.toastrService.success("🚀 The campaign has been updated!", "Success!");}, error => {
          this.toastrService.danger(error, "There was an error on our side😢");
      });
    });
  
  }

  onSubmitBusiness() {
    this.accountService.uploadImage(this.businessLogo).subscribe(bl => {
      this.accountService.updateBusiness({
        id: this.business.id,
        name: this.form2.controls.name.value,
        membership: this.business.membership,
        website: this.form2.controls.website.value,
        address: this.form2.controls.address.value,
        email: this.form2.controls.email.value,
        category: this.form2.controls.category.value,
      }).subscribe(() => {
        this.toastrService.success("🚀 The campaign has been updated!", "Success!");}, error => {
          this.toastrService.danger(error, "There was an error on our side😢");
      });
    })
  }

}
