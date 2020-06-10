import { Component, OnInit, Input } from '@angular/core';
import { NbWindowService, NbToastrService } from '@nebular/theme';
import { Contact } from '../../_models/contact';
import { ContactService } from '../../../@core/apis/contact.service';
import { ContactFormComponent } from './contact-form/contact-form.component';
import { ContactEditComponent } from './contact-edit/contact-edit.component';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'ngx-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.scss']
})
export class ContactComponent implements OnInit {

  constructor(private route: ActivatedRoute, private windowService: NbWindowService, private toastrService: NbToastrService, private contactService: ContactService) { }

  contactListId: number;    
  routeSub: Subscription;
  data: Contact[];

  ngOnInit(): void {    
    this.routeSub =  this.route.params.subscribe(p => {
      this.contactListId = p['id'];      
      this.contactService.SetContactListId(this.contactListId);
      this.contactService.data.subscribe((data: Contact[]) => {
        this.data = data;
      });
    });
  }

  ngOnDestroy() {
    this.routeSub.unsubscribe();
  }

  openModal() {
    this.windowService.open(ContactFormComponent, { title: 'New Contact', context: { contactListId: this.contactListId} });
  }

  openModalForEdit(event) {
    let item: Contact = event;
    this.windowService.open(ContactEditComponent, { title: 'Edit Contact', context: { contactElement: item } });
  }

  delete(id) {
    this.contactService.Delete(id).subscribe(() => {
      this.toastrService.primary("❌ The campaign has been deleted!", "Deleted!");
      this.contactService.refreshData();
      this.contactService.data.subscribe((data: Contact[]) => {
        this.data = data;
      });
    }, error => {
      this.toastrService.warning("⚠ There was an error processing the request!", "Error!");
    })
  }

}
