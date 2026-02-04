export const API_BASE_URL = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:7214'

export type User = {
  id: string
  name: string
  age: number
}

export type Category = {
  id: string
  description: string
  purpose: number
}

export type UserSummaryItem = {
  id: string
  name: string
  totalIncome: number
  totalExpense: number
  balance: number
}

export type SummaryTotals = {
  totalIncome: number
  totalExpense: number
  balance: number
}

export type UserSummaryResponse = {
  items: UserSummaryItem[]
  totals: SummaryTotals
}

export type FinancialRecord = {
  id: string
  userId: string
  categoryId: string
  type: number
  amount: number
  description: string
  categoryDescription: string
  userName: string
  createdAt: string
}

async function http<T>(path: string, init?: RequestInit): Promise<T> {
  const res = await fetch(`${API_BASE_URL}${path}`, {
    headers: { 'Content-Type': 'application/json', ...(init?.headers ?? {}) },
    ...init,
  })

  if (!res.ok) {
    const txt = await res.text().catch(() => '')
    throw new Error(`${res.status} ${res.statusText}${txt ? ` - ${txt}` : ''}`)
  }

  if (res.status === 204) {
    return undefined as unknown as T
  }

  const ct = res.headers.get('content-type') ?? ''
  if (ct.includes('application/json')) {
    return (await res.json()) as T
  }
  return (await res.text()) as unknown as T
}

export const api = {
  users: {
    list: () => http<User[]>('/api/users'),
    create: (name: string, age: number) => http<User>('/api/users', { method: 'POST', body: JSON.stringify({ name, age }) }),
    update: (id: string, name: string, age: number) => http<User>(`/api/users/${id}`, { method: 'PUT', body: JSON.stringify({ name, age }) }),
    remove: (id: string) => http<void>(`/api/users/${id}`, { method: 'DELETE' }),
    summary: () => http<UserSummaryResponse>('/api/users/summary'),
  },
  categories: {
    list: () => http<Category[]>('/api/categories'),
    create: (description: string, purpose: number) => http<Category>('/api/categories', { method: 'POST', body: JSON.stringify({ description, purpose }) }),
  },
  records: {
    listByUser: (userId: string) => http<FinancialRecord[]>(`/api/users/${userId}/financial-records`),
    createForUser: (userId: string, payload: { categoryId: string; type: number; amount: number; description: string }) =>
      http<void>(`/api/users/${userId}/financial-records`, { method: 'POST', body: JSON.stringify(payload) }),
  },
}
