import type { LoginForm } from "@/types/auth";

interface LoginResponse {
    token: string,
    expiresAt: string
}

const API_BASE_URL = import.meta.env.VITE_API_URL

export async function loginUser(payload: LoginForm): Promise<LoginResponse> {
    const response = await fetch(`${API_BASE_URL}/Auth/login`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(payload)
    })

    if (!response.ok) {
        const errorData = await response.json().catch(() => ({}))
        throw new Error(errorData.message || 'Erro ao realizar o Login.')
    }

    return await response.json() as LoginResponse
}