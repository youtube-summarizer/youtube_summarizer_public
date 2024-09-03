import { Component, OnInit } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import { ToastrService } from "ngx-toastr";
import { UserInteractedVideo } from "src/app/models/dtos/user-interacted-video.model";
import { VideoModel } from "src/app/models/dtos/video-model";
import { UserInteractedVideoService } from "src/app/services/user-interacted-video.service";
import { VideosService } from "src/app/services/videos.service";
import { environment } from "src/environments/environment";
import { UserService } from "./../../services/user.service";

@Component({
  selector: "app-homepage",
  templateUrl: "./homepage.component.html",
  styleUrls: ["./homepage.component.scss"],
})
export class HomepageComponent implements OnInit {
  title: string = "ytb-summarizer";
  videos: VideoModel[] = [];
  userInteractedVideos: UserInteractedVideo[] = [];

  constructor(
    translate: TranslateService,
    private toastrService: ToastrService,
    private videosService: VideosService,
    private userService: UserService,
    private userInteractedVideoService: UserInteractedVideoService
  ) {
    translate.setDefaultLang(environment.defaultLanguage);
    translate.use(environment.defaultLanguage);
  }

  ngOnInit(): void {
    this.videosService.getAll().subscribe((result: VideoModel[]) => {
      this.videos = result;
      if (this.videos) {
        this.toastrService.success("Successfully got the videos from the server!");
      }
    });
  }

  regenerateVideo(videoId: string): void {
    this.videosService.regenerate(videoId).subscribe();
  }
}
