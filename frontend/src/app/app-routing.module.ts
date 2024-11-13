import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RouteManagementComponent } from './components/route-management/route-management.component';
import { AirportManagementComponent } from './components/airport-management/airport-management.component';
import { RouteSearchComponent } from './components/route-search/route-search.component';

const routes: Routes = [
  { path: '', redirectTo: 'routes', pathMatch: 'full' },
  { path: 'routes', component: RouteManagementComponent },
  { path: 'airports', component: AirportManagementComponent },
  { path: 'search', component: RouteSearchComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }