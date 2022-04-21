import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ShopComponent } from './shop.component';
import { ProductDetailsComponent } from './product-details/product-details.component';

const routes: Routes =[
  { path: '', component: ShopComponent, data:{breadcrumb: 'Shop'} },
  { path: ':id', component: ProductDetailsComponent, data:{breadcrumb: {alias: 'productDetails'}} }
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ]
})
export class ShopRoutingModule { }
