export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  userId: string;
  email: string;
  role: string;
}

export interface RegisterRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  phoneNumber: string;
}

export interface AuthState {
  isAuthenticated: boolean;
  user: {
    userId?: string;
    email?: string;
    role?: string;
  } | null;
  token: string | null;
}
