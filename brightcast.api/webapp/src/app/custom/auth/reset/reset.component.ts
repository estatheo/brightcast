import { Component, OnInit } from '@angular/core';
import { AccountService, AlertService } from '../../../pages/_services';
import { Router, ActivatedRoute } from '@angular/router';
import { NbToastrService } from '@nebular/theme';

@Component({
  selector: 'ngx-reset',
  templateUrl: './reset.component.html',
  styleUrls: ['./reset.component.scss'],
})
export class ResetComponent implements OnInit {

  loading = false;
  submitted = false;

  constructor(private accountService: AccountService,
    private alertService: AlertService,
    private router: Router,
    private route: ActivatedRoute,
    private toastrService: NbToastrService) { }

  ngOnInit(): void {
  }

  reset(email) {
    this.loading = true;

    this.accountService.requestPasswordReset(email).subscribe(data => {
      this.toastrService.success('We have sent you an email. Please reset password.', 'Success', {duration: 7000});
      this.router.navigate(['../../login'], { relativeTo: this.route });
      this.loading = false;
    },
    error => {
        this.toastrService.danger('❌ ' + error, 'Error');
        this.loading = false;
    });
  }
}
