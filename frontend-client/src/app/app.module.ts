import { CommonModule } from "@angular/common";
import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatIconModule } from "@angular/material/icon";
import { MatInputModule } from "@angular/material/input";
import { MatPaginatorModule } from "@angular/material/paginator";
import { MatSortModule } from "@angular/material/sort";
import { MatTableModule } from "@angular/material/table";
import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { AmplifyAuthenticatorModule } from "@aws-amplify/ui-angular";
import { TranslateLoader, TranslateModule } from "@ngx-translate/core";
import { TranslateHttpLoader } from "@ngx-translate/http-loader";
import { RecaptchaModule } from "ng-recaptcha";
import { ToastrModule } from "ngx-toastr";
import { AppComponent } from "./app-component/app.component";
import { AppRoutingModule } from "./app-routing.module";
import { FooterComponent } from "./components/footer/footer.component";
import { HomepageComponent } from "./components/homepage/homepage.component";
import { LoginComponent } from "./components/login/login.component";
import { NavbarComponent } from "./components/navbar/navbar.component";
import { RecaptchaComponent } from "./components/recaptcha/recaptcha.component";
import { SubscriptionpageComponent } from "./components/subscriptionpage/subscriptionpage.component";
import { VideoComponent } from "./components/video/video.component";
import { AuthInterceptor } from "./interceptors/auth.interceptor";

@NgModule({
  declarations: [
    AppComponent,
    HomepageComponent,
    VideoComponent,
    NavbarComponent,
    FooterComponent,
    LoginComponent,
    RecaptchaComponent,
    SubscriptionpageComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserModule,
    ReactiveFormsModule,
    HttpClientModule,

    //NOTE: ngx-translate and the loader module
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient],
      },
    }),

    //NOTE: ngx-toastr
    CommonModule,
    ToastrModule.forRoot(),
    BrowserAnimationsModule,
    FormsModule,

    //NOTE: AWS Cognito
    AmplifyAuthenticatorModule,

    //MOTE: reCAPTCHA
    RecaptchaModule,

    MatTableModule,
    MatSortModule,
    MatInputModule,
    MatPaginatorModule,
    FormsModule,
    MatIconModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}

//NOTE: required for AOT compilation -- Translate Module
export function HttpLoaderFactory(http: HttpClient): TranslateHttpLoader {
  return new TranslateHttpLoader(http);
}
