import { Injectable } from "@angular/core";
import { Observable, of } from "rxjs";
import { HttpService } from "./base/http.service";

@Injectable({
  providedIn: "root",
})
export class RecaptchaService {
  constructor(private httpService: HttpService) {}

  verifyRecaptcha(captchaResponse: string): Observable<boolean> {
    // return this.httpService.post("recaptcha/verify", { captchaResponse: captchaResponse });
    return of(true);
  }
}
