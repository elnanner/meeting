import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import jwt_decode from "jwt-decode";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUri = environment.restServiceUrl;

  constructor(private http: HttpClient) { }


  login(username: string, password: string) {
    let body = JSON.stringify({'username': username, 'password': password});
    let headers = new HttpHeaders({
      'Content-type': 'application/json'
    });

    return this.http.post<any>(this.baseUri + 'api/authentication', body, { headers }).toPromise();
  }

  logout() {
    localStorage.removeItem('token');
  }

  public get isAuthenticated(): boolean {
    return (localStorage.getItem('token') != null);
  }

  getToken():string {
    return localStorage.getItem('token');
  }

  setToken(token: string): void {
    localStorage.setItem('token', token);
  }

  public get isAdmin() {
    debugger;
    let token = jwt_decode(this.getToken());

    let isAdmin = token["Role"];
    console.log(`Is Admin: ${isAdmin}`);
    return isAdmin == 'Admin';

  }

}
