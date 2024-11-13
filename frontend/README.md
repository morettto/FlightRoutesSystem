# Flight Route Management System

A modern web application for managing flight routes and finding the cheapest connections between airports.

## Requirements

- Node.js 16.x or higher
- npm 8.x or higher
- Angular CLI 16.2.0

## Tech Stack

- Angular 16.2.0
- Angular Material UI
- RxJS 7.8.0
- TypeScript 5.1.3

## Features

- Airport Management
  - Add new airports
  - View existing airports
  - Validate airport codes

- Route Management
  - Create new routes between airports
  - Edit existing routes
  - Delete routes
  - Add intermediate connections
  - Set route prices

- Route Search
  - Find cheapest routes between airports
  - View all connections in the route
  - Display route pricing

## Setup

1. Clone the repository
2. Install dependencies:
   ```bash
   npm install
   ```
## Development
- Run `ng serve` for a dev server
- Run `ng build` to build the project
- Production build will be stored in the `dist/` directory

## Dependencies

Main dependencies:
- @angular/animations: ^16.2.0
- @angular/cdk: ^16.2.0
- @angular/material: ^16.2.0
- @angular/router: ^16.2.0
- rxjs: ~7.8.0
- zone.js: ~0.13.0

Dev dependencies:
- @angular-devkit/build-angular: ^16.2.0
- @angular/cli: ^16.2.0
- typescript: ~5.1.3
