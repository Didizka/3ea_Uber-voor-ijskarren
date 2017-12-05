<<<<<<< HEAD
export class Driver{
  firstName: string;
  lastName: string;
  userID: number;
  phoneNumber: string;
  email: string;
  location: Location;
  distance: number;
  duration: number;
  locationAddress: string;
=======
export interface Driver{
  firstName: string,
  lastName: string,
  driverID: number,
  phoneNumber: string,
  email: string,
  location: Location

>>>>>>> cce0448dfdd5d1473b2b8d7ce0a75ba8c7f9edbe
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
