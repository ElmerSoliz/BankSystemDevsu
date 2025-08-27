import { BaseEntity } from "./baseEntity.model";
import { Client } from "./client.model";
import { AccountType } from "./enums.model";
import { Movement } from "./movement.model";

export interface Account extends BaseEntity {
  accountNumber: string;
  accountType: AccountType;
  initialBalance: number;
  currentBalance: number;
  isActive: boolean;
  clientId: string;
  client: Client;
  movements?: Movement[];
}