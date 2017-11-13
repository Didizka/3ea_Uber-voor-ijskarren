import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ListDriversPage } from './list-drivers';
import { Geolocation } from '@ionic-native/geolocation';
import { AgmCoreModule } from  '@agm/core';

@NgModule({
  declarations: [
    ListDriversPage,
  ],
  imports: [
    IonicPageModule.forChild(ListDriversPage),
    AgmCoreModule.forRoot({
      apiKey: 'AIzaSyAIsmJrEeHefwTuNe4q94adKczIaEcA2AI'
    }),
  ],
  providers: [ Geolocation ],

  // needed to bind lat/lng attributets of agm-map
  schemas:  [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class ListDriversPageModule {}
