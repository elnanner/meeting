import { Component, OnInit, TemplateRef  } from '@angular/core';
import { MeetingService } from '../../services/meeting.service';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { Meeting } from '../../models/meeting.model';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-meeting',
  templateUrl: './meeting.component.html',
  styleUrls: ['./meeting.component.css']
})
export class MeetingComponent implements OnInit {
  modalRef: BsModalRef;

  meetings: Meeting[];
  selectedMeeting: Meeting;

  constructor(
    private router: Router,
    private meetingService: MeetingService,
    public authService: AuthService,
    private modalService: BsModalService
  ) { }

  ngOnInit(): void {
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
      })
  }

  details(id: number) {
    this.selectedMeeting = this.meetings[id];
    console.log(this.meetings[id]);
  }

  create() {
    console.log(`Creanr una meeting`);
  }

  edit(id: number) {
    console.log(`Editar ${id}`);
  }

  delete(id: number) {
    console.log('borrar');
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  close() {
    this.modalRef.hide();
  }

  save() {
    console.log(`Saving....`);
    this.close();
  }
}
