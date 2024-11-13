import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Flight Route Management System';
  
  navLinks = [
    { path: 'routes', label: 'Route Management' },
    { path: 'airports', label: 'Airport Management' },
    { path: 'search', label: 'Search Cheapest Route' }
  ];
}