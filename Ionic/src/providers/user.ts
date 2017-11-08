import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import 'rxjs/add/operator/map';
import {Customer} from "../Models/customer";

@Injectable()
export class UserProvider {

  constructor(public http: Http) {

  }
  private headers: Headers = new  Headers({
    'Accept': 'application/json'
  });
  getAllUsers(){
    return this.http.delete('http://localhost:9000/api/users/7');
  }
  createAccount(user: JSON){
    console.log(user);
    return this.http.post('http://localhost:9000/api/users', user);
  }

  login(email: string, user: JSON){
    console.log(user);
    return this.http.post('http://localhost:9000/api/users/' + email, user, {headers: this.headers });
  }
  getUser(email: string){
    return this.http.get('http://localhost:9000/api/users/' + email);
  }

}
