import {Component, OnInit} from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import {Driver} from "../../Models/driver";
import {UserProvider} from "../../providers/user";
import { Geolocation } from '@ionic-native/geolocation';

//@IonicPage()
@Component({
  selector: 'page-list-drivers',
  templateUrl: 'list-drivers.html',
})
export class ListDriversPage implements OnInit{
  zoom: number = 10;
  title: string = 'Uber voor ijskarren';
  lat: number = 51.2217865;
  lng: number = 4.461651;
  drivers: Driver[]= [];
  distance: number = 5;

  constructor(public navCtrl: NavController,
              public navParams: NavParams,
              private userProvider: UserProvider,
              private geolocation: Geolocation) {
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


  // Does not work with live reload
  setCurrentPosition() {
    console.log('geolocation activated');
    this.geolocation.getCurrentPosition({maximumAge:0, timeout:5000, enableHighAccuracy:true}).then(position => {
      console.log(position);
      this.lat = position.coords.latitude;
      this.lng = position.coords.longitude;
      this.zoom = 8;
    }).catch((err) => {
      console.log(err);
    });
  }
    // if ('geolocation' in navigator) {
    //   navigator.geolocation.getCurrentPosition((position) => {
    //     console.log(position);
    //     this.lat = position.coords.latitude;
    //     this.lng = position.coords.longitude;
    //   });
    // } else {
    //   console.log('no geolocation found');
    // }
  // }

}
