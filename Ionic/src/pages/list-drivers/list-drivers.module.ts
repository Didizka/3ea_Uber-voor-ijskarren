import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ListDriversPage } from './list-drivers';

@NgModule({
  declarations: [
    ListDriversPage,
  ],
  imports: [
    IonicPageModule.forChild(ListDriversPage),
  ],
})
export class ListDriversPageModule {}
