# TaskFlow

A simple task management app with an ASP.NET Core Web API backend and an Angular front end.

- **TaskFlow.WebAPI** – .NET 10 Web API (in-memory task store, seeded with sample data)
- **TaskFlow.ClientApp** – Angular 22 + Angular Material single-page app

## Prerequisites

Install the following before running:

| Tool | Version | Notes |
|------|---------|-------|
| Git | latest | to clone the repo |
| .NET SDK | 10.0 | backend + tests |
| Node.js | 22.x LTS or later | front end (includes npm) |

Verify:

```powershell
git --version
dotnet --version
node --version
npm --version
```

## Getting the code

```powershell
git clone https://github.com/kejazahmed/TaskFlow.git
cd TaskFlow
```

## Running the backend (WebAPI)

```powershell
cd TaskFlow.WebAPI/TaskFlow.WebAPI
dotnet run
```

The API starts at **http://localhost:5080**. All routes are under `/api/tasks`:
`GET /api/tasks`, `GET /api/tasks/{id}`, `POST /api/tasks`, `PUT /api/tasks/{id}`, `DELETE /api/tasks/{id}`.

Data is stored in memory, so it resets each time the API restarts.

To restore/build only:

```powershell
dotnet build
```

## Running the front end (ClientApp)

Open a **second terminal** and keep the backend running.

```powershell
cd TaskFlow.ClientApp
npm install
npm start        # runs "ng serve"
```

Open **http://localhost:4200** in your browser. The app talks to the API at
`http://localhost:5080`, so the backend must be running first.

Other client scripts:

```powershell
npm run build    # production build into dist/
npm test         # unit tests (ng test)
```

## Running the tests

```powershell
cd TaskFlow.WebAPI
dotnet test
```

## Project structure

```
TaskFlow/
├─ TaskFlow.WebAPI/
│  ├─ TaskFlow.WebAPI/      # API project (Controllers, Services, Domain, Models, Infrastructure)
│  └─ TaskFlow.Tests/       # xUnit tests
└─ TaskFlow.ClientApp/      # Angular SPA
```

## Notes

- No database required – tasks are kept in memory.
- CORS is configured to allow only `http://localhost:4200`.
- If the UI shows "Could not load tasks", check that the API is running on port 5080.
