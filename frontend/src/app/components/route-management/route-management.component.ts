import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Airport } from '../../models/airport.model';
import { Route } from '../../models/route.model';
import { AirportService } from '../../services/airport.service';
import { RouteService } from '../../services/route.service';

@Component({
  selector: 'app-route-management',
  templateUrl: './route-management.component.html',
  styleUrls: ['./route-management.component.css']
})
export class RouteManagementComponent implements OnInit {
  routeForm: FormGroup;
  airports: Airport[] = [];
  routes: Route[] = [];
  availableConnections: Airport[] = [];
  selectedConnections: Airport[] = [];
  editMode = false;
  editingRouteId?: number;
  availableOrigins: Airport[] = [];
  availableDestinations: Airport[] = [];
  routeToEdit?: Route;

  displayedColumns = ['origin', 'destination', 'price', 'connections', 'actions'];

  constructor(
    private fb: FormBuilder,
    private airportService: AirportService,
    private routeService: RouteService,
    private snackBar: MatSnackBar
  ) {
    this.routeForm = this.fb.group({
      origin: ['', Validators.required],
      destination: ['', Validators.required],
      price: ['', [Validators.required, Validators.min(1)]],
      connection: ['']
    });
  }

  ngOnInit() {
    this.loadAirports();
    this.loadRoutes();
  }

  private loadAirports() {
    this.airportService.getAirports().subscribe(airports => {
      this.airports = airports;
      this.updateAvailableAirports();
      
      if (this.routeToEdit) {
        this.setEditFormValues(this.routeToEdit);
      }
    });
  }

  private loadRoutes() {
    this.routeService.getRoutes().subscribe(routes => {
      this.routes = routes;
    });
  }

  private updateAvailableAirports() {
    const selectedOrigin = this.routeForm.get('origin')?.value;
    const selectedDestination = this.routeForm.get('destination')?.value;

    if (this.editMode) {
      this.availableOrigins = this.airports.filter(airport => 
        airport.id === selectedOrigin?.id || airport.id !== selectedDestination?.id
      );

      this.availableDestinations = this.airports.filter(airport => 
        airport.id === selectedDestination?.id || airport.id !== selectedOrigin?.id
      );
    } else {
      this.availableOrigins = this.airports.filter(airport => 
        airport.id !== selectedDestination?.id
      );

      this.availableDestinations = this.airports.filter(airport => 
        airport.id !== selectedOrigin?.id
      );
    }
  }

  updateDestinations() {
    const origin = this.routeForm.get('origin')?.value;
    if (origin) {
      const currentDestination = this.routeForm.get('destination')?.value;
      if (currentDestination?.id === origin.id) {
        this.routeForm.get('destination')?.setValue('');
      }
      this.updateAvailableAirports();
      this.updateAvailableConnections();
    }
  }

  updateOrigins() {
    const destination = this.routeForm.get('destination')?.value;
    if (destination) {
      const currentOrigin = this.routeForm.get('origin')?.value;
      if (currentOrigin?.id === destination.id) {
        this.routeForm.get('origin')?.setValue('');
      }
      this.updateAvailableAirports();
      this.updateAvailableConnections();
    }
  }

  updateAvailableConnections() {
    const origin = this.routeForm.get('origin')?.value;
    const destination = this.routeForm.get('destination')?.value;
    
    this.availableConnections = this.airports.filter(airport => 
      airport.id !== origin?.id && 
      airport.id !== destination?.id &&
      !this.selectedConnections.some(conn => conn.id === airport.id)
    );
  }

  addConnection() {
    const connection = this.routeForm.get('connection')?.value;
    if (connection) {
      this.selectedConnections.push(connection);
      this.routeForm.get('connection')?.setValue('');
      this.updateAvailableConnections();
    }
  }

  removeConnection(connection: Airport) {
    this.selectedConnections = this.selectedConnections.filter(c => c.id !== connection.id);
    this.updateAvailableConnections();
  }

  onSubmit() {
    if (this.routeForm.valid) {
      const routeRequest = {
        originId: this.routeForm.get('origin')?.value.id,
        destinyId: this.routeForm.get('destination')?.value.id,
        price: this.routeForm.get('price')?.value,
        airportConnectionIds: this.selectedConnections.map(c => c.id)
      };

      if (this.editMode && this.editingRouteId) {
        this.routeService.updateRoute(this.editingRouteId, routeRequest).subscribe({
          next: () => {
            this.loadRoutes();
            this.resetForm();
            this.snackBar.open('Route updated successfully', 'Close', { duration: 3000 });
          },
          error: () => this.snackBar.open('Error updating route', 'Close', { duration: 3000 })
        });
      } else {
        this.routeService.addRoute(routeRequest).subscribe({
          next: () => {
            this.loadRoutes();
            this.resetForm();
            this.snackBar.open('Route added successfully', 'Close', { duration: 3000 });
          },
          error: () => this.snackBar.open('Error adding route', 'Close', { duration: 3000 })
        });
      }
    }
  }

  private setEditFormValues(route: Route) {
    const originAirport = this.airports.find(a => a.id === route.origin.id);
    const destinyAirport = this.airports.find(a => a.id === route.destiny.id);

    if (!originAirport || !destinyAirport) {
      this.snackBar.open('Error loading route data', 'Close', { duration: 3000 });
      return;
    }

    this.availableOrigins = [...this.airports];
    this.availableDestinations = [...this.airports];
    
    this.routeForm.patchValue({
      origin: originAirport,
      destination: destinyAirport,
      price: route.price
    });
    
    this.updateAvailableAirports();
  }

  editRoute(route: Route) {
    this.editMode = true;
    this.editingRouteId = route.id;
    this.selectedConnections = route.connections.map(c => c.airport);
    this.routeToEdit = route;

    if (this.airports.length > 0) {
      this.setEditFormValues(route);
    }
  }

  deleteRoute(route: Route) {
    this.resetForm();
    this.routeService.deleteRoute(route.id).subscribe({
      next: () => {
        this.loadRoutes();
        this.snackBar.open('Route deleted successfully', 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Error deleting route', 'Close', { duration: 3000 })
    });
  }

  private resetForm() {
    this.editMode = false;
    this.editingRouteId = undefined;
    this.routeToEdit = undefined;
    this.selectedConnections = [];
    this.routeForm.reset();
    this.updateAvailableAirports();
    this.updateAvailableConnections();
  }
}