# Inbound mail fixtures

Claims Manager has no mailbox integration by design: a handler reads what arrived and keys it
in. So the Outlook side of these workflows needs mail that actually exists. Send these into
the test mailbox before running the suite.

**Substitute the addresses.** Everything seeded in the application uses `.example`, which is
reserved and does not route. Pick real test mailboxes and use them consistently:

| Placeholder | Substitute with |
|---|---|
| `INSURED-MAILBOX` | a mailbox you can send from and receive at |
| `ASSESSOR-MAILBOX` | a second mailbox, so the sender differs |
| `UNDERWRITER-MAILBOX` | a third, standing in for the underwriter |
| `SUPERVISOR-MAILBOX` | wherever reports should land, for tests 07 and 09 |

**Attachments.** Any files will do; content is irrelevant. Two or three JPEGs and a PDF are
enough. Name them as given so the steps read true: `damage-01.jpg`, `damage-02.jpg`,
`invoice-4471.pdf`, `inspection-report.pdf`.

---

## FIX-01 — New loss, used by E2E-01 and E2E-09

**From:** `INSURED-MAILBOX`
**Subject:** `Water damage at our unit - urgent`
**Attach:** `damage-01.jpg`, `damage-02.jpg`

```
Good morning,

I need to report a new claim. We came in on Monday to find water across the whole
of the ground floor of our unit. It looks as though a pipe above the ceiling has
failed over the weekend.

Two display units are ruined and we have lost a quantity of stock. Photographs
attached.

We have not claimed before so I am not sure what you need from us.

Regards
Douglas Fairbairn
Fairbairn Joinery Ltd
0141 496 8820
```

Deliberately contains **no policy number and no date of loss**. The match screen should find
nothing to pre-fill from it beyond the sender's surname, which is what makes it a new claim.

---

## FIX-02 — Further documents for a claim already open, used by E2E-02

**From:** `INSURED-MAILBOX`
**Subject:** `Receipts as requested - policy EM/2211907`
**Attach:** `invoice-4471.pdf`

```
Afternoon,

Attached is the invoice you asked for covering the tools taken on the 2nd. The
laptop receipt I cannot find, I bought it from a shop in town in 2024.

Policy number is EM/2211907.

Regards
G Wraith
```

Quotes a policy number **in the format the application recognises**, so the match screen
should pre-fill it and offer the existing claim. Before sending, open Claims Manager, untick
*Open claims only* on the All claims list, and note a real policy number from the seeded
book — then use that instead of `EM/2211907`, since the generated numbers differ per machine.

---

## FIX-03 — Assessor report, used by E2E-04

**From:** `ASSESSOR-MAILBOX`
**Subject:** `Inspection report - 4 pages`
**Attach:** `inspection-report.pdf`

```
Inspection carried out this week. Report attached, four pages.

Conclusion is that the damage is consistent with a sudden failure rather than
gradual seepage, and is in our view covered.

Marsh & Cotter Loss Assessors
```

---

## FIX-04 — Underwriter reply, used by E2E-05 and E2E-09

Send this **only after** the submission in E2E-05 has gone out, so the sequence is honest.

**From:** `UNDERWRITER-MAILBOX`
**Subject:** `RE: Claim submission - [CLAIM REF]`

```
Thank you for the submission.

We are content to deal with this claim as presented. Please proceed to settlement
and send us the payment advice for our records.

Northgate Syndicate
```

Replace `[CLAIM REF]` with the reference the application allocated. It differs per machine,
which is why the tests treat it as a value to be read rather than asserted.
