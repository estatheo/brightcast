import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'ngx-onboarding',
  templateUrl: './onboarding.component.html',
  styleUrls: ['./onboarding.component.scss']
})
export class OnboardingComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit(): void {
  }
  loadCampaigns() {
    this.router.navigate(['pages/dashboard']);
  }
}
