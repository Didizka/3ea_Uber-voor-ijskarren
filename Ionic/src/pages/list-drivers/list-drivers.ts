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
  title: string = 'Uber voor ijskarren';
  lat: number;
  lng: number;
  drivers: Driver[]= [];

  constructor(public navCtrl: NavController,
              public navParams: NavParams,
              private userProvider: UserProvider) {
  }
  ngOnInit(){
    this.setCurrentPosition();
    this.userProvider.getDriversLocation().subscribe(
      data => {
        this.drivers = data;
        // console.log(this.drivers[0].userID);
      },
      err => {
        console.log(err);
      });
  }
  onLogout(){
    this.navCtrl.setRoot('SigninPage');
  }

  setCurrentPosition() {
    if ('geolocation' in navigator) {
      navigator.geolocation.getCurrentPosition((position) => {
        console.log(position);
        this.lat = position.coords.latitude;
        this.lng = position.coords.longitude;
      });
    }
  }

}
