import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { map, refCount, publishReplay } from 'rxjs/operators';
@Injectable()
export class DashboardService {

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

  private requestData() {
    return this.httpClient.get(`${this.apiURL}/dashboard/data`).pipe(map(response => response));
  }
}
