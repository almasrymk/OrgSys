/** Mirrors Domain.Entities.Permission (Id, Key, Name) as returned inside UserDto.Permissions. */
export interface Permission {
  id: number;
  key: string;
  name: string;
}

/** Mirrors Application.DTOs.UserDto, the fields the login response actually populates. */
export interface AuthUser {
  id: number;
  name: string;
  userName: string;
  roleId: number;
  roleName: string | null;
  branchId: number | null;
  branchName: string | null;
  imgPath: string | null;
  permissions: Permission[];
}

/** Mirrors API.Authentication.LoginResponseDto. */
export interface LoginResponse {
  user: AuthUser;
  token: string;
}

export interface LoginRequest {
  userName: string;
  password: string;
}
