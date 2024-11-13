import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Airport } from '../../models/airport.model';
import { AirportService } from '../../services/airport.service';

@Component({
  selector: 'app-airport-management',
  templateUrl: './airport-management.component.html',
  styleUrls: ['./airport-management.component.css']
})
export class AirportManagementComponent implements OnInit {
  airportForm: FormGroup;
  airports: Airport[] = [];
  displayedColumns = ['name'];

  constructor(
    private fb: FormBuilder,
    private airportService: AirportService,
    private snackBar: MatSnackBar
  ) {
    this.airportForm = this.fb.group({
      code: ['', [Validators.required, Validators.pattern('^[A-Z]{3}$')]]
    });
  }

  ngOnInit() {
    this.loadAirports();
  }

  private loadAirports() {
    this.airportService.getAirports().subscribe(airports => {
      this.airports = airports;
    });
  }

  onSubmit() {
    if (this.airportForm.valid) {
      this.airportService.addAirport(this.airportForm.get('code')?.value).subscribe({
        next: () => {
          this.loadAirports();
          this.airportForm.reset();
          this.snackBar.open('Airport added successfully', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Error adding airport', 'Close', { duration: 3000 })
      });
    }
  }
}