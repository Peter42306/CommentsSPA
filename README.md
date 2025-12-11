# Comments SPA

This repository contains a simple comments system implemented as a SPA:
- **Backend**: ASP.NET Core Web API + Entity Framework Core + PostgreSQL
- **Frontend**: React (Create React App) + Bootstrap
- **Database**: PostgreSQL

The goal of this project is to demonstrate:
- API design and backend development with .NET
- Frontend skills with React
- Basic Docker and docker-compose usage
- Database design and documentation (schema.sql for MySQL Workbench)

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

