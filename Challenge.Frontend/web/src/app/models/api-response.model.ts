import { Metadata } from './metadata.model';

export class ApiResponse<T> {
  data: any;
  metadata: Metadata;
}
