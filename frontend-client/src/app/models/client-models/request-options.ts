export class RequestOptions {
  public ShowSuccess?: boolean;
  public IsDeleteAction?: boolean;
  public HandleGenericError?: boolean;
  public ShowLoader?: boolean;
  public ResponseType?: "json" | "text" | "blob";
  public HandleLoginError?: boolean;
  public DoNotAddTimestampToLocalStorage?: boolean;

  public constructor(init?: Partial<RequestOptions>) {
    Object.assign(this, init);
  }
}
