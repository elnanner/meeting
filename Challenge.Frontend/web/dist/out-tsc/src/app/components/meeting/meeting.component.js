import { __decorate } from "tslib";
import { Component } from '@angular/core';
let MeetingComponent = class MeetingComponent {
    constructor(router, meetingService, authService, modalService) {
        this.router = router;
        this.meetingService = meetingService;
        this.authService = authService;
        this.modalService = modalService;
    }
    ngOnInit() {
        this.getMeeting();
    }
    getMeeting() {
        this.meetingService.getAllMeetings()
            .then(result => {
            console.log(result.data);
            console.log(result.metadata);
            this.meetings = result.data;
        })
            .catch(error => {
            console.log(error);
        });
    }
    details(id) {
        this.selectedMeeting = this.meetings[id];
        console.log(this.meetings[id]);
    }
    create() {
        console.log(`Creanr una meeting`);
    }
    edit(id) {
        console.log(`Editar ${id}`);
    }
    delete(id) {
        console.log('borrar');
    }
    openModal(template) {
        this.modalRef = this.modalService.show(template);
    }
    close() {
        this.modalRef.hide();
    }
    save() {
        console.log(`Saving....`);
        this.close();
    }
};
MeetingComponent = __decorate([
    Component({
        selector: 'app-meeting',
        templateUrl: './meeting.component.html',
        styleUrls: ['./meeting.component.css']
    })
], MeetingComponent);
export { MeetingComponent };
//# sourceMappingURL=meeting.component.js.map