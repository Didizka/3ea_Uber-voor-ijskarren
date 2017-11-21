import {Component, OnInit} from '@angular/core';
import {AlertController, IonicPage, LoadingController, NavController, NavParams, ViewController} from 'ionic-angular';
import {OrderProvider} from "../../providers/order-provider";
import {Toast} from "@ionic-native/toast";
import {Flavour} from "../../Models/flavour.model";
import {ListDriversPage} from "../list-drivers/list-drivers";

@IonicPage()
@Component({
  selector: 'page-order',
  templateUrl: 'order.html',
})
export class OrderPage implements OnInit{
  flavours: string[] = [];
  usedFlavours: string[] = [];
  amount: number = 1;
  addFlavour: number = 1;
  selectAlertOpts:any;
  shoppingCart: ShoppingCart = new ShoppingCart([new Icecream([new Flavour("", 1)])]);
  addFlavours: Flavour[] = [new Flavour("", 1)];
  constructor(public navCtrl: NavController,
              private loadingCtrl: LoadingController,
              private orderProvider: OrderProvider,
              private toast: Toast,
              private alertCtrl: AlertController) {
    this.selectAlertOpts = {
      title: 'Flavour',
      subTitle: 'Select your flavour'
    };
  }

  ngOnInit(){
    this.orderProvider.getFlavours().subscribe(
      data => {
        for(let i = 0; i < data.length; i++){
          this.flavours.push(data[i].name);
        }
        this.usedFlavours = this.flavours;
      }
    );
    this.shoppingCart.cart.splice(0);
  }
  increaseAmount(index: number){
    if(this.addFlavours[index].amount >= 4)
      this.addFlavours[index].amount = 4;
    else
      this.addFlavours[index].amount++;
  }
  decreaseAmount(index: number){
    if(this.addFlavours[index].amount <= 1)
      this.addFlavours[index].amount = 1;
    else
      this.addFlavours[index].amount--;
  }
  onAddFlavour(){
    //this.checkFlavourAlreadySelect();
    if(this.addFlavours.length>=4)
      return;
    else
      this.addFlavours.push(new Flavour("", 1));
  }
  onRemoveFlavour(){
    this.addFlavours.splice(this.addFlavours.length-1, 1)
  }
  onAddToCart(){
    this.shoppingCart.cart.push(new Icecream(this.addFlavours));
    this.addFlavours = [new Flavour("", 1)];
  }
  onRemoveFromList(ice:Icecream){
    let index = this.shoppingCart.cart.indexOf(ice);
    this.shoppingCart.cart.splice(index, 1);
  }
  backToMap(){
    this.navCtrl.setRoot(ListDriversPage);
  }
  placeOrder(){
    const loading = this.loadingCtrl.create({
      content: 'Please wait...'
    });
    loading.present();
    this.orderProvider.placeOrder(JSON.parse(JSON.stringify(this.shoppingCart))).subscribe(
      data => {
        console.log(data);
        if(data == true){
          loading.dismiss();
          console.log('Order Placed success');
          // this.toast.showLongBottom("Order Placed success").subscribe(
          //   toast => {
          //     console.log(toast);
          //   }
          // );
          this.shoppingCart.cart=[];
          //this.navCtrl.setRoot('ListDriversPage');
        }else{
          loading.dismiss();
          this.errorMessage();
        }
      },
      err => {
        console.log(err);
      }
    );
  }
  private errorMessage(){
    const alert = this.alertCtrl.create({
      title: 'Error',
      message: 'Dry Again!',
      buttons: ['Ok']
    });
    alert.present();
  }
  private checkFlavourAlreadySelect(){
    for(let i = 0; i < this.addFlavours.length; i++){
      if(this.usedFlavours[i] != "" ){
        let f = this.flavours.indexOf(this.addFlavours[i].name);
        if(f>-1)
          this.usedFlavours.splice(f, 1);
        console.log(this.usedFlavours);
      }
      //this.usedFlavours.push(this.addFlavours[i].name);
    }
  }
}
export class Icecream{
  constructor(public iceCream: Flavour[]){}
}
export class ShoppingCart{
  constructor(public cart: Icecream[]){}
}
