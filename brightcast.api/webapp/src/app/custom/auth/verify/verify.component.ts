import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from '../../../pages/_services';
import { NbToastrService } from '@nebular/theme';

@Component({
  selector: 'ngx-verify',
  templateUrl: './verify.component.html',
  styleUrls: ['./verify.component.scss'],
})
export class VerifyComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private accountService: AccountService,
    private toastrService: NbToastrService) { }

  ngOnInit(): void {
    this.route.params.subscribe(p => {
      const id = p['id'];
      this.accountService.verify(id).subscribe(x => {
        this.router.navigateByUrl('/auth/login');
      }, error => {
        this.toastrService.danger(`⚠ ${error}`, 'Error!');
      });
    });
  }

}
