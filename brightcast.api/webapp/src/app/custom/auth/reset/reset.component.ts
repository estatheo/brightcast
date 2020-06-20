import { Component, OnInit } from '@angular/core';
import { AccountService, AlertService } from '../../../pages/_services';
import { Router, ActivatedRoute } from '@angular/router';
import { NbToastrService } from '@nebular/theme';

@Component({
  selector: 'ngx-reset',
  templateUrl: './reset.component.html',
  styleUrls: ['./reset.component.scss']
})
export class ResetComponent implements OnInit {

  authError: boolean = false;
  alertText = '';
  loading = false;
  submitted = false;
  
  constructor(private accountService: AccountService,
    private alertService: AlertService,
    private router: Router,    
    private route: ActivatedRoute,
    private toastrService: NbToastrService) { }

  ngOnInit(): void {
  }

  onClose() {
    this.authError = false;
    this.alertText = '';
  }

  reset(email) {
    this.accountService.requestPasswordReset(email).subscribe(data => {                    
      this.alertService.success('Registration successful', { keepAfterRouteChange: true });
      this.router.navigate(['../../login'], { relativeTo: this.route });
  },
  error => {
      this.toastrService.danger("❌ There was an error with the request!", "Error!");
      this.alertText = error;
        this.authError = true;
        setTimeout(() =>  this.onClose(), 2000);
      this.alertService.error(error);
      this.loading = false;
  });
  }
}
