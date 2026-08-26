# E2E-06 — Payment is made, advice is sent, the file is closed

**Proves:** that the settled figure is derived from recorded payments rather than typed, that
the application will not accept a settlement it has no money for, and that closure is a status
rather than a deletion.

**Applications:** Claims Manager → Outlook
**Role:** Claims Handler (`jbaker`)
**About:** 6 minutes

## Preconditions

- The claim from E2E-01 is at **Underwriter Agreed**, reserve `7000`.
- Signed in as `jbaker`.

## Steps

### 1. Claims Manager — try to settle with nothing paid
**Do:** Open the claim. **Move claim on...** then **Record settlement**.
**Expect:** Blocked, with a message saying a payment must be recorded on the Payments tab
first. Status unchanged.

### 2. Claims Manager — check the starting figures
**Do:** **Payments** tab.
**Expect:** The grid is empty. The totals line reads net paid `0.00`, reserve `7,000.00`,
outstanding `7,000.00`.

### 3. Claims Manager — record an interim payment
**Do:** In **Record a payment**: type **Interim**, amount `2500`, payee the insured's name,
reference `BACS/100001`, paid on today. **Record payment**.
**Expect:** The row appears. Net paid becomes `2,500.00`, outstanding `4,500.00`. On the
**Details** tab, **Paid net** reads `2,500.00` — and it is read only, because it is derived.

### 4. Claims Manager — record the final payment
**Do:** Type **Final**, amount `3900`, same payee, reference `BACS/100002`, paid today.
**Record payment**.
**Expect:** Net paid `6,400.00`, outstanding `600.00`.

### 5. Claims Manager — record a recovery and watch it subtract
**Do:** Type **Recovery**, amount `400`, payee `Third party insurer`, reference
`REC/100003`, paid today. **Record payment**.
**Expect:** Net paid falls to `6,000.00` and outstanding rises to `1,000.00`. A recovery
reduces the net figure rather than adding to it.

### 6. Claims Manager — settle it
**Do:** **Move claim on...** then **Record settlement**. Confirm.
**Expect:** Status **Settled**. **Paid net** on the Details tab agrees with the Payments tab
totals line.

### 7. Outlook — send the payment advice
**Do:** Compose to `INSURED-MAILBOX`:
- Subject: `Claim <reference> - settlement`
- Body: list each payment with its type, amount, date and reference, taken from the Payments
  tab, and state the net figure.

**Do:** Send. Copy `UNDERWRITER-MAILBOX`, since FIX-04 asked for the advice for their records.

### 8. Claims Manager — record the advice and close the file
**Do:** **Record an item against this claim...** capturing the advice email.
**Do:** **Move claim on...** then **Close claim**. Confirm.
**Expect:** Status **Closed**, shown in grey rather than colour.

### 9. Claims Manager — confirm closure is terminal
**Do:** **Move claim on...**
**Expect:** The button is disabled and reads **No action available**. Closed is terminal for
every role.
**Do:** Sign in as `shollis` and open the claim.
**Expect:** Still no action available. Not even a supervisor reopens a closed claim.

### 10. Claims Manager — confirm nothing was destroyed
**Do:** `Claims > All claims`, untick **Open claims only**, and find the claim.
**Expect:** It is listed, status Closed. Open it: all six tabs still carry their content —
correspondence, documents, notes, payments, and the full status history from Notified to
Closed.

## Pass criteria

- [ ] Settling with no payments was refused, naming the Payments tab
- [ ] Net paid tracked the payments after each entry: 2,500.00 then 6,400.00 then 6,000.00
- [ ] The recovery subtracted rather than added
- [ ] **Paid net** on the Details tab is read only and agrees with the Payments total
- [ ] Status reached **Settled** and then **Closed**
- [ ] No action is offered from Closed, for any role including Supervisor
- [ ] The claim is still fully readable after closure, with nine or more status history rows
- [ ] The payment advice exists in Sent Items and its figures match the Payments tab exactly

## Negative checks

- [ ] Paid net was never typed directly, and cannot be
- [ ] Closing did not delete the claim, its documents, or any history

## Legitimate variation

Payment references and amounts, so long as the arithmetic in the pass criteria is recomputed
to match. Timestamps.

## Reset

The claim is finished. To run the suite again from E2E-01, delete `Claims.mdb` and the
`Documents` folder from beside the executable.
