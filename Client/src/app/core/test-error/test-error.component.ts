import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.scss']
})
export class TestErrorComponent implements OnInit {
  validationErrors: any;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
  }

  get404Error(){
    this.http.get(environment.apiUrl + 'products/42').subscribe({
      next: respone => console.log(respone),
      error: error => console.log(error)
    });
  }

  get500Error(){
    this.http.get(environment.apiUrl + 'buggy/servererror').subscribe({
      next: respone => console.log(respone),
      error: error => console.log(error)
    });
  }
  get400Error(){
    this.http.get(environment.apiUrl + 'buggy/badrequest').subscribe({
      next: respone => console.log(respone),
      error: error => console.log(error)
    });
  }
  get400ValidationError(){
    this.http.get(environment.apiUrl + 'products/test').subscribe({
      next: respone => console.log(respone),
      error: error =>{ console.log(error);
      this.validationErrors = error.errors}
    });
  }
}
