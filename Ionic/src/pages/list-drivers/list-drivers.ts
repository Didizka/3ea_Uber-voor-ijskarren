import {Component, OnInit} from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import {Driver} from "../../Models/driver";
import {UserProvider} from "../../providers/user";

//@IonicPage()
@Component({
  selector: 'page-list-drivers',
  templateUrl: 'list-drivers.html',
})
export class ListDriversPage implements OnInit{
  zoom: number = 10;
  title: string = 'My first AGM project';
  lat: number = 51.2217865;
  lng: number = 4.461651;
  drivers: Driver[]= [];

  constructor(public navCtrl: NavController,
              public navParams: NavParams,
              private userProvider: UserProvider) {
  }
  ngOnInit(){
    this.userProvider.getDriversLocation().subscribe(
      data => {
        this.drivers = data;
        console.log(this.drivers[0].userID);
      },
      err => {
        console.log(err);
      });
  }
  onLogout(){
    this.navCtrl.setRoot('SigninPage');
  }

}
