import { Component, OnInit } from '@angular/core';
import { NbWindowService, NbToastrService } from '@nebular/theme';
import { of } from 'rxjs';
import { CustomerFormComponent } from './customer-form/customer-form.component';
import { ContactListService } from '../../../@core/apis/contactList.service';
import { ContactListElement } from '../../_models/contactListElement';
import { CustomerListEditComponent } from './customer-list-edit/customer-list-edit.component';

@Component({
  selector: 'ngx-customer-list',
  templateUrl: './customer-list.component.html',
  styleUrls: ['./customer-list.component.scss']
})
export class CustomerListComponent implements OnInit {

  data: ContactListElement[];

  constructor(private windowService: NbWindowService, private toastrService: NbToastrService, private contactListService: ContactListService) { }

  ngOnInit(): void {
    this.contactListService.data.subscribe((data: ContactListElement[]) => {
      this.data = data;
    }) 
  }
  openModal() {
    this.windowService.open(CustomerFormComponent, { title: 'New Contact List'});

  }
  openModalForEdit(event) {    
    let item: ContactListElement = event;
    this.windowService.open(CustomerListEditComponent, { title: 'Edit Contact List', context: { contactList: item } });
  }

  delete(id) {
    this.contactListService.Delete(id).subscribe(() => {
      this.toastrService.primary("❌ The Contact List has been deleted!", "Deleted!");
      this.contactListService.refreshData();
      this.contactListService.data.subscribe((data: ContactListElement[]) => {
        this.data = data;
      });
    }, error => {
      this.toastrService.warning("⚠ There was an error processing the request!", "Error!");
    })
  }
}
