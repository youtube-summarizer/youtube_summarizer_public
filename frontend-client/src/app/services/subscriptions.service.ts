import { UserService } from './user.service';
import { SubscriptionModel } from './../models/dtos/subscription-model';
import { Injectable } from '@angular/core';
import { Observable, of } from "rxjs";
import { HttpService } from "./base/http.service";

@Injectable({
  providedIn: 'root'
})
export class SubscriptionsService {
  constructor(private httpService: HttpService, private userService: UserService) {}

  getAll(): Observable<SubscriptionModel[]> {
    const username = this.userService.getUsername();
    return this.httpService.get<SubscriptionModel[]>(`subscriptions/all?userId=${username}`);
  }

  add(subscription: SubscriptionModel): Observable<SubscriptionModel> {
    subscription.userId = this.userService.getUsername();
    return this.httpService.post<SubscriptionModel>("subscriptions/add", subscription);
  }

  remove(subscription: SubscriptionModel): Observable<SubscriptionModel> {
    subscription.userId = this.userService.getUsername();
    return this.httpService.post<SubscriptionModel>('subscriptions/delete', subscription);
  }
}