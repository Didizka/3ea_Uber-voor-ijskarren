import { Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';
import 'rxjs/add/operator/map';
import {Customer} from "../Models/customer";

@Injectable()
export class UserProvider {
  //ip: string = 'http://172.16.220.163';

  // Run webapi backend on port 9000
  // download iisexpress-proxy (see github repo)
  // run iisexpress-proxy 9000 to 8080
  // change ip address to your local ip address in order to test the app on the mobile and have access to the backend

  // Local DEV
  // ip: string = 'http://localhost:9000/api/users/';

  // Home
  // ip: string = 'http://192.168.0.155:8080/api/users/';

  // School
  // ip: string = 'http://172.16.235.4:8080/api/users/';

  // Production server
  ip: string = 'http://cloud-app.ddns.net/api/users/';

  constructor(public http: Http) {

  }
  private headers: Headers = new  Headers({
    'Accept': 'application/json'
  });
  getAllUsers(){
    return this.http.get(this.ip);
  }
  createAccount(user: JSON){
    console.log(user);
    return this.http.post(this.ip, user);
  }

  login(email: string, user: JSON){
    console.log(user);
    return this.http.post(this.ip + email, user, {headers: this.headers })
      .map((response: Response) => {
      return response.json();
    });
  }
  getUser(email: string){
    return this.http.get(this.ip + email)
      .map((response: Response) => {
      return response.json();
    });
  }
  getDriversLocation(){
    return this.http.get(this.ip + 'location')
      .map((response: Response) => {
        return response.json();
      });
  }

}