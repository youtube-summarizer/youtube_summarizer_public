import { Component } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import { UserService } from "src/app/services/user.service";
import { VideosService } from "src/app/services/videos.service";
import { environment } from "src/environments/environment";
@Component({
  selector: "app-navbar",
  templateUrl: "./navbar.component.html",
  styleUrls: ["./navbar.component.scss"],
})
export class NavbarComponent {
  constructor(translate: TranslateService, private userService: UserService, private videoService: VideosService) {
    translate.setDefaultLang(environment.defaultLanguage);
    translate.use(environment.defaultLanguage);
  }

  isLoggedIn(): boolean {
    return this.userService.isLoggedIn();
  }

  logout(): void {
    this.userService.logout();
    window.location.reload();
  }

  onAddVideo(): void {
    const inputElement = document.getElementById("searchInput") as HTMLInputElement;
    const inputValue = inputElement.value;

    this.videoService.addSingleVideo(inputValue, this.userService.getUsername()).subscribe();
  }
}
