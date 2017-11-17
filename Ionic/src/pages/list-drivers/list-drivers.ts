import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { Driver } from "../../Models/driver";
import { UserProvider } from "../../providers/user";
import { Geolocation } from "@ionic-native/geolocation";

declare var google;

//@IonicPage()
@Component({
  selector: 'page-list-drivers',
  templateUrl: 'list-drivers.html',
})
export class ListDriversPage implements OnInit{
  // MAP
  lat: number = 51.22179;
  lng: number =  4.461666;  
  distance: number = 5;
  zoom: number = 11;
  @ViewChild('map') mapElement: ElementRef;
  map: any;

  // APP
  title: string = 'Uber voor ijskarren';

  // DRIVERS
  drivers: Driver[]= [];
  driversInZone: Driver[]= [];

  

  constructor(public navCtrl: NavController,
              public navParams: NavParams,
              private userProvider: UserProvider,
              private geolocation: Geolocation) {
  }

  ngOnInit(){

    this.userProvider.getDriversLocation().subscribe(
      data => {
        this.drivers = data;       
        this.checkRangeOfDrivers(this.distance); 
        this.showMap();         
      },
      err => {
        console.log(err);
      });
  }

  showMap() {
    this.getUserPosition();
    
        let mapOptions = {
          center: new google.maps.LatLng(this.lat, this.lng),
          zoom: this.zoom,
          mapTypeId: google.maps.MapTypeId.ROADMAP,
          scrollwheel: false,
          draggable: false
        }
    
        this.map = new google.maps.Map(this.mapElement.nativeElement, mapOptions);
        this.addUserMarker();
        this.addDriversMarkers();
  }

  addUserMarker() {
      let userMarker = new google.maps.Marker({
        map: this.map,
        animation: google.maps.Animation.DROP,
        position: this.map.getCenter(),
        label: 'U'
      });
  
      let content = "<p>Dit is jouw huidige locatie!</p>";          
      let infoWindow = new google.maps.InfoWindow({
        content: content
      });
  
      google.maps.event.addListener(userMarker, 'click', () => {
        infoWindow.open(this.map, userMarker);
      });
  }

  addDriversMarkers() {
    for (let driver of this.driversInZone) {
      // console.log(driver);
      let driversLocation = new google.maps.LatLng(driver.location.latitude, driver.location.longitude)
      let driverMarker = new google.maps.Marker({
        map: this.map,
        animation: google.maps.Animation.DROP,
        position: driversLocation,
        label: driver.userID.toString()
      });

      let content = "<p>Driver ID: <b>" + driver.userID +"</b>, " + driver.firstName + " " + driver.lastName + "</p>";          
      let infoWindow = new google.maps.InfoWindow({
        content: content
      });
  
      google.maps.event.addListener(driverMarker, 'click', () => {
        infoWindow.open(this.map, driverMarker);
      });

    }
  }

  
  // Does not work with live reload
  getUserPosition() {
    // console.log('geolocation activated');
    let options = {
      enableHighAccuracy: false
    };
    this.geolocation.getCurrentPosition(options).then(position => {
      // console.log(position);
      this.lat = position.coords.latitude;
      this.lng = position.coords.longitude;
    }).catch(err => {
      console.log('Position error: ' + err.message);
    });
  }

  onRangeChange(event: any){
    this.zoom = event.value;
    this.map.setZoom(this.zoom);
    switch (event.value){
      case 12:
        this.distance = 4;
        break;
      case 13:
        this.distance = 3;
        break;
      case 14:
        this.distance = 2;
        break;
      case 14:
        this.distance = 1;
        break;
      default:
        this.distance = 5;        
    }

    this.checkRangeOfDrivers(this.distance);
  }

  onLogout(){
    this.navCtrl.setRoot('SigninPage');
  }



  calculateDistance(lat2, lon2){
    let R = 6371e3; // meters
    let φ1 = this.toRadians(this.lat);
    let φ2 = this.toRadians(lat2);
    let Δφ = this.toRadians((lat2 - this.lat));
    let Δλ = this.toRadians((lon2 - this.lng));

    let a = Math.sin(Δφ / 2) * Math.sin(Δφ / 2) +
      Math.cos(φ1) * Math.cos(φ2) *
      Math.sin(Δλ / 2) * Math.sin(Δλ / 2);
    let c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));

    // in meter
    let d = R * c;
    //in km
    return d / 1000;
    //console.log("Distance is: "+d/1000 + "km")
  }

  checkRangeOfDrivers(range: number){
    this.driversInZone.splice(0);
    console.log("Range: " + range + "km");
    for(let driver of this.drivers){
      // console.log(driver.location);
      let distance = this.calculateDistance(driver.location.latitude, driver.location.longitude);
      if(distance <= range){
        this.driversInZone.push(driver);
      }
    }
    console.log(this.driversInZone);
  }

  toRadians(angle) {
    return angle * (Math.PI / 180);
  }
}
