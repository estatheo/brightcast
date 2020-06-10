import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService, AlertService } from '../../../pages/_services';
import { first } from 'rxjs/operators';

@Component({
  selector: 'ngx-new-password',
  templateUrl: './new-password.component.html',
  styleUrls: ['./new-password.component.scss']
})
export class NewPasswordComponent implements OnInit {

  form: FormGroup;
  loading = false;
  submitted = false;
  authError: boolean = false;
  alertText = '';
  code;
  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private accountService: AccountService,
    private alertService: AlertService
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(p => {
      this.code = p['id'];
      this.form = this.formBuilder.group({
        password: ['', [Validators.required, Validators.minLength(6)]],
        password2: ['']},{validator: this.checkPasswords });
    })
    
  }

  onClose() {
    this.authError = false;
    this.alertText = '';
  }
  
  checkPasswords(group: FormGroup) { // here we have the 'passwords' group
    let pass = group.get('password').value;
    let confirmPass = group.get('password2').value;

    return pass === confirmPass ? null : { notSame: true }     
  }

  get f() { return this.form.controls; }

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
        this.accountService.resetPassword(this.f.password.value, this.code)
            .pipe(first())
            .subscribe(
                data => {                    
                    this.alertService.success('Password Reset successful', { keepAfterRouteChange: true });
                    this.router.navigate(['../login'], { relativeTo: this.route });
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
