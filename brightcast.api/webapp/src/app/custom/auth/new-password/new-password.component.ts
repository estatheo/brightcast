import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from '../../../pages/_services';
import { NbToastrService } from '@nebular/theme';

@Component({
  selector: 'ngx-new-password',
  templateUrl: './new-password.component.html',
  styleUrls: ['./new-password.component.scss'],
})
export class NewPasswordComponent implements OnInit {

  form: FormGroup;
  loading = false;
  submitted = false;
  code;
  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private accountService: AccountService,
    private toastrService: NbToastrService,
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(p => {
      this.code = p['id'];
      this.form = this.formBuilder.group({
        password: ['', [Validators.required, Validators.minLength(6)]],
        password2: ['']}, {validator: this.checkPasswords });
    });

  }

  checkPasswords(group: FormGroup) { // here we have the 'passwords' group
    const pass = group.get('password').value;
    const confirmPass = group.get('password2').value;

    return pass === confirmPass ? null : { notSame: true };
  }

  get f() { return this.form.controls; }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.form.invalid) {
      return;
    }

    this.loading = true;
    this.accountService.resetPassword(this.f.password.value, this.code)
        .subscribe(
            data => {
                this.toastrService.success('✔ The Password Has been reseted!', 'Success!');
                this.router.navigate(['../confirm'], { relativeTo: this.route });
            },
            error => {
                this.toastrService.danger(`⚠ ${error}`, 'Error!');
                this.loading = false;
            });
  }

}
