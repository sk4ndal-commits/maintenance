const BASE_URL = import.meta.env.VITE_API_BASE_URL ?? ''

export const apiClient = {
  post: async <T>(path: string, body: any): Promise<T> => {
    const res = await fetch(`${BASE_URL}${path}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(body),
    })
    if (!res.ok) throw new Error(`API error ${res.status}`)
    return res.json()
  }
}
