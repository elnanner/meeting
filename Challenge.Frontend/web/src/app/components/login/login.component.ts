import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { error } from 'protractor';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  form: FormGroup;
  errors: boolean = false;

  constructor(
    public authService: AuthService,
    private formBuilder: FormBuilder,
    private router: Router
  ) { }

  ngOnInit(): void {
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
         },2000)
         console.log(error);
       })
  }


}
