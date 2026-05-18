# 🏎️ Mario Kart Race Simulator — Full Stack

<img src="./mario-kart-frontend/public/chars/header.gif" alt="Mario Kart" width="220">

Simulador de corrida estilo Mario Kart com:
- **Backend**: ASP.NET Core 8 + MySQL + JWT + Swagger
- **Frontend**: React + Framer Motion (animações em tempo real)
- **Banco**: MySQL com Entity Framework Core (Code First)

Baseado no projeto original em Node.js, recriado com back-end completo e front-end animado.

---

## 👥 Personagens

<table>
  <tr>
    <td align="center">
      <img src="./mario-kart-frontend/public/chars/mario.gif" width="60" alt="Mario">
      <br><b>Mario</b>
      <br>💨 Vel: 4 &nbsp; 🎯 Man: 3 &nbsp; 💪 Pow: 3
    </td>
    <td align="center">
      <img src="./mario-kart-frontend/public/chars/luigi.gif" width="60" alt="Luigi">
      <br><b>Luigi</b>
      <br>💨 Vel: 3 &nbsp; 🎯 Man: 4 &nbsp; 💪 Pow: 4
    </td>
    <td align="center">
      <img src="./mario-kart-frontend/public/chars/yoshi.gif" width="60" alt="Yoshi">
      <br><b>Yoshi</b>
      <br>💨 Vel: 2 &nbsp; 🎯 Man: 4 &nbsp; 💪 Pow: 3
    </td>
    <td align="center">
      <img src="./mario-kart-frontend/public/chars/bowser.gif" width="60" alt="Bowser">
      <br><b>Bowser</b>
      <br>💨 Vel: 5 &nbsp; 🎯 Man: 2 &nbsp; 💪 Pow: 5
    </td>
  </tr>
  <tr>
    <td align="center">
      <img src="./mario-kart-frontend/public/chars/peach.gif" width="60" alt="Peach">
      <br><b>Peach</b>
      <br>💨 Vel: 3 &nbsp; 🎯 Man: 4 &nbsp; 💪 Pow: 2
    </td>
    <td align="center">
      <img src="./mario-kart-frontend/public/chars/dk.gif" width="60" alt="Donkey Kong">
      <br><b>Donkey Kong</b>
      <br>💨 Vel: 2 &nbsp; 🎯 Man: 2 &nbsp; 💪 Pow: 5
    </td>
    <td align="center">
      <img src="./mario-kart-frontend/public/chars/toad.gif" width="60" alt="Toad">
      <br><b>Toad</b>
      <br>💨 Vel: 5 &nbsp; 🎯 Man: 4 &nbsp; 💪 Pow: 1
    </td>
    <td></td>
  </tr>
</table>

---

## 🕹️ Regras & Mecânicas

A lógica do jogo é fiel ao projeto Node.js original:

| Bloco | Atributo usado | Resultado |
|---|---|---|
| **RETA** | Velocidade | Dado + Vel → quem pontua mais ganha 1 pt |
| **CURVA** | Manobrabilidade | Dado + Man → quem pontua mais ganha 1 pt |
| **CONFRONTO** | Poder | Dado + Pow → quem perde, perde 1 pt (mín 0) |

- A cada rodada, um bloco é sorteado aleatoriamente
- Ao final, vence quem acumulou **mais pontos**

---

## 📁 Estrutura do Projeto

```
mario-kart-race/
├── mario-kart-api/              ← Backend ASP.NET Core 8
│   ├── Controllers/
│   │   ├── AuthController.cs    ← Login / Registro + JWT
│   │   ├── RaceController.cs    ← Iniciar corrida
│   │   └── LeaderboardController.cs
│   ├── Models/
│   │   ├── User.cs
│   │   └── Race.cs
│   ├── DTOs/Dtos.cs
│   ├── Data/AppDbContext.cs     ← MySQL com EF Core
│   ├── Services/RaceEngineService.cs  ← Lógica do jogo
│   ├── Program.cs               ← Config JWT + Swagger + CORS
│   ├── appsettings.json
│   └── MarioKartAPI.csproj
│
└── mario-kart-frontend/         ← Frontend React + Framer Motion
    ├── public/
    │   └── chars/               ← ⬅️ COLOQUE OS GIFs AQUI
    │       ├── mario.gif
    │       ├── luigi.gif
    │       ├── yoshi.gif
    │       ├── bowser.gif
    │       ├── peach.gif
    │       ├── dk.gif
    │       ├── toad.gif
    │       └── header.gif
    ├── src/
    │   └── App.jsx
    └── package.json
```

---

## 🖼️ Como Colocar as Imagens (GIFs)

Os GIFs dos personagens devem ficar em:

```
mario-kart-frontend/public/chars/
```

### Passo a passo:
1. Crie a pasta `public/chars/` dentro de `mario-kart-frontend/`
2. Copie os arquivos com **exatamente esses nomes**:

| Arquivo | Personagem |
|---|---|
| `mario.gif` | Mario |
| `luigi.gif` | Luigi |
| `yoshi.gif` | Yoshi |
| `bowser.gif` | Bowser |
| `peach.gif` | Peach |
| `dk.gif` | Donkey Kong |
| `toad.gif` | Toad |
| `header.gif` | Logo do header |

> O Vite (bundler do React) serve tudo que está em `/public` diretamente.
> Então `public/chars/mario.gif` fica acessível como `/chars/mario.gif` no browser.

---

## ⚙️ Setup do Backend

### Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- MySQL 8+

### 1. Configurar banco de dados

```sql
-- No MySQL Workbench ou terminal:
CREATE DATABASE mario_kart_db;
```

### 2. Editar `appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=mario_kart_db;User=root;Password=SUA_SENHA;"
  },
  "Jwt": {
    "Secret": "CHAVE_SECRETA_32_CARACTERES_MINIMO_AQUI!",
    "Issuer": "MarioKartAPI",
    "Audience": "MarioKartClient"
  }
}
```

### 3. Rodar a API

```bash
cd mario-kart-api

# Restaurar pacotes NuGet
dotnet restore

# Criar migration (primeira vez)
dotnet ef migrations add InitialCreate

# Aplicar ao banco
dotnet ef database update

# Rodar
dotnet run
```

✅ API em: `http://localhost:5000`
✅ Swagger em: `http://localhost:5000/` (raiz)

---

## 🎨 Setup do Frontend

### Pré-requisitos
- Node.js 18+

```bash
cd mario-kart-frontend

npm install
npm run dev
```

✅ Frontend em: `http://localhost:5173`

---

## 🔌 Endpoints da API

| Método | Rota | Auth | Descrição |
|---|---|---|---|
| `POST` | `/api/auth/register` | ❌ | Cadastro de usuário |
| `POST` | `/api/auth/login` | ❌ | Login → JWT |
| `GET` | `/api/race/characters` | ❌ | Lista personagens |
| `POST` | `/api/race/start` | ✅ | Simula e salva corrida |
| `GET` | `/api/race/{id}` | ❌ | Detalhes de uma corrida |
| `GET` | `/api/leaderboard` | ❌ | Ranking global |
| `GET` | `/api/leaderboard/user/{id}` | ❌ | Histórico do usuário |

---

## 📦 Pacotes NuGet (Backend)

```
Pomelo.EntityFrameworkCore.MySql
Microsoft.EntityFrameworkCore.Design
Microsoft.AspNetCore.Authentication.JwtBearer
Swashbuckle.AspNetCore
BCrypt.Net-Next
```

Instalar com: `dotnet restore`

## 📦 Pacotes npm (Frontend)

```
react
react-dom
framer-motion
```

Instalar com: `npm install`