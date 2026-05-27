# 🏎️ Mario Kart Race Simulator — Full Stack

<img src="./wwwroot/chars/header.gif" alt="Mario Kart" width="220">

Simulador de corrida estilo Mario Kart com:

- **Backend**: ASP.NET Core 8 + MySQL + JWT + Swagger
- **Frontend**: React + Framer Motion (animações em tempo real)
- **Banco**: MySQL com Entity Framework Core (Code First)

Baseado no projeto original em Node.js, recriado com back-end completo e front-end animado.

---

# 🚀 Tecnologias Utilizadas

## Backend
- ASP.NET Core 8
- Entity Framework Core
- MySQL
- JWT Authentication
- Swagger/OpenAPI
- BCrypt

## Frontend
- React
- Vite
- Framer Motion
- Axios
- CSS3

---

# 👥 Personagens

<table>
  <tr>
    <td align="center">
      <img src="./wwwroot/chars/mario.gif" width="60" alt="Mario">
      <br><b>Mario</b>
      <br>💨 Vel: 4 &nbsp; 🎯 Man: 3 &nbsp; 💪 Pow: 3
    </td>

    <td align="center">
      <img src="./wwwroot/chars/luigi.gif" width="60" alt="Luigi">
      <br><b>Luigi</b>
      <br>💨 Vel: 3 &nbsp; 🎯 Man: 4 &nbsp; 💪 Pow: 3
    </td>

    <td align="center">
      <img src="./wwwroot/chars/yoshi.gif" width="60" alt="Yoshi">
      <br><b>Yoshi</b>
      <br>💨 Vel: 4 &nbsp; 🎯 Man: 4 &nbsp; 💪 Pow: 2
    </td>

    <td align="center">
      <img src="./wwwroot/chars/bowser.gif" width="60" alt="Bowser">
      <br><b>Bowser</b>
      <br>💨 Vel: 2 &nbsp; 🎯 Man: 2 &nbsp; 💪 Pow: 5
    </td>
  </tr>

  <tr>
    <td align="center">
      <img src="./wwwroot/chars/peach.gif" width="60" alt="Peach">
      <br><b>Peach</b>
      <br>💨 Vel: 3 &nbsp; 🎯 Man: 3 &nbsp; 💪 Pow: 4
    </td>

    <td align="center">
      <img src="./wwwroot/chars/donkeykong.gif" width="60" alt="Donkey Kong">
      <br><b>Donkey Kong</b>
      <br>💨 Vel: 2 &nbsp; 🎯 Man: 3 &nbsp; 💪 Pow: 5
    </td>

    <td align="center">
      <img src="./wwwroot/chars/toad.gif" width="60" alt="Toad">
      <br><b>Toad</b>
      <br>💨 Vel: 5 &nbsp; 🎯 Man: 4 &nbsp; 💪 Pow: 1
    </td>

    <td></td>
  </tr>
</table>

---

# 🕹️ Regras & Mecânicas

A lógica do jogo é inspirada no Mario Kart clássico:

| Bloco | Atributo usado | Resultado |
|---|---|---|
| 🏁 RETA | Velocidade | Dado + Speed |
| 🔄 CURVA | Manobrabilidade | Dado + Maneuverability |
| ⚠️ CONFRONTO | Poder | Dado + Power |
| 🟤 LAMA | Poder reduzido | Power / 2 |
| ❄️ GELO | Controle avançado | Maneuverability x1.5 |
| 🌋 VULCÃO | Terreno extremo | Bonus de força |
| 🌊 ÁGUA | Controle aquático | Bonus de manobra |
| 🌈 RAINBOW ROAD | Pista especial | Speed + Maneuverability |

---

# ⭐ Vantagens Especiais

| Personagem | Vantagem |
|---|---|
| 🐢 Bowser | +5 no VULCÃO |
| 🌸 Peach | +4 no GELO |
| 🦕 Yoshi | +4 na ÁGUA |
| 🦍 Donkey Kong | +5 na LAMA |
| 🍄 Mario | +3 na RAINBOW ROAD |
| 💚 Luigi | +3 nas CURVAS |

Além disso:

- 🎲 Tirar 6 no dado ativa bônus crítico
- 🏁 Cada volta atualiza o ranking em tempo real
- 📊 Sistema de leaderboard global
- 💾 Corridas ficam salvas no banco MySQL

---

# 📁 Estrutura do Projeto

```bash
Simulador_Mario_Kart/
│
├── Simulador_Mario_Kart.API/
│   ├── Controllers/
│   ├── DTOs/
│   ├── Data/
│   ├── Models/
│   ├── Services/
│   ├── wwwroot/
│   │   └── chars/
│   │       ├── mario.gif
│   │       ├── luigi.gif
│   │       ├── yoshi.gif
│   │       ├── bowser.gif
│   │       ├── peach.gif
│   │       ├── dk.gif
│   │       ├── toad.gif
│   │       └── header.gif
│   ├── Program.cs
│   └── appsettings.json
│
└── mario-kart-frontend/
    ├── src/
    ├── package.json
    └── vite.config.js
  ```
    
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

✅ API em: `https://localhost:7115/api`
✅ Swagger em: `https://localhost:7115/swagger/index.html` (raiz)

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

## 🎮 Funcionalidades

✅ Login JWT
✅ Cadastro de usuários
✅ Corrida em tempo real
✅ Sistema de voltas
✅ Ranking global
✅ Leaderboard
✅ Sistema de vantagens especiais
✅ Animações com Framer Motion
✅ React + ASP.NET Core
✅ Persistência no MySQL
✅ Swagger UI
✅ Sistema de posições
✅ Histórico de corridas

## 🤝 Contribuindo
1. Faça um Fork do projeto
2. Crie uma branch:
```
git checkout -b feature/AmazingFeature
```
3. Commit suas mudanças:
```
git commit -m 'Add some AmazingFeature'
```
4. Push para a branch:
```
git push origin feature/AmazingFeature
```
5. Abra um Pull Request🚀


## 🤝 Contribuindo
Faça um Fork do projeto
Crie sua Feature Branch (git checkout -b feature/AmazingFeature)
Commit suas mudanças (git commit -m 'Add some AmazingFeature')
Push para a Branch (git push origin feature/AmazingFeature)
Abra um Pull Request

## 📝 Licença
Este projeto está sob a licença MIT. Veja o arquivo LICENSE para mais detalhes.

## 👨‍💻 Contato do Autor
Ricardo Amor
GitHub: @DuasEstrelas1931
LinkedIn: Ricardo Amor Divino
Email: ricardo.amor@exemplo.com

## 🙏 Agradecimentos
Comunidade .NET
React Team
MySQL
Todos os contribuidores e usuários do projeto
EX: 
Programa Ford <Enter>
Senai Cimatec
Professor Marcelo

## ⭐ Demonstração

🏎️ Simulação dinâmica de corridas Mario Kart
🔥 Backend robusto com ASP.NET Core 8
🎨 Frontend animado com React + Framer Motion
📊 Ranking persistido em MySQL