# Comments SPA

A full-stack template for comments with nested replies, file attachments, CAPTCHA protection, XSS safety, and Docker-based deployment.

This project was built with focus on:
- building and deploying a full-stack application end-to-end
- Dockerized backend, frontend and database
- clean REST API design
- ASP.NET Core Web API + React + PostgreSQL stack
- SPA architecture

## Features

- Post a new comment
- Reply to an existing comment (nested structure)
- Rich text input for comments (basic formatting: bold, italic, code, links)
- Pagination and sorting of comments
- Basic validation on both client and server
- CAPTCHA protection
- XSS-safe text rendering (sanitized HTML)
- File attachments for comments (stored on server, path kept in DB)

## Screenshots
![Screenshot 2025-12-13 110433](https://github.com/user-attachments/assets/abff890e-a179-4624-94e9-ea4adbb27fbc)

![Screenshot 2025-12-13 110556](https://github.com/user-attachments/assets/33f7346c-8fc4-4cb0-bde6-2c7a4c2df0ad)

![Screenshot 2025-12-13 110625](https://github.com/user-attachments/assets/379af4f2-bdf1-477b-9e43-b285c5a91845)

![Screenshot 2025-12-13 110828](https://github.com/user-attachments/assets/a11bfbbb-8736-42af-912c-fd3c48955843)


## Technology stack

**Backend**

- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- RESTful API design

**Frontend**

- React (Create React App)
- JavaScript
- Bootstrap 5
- Bootstrap Icons

**Infrastructure**

- Docker
- docker-compose
- Nginx (reverse proxy)
- Linux server deployment

## Project structure

```text
CommentsSPA/
 ├─ CommentsSpaApi/           # ASP.NET Core Web API
 │   ├─ CommentsSpaApi/       # Main project (csproj, Program.cs, Controllers, etc.)
 │   └─ ...
 ├─ comments-spa-app/         # React SPA (Create React App)
 │   ├─ src/
 │   ├─ public/
 │   ├─ nginx/
 │   │   └─ default.conf     # Nginx config for frontend + proxy to API
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

 ## API features

 **Comments**

 - GET    /api/comments/all — retrieve all comments (flat list, used to build nested structure on the client)
 - GET    /api/comments — paginated and sortable list of root comments
 - GET    /api/comments/{id} — get a single comment by id
 - POST   /api/comments — create a new comment (root or reply)

**Comments sorting and pagination (query parameters)**
- page
- pageSize
- sortBy (createdAt, username, email)
- sortDirection (asc, desc)

**Attachments**

- POST   /api/comments/{commentId}/attachments — upload attachment for a comment (multipart/form-data)
- GET    /api/comments/{commentId}/attachments — list attachments for a comment

**CAPTCHA**

- GET    /api/captcha — generate a new CAPTCHA challenge

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
