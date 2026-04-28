import { useState } from 'react'
import CurrencySelect from '../components/CurrencySelect'
import ResultCard from '../components/ResultCard'
import { useHistoricalRates } from '../hooks/useCurrency'
import { useDemoLogin } from '../hooks/useDemoLogin'

export default function HistoricalRatesPage() {
  const [base, setBase] = useState('EUR')
  const [startDate, setStartDate] = useState('2024-01-01')
  const [endDate, setEndDate] = useState('2024-01-10')
  const [page, setPage] = useState(1)
  const pageSize = 5
  const { data, isLoading, isError } = useHistoricalRates(base, startDate, endDate, page, pageSize)
  useDemoLogin('admin')

  return (
    <ResultCard title="Historical rates">
      <div className="grid grid-4">
        <CurrencySelect value={base} onChange={(e) => setBase(e.target.value)} />
        <input type="date" value={startDate} onChange={(e) => setStartDate(e.target.value)} />
        <input type="date" value={endDate} onChange={(e) => setEndDate(e.target.value)} />
        <button onClick={() => setPage(1)}>Refresh</button>
      </div>
      {isLoading && <p>Loading...</p>}
      {isError && <p className="error">Unable to load history.</p>}
      {data && (
        <>
          <table>
            <thead><tr><th>Date</th><th>Rates</th></tr></thead>
            <tbody>
              {data.items.map((item) => (
                <tr key={item.date}>
                  <td>{item.date}</td>
                  <td>{Object.entries(item.rates).map(([code, value]) => `${code}: ${value}`).join(', ')}</td>
                </tr>
              ))}
            </tbody>
          </table>
          <div className="pagination">
            <button disabled={page === 1} onClick={() => setPage((current) => current - 1)}>Previous</button>
            <span>Page {page}</span>
            <button disabled={page * pageSize >= data.totalCount} onClick={() => setPage((current) => current + 1)}>Next</button>
          </div>
        </>
      )}
    </ResultCard>
  )
}
