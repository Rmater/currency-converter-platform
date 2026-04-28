import { useState } from 'react'
import CurrencySelect from '../components/CurrencySelect'
import ResultCard from '../components/ResultCard'
import { useLatestRates } from '../hooks/useCurrency'
import { useDemoLogin } from '../hooks/useDemoLogin'

export default function LatestRatesPage() {
  const [base, setBase] = useState('EUR')
  const { data, isLoading, isError } = useLatestRates(base)
  useDemoLogin('viewer')

  return (
    <ResultCard title="Latest rates">
      <CurrencySelect value={base} onChange={(e) => setBase(e.target.value)} />
      {isLoading && <p>Loading...</p>}
      {isError && <p className="error">Unable to load rates.</p>}
      {data && (
        <table>
          <thead><tr><th>Currency</th><th>Rate</th></tr></thead>
          <tbody>
            {Object.entries(data.rates).map(([currency, rate]) => (
              <tr key={currency}><td>{currency}</td><td>{rate}</td></tr>
            ))}
          </tbody>
        </table>
      )}
    </ResultCard>
  )
}
