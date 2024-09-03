import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import { ToastrService } from "ngx-toastr";
import { Observable } from "rxjs";
import { finalize, tap } from "rxjs/operators";
import { environment } from "src/environments/environment";
import { RequestOptions } from "../../models/client-models/request-options";
import { LoaderService } from "./loader.service";

@Injectable({
  providedIn: "root",
})
export class HttpService {
  private apiUrl = environment.baseApiUrl;

  public constructor(
    private httpClient: HttpClient,
    private toastr: ToastrService,
    private translate: TranslateService,
    private loaderService: LoaderService
  ) {}

  public get<T>(url: string, query?: any, requestOptions?: RequestOptions, header?: HttpHeaders) {
    if (requestOptions === null || requestOptions === undefined) {
      requestOptions = new RequestOptions();
      requestOptions.ShowSuccess = false;
    }

    header = this.addTokenHeader(header);

    const commonOptions = {
      params: query,
      headers: header,
    };

    if (requestOptions.ResponseType === "text") {
      return this.showLoaderAndHideOnCompletion(
        this.httpClient.get(this.apiUrl + url, {
          ...commonOptions,
          responseType: "text",
        }) as unknown as Observable<T>,
        requestOptions
      );
    } else {
      return this.showLoaderAndHideOnCompletion(
        this.httpClient.get<T>(this.apiUrl + url, commonOptions),
        requestOptions
      );
    }
  }

  public post<T>(url: string, data: any, query?: any, requestOptions?: RequestOptions, header?: HttpHeaders) {
    if (requestOptions === null || requestOptions === undefined) {
      requestOptions = new RequestOptions();
    }

    header = this.addTokenHeader(header);

    return this.showLoaderAndHideOnCompletion(
      this.httpClient.post<T>(this.apiUrl + url, data, {
        params: query,
        headers: header,
        responseType: requestOptions.ResponseType === "blob" ? ("blob" as "json") : "json",
      }),
      requestOptions
    );
  }

  public postExternal<T>(url: string, data: any, query?: any, requestOptions?: RequestOptions, apiKey?: string) {
    if (requestOptions === null || requestOptions === undefined) {
      requestOptions = new RequestOptions();
    }

    let headersConfig: { [header: string]: string | string[] } = {};
    if (apiKey) {
      headersConfig["x-api-key"] = apiKey;
    }
    const header = new HttpHeaders(headersConfig);

    return this.showLoaderAndHideOnCompletion(
      this.httpClient.post<T>(url, data, {
        params: query,
        headers: header,
        responseType: requestOptions.ResponseType === "blob" ? ("blob" as "json") : "json",
      }),
      requestOptions
    );
  }

  public put<T>(url: string, data: any, query?: any, requestOptions?: RequestOptions, header?: HttpHeaders) {
    if (requestOptions === null || requestOptions === undefined) {
      requestOptions = new RequestOptions();
    }

    header = this.addTokenHeader(header);

    return this.showLoaderAndHideOnCompletion(
      this.httpClient.put<T>(this.apiUrl + url, data, {
        params: query,
        headers: header,
      }),
      requestOptions
    );
  }

  public patch<T>(url: string, data: any, query?: any, requestOptions?: RequestOptions, header?: HttpHeaders) {
    if (requestOptions === null || requestOptions === undefined) {
      requestOptions = new RequestOptions();
    }

    header = this.addTokenHeader(header);

    return this.showLoaderAndHideOnCompletion(
      this.httpClient.patch<T>(this.apiUrl + url, data, {
        params: query,
        headers: header,
      }),
      requestOptions
    );
  }

  public delete<T>(url: string, query?: any, requestOptions?: RequestOptions, header?: HttpHeaders) {
    if (requestOptions === null || requestOptions === undefined) {
      requestOptions = new RequestOptions();
    }

    requestOptions.IsDeleteAction = true;
    header = this.addTokenHeader(header);

    return this.showLoaderAndHideOnCompletion(
      this.httpClient.delete<T>(this.apiUrl + url, {
        params: query,
        headers: header,
      }),
      requestOptions
    );
  }

  private addTokenHeader(header?: HttpHeaders) {
    if (!header) {
      header = new HttpHeaders();
    }

    return header;
  }

  private showLoaderAndHideOnCompletion<T>(observable: Observable<T>, requestOptions: RequestOptions): Observable<T> {
    if (requestOptions.ShowLoader) {
      this.loaderService.show();
    }

    return observable.pipe(
      tap({
        next: () => {
          if (requestOptions.ShowSuccess) {
            let messageKey = requestOptions.IsDeleteAction ? "Common.DeleteSuccessful" : "Common.Successful";
            messageKey = this.translate.instant(messageKey);
            this.toastr.success(messageKey);
          }
        },
        error: err => {
          if (requestOptions.HandleGenericError) {
            const messageTitle = this.getErrorMessageTitle(err);
            let messageBody = this.getErrorMessageBody(err);
            this.showErrorToaster(messageTitle, messageBody, err);
          }
        },
      }),
      finalize(() => {
        if (requestOptions.ShowLoader) {
          this.loaderService.hide();
        }
      })
    );
  }

  private getErrorMessageTitle(err: any): string {
    return this.translate.instant("Common.FailureTitle");
  }

  private getErrorMessageBody(err: any): string {
    if (err?.status == 404) {
      return this.translate.instant("Common.ResourceNotFoundBody");
    }

    return this.translate.instant("Common.FailureMessage");
  }

  private showErrorToaster(messageTitle: string, messageBody: string, err: { error: { message: string } }) {
    if (err.error?.message) {
      messageBody += " -> " + err.error.message;

      const translatableParts = RegExp(/\{(.*?)\}/).exec(err.error.message);

      if (translatableParts) {
        translatableParts.forEach((messageTemplate: any) => {
          const messageTranslation = this.translate.instant(`Errors.${messageTemplate}`);
          messageBody = messageBody.replace(messageTemplate, messageTranslation);
        });
      }
    }

    this.toastr.error(messageBody, messageTitle);
  }
}
