export class Driver{
  firstName: string;
  lastName: string;
  driverID: number;
  phoneNumber: string;
  email: string;
  location: Location;
  distance: number;
  duration: number;
  locationAddress: string;
}

  
export class Location{
  latitude: any;
  longitude: any;
}
export interface FlavourPrice{
  name: string,
  price: number
}
export interface DriverFlavour{
  firstName: string,
  lastName: string,
  driverID: number,
  email: string,
  location: Location,
  flavours: FlavourPrice[]

}
