import { OnDestroy } from '@angular/core/src/metadata/lifecycle_hooks';
import { UserProvider } from './../../../providers/user';
import { Component, OnInit } from '@angular/core';
import { IonicPage, ModalController, NavController, NavParams } from 'ionic-angular';
import { HubConnection } from '@aspnet/signalr-client';
import { Storage } from '@ionic/storage';

@IonicPage()
@Component({
  selector: 'page-driver-dashboard',
  templateUrl: 'driver-dashboard.html',
})
export class DriverDashboardPage implements OnInit, OnDestroy {
  hubConnection: HubConnection;


  constructor(public navCtrl: NavController, private userProvider: UserProvider, private navParams: NavParams, public modalCtrl: ModalController, private storage: Storage) {
  }

  ngOnInit() {

  }

  ngOnDestroy() {
    this.userProvider.stopSignalRSession();
  }
  onLogout() {
    this.navCtrl.setRoot('SigninPage');
  }
  onUpdateFlavours() {
    let model = this.modalCtrl.create('UpdateFlavoursPage');
    model.present();
  }
  onCurrentOrder(){
    this.navCtrl.push("CurrentOrdersPage");
  }
}
