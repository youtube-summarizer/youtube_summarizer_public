import { Component, EventEmitter, Output } from "@angular/core";

@Component({
  selector: "app-recaptcha",
  templateUrl: "./recaptcha.component.html",
  styleUrls: ["./recaptcha.component.scss"],
})
export class RecaptchaComponent {
  @Output() captchaResponseChange = new EventEmitter<string>();

  handleCaptchaResponse(response: string): void {
    this.captchaResponseChange.emit(response);
  }
}
