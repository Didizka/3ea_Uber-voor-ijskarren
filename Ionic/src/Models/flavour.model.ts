
export class Flavour{
  constructor(public name: string, public amount: number){}
}
export class FlavourUpdate{
  constructor(public name: string, public price: number){}
}
export class FlavourUpdateJson{
  constructor(public Flavours: FlavourUpdate[]){}
}
