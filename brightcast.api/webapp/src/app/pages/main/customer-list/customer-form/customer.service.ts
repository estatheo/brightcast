import { Injectable } from '@angular/core';
import { of as observableOf, Observable } from 'rxjs';

@Injectable()
export class CustomerService {
    constructor() { }

    observable: Observable<any>

    setState(observable: Observable<any>) {
        this.observable = observable;
    }

    getState() {
        return this.observable;
    }
} 