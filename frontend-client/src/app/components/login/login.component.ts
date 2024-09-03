import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { Auth } from "aws-amplify";
import { ToastrService } from "ngx-toastr";
import { RecaptchaService } from "src/app/services/recaptcha.service";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.scss"],
})
export class LoginComponent {
  captchaResponse: string | undefined = "TODO: DELETE ME";

  constructor(
    private router: Router,
    private toastrService: ToastrService,
    private recaptchaService: RecaptchaService
  ) {}

  goToHomePage(): void {
    this.router.navigate(["/home"]);
  }

  customSignIn(username: string, password: string): void {
    if (!this.captchaResponse) {
      this.toastrService.warning("Please complete the reCAPTCHA challenge.");
      return;
    }

    this.recaptchaService.verifyRecaptcha(this.captchaResponse).subscribe({
      next: isValid => {
        if (!isValid) {
          this.toastrService.error("Invalid reCAPTCHA.");
          return;
        }
        Auth.signIn(username, password)
          .then(() => {
            this.toastrService.success("Successfully signed in.");
          })
          .catch(error => {
            console.error("Error during sign-in:", error);
          });
      },
      error: error => {
        console.error("Error verifying reCAPTCHA:", error);
      },
    });
  }

  //TODO: Extract the common logic between customSignIn and customSignUp into a single method.

  customSignUp(username: string, password: string): void {
    if (!this.captchaResponse) {
      this.toastrService.warning("Please complete the reCAPTCHA challenge.");
      return;
    }

    this.recaptchaService.verifyRecaptcha(this.captchaResponse).subscribe({
      next: isValid => {
        if (!isValid) {
          this.toastrService.error("Invalid reCAPTCHA.");
          return;
        }
        Auth.signUp({
          username,
          password,
        })
          .then(() => {
            this.toastrService.success("Successfully signed in.");
          })
          .catch(error => {
            console.error("Error during sign-up:", error);
          });
      },
      error: error => {
        console.error("Error verifying reCAPTCHA:", error);
      },
    });
  }

  handleCaptchaResponse(response: any): void {
    this.captchaResponse = response;
  }
}
