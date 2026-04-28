import { useEffect } from 'react'
import { currencyApi } from '../api/currencyApi'

export function useDemoLogin(role: 'viewer' | 'admin') {
  useEffect(() => {
    currencyApi.loginAs(role).catch(() => undefined)
  }, [role])
}
