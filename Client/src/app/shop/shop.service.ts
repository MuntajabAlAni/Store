import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IBrand } from '../shared/models/brand';
import { IPagination, Pagination } from '../shared/models/pagination';
import { IProduct } from '../shared/models/product';
import { IType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = environment.apiUrl;
  products: IProduct[] = [];
  brands: IBrand[] = [];
  types: IType[] = [];
  pagination = new Pagination();
  shopParams = new ShopParams();
  productCach = new Map();

  constructor(private http: HttpClient) { }

  getProducts(useCach: boolean) {

    if (this.productCach.size > 0 && useCach) {
      if (this.productCach.has(Object.values(this.shopParams).join('-'))) {
        this.pagination.data = this.productCach.get(Object.values(this.shopParams).join('-'));
        return of(this.pagination);
      }
    }

    if (!useCach) {
      this.productCach = new Map();
    }
    let params = new HttpParams();
    if (this.shopParams.brandId > 0) {
      params = params.append('brandId', this.shopParams.brandId.toString());
    }
    if (this.shopParams.typeId > 0) {
      params = params.append('typeId', this.shopParams.typeId.toString());
    }
    if (this.shopParams.search) {
      params = params.append('search', this.shopParams.search);
    }

    params = params.append('Sort', this.shopParams.sort);
    params = params.append('PageIndex', this.shopParams.pageNumber.toString());
    params = params.append('PageSize', this.shopParams.pageSize.toString());

    return this.http.get<IPagination>(this.baseUrl + 'products', { observe: "response", params })
      .pipe(
        map(response => {
          this.productCach.set(Object.values(this.shopParams).join('-'), response.body!.data);
          this.pagination = response.body!;
          return this.pagination;
        })
      );
  }

  setShopParams(params: ShopParams) {
    this.shopParams = params;
  }

  getShopParams() {
    return this.shopParams;
  }

  getProduct(id: number) {
    let product: IProduct | undefined;
    this.productCach.forEach((products: IProduct[]) => { product = products.find(p => p.id === id) });

    if (product) return of(product);

    return this.http.get<IProduct>(this.baseUrl + 'products/' + id);
  }

  getTypes() {
    if (this.types.length > 0)
      return of(this.types);

    return this.http.get<IType[]>(this.baseUrl + 'Products/types').pipe(
      map(response => {
        this.types = response;
        return response;
      })
    );
  }

  getBrands() {
    if (this.brands.length > 0)
      return of(this.brands);

    return this.http.get<IBrand[]>(this.baseUrl + 'Products/brands').pipe(
      map(response => {
        this.brands = response;
        return response;
      })
    );
  }
}
