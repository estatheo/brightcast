import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from '../../../environments/environment';
import { User } from '../_models';
import { Business } from '../_models/business';
import { Role } from '../_models/role';
import { UserProfile } from '../_models/userProfile';
import { Campaign } from '../_models/campaign';
import { ContactList } from '../_models/contactList';

@Injectable({ providedIn: 'root' })
export class AccountService {
    private userSubject: BehaviorSubject<User>;
    public user: Observable<User>;
    constructor(
        private router: Router,
        private http: HttpClient,
    ) {
        this.userSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('user')));
        this.user = this.userSubject.asObservable();
    }

    public get userValue(): User {
        return this.userSubject.value;
    }

    login(username, password) {
        return this.http.post<User>(`${environment.apiUrl}/user/authenticate`, { username, password })
            .pipe(map(user => {
                // store user details and jwt token in local storage to keep user logged in between page refreshes
                localStorage.setItem('user', JSON.stringify(user));
                if (localStorage.getItem('rememberme') === 'true') {
                    localStorage.setItem('username', username);
                //    localStorage.setItem('password', CryptoJS.AES.encrypt(user.password, 'brightcast').toString());
                }
                this.userSubject.next(user);
                this.onboardingCheck();
                return user;
            }));
    }

    logout() {
        // remove user from local storage and set current user to null
        localStorage.removeItem('user');
        this.userSubject.next(null);
        this.router.navigate(['/auth/login']);
    }

    getUserProfile() {
        return this.http.get(`${environment.apiUrl}/userprofile`);
    }

    register(user: User) {
        return this.http.post(`${environment.apiUrl}/user/register`, user);
    }

    getAll() {
        return this.http.get<User[]>(`${environment.apiUrl}/user`);
    }

    getById(id: string) {
        return this.http.get<User>(`${environment.apiUrl}/user`);
    }

    getSettingsData() {
        return this.http.get(`${environment.apiUrl}/userprofile/settings/data`);
    }

    update(id, params) {
        return this.http.put(`${environment.apiUrl}/userprofile/updateProfile`, params)
            .pipe(map(x => {
                // update stored user if the logged in user updated their own record
                if (id === this.userValue.id) {
                    // update local storage
                    const user = { ...this.userValue, ...params };
                    localStorage.setItem('user', JSON.stringify(user));

                    // publish updated user to subscribers
                    this.userSubject.next(user);
                }
                return x;
            }));
    }

    updateBusiness(params) {
        return this.http.put(`${environment.apiUrl}/userprofile/updateBusiness`, params);
    }

    delete(id: string) {
        return this.http.delete(`${environment.apiUrl}/user`)
            .pipe(map(x => {
                // auto logout if the logged in user deleted their own record
                if (id === this.userValue.id) {
                    this.logout();
                }
                return x;
            }));
    }

    onboardingCheck() {
        return this.http.get(`${environment.apiUrl}/userprofile/onboardingCheck`).subscribe(data => {
            if (data['onboard']) {
                this.router.navigate(['/onboarding']);
            }
        });
    }

    onboarding(
        business: Business, role: Role, userProfile: UserProfile, campaign: Campaign, contactList: ContactList ) {
        return this.http.post(`${environment.apiUrl}/userProfile/onboarding`, {
            business,
            role,
            userProfile,
            contactList,
            campaign,
        });
    }

    verify(id) {
        return this.http.get(`${environment.apiUrl}/user/verify/${id}`);
    }

    requestPasswordReset(email) {
        return this.http.post(`${environment.apiUrl}/user/resetPassword`,  {username: email});
    }

    resetPassword(password, code) {
        return this.http.post(`${environment.apiUrl}/user/resetPassword/confirm`,  {code: code, password: password});
    }

    uploadImage(form: FormData) {
        return this.http.post(`${environment.apiUrl}/file/uploadImage`, form);
    }

    uploadDoc(form: FormData) {
        return this.http.post(`${environment.apiUrl}/file/uploadDoc`, form);
    }

}
