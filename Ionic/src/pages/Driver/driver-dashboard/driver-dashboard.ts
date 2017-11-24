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

  onUpdateFlavours() {
    let model = this.modalCtrl.create('UpdateFlavoursPage');
    model.present();
  }

}
