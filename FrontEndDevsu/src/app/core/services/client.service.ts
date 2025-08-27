import { Injectable } from "@angular/core";
import { Client } from "../models/client.model";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";

@Injectable({ providedIn: 'root' })
export class ClientService {
  private baseUrl = 'http://localhost:32768/clients';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Client[]> {
    return this.http.get<Client[]>(this.baseUrl);
  }

  getById(id: string): Observable<Client> {
    return this.http.get<Client>(`${this.baseUrl}/${id}`);
  }

  create(dto: Partial<Client>): Observable<Client> {
    return this.http.post<Client>(this.baseUrl, dto);
  }

  update(id: string, dto: Partial<Client>): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, dto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}