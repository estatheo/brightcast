import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { map, refCount, publishReplay } from 'rxjs/operators';
@Injectable()
export class ContactService {

  apiURL: string = environment.apiUrl;
  private cache$: Observable<Object>;
  contactListId: number;

  constructor(private httpClient: HttpClient) {
  }

  get data() {
    if ( !this.cache$ ) {
      this.cache$ = this.requestData().pipe(
        publishReplay(1),
        refCount(),
      );
    }
    return this.cache$;
  }

  refreshData() {
    this.cache$ = null;
  }

  private requestData() {
    return this.httpClient.get(`${this.apiURL}/contact/ofList/${this.contactListId}/`).pipe(map(response => response));
  }

  NewContact(contact) {
    return this.httpClient.post(`${this.apiURL}/contact/new`, contact).pipe(map(response => response));
  }

  Delete(id) {
    return this.httpClient.delete(`${this.apiURL}/contact/${id}`);
  }

  Update(contact) {
    return this.httpClient.put(`${this.apiURL}/contact/${contact.id}`, contact);
  }

  SetContactListId(id) {
    this.contactListId = id;
  }
}
