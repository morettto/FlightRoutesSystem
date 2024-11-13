import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Airport } from '../models/airport.model';

@Injectable({
  providedIn: 'root'
})
export class AirportService {
  private baseUrl = 'https://localhost:5001/api/airport';

  constructor(private http: HttpClient) {}

  getAirports(): Observable<Airport[]> {
    return this.http.get<Airport[]>(this.baseUrl);
  }

  addAirport(name: string): Observable<Airport> {
    return this.http.post<Airport>(this.baseUrl, { name });
  }
}