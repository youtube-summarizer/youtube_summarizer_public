import { Injectable } from "@angular/core";
import { Subject } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class LoaderService {
  private isLoading = new Subject<boolean>();

  isSidenavOpen = true;

  private count = 0;

  public get loadingState() {
    return this.isLoading.asObservable();
  }

  public show() {
    this.count++;
    if (this.count > 0) {
      this.isLoading.next(true);
    }
  }

  public hide() {
    this.count--;
    if (this.count < 0) {
      this.count = 0;
    }

    if (this.count == 0) {
      this.isLoading.next(false);
    }
  }
}
