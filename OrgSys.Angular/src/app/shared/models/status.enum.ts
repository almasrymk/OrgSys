/** Mirrors Domain.Enums.Status (Domain/Enums/CommandType.cs) — the universal document status. */
export enum Status {
  New = 0,
  Deleted = 5,
  UnderReview = 10,
  Approved = 15,
  Rejected = 20,
  Locked = 25,
  Hold = 30,
  Cancel = 35,
  Reversed = 40,
}
