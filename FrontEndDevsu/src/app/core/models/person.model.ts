import { BaseEntity } from "./baseEntity.model";
import { Gender } from "./enums.model";

export interface Person extends BaseEntity {
  name: string;
  gender: Gender;
  age: number;
  identification: string;
  address: string;
  phone: string;
}