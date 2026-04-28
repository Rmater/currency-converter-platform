import { NavLink, Route, Routes } from 'react-router-dom'
import ConvertPage from './pages/ConvertPage'
import LatestRatesPage from './pages/LatestRatesPage'
import HistoricalRatesPage from './pages/HistoricalRatesPage'

export default function App() {
  return (
    <div className="shell">
      <header>
        <h1>Currency Converter</h1>
        <nav>
          <NavLink to="/">Convert</NavLink>
          <NavLink to="/latest">Latest Rates</NavLink>
          <NavLink to="/history">History</NavLink>
        </nav>
      </header>
      <main>
        <Routes>
          <Route path="/" element={<ConvertPage />} />
          <Route path="/latest" element={<LatestRatesPage />} />
          <Route path="/history" element={<HistoricalRatesPage />} />
        </Routes>
      </main>
    </div>
  )
}
