import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ListDriversPage } from './list-drivers';
import { Geolocation } from '@ionic-native/geolocation';

@NgModule({
  declarations: [
    ListDriversPage,
  ],
  imports: [
    IonicPageModule.forChild(ListDriversPage)
  ],
  providers: [ Geolocation ],

  // needed to bind lat/lng attributets of agm-map
  schemas:  [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class ListDriversPageModule {}
