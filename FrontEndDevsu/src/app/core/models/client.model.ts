import { Account } from "./account.model";
import { Person } from "./person.model";

export interface Client extends Person {
  passwordHash?: string;
  password?: string;
  isActive: boolean;
  accounts?: Account[];
}
