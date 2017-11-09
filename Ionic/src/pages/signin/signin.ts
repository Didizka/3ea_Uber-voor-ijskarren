import { Component } from '@angular/core';
import {AlertController, IonicPage, Loading, LoadingController, NavController, NavParams} from 'ionic-angular';
import {NgForm} from "@angular/forms";
import {UserProvider} from "../../providers/user";
import {ListDriversPage} from "../list-drivers/list-drivers";

@IonicPage()
@Component({
  selector: 'page-signin',
  templateUrl: 'signin.html',
})
export class SigninPage {


  constructor(private navCtrl: NavController,
              private alertCtrl: AlertController,
              private loadingCtrl: LoadingController,
              private userProvider: UserProvider) { }

  onCreateAccount(){
    this.navCtrl.push('SignupPage');
  }

  onLogin(form:NgForm){
    const loading = this.loadingCtrl.create({
      content: 'Please wait...'
    });
    loading.present();
    this.userProvider.login(form.value.email, form.value).subscribe(
      data => {
        console.log(data);
        if(data == true){
          loading.dismiss();
          this.navCtrl.setRoot(ListDriversPage);
        } else if(data == false) {
          loading.dismiss();
          this.createErrorMsg('Password incorrect!');
        }else{
          console.log('no account');
          loading.dismiss();
          this.presentConfirm();
        }
      },
      err => {
        console.log(err);
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
            this.navCtrl.push('SignupPage');
          }
        }
      ]
      }
    );
    alert.present();
  }

  createErrorMsg(msg: string){
    const alert = this.alertCtrl.create({
      title: 'Signup failed!',
      message: msg,
      buttons: ['Ok']
    });
    alert.present();
  }
}
