# E2E-09 — One claim from email to closed, then reported to management both ways

**Proves:** the whole thing. A loss arrives as an email, travels the full twelve-state
workflow across three roles with correspondence going out at each boundary, is paid and
closed, and then shows up in the reports that go to management by Teams and by email.

This is the long one. It is the test to run when you want to demonstrate the application
rather than probe a specific behaviour.

**Applications:** Outlook, File Explorer, Claims Manager, Excel, Teams — all five
**Roles:** Handler (`jbaker`), Adjuster (`dcampbell`), Supervisor (`shollis`)
**About:** 25 minutes

## Preconditions

- Database freshly rebuilt. Delete `Claims.mdb` and the `Documents` folder beside the
  executable, then start the application once and let it regenerate.
- Excel installed. Teams open with a writable channel.
- Fixtures **FIX-01** and **FIX-04** ready to send. Real test mailboxes chosen.

## Part 1 — In through the mailbox

### 1. Outlook
**Do:** Send **FIX-01** to the test mailbox. Open it, read it, save both photographs to disk.

### 2. Claims Manager — capture, judge, create
**Do:** Sign in as `jbaker`. `Intake > Record inbound item...` and capture the email in full,
attaching both photographs as **Photo**. **Save and find claim...**
**Expect:** `No existing claim matches. This looks like a new loss.`
**Do:** **Create new claim from this item**. Complete the file: policy `NGC/990114`, peril
**Escape of Water**, estimate `11500`, reserve `13000`, date of loss the preceding Saturday,
loss adjuster **Douglas Campbell**. **Save claim**.
**Do:** Record the reference. Call it `REF`.

## Part 2 — Chasing, and the adjuster

### 3. Claims Manager → Outlook
**Do:** **Move claim on...** **Request further documents**, reason naming a plumber's report
and a stock schedule.
**Do:** In Outlook, email the insured asking for exactly those, quoting `REF`.
**Do:** Back in Claims Manager, record the outbound item against the claim.

### 4. Outlook → Claims Manager
**Do:** Reply from the insured's mailbox attaching `invoice-4471.pdf`. Save it, record it
against the claim as **Invoice**.
**Do:** **Move claim on...** **Refer to loss adjuster**.
**Expect:** Accepted. Every gate condition is satisfied.

### 5. Claims Manager — the adjuster decides
**Do:** Sign out. Sign in as `dcampbell`.
**Expect:** Lands on the **Adjuster review queue**, and `REF` is in it.
**Do:** Open it. Confirm the Details tab is read only.
**Do:** **Move claim on...** **Approve claim**, with a reason.
**Expect:** **Adjuster Approved**.

## Part 3 — Out to the underwriter

### 6. Claims Manager → Explorer → Outlook
**Do:** Sign in as `jbaker`. Set **Underwriter** to **Ellesmere Mutual**. **Save claim**.
**Do:** Open `Documents\REF` in Explorer. Compose to `UNDERWRITER-MAILBOX` with the claim
summary and every document attached. Send.
**Do:** **Move claim on...** **Send to underwriter**. Record the outbound item.

### 7. Outlook → Claims Manager
**Do:** Send **FIX-04** from `UNDERWRITER-MAILBOX`, with `REF` filled in. Record it against
the claim.
**Do:** **Move claim on...** **Record underwriter agreement**.
**Expect:** **Underwriter Agreed**.

## Part 4 — Paying and closing

### 8. Claims Manager
**Do:** **Payments** tab. Record **Interim** `5000`, then **Final** `6200`, both to the
insured.
**Expect:** Net paid `11,200.00`. **Paid net** on the Details tab agrees.
**Do:** **Move claim on...** **Record settlement**. Then email the payment advice to the
insured, copying the underwriter, and record it.
**Do:** **Move claim on...** **Close claim**.
**Expect:** **Closed**, and no further action offered.

### 9. Claims Manager — read the whole story back
**Do:** **History** tab.
**Expect:** The status history reads, in order: Notified, Documents Pending, With Adjuster,
Adjuster Approved, Sent To Underwriter, Underwriter Agreed, Settled, Closed. Eight rows, each
with its user and, where required, its reason. The audit grid beneath carries entries for the
claim's creation, every document filed, every payment and every status change.

## Part 5 — Reporting it to management

### 10. Claims Manager → Excel
**Do:** `Reports > Export to Excel...`
**Do:** With all statuses ticked, **Export claim value over time...** as `value.xlsx`.
**Do:** **Export claim frequency by month...** as `frequency.xlsx`.
**Expect:** Both open in Excel with native line charts.

### 11. Excel — find your own claim in the figures
**Do:** In `value.xlsx`, find the row for the month you notified `REF`.
**Expect:** That month's **Claims** count includes yours, and **Paid to date** includes your
`11,200.00`, because the claim settled inside the reporting window.
**Do:** In `frequency.xlsx`, find the same month and the month you closed it.
**Expect:** *Claims notified* counts yours in the first, and *Claims concluded* counts it in
the second. If both happened in the same month, both figures include it and the net effect on
*Open at month end* is nil.

### 12. Teams — post to the management channel
**Do:** New post in the channel: subject `Monthly claims summary - <period>`. Body covering
total notified, total concluded, closing open figure, and the paid total for the period. Attach
both workbooks. Post.
**Expect:** The post appears with both files attached, and both appear in the channel Files tab.

### 13. Outlook — email the supervisor
**Do:** Compose to `SUPERVISOR-MAILBOX`, subject `Monthly claims summary - <period>`, the same
figures as the Teams post, both workbooks attached, and a closing line noting that `REF`
completed its full lifecycle within the period.
**Do:** Send.
**Expect:** In Sent Items with both attachments.

### 14. Cross-check the two channels agree
**Do:** Compare the figures in the Teams post against the email.
**Expect:** Identical. They were read from the same two workbooks.

## Pass criteria

- [ ] `REF` was created from an email with no policy number, and correctly judged a new loss
- [ ] The claim passed through all eight statuses in the order listed in step 9
- [ ] Every reason-requiring transition refused an empty reason
- [ ] Three roles were used, each seeing a different set of available actions
- [ ] Correspondence was recorded at all four boundary crossings: the inbound notification,
      the chaser out, the submission out, and the underwriter's reply in
- [ ] Net paid is `11,200.00` and **Paid net** on Details agrees with it
- [ ] The claim is Closed, terminal, and still fully readable
- [ ] Both reports exported with native charts
- [ ] `REF` is visible in both reports, in the months it was notified and concluded
- [ ] The Teams post and the email carry identical figures, with both workbooks attached

## Negative checks

- [ ] At no point did Claims Manager send or receive anything. Outlook and Teams were driven
      by the operator; the application holds only the record that it happened.
- [ ] No status was reached by any route other than an offered action
- [ ] The claim was never deleted, only closed

## Legitimate variation

The claim reference, all timestamps, and every report figure other than the `11,200.00` your
own payments contributed. The month boundaries will differ depending on the date you run it —
if you run it near the start of a month, the notification and closure may fall in different
months, which changes step 11's expectation from "both in one month" to one in each.

## Reset

Delete `Claims.mdb` and the `Documents` folder beside the executable. Delete the workbooks,
the Teams post and the test mail.
