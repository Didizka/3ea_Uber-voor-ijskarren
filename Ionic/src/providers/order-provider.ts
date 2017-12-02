import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';
import 'rxjs/add/operator/map';

@Injectable()
export class OrderProvider {

  //ip: string = 'http://localhost:9000/api/order/';
  //ip: string = 'http://172.16.212.115:80/api/order/';
  ip: string = 'http://192.168.0.172:80/api/order/';

  constructor(public http: Http) {
    //console.log('Hello ProvidersOrderProvider Provider');
  }
  private headers: Headers = new  Headers({
    'Accept': 'application/json'
  });
  getFlavours(){
    return this.http.get(this.ip)
      .map((response: Response) => {
      return response.json();
    });
  }
  placeOrder(shoppingCart: JSON, currentUser: string){
    console.log(shoppingCart);
    return this.http.post(this.ip + currentUser, shoppingCart, {headers: this.headers})
      .map((response: Response) => {
      return response.json();
    });
  }
}
