import { __decorate } from "tslib";
import { Component } from '@angular/core';
import { Validators } from '@angular/forms';
let LoginComponent = class LoginComponent {
    constructor(authService, formBuilder, router) {
        this.authService = authService;
        this.formBuilder = formBuilder;
        this.router = router;
        this.errors = false;
    }
    ngOnInit() {
        this.form = this.formBuilder.group({
            username: [null, [Validators.required, Validators.email]],
            password: [null, Validators.required]
        });
    }
    login() {
        this.authService.login(this.form.get('username').value, this.form.get('password').value)
            .then(result => {
            var token = result.data.token;
            this.authService.setToken(token);
            this.router.navigate(['/home']);
        })
            .catch(error => {
            this.errors = true;
            setTimeout(() => {
                this.errors = false;
            }, 2000);
            console.log(error);
        });
    }
};
LoginComponent = __decorate([
    Component({
        selector: 'app-login',
        templateUrl: './login.component.html',
        styleUrls: ['./login.component.css']
    })
], LoginComponent);
export { LoginComponent };
//# sourceMappingURL=login.component.js.map