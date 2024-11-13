import { Airport } from './airport.model';

export interface Connection {
  airport: Airport;
  id: number;
}

export interface Route {
  id: number;
  origin: Airport;
  destiny: Airport;
  connections: Connection[];
  price: number;
}

export interface RouteRequest {
  originId: number;
  destinyId: number;
  price: number;
  airportConnectionIds: number[];
}