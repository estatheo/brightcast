import { Component } from '@angular/core';
import { NbWindowRef, NbToastrService } from '@nebular/theme';
import { Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import { ContactListService } from '../../../../@core/apis/contactList.service';
import { AccountService } from '../../../_services';


@Component({
    template: `<form class="form" [formGroup]="form">
    <div class="form-group">
        <label for="name" class="label" >Contact List Name</label>
        <input nbInput fullWidth id="name" type="text" value="" formControlName="name">
    </div>
    <label for="file" class="label">Contacts file</label>
    <input #image type="file" multiple accept=".csv" (change)="uploadImage(image.files)" nbInput fullWidth id="file">


    <button type="submit" style="margin-top: 10px" nbButton status="primary" (click)="onSubmit()">Save</button>
</form>`,
    styleUrls: ['customer-form.component.scss'],
})
export class CustomerFormComponent {

    doc: FormData;
    event;
    form;
    constructor(private router: Router, private formBuilder: FormBuilder, private accountService: AccountService, public windowRef: NbWindowRef, private toastrService: NbToastrService, private contactListService: ContactListService) {
        this.form = this.formBuilder.group({
            name: ['', Validators.required],
            message: ['', Validators.required],
            contactListId: ['', Validators.required],
          });
    }

    uploadImage(files) {
      if (files.length === 0) {
        return;
      }  

      this.doc = new FormData();

      for (let file of files) {
        this.doc.append(file.name, file);
      }
      
    }

    onSubmit() {
      this.accountService.uploadDoc(this.doc).subscribe(im => {
        this.contactListService.NewContactList({
          name: this.form.controls.name.value,
          fileUrl: im['name']
        }).subscribe(() => {
          this.toastrService.success("🚀 The Contact List has been added!", "Success!");
          this.contactListService.refreshData();
          this.router.navigateByUrl('/',{skipLocationChange: true}).then(() => {
            this.router.navigate(['/pages/main/customer-list']);
            this.close();
          });
        }, error => {
          this.toastrService.danger(error, "There was an error on our side😢");
        });
      });

    }
    
    close() {
        this.windowRef.close();
    }
}
