import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BasketService } from 'src/app/basket/basket.service';
import { IOrder } from 'src/app/shared/models/order';
import { BreadcrumbService } from 'xng-breadcrumb';
import { OrdersService } from '../orders.service';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.scss']
})
export class OrderDetailsComponent implements OnInit {
  order!: IOrder;

  constructor(private ordersService: OrdersService, private activatedRoute: ActivatedRoute,
    private bcService: BreadcrumbService) {
    bcService.set('@orderDetails', ' ');
  }

  ngOnInit(): void {
    this.loadOrder();
  }

  loadOrder() {
    this.ordersService.getOrder(+this.activatedRoute.snapshot.paramMap.get('id')!).subscribe({
      next: (response) => {
        this.order = response;
        this.bcService.set('@orderDetails', `Order# ${response.id} - ${response.status}`);
      },
      error: error => { console.log(error) }
    });
  }

}
