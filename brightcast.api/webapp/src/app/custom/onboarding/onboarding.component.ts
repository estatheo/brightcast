import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import { AccountService } from '../../pages/_services';
import { NbToastrService } from '@nebular/theme';

@Component({
  selector: 'ngx-onboarding',
  templateUrl: './onboarding.component.html',
  styleUrls: ['./onboarding.component.scss'],
})
export class OnboardingComponent implements OnInit {
  linearMode = true;
  loading = false;
  userPicture: FormData;
  businessLogo: FormData;
  contactListData: FormData;
  mediaData: FormData;
  model;
  form1: any;
  form2: any;
  form3: any;
  form4: any;
  constructor(
    private router: Router,
    private formBuilder: FormBuilder,
    private accountService: AccountService,
    private toastrService: NbToastrService) { }

  ngOnInit(): void {
    this.toastrService.info('', 'You should fill all input fields.', { duration: 5000 });

    this.form1 = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      picture: ['', Validators.required],
      phone: ['', Validators.required],
      role: ['Founder', Validators.required],
    });
    this.form2 = this.formBuilder.group({
      name: ['', Validators.required],
      email: ['', Validators.required],
      website: ['', Validators.required],
      address: ['', Validators.required],
      category: ['Automotive', Validators.required],
    });
    this.form3 = this.formBuilder.group({
      contactListName: ['', Validators.required],
      contactList: ['', Validators.required],
    });
    this.form4 = this.formBuilder.group({
      name: ['', Validators.required],
      message: ['', Validators.required],
      file: [''],
    });
  }

  upload(files, isContactList) {
    if (files.length === 0) {
      return;
    }

    if (isContactList) {
      this.contactListData = new FormData();

      for (const file of files) {
        this.contactListData.append(file.name, file);
      }
    } else {
      this.mediaData = new FormData();

      for (const file of files) {
        this.mediaData.append(file.name, file);
      }
    }
  }

  uploadImage(files, isBusiness) {
    if (files.length === 0) {
      return;
    }

    if (isBusiness) {
      this.businessLogo = new FormData();

      for (const file of files) {
        this.businessLogo.append(file.name, file);
      }
    } else {
      this.userPicture = new FormData();

      for (const file of files) {
        this.userPicture.append(file.name, file);
      }
    }
  }


  onSubmit() {
    this.loading = true;

    if (this.mediaData == null || this.mediaData === undefined) {
      this.accountService.uploadImage(this.userPicture).subscribe(up => {
        this.accountService.uploadDoc(this.contactListData).subscribe(dd => {
          this.accountService.onboarding({
            address: this.form2.controls.address.value,
            category: this.form2.controls.category.value,
            email: this.form2.controls.email.value,
            name: this.form2.controls.name.value,
            website: this.form2.controls.website.value,
            membership: 'free'  }, {
              name: 'user',
              scope: ['user', this.form1.controls.role.value],
            }, {
              Default: true,
              Phone: this.form1.controls.phone.value,
              firstName: this.form1.controls.firstName.value,
              lastName: this.form1.controls.lastName.value,
              pictureUrl: up['name'],
            }, {
              name: this.form4.controls.name.value,
              message: this.form4.controls.message.value,
              fileUrl: '',
            }, {
              name: this.form3.controls.contactListName.value,
              fileUrl: dd['name'],
            }).subscribe(result => {this.router.navigate(['pages/dashboard']); }, error => {
              this.toastrService.danger(`⚠ ${error}`, 'Error!');
          });
        }, error => {
          this.toastrService.danger(`⚠ ${error}`, 'Error!');
      });
      }, error => {
        this.toastrService.danger(`⚠ ${error}`, 'Error!');
    });
    } else {
      this.accountService.uploadDoc(this.mediaData).subscribe(md => {
        this.accountService.uploadImage(this.userPicture).subscribe(up => {
          this.accountService.uploadDoc(this.contactListData).subscribe(dd => {
            this.accountService.onboarding({
              address: this.form2.controls.address.value,
              category: this.form2.controls.category.value,
              email: this.form2.controls.email.value,
              name: this.form2.controls.name.value,
              website: this.form2.controls.website.value,
              membership: 'free'  }, {
                name: 'user',
                scope: ['user', this.form1.controls.role.value],
              }, {
                Default: true,
                Phone: this.form1.controls.phone.value,
                firstName: this.form1.controls.firstName.value,
                lastName: this.form1.controls.lastName.value,
                pictureUrl: up['name'],
              }, {
                name: this.form4.controls.name.value,
                message: this.form4.controls.message.value,
                fileUrl: md['name'],
              }, {
                name: this.form3.controls.contactListName.value,
                fileUrl: dd['name'],
              }).subscribe(result => {this.router.navigate(['pages/dashboard']); }, error => {
                this.toastrService.danger(`⚠ ${error}`, 'Error!');
            });
          }, error => {
            this.toastrService.danger(`⚠ ${error}`, 'Error!');
        });
        }, error => {
          this.toastrService.danger(`⚠ ${error}`, 'Error!');
      });
      }, error => {
        this.toastrService.danger(`⚠ ${error}`, 'Error!');
    });

    }


  }

  loadCampaigns() {
    this.router.navigate(['pages/dashboard']);
  }
}
