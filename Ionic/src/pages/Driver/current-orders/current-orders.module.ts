import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { CurrentOrdersPage } from './current-orders';

@NgModule({
  declarations: [
    CurrentOrdersPage,
  ],
  imports: [
    IonicPageModule.forChild(CurrentOrdersPage),
  ],
})
export class CurrentOrdersPageModule {}
