export class UserUpdateDetails {
  constructor(
    public userID: number,
    public name: string,
    public email: string,
    public password: string,
    public phone: string,
    public userName: string,
    public location: string,
    public age: number,
    public sex: string,
    public photo?: File|string|null,
    public previewPhoto?: string | ArrayBuffer | null,
    public roleID?: number
      ){}
}
