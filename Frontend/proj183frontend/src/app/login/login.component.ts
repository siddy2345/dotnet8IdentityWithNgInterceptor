import { Component, OnInit, inject, signal } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import {
  AUTH_TOKEN_LOCAL_STORAGE,
  REFRESH_TOKEN_LOCAL_STORAGE,
  User,
} from '../models/shared.models';
import { AuthService } from '../services/auth.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatRadioChange, MatRadioModule } from '@angular/material/radio';
import { AccountProcessOption } from './login.models';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { EventService } from '../services/event.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatRadioModule,
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent implements OnInit {
  public userReactiveForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [
      Validators.required,
      Validators.pattern(
        /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d])(.{6,})$/
      ),
    ]),
  });

  public accountProcessOptions = AccountProcessOption;
  public accountProcessOptionSig = signal(AccountProcessOption.Register);

  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly eventService = inject(EventService);

  public ngOnInit(): void {
    const authToken = localStorage.getItem(AUTH_TOKEN_LOCAL_STORAGE);
    const refreshToken = localStorage.getItem(REFRESH_TOKEN_LOCAL_STORAGE);
    if (
      (authToken !== null && refreshToken !== null) ||
      refreshToken !== null
    ) {
      console.log(this.eventService.desiredRouteSig());

      this.router.navigate([`${this.eventService.desiredRouteSig()}`]);
    }
  }

  public onRegister(): void {
    const email = this.userReactiveForm.controls.email;
    const password = this.userReactiveForm.controls.password;
    if (
      this.userReactiveForm.valid &&
      typeof email === 'string' &&
      typeof password === 'string'
    )
      this.authService.register({ email, password }).subscribe();
  }

  public onLogin(): void {
    const email = this.userReactiveForm.controls.email.value;
    const password = this.userReactiveForm.controls.password.value;
    if (
      this.userReactiveForm.valid &&
      typeof email === 'string' &&
      typeof password === 'string'
    )
      this.authService.login({ email, password }).subscribe({
        next: (authReturn) => {
          localStorage.setItem(
            AUTH_TOKEN_LOCAL_STORAGE,
            authReturn.accessToken
          );
          localStorage.setItem(
            REFRESH_TOKEN_LOCAL_STORAGE,
            authReturn.refreshToken
          );

          this.router.navigate(['courts']);
        },
      });
  }

  public onChangeAccProcessOption(accountProcessOption: MatRadioChange): void {
    accountProcessOption.value === AccountProcessOption.Loging
      ? this.accountProcessOptionSig.set(AccountProcessOption.Loging)
      : this.accountProcessOptionSig.set(AccountProcessOption.Register);
  }
}
