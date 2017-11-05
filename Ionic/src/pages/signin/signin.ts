import { Component } from '@angular/core';
import {AlertController, IonicPage, Loading, LoadingController, NavController, NavParams} from 'ionic-angular';

@IonicPage()
@Component({
  selector: 'page-signin',
  templateUrl: 'signin.html',
})
export class SigninPage {


  constructor(private nav: NavController,
              private alertCtrl: AlertController,
              private loadingCtrl: LoadingController) { }

  onCreateAccount(){
    this.nav.push('SignupPage');
  }

}
