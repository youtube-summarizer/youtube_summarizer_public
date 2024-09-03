import { Injectable, OnInit } from '@angular/core';
import { Auth } from 'aws-amplify';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor() {
    Auth.currentAuthenticatedUser().then(user => {
      if (user) this.user = user;
    })
  }

  user: any = null;

  getUsername(): string {
    return this.user?.username ?? '';
  }

  isLoggedIn(): boolean {
    return this.getUsername() !== '';
  }

  logout() {
    localStorage.clear();
  }
}
