import { Component, ViewChild } from "@angular/core";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import { TranslateService } from "@ngx-translate/core";
import { ToastrService } from "ngx-toastr";
import { SubscriptionModel } from "src/app/models/dtos/subscription-model";
import { environment } from "src/environments/environment";
import { SubscriptionsService } from "./../../services/subscriptions.service";

@Component({
  selector: "app-subscriptionpage",
  templateUrl: "./subscriptionpage.component.html",
  styleUrls: ["./subscriptionpage.component.scss"],
})
export class SubscriptionpageComponent {
  displayedColumns: string[] = ["channelId", "action"];
  dataSource!: MatTableDataSource<SubscriptionModel>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    translate: TranslateService,
    private toastrService: ToastrService,
    private subscriptionsService: SubscriptionsService
  ) {
    translate.setDefaultLang(environment.defaultLanguage);
    translate.use(environment.defaultLanguage);
  }

  subscriptions: SubscriptionModel[] = [];
  newSubscription: SubscriptionModel = new SubscriptionModel();

  ngOnInit(): void {
    this.subscriptionsService.getAll().subscribe((result: SubscriptionModel[]) => {
      this.subscriptions = result;
      this.dataSource = new MatTableDataSource(this.subscriptions);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
      if (this.subscriptions) this.toastrService.success("Successuflly got the subscriptions from the server!");
    });
  }

  addNewSubscription() {
    this.subscriptionsService.add(this.newSubscription).subscribe((result: SubscriptionModel) => {
      if (result) {
        this.subscriptions.push(this.newSubscription);
        this.toastrService.success("Successfully added the subscription!");
      } else {
        this.toastrService.error("Failed to add the subscription!");
      }

      this.newSubscription = new SubscriptionModel();
    });
  }

  removeSubscription(subscription: SubscriptionModel) {
    this.subscriptionsService.remove(subscription).subscribe((result: SubscriptionModel) => {
      if (result) {
        this.subscriptions = this.subscriptions.filter(sub => sub !== subscription);
        this.toastrService.success("Successfully removed the subscription!");
      } else {
        this.toastrService.error("Failed to remove the subscription!");
      }
    });
  }

  filterData(filterValue: any) {
    this.dataSource.filter = (filterValue.target as HTMLInputElement).value.trim().toLowerCase();
  }
}
