import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Auth } from "aws-amplify";
import { Observable, from } from "rxjs";
import { catchError, switchMap } from "rxjs/operators";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return from(Auth.currentSession()).pipe(
      switchMap(session => {
        const token = session.getIdToken().getJwtToken();
        if (token) {
          const clonedReq = req.clone({
            headers: req.headers.set("Authorization", `Bearer ${token}`),
          });
          return next.handle(clonedReq);
        } else {
          return next.handle(req);
        }
      }),
      catchError(() => {
        //NOTE: If there's an error (no session/token), simply forward the original request.
        return next.handle(req);
      })
    );
  }
}
