export interface Customer{
  firstName: string,
  lastName: string,
  phoneNumber: string,
  address: Address,
  email: string,
  password: string,
  userRoleType: number

}
export interface Address{
  streetName: string,
  streetNumber: string,
  zipCode: number
}
