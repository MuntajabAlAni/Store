import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { map, of, ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IAddress } from '../shared/models/address';
import { IUser } from '../shared/models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<IUser | null>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private router: Router) { }

  loadCurrentUser(token: string | null){
    if(token === null){
      this.currentUserSource.next(null);
      return;
    }

    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);

    return this.http.get(this.baseUrl + 'account', {headers}).pipe(
      map((user) =>{
        if(user){
          localStorage.setItem('token', (user as IUser).token);
          this.currentUserSource.next(user as IUser);
        }
      })
    );
  }

  login(values: any) {
    return this.http.post(this.baseUrl + 'account/login', values).pipe(
      map((user) => {
        if (user) {
          localStorage.setItem('token', (user as IUser).token);
          this.currentUserSource.next(user as IUser);
        }
      })
    );
  }

  regisetr(values: any) {
    return this.http.post(this.baseUrl + 'account/register', values).pipe(
      map((user) => {
        if (user) {
          localStorage.setItem('token', (user as IUser).token);
          this.currentUserSource.next(user as IUser);
        }
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
  }

  checkEmailExists(email: string) {
    return this.http.get(this.baseUrl + 'account/emailexists?email=' + email);
  }

  getUserAddress(){
    return this.http.get<IAddress>(this.baseUrl + 'account/address');
  }

  updateUserAddress(address: IAddress){
    return this.http.put<IAddress>(this.baseUrl + 'account/address', address);
  }
}
