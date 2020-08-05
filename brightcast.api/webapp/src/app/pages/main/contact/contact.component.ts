import { Component, OnInit, OnDestroy } from '@angular/core';
import { NbWindowService, NbToastrService } from '@nebular/theme';
import { Contact } from '../../_models/contact';
import { ContactList } from '../../_models/contactList';
import { ContactService } from '../../../@core/apis/contact.service';
import { ContactListService } from '../../../@core/apis/contactList.service';
import { ContactFormComponent } from './contact-form/contact-form.component';
import { ContactEditComponent } from './contact-edit/contact-edit.component';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'ngx-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.scss'],
})
export class ContactComponent implements OnInit, OnDestroy {

  constructor(
    private route: ActivatedRoute,
    private windowService: NbWindowService,
    private toastrService: NbToastrService,
    private contactService: ContactService,
    private contactListService: ContactListService) { }

  contactListId: number;
  contactList: ContactList;
  routeSub: Subscription;
  data: Contact[];
  shareLink: string;
  ngOnInit(): void {
    this.routeSub =  this.route.params.subscribe(p => {
      this.contactListId = p['id'];
      this.contactListService.GetContactList(this.contactListId).subscribe((data: ContactList) => {
        this.contactList = data;
        this.shareLink = location.protocol + '//' + location.host + '/signup_contact/' + this.contactList.keyString;
      });

      this.contactService.SetContactListId(this.contactListId);
      this.contactService.data.subscribe((data: Contact[]) => {
        this.contactService.refreshData();
        this.data = data;
      });
    });
  }

  ngOnDestroy() {
    this.routeSub.unsubscribe();
  }

  openModal() {
    this.windowService.open(
      ContactFormComponent, { title: 'New Contact', context: { contactListId: this.contactListId} });
  }

  openModalForEdit(event) {
    const item: Contact = event;
    this.windowService.open(ContactEditComponent, { title: 'Edit Contact', context: { contactElement: item } });
  }

  delete(id) {
    this.contactService.Delete(id).subscribe(() => {
      this.toastrService.primary('❌ The contact has been deleted!', 'Deleted!');
      this.contactService.refreshData();
      this.contactService.data.subscribe((data: Contact[]) => {
        this.data = data;
      });
    }, error => {
      this.toastrService.warning('⚠ There was an error processing the request!', 'Error!');
    });
  }

  copyLink() {
    this.toastrService.warning('The Link was copied to clipboard!', 'Note');
  }
}
