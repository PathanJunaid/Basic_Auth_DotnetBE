# Basic Auth Web API

## Overview
This is a .NET Web API project implementing basic authentication.

## Project Structure
```
Basic_Auth/
├── Basic_Auth/                # Main project folder
│   ├── Controllers/           # API Controllers
│   ├── Models/                # Data models
│   ├── Services/              # Business logic
│   ├── appsettings.json       # Configuration file
│   ├── Program.cs             # Entry point of the application
│   ├── bin/                   # Ignored - Build files
│   ├── obj/                   # Ignored - Temporary files
├── basic_Auth.sln             # Solution file
├── .gitignore                 # Git ignore file
└── README.md                  # Documentation
```

## Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/en-us/download)
- Visual Studio / VS Code
- SQL Server 

## Setup & Installation
1. **Clone the repository**
   ```sh
   git clone https://github.com/yourusername/Basic_Auth.git
   cd Basic_Auth
   ```

2. **Restore dependencies**
   ```sh
   dotnet restore
   ```

3. **Build the project**
   ```sh
   dotnet build
   ```

4. **Run the application**
   ```sh
   dotnet run
   ```

## API Endpoints
| Method | Endpoint         | Description         |
|--------|----------------|--------------------|
| POST   | `/api/login`   | Authenticates user |
| GET    | `/api/data`    | Protected endpoint |

## Environment Variables
Create a `.env` file and set the following:
```
"ConnectionStrings": {
    "DefaultConnection": "Server=Server_Name_;Database=DBName;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "ValidIssuer": "https://localhost:7237",
    "ValidAudience": "https://localhost:7237",
    "JWTSECRET": "JWTSECRET HmacSha256"

  }
```

## Deployment
1. **Publish the project**
   ```sh
   dotnet publish -c Release -o out
   ```
2. **Deploy to a server (IIS, Docker, etc.)**

## Contributing
- Fork the repository
- Create a new branch
- Commit your changes
- Create a pull request

## License
MIT License

