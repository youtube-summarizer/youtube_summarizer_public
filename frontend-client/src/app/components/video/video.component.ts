import { Component, Input, OnInit } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import { ToastrService } from "ngx-toastr";
import { VideoModel } from "src/app/models/dtos/video-model";
import { UserService } from "src/app/services/user.service";
import { VideosService } from "src/app/services/videos.service";
import { environment } from "src/environments/environment";
import { UserInteractedVideo } from "./../../models/dtos/user-interacted-video.model";
import { UserInteractedVideoService } from "./../../services/user-interacted-video.service";

@Component({
  selector: "app-video",
  templateUrl: "./video.component.html",
  styleUrls: ["./video.component.scss"],
})
export class VideoComponent implements OnInit {
  userInteractedVideos: UserInteractedVideo[] = [];
  showingSummary: boolean = false;
  interaction?: UserInteractedVideo;
  score: number = 0;
  rated: boolean = false;

  @Input() video!: VideoModel;

  constructor(
    translate: TranslateService,
    private toastrService: ToastrService,
    private userService: UserService,
    private userInteractedVideoService: UserInteractedVideoService,
    private videoService: VideosService
  ) {
    translate.setDefaultLang(environment.defaultLanguage);
    translate.use(environment.defaultLanguage);
  }

  ngOnInit(): void {
    if (this.userService.isLoggedIn()) {
      this.userInteractedVideoService.getAll().subscribe((result: UserInteractedVideo[]) => {
        this.userInteractedVideos = result;
        if (this.userInteractedVideos) {
          const existing = this.userInteractedVideos.find(
            interaction => interaction.videoId === this.video.youtubeVideoId
          );
          this.interaction =
            existing ?? new UserInteractedVideo(this.userService.getUsername(), this.video.youtubeVideoId, 0);
          this.rated = existing !== undefined;
          this.score = this.interaction.score ?? 0;
        }
      });
    }
  }

  setRating(score: number): void {
    if (this.interaction) {
      this.interaction.score = score;
      this.score = score;
    }

    this.userInteractedVideoService
      .edit(
        this.interaction ??
          new UserInteractedVideo(this.userService.getUsername(), this.video.youtubeVideoId, this.score)
      )
      .subscribe((result: any) => {
        console.log(result);
        if (result) {
          this.toastrService.success("Successfully rated the video!");
          this.rated = true;
        } else {
          this.toastrService.error("Failed to rate the video!");
        }
      });
  }

  // todo: move to utils
  getThumbnailUrl(videoId: string): string {
    return `https://img.youtube.com/vi/${videoId}/hqdefault.jpg`;
  }

  getVideoUrl(videoId: string): string {
    return `https://www.youtube.com/embed/${videoId}`;
  }

  getYouTubeUrl(videoId: string): string {
    return `https://www.youtube.com/watch?v=${videoId}`;
  }

  toggleSummary(): void {
    this.showingSummary = !this.showingSummary;
  }

  onRegenerate(): void {
    this.videoService.regenerate(this.video.youtubeVideoId).subscribe();
  }
}
