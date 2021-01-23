import { __decorate } from "tslib";
import { Injectable } from '@angular/core';
import { HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import jwt_decode from "jwt-decode";
let AuthService = class AuthService {
    constructor(http) {
        this.http = http;
        this.baseUri = environment.restServiceUrl;
    }
    login(username, password) {
        let body = JSON.stringify({ 'username': username, 'password': password });
        let headers = new HttpHeaders({
            'Content-type': 'application/json'
        });
        return this.http.post(this.baseUri + 'api/authentication', body, { headers }).toPromise();
    }
    logout() {
        localStorage.removeItem('token');
    }
    get isAuthenticated() {
        return (localStorage.getItem('token') != null);
    }
    getToken() {
        return localStorage.getItem('token');
    }
    setToken(token) {
        localStorage.setItem('token', token);
    }
    get isAdmin() {
        debugger;
        let token = jwt_decode(this.getToken());
        let isAdmin = token["Role"];
        console.log(`Is Admin: ${isAdmin}`);
        return isAdmin == 'Admin';
    }
};
AuthService = __decorate([
    Injectable({
        providedIn: 'root'
    })
], AuthService);
export { AuthService };
//# sourceMappingURL=auth.service.js.map