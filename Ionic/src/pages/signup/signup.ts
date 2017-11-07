import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import {Customer} from "../../Models/customer";
import {NgForm} from "@angular/forms";
import {UserProvider} from "../../providers/user";

@IonicPage()
@Component({
  selector: 'page-signup',
  templateUrl: 'signup.html',
})
export class SignupPage {
  user: string = 'Customer';
  customer: Customer = {
    firstName: '',
    lastName: '',
    phoneNumber: '',
    email:'',
    password:'',
    userRoleType: 0,
    address: {
      streetName: '',
      streetNumber: '',
      zipCode: null
    }
  };
  constructor(public navCtrl: NavController,
              public navParams: NavParams,
              private userProvider: UserProvider) {
  }
  onRegister(form: NgForm, type: string){
    let user= form.value;//JSON.parse(form.value);

    console.log(type);
    if(type == 'Customer'){
      user['userRoleType'] = 0;
    }else {
      user['userRoleType'] = 1;
    }
    this.userProvider.createAccount(user).subscribe(
      data => {
        console.log(data)
      },
      err => {
        console.log(err);
      }
    );
    //console.log(this.user);
  }

}
