import { Component } from '@angular/core';
import {AlertController, IonicPage, Loading, LoadingController, NavController, NavParams} from 'ionic-angular';
import {NgForm} from "@angular/forms";
import {UserProvider} from "../../providers/user";

@IonicPage()
@Component({
  selector: 'page-signin',
  templateUrl: 'signin.html',
})
export class SigninPage {


  constructor(private nav: NavController,
              private alertCtrl: AlertController,
              private loadingCtrl: LoadingController,
              private userProvider: UserProvider) { }

  onCreateAccount(){
    this.nav.push('SignupPage');
  }

  onLogin(form:NgForm){
    this.userProvider.login(form.value.email, form.value).subscribe(
      data => {
        console.log(data)
      },
      err => {
        console.log(err);
      }
    );
  }
}
