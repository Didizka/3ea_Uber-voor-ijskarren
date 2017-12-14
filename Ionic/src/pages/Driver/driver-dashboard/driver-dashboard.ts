import { Component } from '@angular/core';
import {IonicPage, ModalController, NavController, NavParams} from 'ionic-angular';

@IonicPage()
@Component({
  selector: 'page-driver-dashboard',
  templateUrl: 'driver-dashboard.html',
})
export class DriverDashboardPage {

  constructor(public navCtrl: NavController, public modalCtrl: ModalController) {
  }
  onLogout() {
    this.navCtrl.setRoot('SigninPage');
  }
  onUpdateFlavours() {
    let model = this.modalCtrl.create('UpdateFlavoursPage');
    model.present();
  }

}
