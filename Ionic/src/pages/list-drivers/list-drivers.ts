import {Component, OnInit} from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import {Driver} from "../../Models/driver";
import {UserProvider} from "../../providers/user";
import { Geolocation } from "@ionic-native/geolocation";

//@IonicPage()
@Component({
  selector: 'page-list-drivers',
  templateUrl: 'list-drivers.html',
})
export class ListDriversPage implements OnInit{
  zoom: number = 10;
  title: string = 'Uber voor ijskarren';
  //lat: number = 51.2217865;5
  lat: number = 51.221791;
  //lng: number = 4.461651;lat: number = 51.2217865;51.221791, 4.461666
  lng: number = 4.461666;
  drivers: Driver[]= [];
  driversInZone: Driver[]= [];

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
        this.checkRangeOfDrivers(5);
      },
      err => {
        console.log(err);
      });
  }
  onRangeChange(event: any){
    switch (event.value){
      case 10:
        this.checkRangeOfDrivers(4);
        break;
      case 11:
        this.checkRangeOfDrivers(3);
        break;
      case 12:
        this.checkRangeOfDrivers(2);
        break;
      case 13:
        this.checkRangeOfDrivers(1);
        break;
      default:
        this.checkRangeOfDrivers(5);
    }
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

  calculateDistance(lat2, lon2){
    let R = 6371e3; // metres
    let φ1 = this.toRadians(this.lat);
    let φ2 = this.toRadians(lat2);
    let Δφ = this.toRadians((lat2-this.lat));
    let Δλ = this.toRadians((lon2-this.lng));

    let a = Math.sin(Δφ/2) * Math.sin(Δφ/2) +
      Math.cos(φ1) * Math.cos(φ2) *
      Math.sin(Δλ/2) * Math.sin(Δλ/2);
    let c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1-a));

    // in meter
    let d = R * c;
    //in km
    return d/1000;
    //console.log("Distance is: "+d/1000 + "km")
  }

  checkRangeOfDrivers(range: number){
    this.driversInZone.splice(0);
    console.log("Range: " +range + "km");
    for(let driver of this.drivers){
      let distance = this.calculateDistance(driver.location.latitude, driver.location.longitude);
      if(distance < range){
        this.driversInZone.push(driver);
      }
    }
  }

  toRadians(angle) {
    return angle * (Math.PI / 180);
  }
}
