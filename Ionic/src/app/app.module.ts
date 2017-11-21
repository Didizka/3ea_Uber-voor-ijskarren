import { ListDriversPageModule } from './../pages/list-drivers/list-drivers.module';
import { BrowserModule } from '@angular/platform-browser';
import { ErrorHandler, NgModule } from '@angular/core';
import { IonicApp, IonicErrorHandler, IonicModule } from 'ionic-angular';
import { SplashScreen } from '@ionic-native/splash-screen';
import { StatusBar } from '@ionic-native/status-bar';

import { MyApp } from './app.component';
import { UserProvider } from '../providers/user';
import {HttpModule} from "@angular/http";
import {ListDriversPage} from "../pages/list-drivers/list-drivers";
import {OrderProvider} from '../providers/order-provider';
import {CommonModule} from "@angular/common";
import {Toast} from "@ionic-native/toast";

@NgModule({
  declarations: [
    MyApp
  ],
  imports: [
    BrowserModule,
    HttpModule,
    CommonModule,
    IonicModule.forRoot(MyApp),
    ListDriversPageModule
  ],
  bootstrap: [IonicApp],
  entryComponents: [
    MyApp
  ],
  providers: [
    StatusBar,
    SplashScreen,
    Toast,
    {provide: ErrorHandler, useClass: IonicErrorHandler},
    UserProvider,
    OrderProvider
  ]
})
export class AppModule {}
