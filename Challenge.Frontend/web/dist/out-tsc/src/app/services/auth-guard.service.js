import { __decorate } from "tslib";
import { Injectable } from '@angular/core';
let AuthGuardService = class AuthGuardService {
    constructor(authservice, router) {
        this.authservice = authservice;
        this.router = router;
    }
    canActivate(next, state) {
        if (this.authservice.isAuthenticated) {
            return true;
        }
        else {
            this.router.navigate(['login']);
            return false;
        }
    }
};
AuthGuardService = __decorate([
    Injectable({
        providedIn: 'root'
    })
], AuthGuardService);
export { AuthGuardService };
//# sourceMappingURL=auth-guard.service.js.map