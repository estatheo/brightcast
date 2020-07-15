import { Component } from '@angular/core';

@Component({
  selector: 'ngx-footer',
  styleUrls: ['./footer.component.scss'],
  template: `
    <span class="col-md-4 col-sm-12 created-by">
      © 2020 brightcast.io - All Rights Reserved.
    </span>
    <ul class="col-md-4 col-sm-6 terms-conditions">
      <li>
        <a href="#" target="_blank" >Terms of Use</a>
      </li>
      <li>
        <a href="#" target="_blank" >Conditions</a>
      </li>
    </ul>
    <div class="col-md-4 col-sm-6 socials">
      <a href="https://linkedin.com/" target="_blank" class="ion ion-social-linkedin"></a>
    </div>
  `,
})
export class FooterComponent {
}
