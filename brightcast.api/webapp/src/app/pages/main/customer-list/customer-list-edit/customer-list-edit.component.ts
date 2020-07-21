import { Component, OnInit } from '@angular/core';
import { NbWindowRef, NbToastrService } from '@nebular/theme';
import { ContactListElement } from '../../../_models/contactListElement';
import { ContactListService } from '../../../../@core/apis/contactList.service';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from '../../../_services';


@Component({
    template: `
    <form class="form" [formGroup]="form">
      <div class="form-group">
          <label for="name" class="label">Contact List Name</label>
          <input nbInput fullWidth id="name" type="text"  value="{{contactList.name}}" formControlName="name">
      </div>
      <nb-alert outline="warning">
        <p>Download the <a href="https://brightcaststrgprd.blob.core.windows.net/assets/ContactList.csv" download="template.csv" target="blank" style="color: hotpink;">.csv template</a>, add your contacts and reupload the file!</p>
      </nb-alert>
      <label for="file" class="label">Contacts file</label>
      <input #docfile type="file" multiple accept=".csv" (change)="upload(docfile.files)" nbInput fullWidth id="file">
      <button [nbSpinner]="loading" nbSpinnerStatus="success" type="submit" style="margin-top: 10px" nbButton status="primary" (click)="onSubmit()">Save</button>
    </form>
    `,
    styleUrls: ['customer-list-edit.component.scss'],
})
export class CustomerListEditComponent implements OnInit {
    loading = false;
    docData: FormData;
    contactList: ContactListElement;
    form;
    constructor(
      private router: Router,
      private formBuilder: FormBuilder,
      private accountService: AccountService,
      public windowRef: NbWindowRef,
      private contactListService: ContactListService,
      private toastrService: NbToastrService) {

    }

    ngOnInit() {
      this.form = this.formBuilder.group({
        name: [this.contactList.name, Validators.required],
      });
    }

    close() {
        this.windowRef.close();
    }

    upload(files) {
      if (files.length === 0) {
        return;
      }

      this.docData = new FormData();

      for (const file of files) {
        this.docData.append(file.name, file);
      }
    }

    onSubmit() {
      this.loading = true;

      this.accountService.uploadDoc(this.docData).subscribe(csvfile => {
        this.contactListService.Update({
          id: this.contactList.id,
          name: this.form.controls.name.value,
          fileUrl: csvfile['name'],
        }).subscribe(() => {
            this.toastrService.success('🚀 The Contact List has been updated!', 'Success!');
            this.contactListService.refreshData();
            this.router.navigateByUrl('/', {skipLocationChange: true}).then(() => {
              this.router.navigate(['/pages/main/customer-list']);
              this.close();
            });
          }, error => {
            this.toastrService.danger(error, 'There was an error on our side😢');
            this.loading = false;
        });
      });

      setTimeout(() => {
        this.loading = false;
      }, 7000);
    }
}
