import { Injectable } from '@angular/core';
import { PeriodsService } from '../mock/periods.service';
import { OrdersChart, OrdersChartData } from '../data/orders-chart';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { OrderProfitChartSummary } from '../data/orders-profit-chart';
import { of ,  Observable } from 'rxjs';
import { shareReplay, map, refCount, publishReplay } from 'rxjs/operators';
@Injectable()
export class DashboardService {

  apiURL: string = environment.apiUrl;
  private cache$: Observable<Object>;
  private year = [
    '2012',
    '2013',
    '2014',
    '2015',
    '2016',
    '2017',
    '2018',
    '2019',
    '2020',
  ];

  constructor(private httpClient: HttpClient) {   
  }

  get data() {
    if( !this.cache$ ) {
      this.cache$ = this.requestData().pipe(
        publishReplay(1),
        refCount()
      );
    }
    return this.cache$;
  }

  private requestData() {
    return this.httpClient.get(`${this.apiURL}/dashboard/data`).pipe(map(response => response));
  }
}
  