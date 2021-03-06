import { Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';
import 'rxjs/add/operator/map';
import { Storage } from '@ionic/storage';
import { HubConnection } from '@aspnet/signalr-client';
import { App } from 'ionic-angular';
import { OrderInProgressPage } from '../pages/order/order-in-progress/order-in-progress';

@Injectable()
export class UserProvider {
  private hubConnection;

  //ip: string = 'http://146.185.141.77:9000/api/users/';

  // Run webapi backend on port 9000
  // download iisexpress-proxy (see github repo)
  // run iisexpress-proxy 9000 to 8080
  // change ip address to your local ip address in order to test the app on the mobile and have access to the backend


  // Local DEV
  //ip: string = 'http://localhost:9000/api/users/';
  //ip: string = 'http://172.16.251.76:80/api/users/';
  // ip: string = 'http://localhost:9000/api/users/';
  // ip: string = 'http://172.16.251.76:80/api/users/';
  //school-sanjy
  ip: string = 'http://172.16.196.203:80/api/users/';
  //thuis-sanjy
  //ip: string = 'http://192.168.0.172:80/api/users/';
  // ip: string = 'http://192.168.0.172:80/api/users/';
  // ip: string = 'http://192.168.0.172:80/api/users/';
  //ip: string = 'http://172.16.229.9:80/api/users/';
  // ip: string = 'http://192.168.0.155:8080/api/users/';

  // School
  //ip: string = 'http://172.16.229.91:80/api/users/';

  // Production server
  // ip: string = 'http://cloud-app.ddns.net/api/users/';

  constructor(public http: Http, private storage: Storage, private app: App) {

  }
  private headers: Headers = new Headers({
    'Accept': 'application/json'
  });
  getAllUsers() {
    return this.http.get(this.ip);
  }
  createAccount(user: JSON) {
    console.log(user);
    return this.http.post(this.ip, user);
  }

  login(email: string, user: JSON) {

    return this.http.post(this.ip + email, user, { headers: this.headers })
      .map((response: Response) => {
        if (response.json() == 'CUSTOMER' || response.json() == 'DRIVER') {
          this.storage.ready().then(() => {
            this.storage.set('currentUser', email);
          });
        }
        return response.json();
      });
  }
  getUser(email: string) {
    return this.http.get(this.ip + email)
      .map((response: Response) => {
        return response.json();
      });
  }
  getCurrentUser() {
    return this.storage.get('currentUser').then();
  }
  getDrivers() {
    return this.http.get(this.ip + 'drivers')
      .map((response: Response) => {
        return response.json();
      });
  }

  startSignalRSession(email: string) {
     this.hubConnection = new HubConnection('http://localhost:9000/orderhub?email=' + email);
    //this.hubConnection = new HubConnection('http://http://146.185.141.77:9000/orderhub?email=' + email);
      // Method used to debug server info
      this.hubConnection.on('Debug', (data: any) => {
        console.log(data);
      });

      this.hubConnection.on('OrderNotification', (data: any) => {
        this.app.getRootNavs()[0].setRoot('CurrentOrdersPage', { 'data' : data.value });
      });

      this.hubConnection.on('CustomerNotification', (data: any) => {
        console.log(data);
        this.app.getRootNavs()[0].setRoot('OrderInProgressPage', { 'driver' : data });
      });

      this.hubConnection.start()
        .then(() => {
          console.log('Hub connection started');
          console.log(this.hubConnection);
          localStorage.setItem('hubConnection', this.hubConnection);
        })
        .catch(err => {
          console.log('Error while establishing connection')
        });
  }

  askForDriverLocation(email: string) {
    // console.log('ok');
    return this.http.get('http://146.185.141.77:9000/api/order/driver/location/' + email)
      .map(resp => {
        return resp.json();
      })
  }

  stopSignalRSession() {
    if (this.hubConnection != null) this.hubConnection.stop();
  }
}
