import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Route, RouteRequest } from '../models/route.model';

@Injectable({
  providedIn: 'root'
})
export class RouteService {
  private baseUrl = 'https://localhost:5001/api/route';

  constructor(private http: HttpClient) {}

  getRoutes(): Observable<Route[]> {
    return this.http.get<Route[]>(this.baseUrl);
  }

  addRoute(route: RouteRequest): Observable<Route> {
    return this.http.post<Route>(this.baseUrl, route);
  }

  updateRoute(id: number, route: RouteRequest): Observable<Route> {
    return this.http.put<Route>(`${this.baseUrl}/${id}`, route);
  }

  deleteRoute(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  getCheapestRoute(originId: number, destinyId: number): Observable<Route> {
    return this.http.get<Route>(`${this.baseUrl}/cheapest-route/${originId}/${destinyId}`);
  }
}