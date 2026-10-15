export enum TaskStatus {
  Todo = 0,
  InProgress = 1,
  Done = 2
}

export interface TaskItem {
  id: string;
  title: string;
  description: string | null;
  status: TaskStatus;
  dueDate: string | null;
  createdAt: string;
  updatedAt: string;
}

export interface SaveTaskRequest {
  title: string;
  description: string | null;
  status: TaskStatus;
  dueDate: string | null;
}

export const TASK_STATUSES = [
  { text: 'To do', value: TaskStatus.Todo },
  { text: 'In progress', value: TaskStatus.InProgress },
  { text: 'Done', value: TaskStatus.Done }
];

export function statusText(status: TaskStatus): string {
  return TASK_STATUSES.find((item) => item.value === status)?.text ?? 'To do';
}
