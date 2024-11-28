export const AUTH_TOKEN_LOCAL_STORAGE = 'proj183_authtoken';
export const REFRESH_TOKEN_LOCAL_STORAGE = 'proj183_refreshtoken';
export const USER_EMAIL = 'proj183_useremail';

export interface User {
  email: string;
  password: string;
}

export interface Authentication {
  tokenType: string;
  accessToken: string;
  expiresIn: number;
  refreshToken: string;
}

export interface UserInfo {
  email: string;
  isEmailConfirmed: boolean;
}

export interface CourtViewModel {
  courtId: number;
  title: string;
  description: string;
  userEmail: string;
  street: string;
  number: string;
  city: string;
  canton: string;
  plz: number;
  userId: number;
  addressId: number;
}
