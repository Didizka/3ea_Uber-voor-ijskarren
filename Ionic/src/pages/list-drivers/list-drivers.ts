import { Driver } from './../../Models/driver';
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { UserProvider } from "../../providers/user";
import { Geolocation } from "@ionic-native/geolocation";
import { Storage } from "@ionic/storage"
import { OrderProvider } from "../../providers/order";
import { isNumber } from "ionic-angular/util/util";

declare var google;

//@IonicPage()
@Component({
  selector: 'page-list-drivers',
  templateUrl: 'list-drivers.html',
})
export class ListDriversPage implements OnInit {
  // MAP
  lat: number = null;
  lng: number = null;
  distance: number = 5;
  zoom: number = 11;
  orderId: number = null;
  @ViewChild('map') mapElement: ElementRef;
  map: any;
  driversMarkers: any[] = [];

  // APP
  title: string = 'Uber voor ijskarren';

  // DRIVERS
  drivers: Driver[] = [];
  driversInZone: Driver[] = [];
  areDriversProcessed: boolean = false;

  //orderCancel
  isOrderCanceled = false;

  constructor(public navCtrl: NavController,
    private navParams: NavParams,
    private storage: Storage,
    private userProvider: UserProvider,
    private orderProvider: OrderProvider,
    private geolocation: Geolocation) {
    this.isOrderCanceled = this.navParams.get("cancellation")
  }

  ngOnInit() {
    // Remove price label from drivers list if order has been cancelled
    if (this.isOrderCanceled) {
      this.removeOrder();
    }

    // Check if current position has been saved in the localstorage
    if (JSON.parse(localStorage.getItem('coords')) != null) {
      // if it has, check if lat and lng are not zero's
      this.lat = JSON.parse(localStorage.getItem('coords')).lat;
      this.lng = JSON.parse(localStorage.getItem('coords')).lng;
      // If they are zero's, get users current position
      if (this.lat == 0 || this.lng == 0) {
        this.getUserPosition();
        // if they have been set, display the map and get drivers info
      } else {
        this.initMap();
        this.getDriversInfo();
      }
      // if current position does not exist in the localstorage, get users current location
    } else {
      this.getUserPosition();
    }
  }


  getDriversInfo() {
    // Check if an order has been placed
    // if it has, display the list of drivers with their prices
    this.storage.ready().then(() => {
      this.storage.get('orderId').then(orderId => {
        if (isNumber(orderId)) {
          this.loadDriversWithTotalPrice(orderId);
        } else {
          // if no order has been placed, display the list of drivers without their prices
          this.userProvider.getDrivers().subscribe(
            data => {
              //console.log(data);
              this.drivers = data;
              this.checkRangeOfDrivers().then(data => {
                this.driversInZone = data as Driver[];
                // console.log(data)
              });
              this.areDriversProcessed = true;
            },
            err => {
              console.log(err);
            });
        }

      });
    });
  }
  
  initMap() {
    let mapOptions = {
      center: new google.maps.LatLng(this.lat, this.lng),
      zoom: this.zoom,
      mapTypeId: google.maps.MapTypeId.ROADMAP,
      scrollwheel: false,
      draggable: false
    };
    this.map = new google.maps.Map(this.mapElement.nativeElement, mapOptions);
    this.addUserMarker();
  }


  onChooseDriver(driver: Driver) {
    if (!driver.totalPrice) {
      console.log("You have to place order first!");
    } else {
      this.navCtrl.push("OrderConfirmPage", { driver: driver });
    }
  }

  addUserMarker() {
    // Define user marker
    let userMarker = new google.maps.Marker({
      map: this.map,
      animation: google.maps.Animation.DROP,
      position: this.map.getCenter(),
      label: 'U'
    });

    // Get users address based on his coordinates and put it into the infowindow
    let geocoder = new google.maps.Geocoder();
    geocoder.geocode({ location: new google.maps.LatLng(this.lat, this.lng) }, function (resp, status) {
      let content = "<p>" + resp[0].formatted_address + "</p>";
      let infoWindow = new google.maps.InfoWindow({
        content: content
      });

      // display the infowindow when clicking on the marker
      google.maps.event.addListener(userMarker, 'click', () => {
        infoWindow.open(this.map, userMarker);
      });
    });

    // display the marker on the map
    userMarker.setMap(this.map);
  }

  addDriversMarkers(driver: Driver) {
    // Drivers location
    let driversLocation = new google.maps.LatLng(driver.location.latitude, driver.location.longitude)

    // Drivers marker
    let driverMarker = new google.maps.Marker({
      map: this.map,
      animation: google.maps.Animation.DROP,
      position: driversLocation
      // icon: 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS_QnIIaRF01p59K0GhwiodnX4f20Sc7lWM-RbjYLOqSEQjdIIfNw'
    });

    // Translate drivers coordinates to address and display it on the infowindow
    let content =
      "<p><b>Naam: </b>" + driver.firstName + " " + driver.lastName +
      "<p><b>Afstand: </b>" + (driver.distance / 1000).toFixed(2) + " km</p>" +
      "<p><b>Duur: </b>" + (driver.duration / 60).toFixed(1) + " min</p>" +
      "<p><b>Locatie: </b>" + driver.locationAddress + "</p>"
    let infoWindow = new google.maps.InfoWindow({
      content: content
    });

    // display the infowindow when clicking on the marker
    google.maps.event.addListener(driverMarker, 'click', () => {
      infoWindow.open(this.map, driverMarker);
    });

    // display the driver marker on the map
    this.driversMarkers.push(driverMarker);
  }


  // Does not work with live reload
  getUserPosition() {
    let options = {
      enableHighAccuracy: true
    };

    // Try to get users location using geolocator
    this.geolocation.getCurrentPosition(options).then(position => {
      let geocoder = new google.maps.Geocoder();
      // Translate current coordinates to the address and ask the user whether the address is correct
      geocoder.geocode({ location: new google.maps.LatLng(position.coords.latitude, position.coords.longitude) }, function (resp, status) {
        let isAddressCorrect = confirm('Uw locatie is: ' + resp[0].formatted_address + '. Is deze correct bepaald?');
        // if the address is correct, save the coordinates and show the map and drivers in range
        if (isAddressCorrect) {
          this.lat = position.coords.latitude;
          this.lng = position.coords.longitude;
          localStorage.setItem("coords", JSON.stringify(new google.maps.LatLng(this.lat, this.lng)));
          this.initMap();
          this.getDriversInfo();
          // if the address is not correct, ask the user to provide his address and translate it to coordinates, then show the map with drivers in range
        } else {
          let address = prompt('Geef hier uw adres in');
          geocoder.geocode({ address: address }, function (resp, status) {
            this.lat = resp[0].geometry.location.lat();
            this.lng = resp[0].geometry.location.lng();
            localStorage.setItem("coords", JSON.stringify(new google.maps.LatLng(this.lat, this.lng)));
            console.log(this.lat);
            console.log(this.lng);
            this.initMap();
            this.getDriversInfo();
            // this.initMap();
          }.bind(this));
        }
      }.bind(this));
    }).catch(err => {
      console.log('Position error: ' + err.message);
    });
  }

  loadDriversWithTotalPrice(orderId: number) {
    this.orderProvider.getDriversWithTotalPrice(orderId).subscribe(
      data => {
        //console.log(data);
        this.drivers = data;
        this.checkRangeOfDrivers().then(data => {
          this.driversInZone = data as Driver[];
          this.areDriversProcessed = true;
          console.log(data)
        })
      },
      err => {
        console.log(err);
      });
  }

  // if the user changes the range, recalculate the drivers whithin that range
  onRangeChange(event: any) {
    this.zoom = event.value;
    this.map.setZoom(this.zoom);
    switch (event.value) {
      case 11:
        this.distance = 5;
        break;
      case 12:
        this.distance = 4;
        break;
      case 13:
        this.distance = 3;
        break;
      case 14:
        this.distance = 2;
        break;
      case 15:
        this.distance = 1;
        break;
      default:
        this.distance = 0;
    }
    this.checkRangeOfDrivers().then(data => {
      this.driversInZone = data as Driver[];
      this.areDriversProcessed = true;
      console.log(data);
    });
  }

  onLogout() {
    this.navCtrl.setRoot('SigninPage');
  }

  placeOrder() {
    this.navCtrl.push('OrderPage');
  }


  checkRangeOfDrivers() {
    // Hide the list with drivers untill the distance has been calculated
    this.areDriversProcessed = false;

    // Clear driversInZone array
    while (this.driversInZone.length > 0) {
      this.driversInZone.pop();
    }

    // Clear driver markers
    for (let driverMarker of this.driversMarkers) {
      driverMarker.setMap(null);
    }
    this.driversMarkers = [];

    // Return promise to use with then in ngOnInit
    var promise = new Promise((resolve, reject) => {

      // local placeholder for drivers in range
      let driversInZone: Driver[] = [];

      // Wait until the users position has been determined
      if (this.lat != null && this.lng != null) {
        // Distance calculator
        let origin = new google.maps.LatLng(this.lat, this.lng);
        let distanceCalculator: any = new google.maps.DistanceMatrixService();
        console.log("Range: " + this.distance + "km");

        // calculate distance between origin and destination for each driver
        for (let driver of this.drivers) {
          distanceCalculator.getDistanceMatrix({
            origins: [origin],
            destinations: [new google.maps.LatLng(driver.location.latitude, driver.location.longitude)],
            travelMode: google.maps.TravelMode.DRIVING,
            drivingOptions: {
              departureTime: new Date(Date.now())
            }
            // Callback function when the calculation has been finished
          }, function (resp, status) {
            // Save driver's info to drivers object
            driver.locationAddress = resp.destinationAddresses;
            driver.distance = resp.rows[0].elements[0].distance.value;
            driver.duration = resp.rows[0].elements[0].duration.value;

            // console.log('------------ALL DRIVERS-----------------------');
            // console.log('Driver ID: ' + driver.userID);
            // console.log('Range: ' + this.distance);
            // console.log('Km: ' + (driver.distance / 1000));
            // console.log('Rounded: ' + Math.round((driver.distance / 1000)));
            // If the driver is in the range zone, add him to the temporary array and create a marker
            if ((driver.distance / 1000) < this.distance) {
              // console.log('------------DRIVERS IN ZONE-----------------------------');
              // console.log('Driver ID: ' + driver.userID);
              // console.log('Driver Name: ' + driver.firstName + ' ' + driver.lastName);
              // console.log('From: ' + resp.originAddresses);
              // console.log('To: ' + resp.destinationAddresses);
              // if (driver.distance > 1000) {
              //   console.log('Distance: ' + driver.distance / 1000 + ' km'); // distance in meters => / 1000 to get km's
              // } else {
              //   console.log('Distance: ' + driver.distance + ' m'); // distance in meters
              // }
              // console.log('Duration: ' + driver.duration / 60 + ' min'); //value = in seconds => / 60 to get minutes
              // console.log(driver);
              driversInZone.push(driver);
              this.addDriversMarkers(driver);
            }
          }.bind(this)); // Access this scope in callback
        }
      }
      else {
        setTimeout(() => {
          this.checkRangeOfDrivers();
        }, 500);
      }
      resolve(driversInZone);
    });
    return promise;
  };
  
  removeOrder() {
    this.storage.set('orderId', null);
    for (let driver of this.drivers) {
      driver.totalPrice = null;
    }
  }
}