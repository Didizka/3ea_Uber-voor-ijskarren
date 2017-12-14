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
export class DriverDashboardPage implements OnInit {
  hubConnection: HubConnection;


  constructor(public navCtrl: NavController, private userProvider: UserProvider, private navParams: NavParams, public modalCtrl: ModalController, private storage: Storage) {
  }

  ngOnInit() {
    this.userProvider.getCurrentUser().then(email => {
      this.userProvider.startSignalRSession(email);
    });
  }

  onUpdateFlavours() {
    let model = this.modalCtrl.create('UpdateFlavoursPage');
    model.present();
  }


  OrderNotification() {
    this.hubConnection.on('OrderNotification', (data: any) => {
      console.log(data);
    });
  }

}
