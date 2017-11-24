import { Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';
import { Storage } from '@ionic/storage'
import 'rxjs/add/operator/map';
import {FlavourUpdate, FlavourUpdateJson} from "../Models/flavour.model";

@Injectable()
export class DriverProvider {
  ip: string = 'http://172.16.235.156:80/api/driver/';
  //ip: string = 'http://192.168.0.172:80/api/driver/';
  private headers: Headers = new  Headers({
    'Accept': 'application/json'
  });
  constructor(public http: Http, private storage: Storage) {
  }
  postFlavours: FlavourUpdateJson = new FlavourUpdateJson([new FlavourUpdate("", 0)]);

  updateFlavours(flavours: FlavourUpdate[], currentDriver: string){
    console.log(flavours);
      return this.http.post(this.ip + currentDriver, flavours, {headers: this.headers })
        .map((response: Response) => {
          return response.json();
        });
  }
  getCurrentUser(){
    return this.storage.get("currentUser").then();
  }

}