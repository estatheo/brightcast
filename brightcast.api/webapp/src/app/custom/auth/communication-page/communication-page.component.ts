import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'ngx-communication-page',
  templateUrl: './communication-page.component.html',
  styleUrls: ['./communication-page.component.scss'],
})
export class CommunicationPageComponent implements OnInit, OnDestroy {

  routeValue: string;
  routeSub: Subscription;

  constructor(private route: ActivatedRoute) { }


  ngOnInit(): void {
    this.routeSub =  this.route.params.subscribe(p => {
      this.routeValue = p['route'];
    });
  }

  ngOnDestroy() {
    this.routeSub.unsubscribe();
  }

}
