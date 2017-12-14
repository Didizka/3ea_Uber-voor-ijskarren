import { Component, OnInit } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';

@IonicPage()
@Component({
  selector: 'page-order-in-progress',
  templateUrl: 'order-in-progress.html',
})

export class OrderInProgressPage implements OnInit {
  driver: any;
  constructor(public navCtrl: NavController, public navParams: NavParams) {
  }

  ngOnInit() {
    this.driver = this.navParams.get('driver');
  }

}
