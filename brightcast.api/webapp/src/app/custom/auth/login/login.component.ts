import { Component, OnInit } from '@angular/core';
import { NbLoginComponent } from '@nebular/auth';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from '../../../pages/_services';
import { NbToastrService } from '@nebular/theme';
import { first } from 'rxjs/operators';

@Component({
  selector: 'ngx-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent extends NbLoginComponent implements OnInit {
  form: any;
  loading = false;
  submitted = false;
  returnUrl: string;
  constructor(private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    public router: Router,
    private accountService: AccountService,
    private toastrService: NbToastrService) {
    super(null, null, null, router);
  }

  ngOnInit(): void {
    let username: string = '';
    // let password: string = '';
    if (localStorage.getItem('username') && localStorage.getItem('rememberme') === 'true') {
      username = localStorage.getItem('username');
    //  password = CryptoJS.AES.decrypt(localStorage.getItem('password'), 'brightcast').toString();
    }
    this.form = this.formBuilder.group({
      username: [username, Validators.required],
      password: ['', Validators.required],
      isRememberme: [(localStorage.getItem('rememberme') === 'true' ? 'true' : '')],
    });

    // get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';

  }

  // convenience getter for easy access to form fields
  get f() { return this.form.controls; }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.form.invalid) {
        return;
    }

    // set rememberme in localStorage
    localStorage.setItem('rememberme', this.f.isRememberme.value);

    this.loading = true;
    this.accountService.login(this.f.username.value, this.f.password.value)
        .pipe(first())
        .subscribe(
            data => {
              this.router.navigate([this.returnUrl]);
            },
            error => {
                this.toastrService.danger(`⚠ ${error}`, 'Error!');
                this.loading = false;
            });
    }
}
