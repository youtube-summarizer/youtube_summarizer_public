import { UserInteractedVideo } from './../models/dtos/user-interacted-video.model';
import { Injectable } from '@angular/core';
import { Observable, of } from "rxjs";
import { HttpService } from "./base/http.service";
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class UserInteractedVideoService {
  constructor(private httpService: HttpService, private userService: UserService) {}

  getAll(): Observable<UserInteractedVideo[]> {
    const username = this.userService.getUsername();
    return this.httpService.get<UserInteractedVideo[]>(`UserInteractVideo/all?userId=${username}`);
  }
  
  get(userInteractedVideo: UserInteractedVideo): Observable<UserInteractedVideo[]> {
    return this.httpService.post<UserInteractedVideo[]>('UserInteractVideo/get', userInteractedVideo);
  }
  
  edit(userInteractedVideo: UserInteractedVideo): Observable<UserInteractedVideo> {
    return this.httpService.post<UserInteractedVideo>("UserInteractVideo/edit", userInteractedVideo);
  }
}