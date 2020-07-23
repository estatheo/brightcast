import { Component, OnInit } from '@angular/core';
import { NbToastrService } from '@nebular/theme';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService, AlertService } from '../../../pages/_services';
import { first } from 'rxjs/operators';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { NbToastrService } from '@nebular/theme';

@Component({
  selector: 'ngx-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {

  form: FormGroup;
  loading = false;
  submitted = false;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private accountService: AccountService,
    private alertService: AlertService,
    private toastrService: NbToastrService,
  ) { }

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
      cb: [false, Validators.pattern('true')],
      password2: ['']}, {validator: this.checkPasswords });
  }

  checkPasswords(group: FormGroup) { // here we have the 'passwords' group
    const pass = group.get('password').value;
    const confirmPass = group.get('password2').value;

    return pass === confirmPass ? null : { notSame: true };
  }

  // convenience getter for easy access to form fields
  get f() { return this.form.controls; }

  onSubmit() {
    this.submitted = true;

    // reset alerts on submit
    this.alertService.clear();

    // stop here if form is invalid
    if (this.form.invalid) {
      this.toastrService.danger('Incorrect email or password.', 'Error');
      return;
    }

    this.loading = true;
    this.accountService.register(this.form.value)
        .subscribe(
            data => {
              //  this.alertService.success('Registration successful', { keepAfterRouteChange: true });
                this.toastrService.success('Registered successfully. We have sent you an email. Please verify it\'s you.', 'Success', {duration: 7000});
                this.router.navigate(['auth/register/confirm'], { relativeTo: this.route });
                this.loading = false;
            },
            error => {
                this.toastrService.danger(error, 'Error');
                this.loading = false;
            });
  }

}
