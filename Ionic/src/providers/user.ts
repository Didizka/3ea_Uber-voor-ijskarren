import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';
import {Customer} from "../Models/customer";

@Injectable()
export class UserProvider {

  constructor(public http: Http) {

  }
  getAllUsers(){
    return this.http.delete('http://localhost:52468/api/users/7');
  }
  createAccount(user: JSON){
    console.log(user);
    return this.http.post('http://localhost:52468/api/users', user);
  }

  login(email: string, user: JSON){
    console.log(user);
    return this.http.post('http://localhost:52468/api/users/' + email, user);
  }

}
