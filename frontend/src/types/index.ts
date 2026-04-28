export interface LatestRatesResponse {
  base: string
  date: string
  rates: Record<string, number>
}

export interface ConversionRequest {
  amount: number
  from: string
  to: string
}

export interface ConversionResponse {
  amount: number
  from: string
  to: string
  rate: number
  convertedAmount: number
  date: string
}

export interface HistoricalRateItem {
  date: string
  rates: Record<string, number>
}

export interface HistoricalRatesResponse {
  base: string
  page: number
  pageSize: number
  totalCount: number
  items: HistoricalRateItem[]
}
