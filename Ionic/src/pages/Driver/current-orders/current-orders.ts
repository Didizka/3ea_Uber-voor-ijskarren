import {Component, OnInit} from '@angular/core';
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

  constructor(public navCtrl: NavController, public navParams: NavParams,
              private orderProvider: OrderProvider, private storage: Storage) {
  }

  ngOnInit() {
    this.storage.get('currentUser').then(
      driver => {
        this.orderProvider.getDriverCurrentOrder(driver).subscribe(
          data => {

            this.customer = data.customer;
            this.shoppingCart = data.shoppingCart;
            this.totalPrice = data.totalPrice;
            this.getAddress();
          }
        );
      }
    );
  }

  onClickMe() {
    console.log(this.address);
  }

  getAddress() {
    /*let geocoder = new google.maps.Geocoder();
    geocoder.rever
    return geocoder.geocode("antwerpen").then(function (res) {
      console.log(res);
    })   ;
  }*/
  }
}
