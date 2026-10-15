# TaskFlow

A small task manager with an ASP.NET Core Web API backend and an Angular front end.

- TaskFlow.WebAPI: a .NET 10 Web API with an in-memory task store, seeded with a few sample tasks.
- TaskFlow.ClientApp: an Angular 22 + Angular Material single-page app.

## Prerequisites

You'll need these installed:

| Tool | Version | Used for |
|------|---------|----------|
| Git | latest | cloning the repo |
| .NET SDK | 10.0 | the API and its tests |
| Node.js | 22.x LTS or later | the front end (npm comes with it) |

Check the versions:

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

The API listens on http://localhost:5080, and everything is under /api/tasks:
GET /api/tasks, GET /api/tasks/{id}, POST /api/tasks, PUT /api/tasks/{id}, and
DELETE /api/tasks/{id}.

Tasks live in memory, so they reset every time the API restarts. Logs go to a rolling
file, covered in Logging below.

To compile without running:

```powershell
dotnet build
```

## Running the front end (ClientApp)

Leave the API running and open a second terminal.

```powershell
cd TaskFlow.ClientApp
npm install
npm start        # runs "ng serve"
```

Then open http://localhost:4200 in your browser. The app calls the API at
http://localhost:5080, so the backend has to be running first.

Other scripts:

```powershell
npm run build    # production build into dist/
npm test         # unit tests (ng test)
```

## Running the tests

```powershell
cd TaskFlow.WebAPI
dotnet test
```

## Logging

The API logs with log4net, configured in
TaskFlow.WebAPI/TaskFlow.WebAPI/log4net.config.

Logs are written to logs/taskflow.log inside the build output folder, for example
TaskFlow.WebAPI/TaskFlow.WebAPI/bin/Debug/net10.0/logs/taskflow.log. The file rolls
over at 10 MB and keeps the last 5 files.

To change the log level or where logs go, edit log4net.config.

## Project structure

```
TaskFlow/
├─ TaskFlow.WebAPI/
│  ├─ TaskFlow.WebAPI/           # API project (Controllers, Services, Domain, Models, Infrastructure, Middleware)
│  │  └─ log4net.config          # log4net settings
│  └─ TaskFlow.Tests/            # xUnit tests
└─ TaskFlow.ClientApp/           # Angular SPA
```

## Notes

- Tasks are kept in memory, so no database is needed.
- CORS only allows http://localhost:4200.
- If the UI says "Could not load tasks", make sure the API is running on port 5080.
