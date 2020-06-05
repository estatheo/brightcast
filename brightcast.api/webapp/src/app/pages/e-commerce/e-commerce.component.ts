import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../_services';
@Component({
  selector: 'ngx-ecommerce',
  templateUrl: './e-commerce.component.html',
  styleUrls: ['e-commerce.component.scss']
})
export class ECommerceComponent implements OnInit {

  intermediate1;

  absoluteValues = [10, 25, 15, 7, 9];
  absoluteValues1 = [0, 0, 0, 0, 0];
  percentageValues = [+2.5, -1.5, +2.5, -1.5, -1.5];

  selectedChart = "Delivered";
  constructor(private router: Router, private accountService: AccountService) {

  }


  ngOnInit() {
    const maxValue = Math.max.apply(null, this.absoluteValues);
    const self = this;
    let initial = 0;
    this.accountService.onboardingCheck();

  }
}
