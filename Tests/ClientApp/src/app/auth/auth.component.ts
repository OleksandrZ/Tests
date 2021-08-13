import { ThisReceiver } from "@angular/compiler";
import { Component, OnInit } from "@angular/core";
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { first } from "rxjs/operators";
import { AuthService } from "./../core/services/auth.service";

@Component({
  selector: "app-auth",
  templateUrl: "./auth.component.html",
  styleUrls: ["./auth.component.css"],
})
export class AuthComponent implements OnInit {
  authType: string = "";
  title: string = "";
  authorizationForm: FormGroup;
  fieldTextType: boolean;
  returnUrl: string = "";

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService
  ) {
    this.authorizationForm = this.fb.group({
      email: ["", [Validators.required, Validators.email]],
      password: [
        "",
        [
          Validators.required,
          Validators.minLength(8),
          Validators.pattern(
            "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$"
          ),
        ],
      ],
    });
  }

  ngOnInit() {
    this.route.url.subscribe((data) => {
      this.authType = data[data.length - 1].path;
      this.title = this.authType === "login" ? "Sign in" : "Sign up";
      this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
      if (this.authType === "register") {
        this.authorizationForm.addControl(
          "username",
          new FormControl("", [Validators.required, Validators.minLength(3)])
        );
      } else {
        this.authorizationForm.addControl("rememberMe", new FormControl());
      }
    });
  }
  Login() {
    if (this.authorizationForm.invalid) {
      return;
    }

    let email = this.authorizationForm.value.email;
    let password = this.authorizationForm.value.password;
    let rememberMe: boolean = this.authorizationForm.value.rememberMe;

    if (rememberMe !== true) {
      rememberMe = false;
    }

    this.authService
      .login(email, password, rememberMe)
      .pipe(first())
      .subscribe({
        next: () => {
          console.log("Ok");
          this.router.navigate([this.returnUrl]);
        },
        error: (error) => {
          console.log("Error");
          console.log(error);
        },
      });
  }
  Register() {
    if (this.authorizationForm.invalid) {
      return;
    }

    let email = this.authorizationForm.value.email;
    let password = this.authorizationForm.value.password;
    let username = this.authorizationForm.value.username;

    this.authService
      .register(username, email, password)
      .pipe(first())
      .subscribe({
        next: () => {
          this.router.navigate(["/login"]);
        },
        error: (error) => {
          console.log(error);
        },
      });
  }

  toggleFieldTextType() {
    this.fieldTextType = !this.fieldTextType;
  }
}
