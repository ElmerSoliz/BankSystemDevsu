import { Account } from "./account.model";
import { BaseEntity } from "./baseEntity.model";
import { MovementType } from "./enums.model";

export interface Movement extends BaseEntity {
  dateUtc: string;
  movementType: MovementType;
  amount: number;
  balanceAfterTransaction: number;
  accountId: string;
  account?: Account;
}
