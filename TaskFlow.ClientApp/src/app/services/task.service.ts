import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { SaveTaskRequest, TaskItem } from '../models/task.model';

@Injectable({ providedIn: 'root' })
export class TaskService {
  private readonly http = inject(HttpClient);
  private readonly apiBaseUrl = 'http://localhost:5080/api/tasks';

  getAll(): Observable<TaskItem[]> {
    return this.http.get<TaskItem[]>(this.apiBaseUrl);
  }

  create(request: SaveTaskRequest): Observable<TaskItem> {
    return this.http.post<TaskItem>(this.apiBaseUrl, request);
  }

  update(id: string, request: SaveTaskRequest): Observable<TaskItem> {
    return this.http.put<TaskItem>(`${this.apiBaseUrl}/${id}`, request);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiBaseUrl}/${id}`);
  }
}
