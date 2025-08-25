import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Account } from "../models/account.model";
import { HttpClient } from "@angular/common/http";

@Injectable({ providedIn: 'root' })
export class AccountService {
//   private baseUrl = 'http://localhost:32768/accounts';
  
  private baseUrl = 'http://localhost:8080/accounts';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Account[]> {
    return this.http.get<Account[]>(this.baseUrl);
  }

  getById(id: string): Observable<Account> {
    return this.http.get<Account>(`${this.baseUrl}/${id}`);
  }

  create(dto: Partial<Account>): Observable<Account> {
    return this.http.post<Account>(this.baseUrl, dto);
  }

  update(id: string, dto: Partial<Account>): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, dto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
