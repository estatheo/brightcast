import { Component } from '@angular/core';
import { NbWindowRef } from '@nebular/theme';
import { CustomerService } from './customer.service';


@Component({
    template: `<form class="form">
    <div class="form-group">
        <label for="name" class="label">Contact List Name</label>
        <input nbInput fullWidth id="name" type="text" value="">
    </div>
    <label for="file" class="label">Contacts file</label>
    <input type="file" nbInput fullWidth id="file">


    <button type="submit" style="margin-top: 10px" nbButton status="primary">Save</button>
</form>`,
    styleUrls: ['customer-form.component.scss'],
})
export class CustomerFormComponent {

    event;
    constructor(public windowRef: NbWindowRef, private customerService: CustomerService) {

    }

    close() {
        this.windowRef.close();
    }
}
