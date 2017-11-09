import { Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';
import 'rxjs/add/operator/map';
import {Customer} from "../Models/customer";

@Injectable()
export class UserProvider {
  //ip: string = 'http://172.16.220.163';
  ip: string = 'http://localhost';
  constructor(public http: Http) {

  }
  private headers: Headers = new  Headers({
    'Accept': 'application/json'
  });
  getAllUsers(){
    return this.http.get(this.ip+':9000/api/users');
  }
  createAccount(user: JSON){
    console.log(user);
    return this.http.post(this.ip+':9000/api/users', user);
  }

  login(email: string, user: JSON){
    console.log(user);
    return this.http.post(this.ip+':9000/api/users/' + email, user, {headers: this.headers })
      .map((response: Response) => {
      return response.json();
    });
  }
  getUser(email: string){
    return this.http.get(this.ip+':9000/api/users/' + email)
      .map((response: Response) => {
      return response.json();
    });
  }
  getDriversLocation(){
    return this.http.get('http://localhost:9000/api/users/location')
      .map((response: Response) => {
        return response.json();
      });
  }

}
