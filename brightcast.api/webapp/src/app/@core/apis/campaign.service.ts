import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { map, refCount, publishReplay } from 'rxjs/operators';
@Injectable()
export class CampaignService {

  apiURL: string = environment.apiUrl;
  private cache$: Observable<Object>;


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

  requestData() {
    return this.httpClient.get(`${this.apiURL}/campaign/data`).pipe(map(response => response));
  }

  NewCampaign(campaign) {
    return this.httpClient.post(`${this.apiURL}/campaign/new`, campaign).pipe(map(response => response));
  }

  Delete(id) {
    return this.httpClient.delete(`${this.apiURL}/campaign/${id}`);
  }

  Update(campaign) {
    return this.httpClient.put(`${this.apiURL}/campaign/${campaign.id}`, campaign);
  }

  SendCampaign(campaign) {
    return this.httpClient.post(`${this.apiURL}/campaign/send`, campaign).pipe(map(response => response));
  }
}
