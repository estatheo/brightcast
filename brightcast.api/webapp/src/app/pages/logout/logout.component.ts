import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services';

@Component({
  selector: 'ngx-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.scss'],
})
export class LogoutComponent implements OnInit {

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
    this.accountService.logout();
  }

}
