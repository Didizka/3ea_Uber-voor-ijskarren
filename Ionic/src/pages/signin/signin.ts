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
        this.presentConfirm();
      }
    );
  }

  // Show alert if email is not correct
  presentConfirm() {
    let alert = this.alertCtrl.create({
      title: 'Account werd niet gevonden',
      message: 'Een nieuwe account registreren?',
      buttons: [
        {
          text: 'Cancel',
          role: 'cancel',
          handler: () => {
            console.log('Cancel clicked');
          }
        },
        {
          text: 'Ja',
          handler: () => {
            this.nav.push('SignupPage');
          }
        }
      ]
    });
    alert.present();
  }
}
