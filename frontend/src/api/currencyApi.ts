import { apiClient } from './client'
import { ConversionRequest, ConversionResponse, HistoricalRatesResponse, LatestRatesResponse } from '../types'

export const currencyApi = {
  login: async (username: string, password: string) => {
    const { data } = await apiClient.post('/auth/login', { username, password })
    localStorage.setItem('accessToken', data.accessToken)
    return data
  },
  loginAs: async (role: 'viewer' | 'admin') => {
    const credentials = role === 'admin'
      ? { username: 'admin', password: 'Admin123!' }
      : { username: 'viewer', password: 'Viewer123!' }

    const { data } = await apiClient.post('/auth/login', credentials)
    localStorage.setItem('accessToken', data.accessToken)
    return data
  },
  getLatestRates: async (base: string) => {
    const { data } = await apiClient.get<LatestRatesResponse>(`/currency/latest?base=${base}`)
    return data
  },
  convert: async (payload: ConversionRequest) => {
    const { data } = await apiClient.post<ConversionResponse>('/currency/convert', payload)
    return data
  },
  getHistory: async (base: string, startDate: string, endDate: string, page: number, pageSize: number) => {
    const { data } = await apiClient.get<HistoricalRatesResponse>(`/currency/history?base=${base}&startDate=${startDate}&endDate=${endDate}&page=${page}&pageSize=${pageSize}`)
    return data
  },
}
