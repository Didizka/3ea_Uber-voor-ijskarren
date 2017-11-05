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
  user: string = 'customer';
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
  onRegister(form: NgForm){
    let customer= form.value;//JSON.parse(form.value);
    customer['userRoleType'] = 0;
    this.userProvider.createAccount(customer).subscribe(
      data => {
        console.log(data)
      },
      error2 => {
        console.log(error2);
      }
    );
    //console.log(this.customer);
  }
  register(){
    this.userProvider.getAllUsers().subscribe(
      data => {
        console.log(data)
      },
      error2 => {
        console.log(error2);
      }
    );
  }

}
