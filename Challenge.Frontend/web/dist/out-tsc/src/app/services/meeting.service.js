import { __decorate } from "tslib";
import { Injectable } from '@angular/core';
import { HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
let MeetingService = class MeetingService {
    constructor(http, authService) {
        this.http = http;
        this.authService = authService;
        this.baseUri = environment.restServiceUrl;
        this.options = { headers: new HttpHeaders().set('Content-Type', 'application/json') };
    }
    getAllMeetings() {
        return this.http.get(this.baseUri + 'api/meeting', this.options).toPromise();
    }
    create() {
        let body = JSON.stringify({});
        let headers = new HttpHeaders({
            'Content-type': 'aplication/json'
        });
    }
};
MeetingService = __decorate([
    Injectable({
        providedIn: 'root'
    })
], MeetingService);
export { MeetingService };
//# sourceMappingURL=meeting.service.js.map