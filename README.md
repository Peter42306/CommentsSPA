# Comments SPA

This repository contains a simple application for comments with nested replies, images and text attachments, and captcha protection implemented as a Single Page Application (SPA):

- **Backend**: ASP.NET Core Web API + Entity Framework Core + PostgreSQL
- **Frontend**: React (Create React App) + Bootstrap
- **Database**: PostgreSQL

The goal of this project is to demonstrate:
- API design and backend development with .NET
- Frontend skills with React
- Basic Docker and docker-compose usage
- Database design and documentation (schema.sql)

## Screenshots
![Screenshot 2025-12-13 110433](https://github.com/user-attachments/assets/abff890e-a179-4624-94e9-ea4adbb27fbc)

![Screenshot 2025-12-13 110556](https://github.com/user-attachments/assets/33f7346c-8fc4-4cb0-bde6-2c7a4c2df0ad)

![Screenshot 2025-12-13 110625](https://github.com/user-attachments/assets/379af4f2-bdf1-477b-9e43-b285c5a91845)

![Screenshot 2025-12-13 110828](https://github.com/user-attachments/assets/a11bfbbb-8736-42af-912c-fd3c48955843)


## Features

- Post a new comment
- Reply to an existing comment (nested structure)
- Pagination and sorting of comments
- Basic validation on both client and server
- XSS-safe text rendering (sanitized HTML)
- File attachments for comments (stored on server, path kept in DB)

## Technology stack

**Backend**

- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL

**Frontend**

- React (Create React App)
- JavaScript
- Bootstrap 5
- Bootstrap Icons

**Infrastructure**

- Docker
- docker-compose

## Project structure

```text
CommentsSPA/
 ├─ CommentsSpaApi/           # ASP.NET Core Web API
 │   ├─ CommentsSpaApi/       # Main project (csproj, Program.cs, Controllers, etc.)
 │   └─ ...
 ├─ comments-spa-app/         # React SPA (Create React App)
 │   ├─ src/
 │   ├─ public/
 │   └─ package.json
 ├─ db/
 │   └─ schema.sql            # Database schema
 ├─ uploads/
 │   ├─ images/
 │   └─ text/
 ├─ Dockerfile.api            # Dockerfile for the backend
 ├─ Dockerfile.frontend       # Dockerfile for the frontend
 ├─ docker-compose.yml        # Orchestrates API, DB and frontend
 ├─ .dockerignore
 ├─ .gitignore
 └─ README.md
```

 ## Running project with Docker

### Clone repository

```text
git clone https://github.com/Peter42306/CommentsSPA
cd CommentsSPA
```

 ### Build and run containers

 ```text
docker-compose build
docker-compose up -d
```

### Check running containers

```text
docker ps
```

### Ports

```text
Frontend: http://localhost:3001
Backend API: http://localhost:5004/api
```
