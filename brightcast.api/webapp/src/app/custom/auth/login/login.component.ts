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
    var username: string = '';
    var password: string = '';
    if (localStorage.getItem('username') && localStorage.getItem('rememberme') === 'true') {
      username = localStorage.getItem('username');
    //  password = CryptoJS.AES.decrypt(localStorage.getItem('password'), 'brightcast').toString();
    }
    this.form = this.formBuilder.group({
      username: [username, Validators.required],
      password: [password, Validators.required],
      isRememberme: [(localStorage.getItem('rememberme') === 'true' ? 'true' : '')]
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

    // set rememberme in localStorage
    localStorage.setItem('rememberme', this.f.isRememberme.value);

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
