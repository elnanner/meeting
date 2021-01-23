import { City } from './city.model';

export class Meeting {
  meetindId: number;
  adminId: number;
  description: string;
  date: Date;
  maxPeople: number;
  city: City;
}
