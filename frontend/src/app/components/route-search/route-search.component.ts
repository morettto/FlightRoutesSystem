import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Airport } from '../../models/airport.model';
import { Route } from '../../models/route.model';
import { AirportService } from '../../services/airport.service';
import { RouteService } from '../../services/route.service';

@Component({
  selector: 'app-route-search',
  templateUrl: './route-search.component.html',
  styleUrls: ['./route-search.component.css']
})
export class RouteSearchComponent implements OnInit {
  searchForm: FormGroup;
  airports: Airport[] = [];
  cheapestRoute: Route | null = null;
  notFound = false;
  availableOrigins: Airport[] = [];
  availableDestinations: Airport[] = [];
  originName = '';
  destinationName = '';

  constructor(
    private fb: FormBuilder,
    private airportService: AirportService,
    private routeService: RouteService
  ) {
    this.searchForm = this.fb.group({
      origin: ['', Validators.required],
      destination: ['', Validators.required]
    });
  }

  ngOnInit() {
    this.loadAirports();
  }

  private loadAirports() {
    this.airportService.getAirports().subscribe(airports => {
      this.airports = airports;
      this.updateAvailableAirports();
    });
  }

  private updateAvailableAirports() {
    const selectedOrigin = this.searchForm.get('origin')?.value;
    const selectedDestination = this.searchForm.get('destination')?.value;

    this.availableOrigins = this.airports.filter(airport => 
      airport.id !== selectedDestination?.id
    );

    this.availableDestinations = this.airports.filter(airport => 
      airport.id !== selectedOrigin?.id
    );
  }

  updateDestinations() {
    const origin = this.searchForm.get('origin')?.value;
    if (origin) {
      const currentDestination = this.searchForm.get('destination')?.value;
      if (currentDestination?.id === origin.id) {
        this.searchForm.get('destination')?.setValue('');
      }
      this.updateAvailableAirports();
    }
  }

  updateOrigins() {
    const destination = this.searchForm.get('destination')?.value;
    if (destination) {
      const currentOrigin = this.searchForm.get('origin')?.value;
      if (currentOrigin?.id === destination.id) {
        this.searchForm.get('origin')?.setValue('');
      }
      this.updateAvailableAirports();
    }
  }

  onSubmit() {
    if (this.searchForm.valid) {
      const origin = this.searchForm.get('origin')?.value;
      const destination = this.searchForm.get('destination')?.value;
      
      this.originName = origin.name;
      this.destinationName = destination.name;

      this.routeService.getCheapestRoute(origin.id, destination.id).subscribe({
        next: (route) => {
          this.cheapestRoute = route;
          if(route)
            this.notFound = false;
          else
            this.notFound = true;
        },
        error: () => {
          this.cheapestRoute = null;
          this.notFound = true;
        }
      });
    }
  }
}