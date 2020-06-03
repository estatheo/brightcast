import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
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
  constructor(private router: Router) {

  }


  ngOnInit() {
    console.log("Ecommerce Component Loaded");
    const maxValue = Math.max.apply(null, this.absoluteValues);
    console.log("max value", maxValue);
    const self = this;
    let initial = 0;
    // setTimeout(function () {
    //   self.router.navigate(['pages/main/onboarding']);
    // }, 5000)

  }
}
