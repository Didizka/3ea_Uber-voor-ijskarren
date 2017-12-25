import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import {OrderProvider} from "../../../providers/order";
import {Customer} from "../../../Models/customer";
import {Storage} from "@ionic/storage"
import {ShoppingCart} from "../../../Models/flavour.model";

declare var google;
@IonicPage()
@Component({
  selector: 'page-current-orders',
  templateUrl: 'current-orders.html',
})
export class CurrentOrdersPage implements OnInit {

  customer: Customer;
  shoppingCart: ShoppingCart;
  totalPrice: number;
  address: string;
  data: any;
  orderId: number;

  constructor(public navCtrl: NavController, public navParams: NavParams,
              private orderProvider: OrderProvider,
              private storage: Storage,
              private ref: ChangeDetectorRef) {
  }

  ngOnInit() {
    this. data = this.navParams.get("data");
    this.shoppingCart = this.data.shoppingCart;
    this.customer = this.data.customer;
    this.totalPrice = this.data.totalPrice;
    let geocoder = new google.maps.Geocoder();
    geocoder.geocode({ location: new google.maps.LatLng(this.customer.location.latitude, this.customer.location.longitude) }, function (resp, status) {

      this.address = resp[0].formatted_address;
      this.ref.detectChanges();
    }.bind(this));
  }

}
