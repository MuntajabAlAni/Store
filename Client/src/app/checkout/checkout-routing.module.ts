import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule, Routes } from '@angular/router';
import { CheckoutComponent } from './checkout.component';
import { CheckoutSuccesComponent } from './checkout-succes/checkout-succes.component';

const routes: Routes = [
  { path: '', component: CheckoutComponent, data: { breadcrumb: 'checkout' } },
  { path: 'success', component: CheckoutSuccesComponent }];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class CheckoutRoutingModule { }
