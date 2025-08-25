import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Movement } from "../models/movement.model";
import { HttpClient, HttpParams } from "@angular/common/http";

@Injectable({ providedIn: 'root' })
export class MovementService {
  private baseUrl = 'http://localhost:32768/movements';

  constructor(private http: HttpClient) {}

  listByAccount(accountId: string, fromUtc?: string, toUtc?: string): Observable<Movement[]> {
    let params = new HttpParams();
    if (fromUtc) params = params.set('fromUtc', fromUtc);
    if (toUtc) params = params.set('toUtc', toUtc);

    return this.http.get<Movement[]>(`${this.baseUrl}/by-account/${accountId}`, { params });
  }

  create(dto: Partial<Movement>): Observable<Movement> {
    return this.http.post<Movement>(this.baseUrl, dto);
  }
}
