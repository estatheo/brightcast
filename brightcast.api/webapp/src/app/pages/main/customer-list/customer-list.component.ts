import { Component, OnInit } from '@angular/core';
import { NbWindowService } from '@nebular/theme';
import { CustomerService } from './customer-form/customer.service';
import { of } from 'rxjs';
import { CustomerFormComponent } from './customer-form/customer-form.component';

@Component({
  selector: 'ngx-customer-list',
  templateUrl: './customer-list.component.html',
  styleUrls: ['./customer-list.component.scss']
})
export class CustomerListComponent implements OnInit {
  contacts = [
    {},
    {},
  ]
  data = [
    {
      name: "primary",
      contacts: 10,
      campaigns: 2,
      unsubscribed: 3,
    },
    {
      name: "secondary",
      contacts: 50,
      campaigns: 3,
      unsubscribed: 10,
    },
  ]

  constructor(private windowSerivce: NbWindowService, private customerService: CustomerService) { }

  ngOnInit(): void {

  }
  openModal() {
    console.log("open modal clicked");

  }
  openModalForEdit(event) {
    console.log("The item for edit: ", event);
    let observable = of(event);
    this.customerService.setState(observable);
    this.windowSerivce.open(CustomerFormComponent, { title: 'Customer List' });
  }
}
