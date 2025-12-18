export interface LoginForm {
    email: string
    password: string
}

export interface LoginResponse {
    token: string,
    expiresAt: string
}

export interface RegisterForm {
    username: string
    email: string
    password: string
    password_confirm: string
}

export interface RegisterRequest {
    username: string,
    email: string,
    password: string
}

export interface RegisterResponse {
    id: string,
    username: string,
    email: string,
    createdAt: string,
    updatedAt: string
}