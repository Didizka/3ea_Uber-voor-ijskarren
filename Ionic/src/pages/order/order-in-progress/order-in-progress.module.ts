import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { OrderInProgressPage } from './order-in-progress';

@NgModule({
  declarations: [
    OrderInProgressPage,
  ],
  imports: [
    IonicPageModule.forChild(OrderInProgressPage),
  ],
})
export class OrderInProgressPageModule {}
