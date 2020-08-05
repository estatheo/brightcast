import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { map, refCount, publishReplay } from 'rxjs/operators';
@Injectable()
export class ContactListService {

  apiURL: string = environment.apiUrl;
  private cache: Observable<Object>;



  constructor(private httpClient: HttpClient) {
  }

  get data() {
    if (!this.cache ) {
      this.cache = this.requestData().pipe(
        publishReplay(1),
        refCount(),
      );
    }
    return this.cache;
  }

  refreshData() {
    this.cache = null;
  }

  private requestData() {
    return this.httpClient.get(`${this.apiURL}/contactList/data`).pipe(map(response => response));
  }

  GetContactList(id) {
    return this.httpClient.get(`${this.apiURL}/contactList/${id}`).pipe(map(response => response));
  }

  GetContactListId(keyString) {
    return this.httpClient.get(`${this.apiURL}/contactList/key/${keyString}`).pipe(map(response => response));
  }
  NewContactList(contactList) {
    return this.httpClient.post(`${this.apiURL}/contactList/new`, contactList).pipe(map(response => response));
  }

  Delete(id) {
    return this.httpClient.delete(`${this.apiURL}/contactList/${id}`);
  }

  Update(contactList) {
    return this.httpClient.put(`${this.apiURL}/contactList/${contactList.id}`, contactList);
  }
}
