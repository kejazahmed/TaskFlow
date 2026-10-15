import { DatePipe } from '@angular/common';
import { Component, OnInit, ViewChild,ChangeDetectorRef, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { finalize } from 'rxjs';
import { TaskFormDialogComponent, TaskFormDialogData } from './task-form-dialog/task-form-dialog.component';
import { TaskItem, TaskStatus, statusText } from './models/task.model';
import { TaskService } from './services/task.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    DatePipe, MatButtonModule, MatTableModule, MatSortModule, MatPaginatorModule,
    MatDialogModule, MatSnackBarModule
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  private readonly taskService = inject(TaskService);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);
  private readonly cdr = inject(ChangeDetectorRef);

  tasks: TaskItem[] = [];
  loading = false;
  dataSource = new MatTableDataSource<TaskItem>([]);

  @ViewChild(MatPaginator)
  set paginator(paginator: MatPaginator | undefined) {
    this.dataSource.paginator = paginator ?? null;
  }

  @ViewChild(MatSort)
  set sort(sort: MatSort | undefined) {
    this.dataSource.sort = sort ?? null;
  }

  readonly statusText = statusText;
  readonly TaskStatus = TaskStatus;
  readonly displayedColumns = ['title', 'status', 'dueDate', 'actions'];

  get todoCount(): number { return this.tasks.filter((task) => task.status === TaskStatus.Todo).length; }
  get inProgressCount(): number { return this.tasks.filter((task) => task.status === TaskStatus.InProgress).length; }
  get doneCount(): number { return this.tasks.filter((task) => task.status === TaskStatus.Done).length; }

  ngOnInit(): void { this.loadTasks(); }

  loadTasks(): void {                
    this.taskService.getAll().pipe(finalize(() => { this.loading = false;  this.cdr.markForCheck();})).subscribe({
      next: (tasks) => {               
        this.tasks = tasks;
        this.dataSource.data = [...tasks];
        this.loading = false;             
      },
      error: () => this.snackBar.open('Could not load tasks. Check that the API is running on http://localhost:5080.', 'Dismiss', { duration: 5000, panelClass: 'snack-error' })
    });
  }

  openCreateDialog(): void {
    this.dialog.open<TaskFormDialogComponent, TaskFormDialogData, boolean>(TaskFormDialogComponent, {
      data: { task: null },
      width: '520px'
    }).afterClosed().subscribe((saved) => {
      if (saved) {
        this.snackBar.open('Task created.', 'Dismiss', { duration: 3000, panelClass: 'snack-success' });
        this.loadTasks();
      }
    });
  }

  openEditDialog(task: TaskItem): void {
    this.dialog.open<TaskFormDialogComponent, TaskFormDialogData, boolean>(TaskFormDialogComponent, {
      data: { task },
      width: '520px'
    }).afterClosed().subscribe((saved) => {
      if (saved) {
        this.snackBar.open('Task updated.', 'Dismiss', { duration: 3000, panelClass: 'snack-success' });
        this.loadTasks();
      }
    });
  }

  deleteTask(task: TaskItem): void {
    if (!confirm(`Delete "${task.title}"?`)) return;
    this.taskService.delete(task.id).subscribe({
      next: () => { this.snackBar.open('Task deleted.', 'Dismiss', { duration: 3000, panelClass: 'snack-success' }); this.loadTasks(); },
      error: () => this.snackBar.open('Could not delete the task.', 'Dismiss', { duration: 4000, panelClass: 'snack-error' })
    });
  }
}
