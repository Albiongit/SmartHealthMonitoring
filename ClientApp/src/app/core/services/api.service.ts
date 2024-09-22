import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = 'http://localhost:11434/api/generate';

  constructor(private http: HttpClient) {}

  generateResponse(prompt: string): Observable<string> {
    const body = {
      model: 'llama2',
      prompt: prompt,
      format: 'json',
      stream: false
    };

      return this.http.post<any>(this.apiUrl, body).pipe(map(response => response.response)
    );
  }
}
