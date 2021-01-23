import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { AuthService } from './auth.service';
import { ApiResponse } from '../models/api-response.model';
import { Meeting } from '../models/meeting.model';

@Injectable({
  providedIn: 'root'
})
export class MeetingService {

  private baseUri = environment.restServiceUrl;
  private options = { headers: new HttpHeaders().set('Content-Type', 'application/json') };

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) { }


  getAllMeetings() {
    return this.http.get<ApiResponse<Meeting[]>>(this.baseUri + 'api/meeting', this.options).toPromise();
  }
  create() {
    let body = JSON.stringify({});
    let headers = new HttpHeaders({
      'Content-type': 'aplication/json'
    });
  }
}
