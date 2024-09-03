export class UserInteractedVideo {
    userId!: string;
    videoId!: string;
    score?: number;

    constructor(userId: string, videoId: string, score?: number) {
        this.userId = userId;
        this.videoId = videoId;
        this.score = score;
    }
}