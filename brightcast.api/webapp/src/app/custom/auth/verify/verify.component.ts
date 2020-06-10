import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from '../../../pages/_services';

@Component({
  selector: 'ngx-verify',
  templateUrl: './verify.component.html',
  styleUrls: ['./verify.component.scss']
})
export class VerifyComponent implements OnInit {

  constructor(private route: ActivatedRoute, private router: Router, private accountService: AccountService) { }

  ngOnInit(): void {
    this.route.params.subscribe(p => {
      let id = p['id'];
      console.log(id);
      this.accountService.verify(id).subscribe(x => {
        this.router.navigateByUrl('/auth/login');
      });
    })
  }

}
