import { useMutation, useQuery } from '@tanstack/react-query'
import { currencyApi } from '../api/currencyApi'
import { ConversionRequest } from '../types'

export const useLatestRates = (base: string) => useQuery({
  queryKey: ['latest-rates', base],
  queryFn: () => currencyApi.getLatestRates(base),
})

export const useConvertCurrency = () => useMutation({
  mutationFn: (payload: ConversionRequest) => currencyApi.convert(payload),
})

export const useHistoricalRates = (base: string, startDate: string, endDate: string, page: number, pageSize: number) => useQuery({
  queryKey: ['history-rates', base, startDate, endDate, page, pageSize],
  queryFn: () => currencyApi.getHistory(base, startDate, endDate, page, pageSize),
  enabled: Boolean(base && startDate && endDate),
})
