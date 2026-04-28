# Currency Converter

This is my submission for the full-stack assessment.

I kept the solution intentionally simple and focused on the requested scope:
- ASP.NET Core Web API
- React + TypeScript frontend
- Frankfurter API integration
- JWT authentication with simple role-based access
- caching, retry, circuit breaker, and rate limiting
- basic logging and test scaffolding

I tried to balance clean structure with not overengineering the task.

## What the app does

### Backend
The API provides:
- `POST /api/v1/auth/login`
- `GET /api/v1/currency/latest?base=EUR`
- `POST /api/v1/currency/convert`
- `GET /api/v1/currency/history?base=EUR&startDate=2024-01-01&endDate=2024-01-10&page=1&pageSize=5`

Business rule included in the task:
- `TRY`, `PLN`, `THB`, and `MXN` are rejected with `400 Bad Request`

### Frontend
The React app includes 3 simple screens:
- currency conversion
- latest exchange rates
- historical exchange rates with pagination

## Demo users
To keep the task lightweight, I used simple demo users instead of a full identity provider.

- viewer / `Viewer123!`
- admin / `Admin123!`

Permissions:
- `viewer` can access latest rates and conversion
- `admin` can access latest rates, conversion, and history

## Project structure

```text
backend/
frontend/
README.md
```

## How to run

### Backend

```bash
cd backend/src/CurrencyConverter.Api
dotnet restore
dotnet run
```

By default the API should run locally and expose Swagger as well.

### Frontend

```bash
cd frontend
npm install
npm run dev
```

If needed, set the API base URL:

```bash
VITE_API_BASE_URL=https://localhost:5001/api/v1
```

## Main implementation notes

### Backend choices
- I used a small layered structure to keep responsibilities separated without making the project too heavy.
- Frankfurter is wrapped behind a provider interface so another provider can be added later.
- `IMemoryCache` is used to reduce unnecessary calls to the external API.
- `HttpClient` uses retry and circuit breaker policies for resilience.
- JWT auth and role-based authorization are included because they were part of the requirements.
- API versioning is done through the route: `/api/v1/...`

### Frontend choices
- I used React with TypeScript and kept the UI intentionally simple.
- The pages focus on the required flows rather than visual polish.
- React Query is used for fetching and caching API responses.

## Logging and observability
The API logs basic request details such as:
- method
- path
- status code
- response time
- client IP
- client ID from JWT when available
- correlation ID

This is intentionally lightweight, but it gives enough visibility for the assessment scope.

## Testing
I added:
- unit test project for service-level logic
- integration test project for API behavior
- basic frontend test example for a key page

I focused on covering the main business paths first.

## AI usage
I used AI tools during development mainly for speed and review, not as a replacement for decision making.

I used AI for:
- brainstorming the initial project structure
- speeding up repetitive scaffolding
- suggesting test cases and README wording
- reviewing some implementation details and edge cases

What I reviewed manually:
- API shape and route design
- unsupported currency rule
- role split between viewer and admin
- how much architecture was actually reasonable for a short assessment

What I did **not** just accept blindly:
- overly abstract architecture
- unnecessary patterns that would make the task bigger without adding value
- generated code that did not match the assessment requirements

## Assumptions and trade-offs
- I used in-memory cache instead of Redis to keep the solution simple.
- I used demo login instead of integrating with a real identity provider.
- The logging is practical and enough for the task, but could be expanded later.
- The UI is intentionally minimal and focuses more on correctness than design.

## Future improvements
If this were extended further, I would probably add:
- Redis distributed caching
- OpenTelemetry traces/metrics
- Docker setup
- stronger integration test coverage around upstream failures
- CI pipeline with coverage reporting
- improved UI polish and form validation

## Final note
I aimed for a solution that is readable, practical, and easy to run.
If I had more time, I would mainly expand test coverage and polish the frontend UX a bit more.


## Local run notes

The frontend runs on `http://localhost:5173` and the backend is configured to run on `http://localhost:5080` (and `https://localhost:7080`).

If the frontend shows a generic `Conversion failed` message, the first thing to check is that the API is actually running on `http://localhost:5080`. This project includes a `launchSettings.json` file so `dotnet run` should bind to that port in development.

You can also verify the API quickly by opening:
- `http://localhost:5080/health`
- `http://localhost:5080/swagger`

The frontend uses a simple demo login automatically, so you do not need to log in manually while testing the UI.
