export class UserClient {
    constructor(
  public name: string,
  public email: string,
  public password: string,
  public phone: string,
  public userName: string,
  public location: string,
  public age: number|null,
  public sex: string,
  public photo: File|null,
  public roleID: number
    )
    {}
}
