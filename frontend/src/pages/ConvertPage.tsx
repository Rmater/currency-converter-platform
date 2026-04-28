import { FormEvent, useState } from 'react'
import CurrencySelect from '../components/CurrencySelect'
import ResultCard from '../components/ResultCard'
import { useConvertCurrency } from '../hooks/useCurrency'
import { useDemoLogin } from '../hooks/useDemoLogin'

export default function ConvertPage() {
  const [amount, setAmount] = useState(100)
  const [from, setFrom] = useState('EUR')
  const [to, setTo] = useState('USD')
  const mutation = useConvertCurrency()
  useDemoLogin('viewer')

  const submit = (event: FormEvent) => {
    event.preventDefault()
    mutation.mutate({ amount, from, to })
  }

  return (
    <ResultCard title="Convert">
      <form onSubmit={submit} className="grid">
        <input type="number" min="0.01" step="0.01" value={amount} onChange={(e) => setAmount(Number(e.target.value))} />
        <CurrencySelect value={from} onChange={(e) => setFrom(e.target.value)} />
        <CurrencySelect value={to} onChange={(e) => setTo(e.target.value)} />
        <button type="submit" disabled={mutation.isPending}>Convert</button>
      </form>

      {mutation.isError && <p className="error">{(mutation.error as any)?.response?.data?.message ?? 'Conversion failed.'}</p>}
      {mutation.data && (
        <div className="result">
          <p>{mutation.data.amount} {mutation.data.from} → {mutation.data.convertedAmount.toFixed(2)} {mutation.data.to}</p>
          <p>Rate: {mutation.data.rate.toFixed(4)}</p>
          <p>Date: {mutation.data.date}</p>
        </div>
      )}
    </ResultCard>
  )
}
