import { Component, OnInit } from '@angular/core';
import { NbWindowRef, NbToastrService } from '@nebular/theme';
import { ContactListElement } from '../../../_models/contactListElement';
import { ContactListService } from '../../../../@core/apis/contactList.service';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from '../../../_services';


@Component({
    template: `<form class="form" [formGroup]="form">
    <div class="form-group">
        <label for="name" class="label">Contact List Name</label>
        <input nbInput fullWidth id="name" type="text"  value="{{contactList.name}}" formControlName="name">
    </div>
    <label for="file" class="label">Contacts file</label>
    <input #image type="file" multiple accept="image/x-png,image/gif,image/jpeg" (change)="uploadImage(image.files)" nbInput fullWidth id="file">


    <button type="submit" style="margin-top: 10px" nbButton status="primary" (click)="onSubmit()">Save</button>
</form>`,
    styleUrls: ['customer-list-edit.component.scss'],
})
export class CustomerListEditComponent implements OnInit{
    image: FormData;
    contactList: ContactListElement;
    form;
    constructor(private router: Router, private formBuilder: FormBuilder, private accountService: AccountService, public windowRef: NbWindowRef, private contactListService: ContactListService, private toastrService: NbToastrService) {
        
    }

    ngOnInit(){
      this.form = this.formBuilder.group({
        name: [this.contactList.name, Validators.required],
      });
    }

    close() {
        this.windowRef.close();
    }

    uploadImage(files) {
      if (files.length === 0) {
        return;
      }  

      this.image = new FormData();

      for (let file of files) {
        this.image.append(file.name, file);
      }
      
    }

    onSubmit() {
      this.accountService.uploadImage(this.image).subscribe(im => {
        this.contactListService.Update({
          id: this.contactList.id,
          name: this.form.controls.name.value,
          fileUrl: im['name']
        }).subscribe(() => {
            this.toastrService.success("🚀 The Contact List has been updated!", "Success!");
            this.contactListService.refreshData();
            this.router.navigateByUrl('/',{skipLocationChange: true}).then(() => {
              this.router.navigate(['/pages/main/customer-list'])
            });
          }, error => {
            this.toastrService.danger(error, "There was an error on our side😢");
        });
      });
    }
}
