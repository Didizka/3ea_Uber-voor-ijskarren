export interface Driver{
  firstName: string,
  lastName: string,
  userID: number,
  phoneNumber: string,
  email: string,
  location: Location

}
export interface Location{
  latitude: any,
  longitude: any
}
