import { Component } from '@angular/core';
import { NbWindowRef, NbToastrService } from '@nebular/theme';
import { ContactListElement } from '../../../_models/contactListElement';
import { ContactListService } from '../../../../@core/apis/contactList.service';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';


@Component({
    template: `<form class="form" [formGroup]="form">
    <div class="form-group">
        <label for="name" class="label">Contact List Name</label>
        <input nbInput fullWidth id="name" type="text"  value="{{contactList.name}}" formControlName="name">
    </div>
    <label for="file" class="label">Contacts file</label>
    <input type="file" nbInput fullWidth id="file">


    <button type="submit" style="margin-top: 10px" nbButton status="primary" (click)="onSubmit()">Save</button>
</form>`,
    styleUrls: ['customer-list-edit.component.scss'],
})
export class CustomerListEditComponent {
    contactList: ContactListElement;
    form;
    constructor(private router: Router, private formBuilder: FormBuilder, public windowRef: NbWindowRef, private contactListService: ContactListService, private toastrService: NbToastrService) {
        this.form = this.formBuilder.group({
            name: ['', Validators.required],
            fileUrl: ['', Validators.required]
          });
    }

    close() {
        this.windowRef.close();
    }

    onSubmit() {
        console.log(this.contactList);
        this.contactListService.Update({
          id: this.contactList.id,
          name: this.form.controls.name.value,
          fileUrl: ''
        }).subscribe(() => {
            this.toastrService.success("🚀 The Contact List has been updated!", "Success!");
            this.contactListService.refreshData();
            this.router.navigateByUrl('/',{skipLocationChange: true}).then(() => {
              this.router.navigate(['/pages/main/customer-list'])
            });
          }, error => {
            this.toastrService.danger(error, "There was an error on our side😢");
          })
      }
}
