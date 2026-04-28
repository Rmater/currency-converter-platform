const currencies = ['EUR', 'USD', 'GBP', 'SAR', 'AED', 'EGP', 'JPY', 'CAD']

export default function CurrencySelect(props: React.SelectHTMLAttributes<HTMLSelectElement>) {
  return (
    <select {...props}>
      {currencies.map((currency) => (
        <option key={currency} value={currency}>{currency}</option>
      ))}
    </select>
  )
}
