import {Component, OnInit} from '@angular/core';
import {Storage} from '@ionic/storage';
import {AlertController, IonicPage, NavController, NavParams} from 'ionic-angular';
import {Driver, DriverFlavour} from "../../../Models/driver";
import {OrderProvider} from "../../../providers/order";
import {ShoppingCart} from "../../../Models/flavour.model";
import {ListDriversPage} from "../../list-drivers/list-drivers";

@IonicPage()
@Component({
  selector: 'page-order-confirm',
  templateUrl: 'order-confirm.html',
})
export class OrderConfirmPage implements OnInit{
  driverWithPrice: DriverFlavour;
  shoppingCart: ShoppingCart;
  chosenDiver: Driver;
  constructor(private navCtrl: NavController,
              private navParams: NavParams,
              private orderProvider: OrderProvider,
              private storage: Storage,
              private alertCtrl: AlertController) {
  }

  ngOnInit(){
    this.chosenDiver = this.navParams.get('driver');
    this.storage.ready().then(
      ()=>{
        this.storage.get('orderId').then(
          id => {
            if(id != null){
              this.orderProvider.getOrderBackWithTotalPrice(id, this.chosenDiver.email).subscribe(
                data => {
                  this.shoppingCart = data.shoppingCart;
                  //console.log(this.shoppingCart);
                  //console.log(data.shoppingCart);
                }
              );
            }
          }
        );
      });
  }

  onCancelOrder(){
    let confirm = this.alertCtrl.create({
      title: 'Order cancellation ',
      message: 'Do you want remove this order?',
      buttons: [
        {
          text: 'No',
          handler: () => {
            console.log('Disagree clicked');
          }
        },
        {
          text: 'Yes',
          handler: () => {
            this.navCtrl.setRoot(ListDriversPage, {cancellation: true})
          }
        }
      ]
    });
    confirm.present();
  }

}
