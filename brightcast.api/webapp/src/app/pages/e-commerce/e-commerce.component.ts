import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../_services';
import { DashboardService } from '../../@core/apis/dashboard.service';


@Component({
  selector: 'ngx-ecommerce',
  templateUrl: './e-commerce.component.html',
  styleUrls: ['e-commerce.component.scss']
})
export class ECommerceComponent implements OnInit, AfterViewInit {

  absoluteValues;
  percentageValues;
  selectedChart = "Delivered";
  dataReady = false;
  dashboardData;
  constructor(private router: Router, private accountService: AccountService, private dashboardService: DashboardService) {

  }

  ngOnInit(){

  }

  ngAfterViewInit() {
    const maxValue = Math.max.apply(null, this.absoluteValues);
    const self = this;
    let initial = 0;
    console.log('bbb',this.absoluteValues);
    this.accountService.onboardingCheck();
    this.dashboardService.data.subscribe(data => {
      console.log('aaaaaaa', data);
      this.absoluteValues = 
      [
        data['delivered']['value'],
        data['read']['value'],
        data['newSubscribers']['value'],
        data['unsubscribed']['value'],
        data['replies']['value'],
      ];
      this.percentageValues = 
      [
        data['delivered']['percentage'],
        data['read']['percentage'],
        data['newSubscribers']['percentage'],
        data['unsubscribed']['percentage'],
        data['replies']['percentage'],
      ];         
      this.dataReady = true;
      console.log('bbb',this.absoluteValues);
    });
    
  }

  getOption(){
    return this.selectedChart.replace(/\s/g, '').toLowerCase();
  }

}
