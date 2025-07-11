2 service: AuthService(docker http://localhost:5001, local https://localhost:5001), TranslaterService (docker http://localhost:5000, local https://localhost:5000)

Authentication JWT-based using ASP.NET Core Identity

Authorization policy:Role.Admin (email:admin@mail.ru, password:admin)

Frontened: React (docker and local http://localhost:3000)

docker-compose.yml is configured for lunch (docker compose up --build)

If you will check local that  baseURL in:
TranslaterReact/src/api.js (baseURL: 'https://localhost:5000')
TranslaterReact/src/authApi.js (baseURL: 'https://localhost:5001')
