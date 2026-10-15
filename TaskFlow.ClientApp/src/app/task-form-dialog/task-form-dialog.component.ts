import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { finalize } from 'rxjs';
import { SaveTaskRequest, TaskItem, TaskStatus, TASK_STATUSES } from '../models/task.model';
import { TaskService } from '../services/task.service';

export interface TaskFormDialogData {
  task: TaskItem | null;
}

@Component({
  selector: 'app-task-form-dialog',
  standalone: true,
  imports: [
    ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule,
    MatSelectModule, MatDatepickerModule, MatButtonModule
  ],
  templateUrl: './task-form-dialog.component.html',
  styleUrl: './task-form-dialog.component.scss'
})
export class TaskFormDialogComponent {
  private readonly fb = inject(FormBuilder);
  private readonly taskService = inject(TaskService);
  private readonly snackBar = inject(MatSnackBar);
  private readonly dialogRef = inject(MatDialogRef<TaskFormDialogComponent, boolean>);
  readonly data = inject<TaskFormDialogData>(MAT_DIALOG_DATA);

  readonly statuses = TASK_STATUSES;
  readonly isEdit = this.data.task !== null;
  saving = false;

  readonly form = this.fb.nonNullable.group({
    title: [this.data.task?.title ?? '', [Validators.required, Validators.maxLength(120)]],
    description: [this.data.task?.description ?? '', [Validators.maxLength(2000)]],
    status: [this.data.task?.status ?? TaskStatus.Todo, Validators.required],
    dueDate: [this.data.task?.dueDate ? new Date(`${this.data.task.dueDate}T00:00:00`) : null as Date | null]
  });

  save(): void {
    this.form.markAllAsTouched();
    if (this.form.invalid || this.saving) return;

    const value = this.form.getRawValue();
    const request: SaveTaskRequest = {
      title: value.title.trim(),
      description: value.description.trim() || null,
      status: value.status,
      dueDate: value.dueDate ? this.toDateOnly(value.dueDate) : null
    };

    this.saving = true;
    const request$ = this.data.task
      ? this.taskService.update(this.data.task.id, request)
      : this.taskService.create(request);

    request$.pipe(finalize(() => this.saving = false)).subscribe({
      next: () => this.dialogRef.close(true),
      error: () => this.snackBar.open('Could not save the task. Please try again.', 'Dismiss', { duration: 4000, panelClass: 'snack-error' })
    });
  }

  private toDateOnly(date: Date): string {
    const year = date.getFullYear();
    const month = `${date.getMonth() + 1}`.padStart(2, '0');
    const day = `${date.getDate()}`.padStart(2, '0');
    return `${year}-${month}-${day}`;
  }
}
