import { Component, OnInit } from '@angular/core';
import { NbLoginComponent, NbAuthService } from '@nebular/auth';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService, AlertService } from '../../../pages/_services';
import { first } from 'rxjs/operators';

@Component({
  selector: 'ngx-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent extends NbLoginComponent {
  form: any;
  loading = false;
  submitted = false;
  returnUrl: string;
  authError: boolean = false;
  alertText = '';
  constructor(private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    public router: Router,
    private accountService: AccountService,
    private alertService: AlertService) {
    super(null, null, null, router);
  }

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });

    // get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    
  }

  // convenience getter for easy access to form fields
  get f() { return this.form.controls; }

  onClose() {
    this.authError = false;
    this.alertText = '';
  }

  onSubmit() {
    this.submitted = true;

    // reset alerts on submit
    this.alertService.clear();

    // stop here if form is invalid
    if (this.form.invalid) {
        return;
    }
    this.authError = false; 
    this.alertText = '';

    this.loading = true;
    this.accountService.login(this.f.username.value, this.f.password.value)
        .pipe(first())
        .subscribe(
            data => {
              this.router.navigate([this.returnUrl]);
            },
            error => {
                this.alertText = error;
                this.authError = true;
                setTimeout(() =>  this.onClose(), 2000);
                this.alertService.error(error);
                this.loading = false;
            });
}


}
