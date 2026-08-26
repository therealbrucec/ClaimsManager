# E2E-03 — The insured is chased for what is missing

**Proves:** that a status change carries its reason into the permanent record, that the reason
is what the outbound email is built from, and that the reply closes the loop back into the
claim file.

**Applications:** Claims Manager → Outlook → Claims Manager
**Role:** Claims Handler (`jbaker`)
**About:** 6 minutes

## Preconditions

- The claim from **E2E-01** exists and is at status **Notified**.
- Outlook open, with the ability to send to `INSURED-MAILBOX`.

## Steps

### 1. Claims Manager — open the claim and request documents
**Do:** `Claims > Open by reference...` and enter the reference from E2E-01.
**Do:** **Move claim on...** and choose **Request further documents**.
**Expect:** The confirmation dialog opens, titled `Confirm decision - reason required`. The
grey line beneath the box states that a reason is required.

### 2. Claims Manager — try to proceed without a reason
**Do:** Leave the reason empty and click **Confirm**.
**Expect:** Blocked, with a message telling you to type the reason before confirming. The
status has not changed.

### 3. Claims Manager — give the reason
**Do:** Enter a reason naming exactly what is outstanding, for example:
`Awaiting a plumber's report on the failed pipe, a schedule of damaged stock with values, and
the receipt for the second display unit.`
**Do:** **Confirm**.
**Expect:** Status changes to **Documents Pending**, shown in the header.

### 4. Claims Manager — read back what will go in the letter
**Do:** Open the **History** tab.
**Expect:** The status history shows `Notified` to `Documents Pending`, timestamped, against
`jbaker`, with the reason text exactly as typed.
**Do:** Open the **Details** tab and note the insured's name and contact email.

### 5. Outlook — write the chaser
**Do:** Compose a new message to `INSURED-MAILBOX`.
- Subject: `Your claim <reference> - information needed`
- Body: address the insured by name, quote the claim reference, and list the three items from
  the reason text you entered in step 3.

**Do:** Send it.
**Expect:** The message appears in Sent Items.

### 6. Claims Manager — record that it went out
**Do:** On the claim, **Correspondence** tab, **Record an item against this claim...**
**Do:** Arrived by **Email**, from your own address, subject as sent, and paste the body.
**Do:** **Save to claim**.
**Expect:** The item appears on the Correspondence tab.

### 7. Outlook — the insured replies
**Do:** Reply from `INSURED-MAILBOX` attaching `invoice-4471.pdf`, saying the plumber's report
will follow.
**Do:** Save the attachment to disk.

### 8. Claims Manager — file the reply and move the claim on
**Do:** **Record an item against this claim...**, capture the reply, attach the PDF as type
**Invoice**, save to claim.
**Do:** **Move claim on...** and choose **Refer to loss adjuster**.
**Expect:** The claim moves to **With Adjuster** without complaint, because a date of loss, a
peril, an estimate above zero, a named adjuster and at least one document are all present.

## Pass criteria

- [ ] Confirming with an empty reason was refused, and the status did not change
- [ ] The reason text appears verbatim in the status history against the right user and time
- [ ] The chaser email exists in Sent Items and its content matches the recorded reason
- [ ] Three items are on the Correspondence tab: the original email, the outbound chaser, and
      the reply
- [ ] The claim is now **With Adjuster**
- [ ] The `Docs` count on the reply row is 1, and the invoice is on the Documents tab

## Negative checks

- [ ] Claims Manager did not send the email. The operator composed and sent it in Outlook. The
      application only holds the record that it happened.
- [ ] The outbound item is recorded as arriving by Email but is distinguishable from inbound
      mail by its `Direction` column.

## Legitimate variation

Wording of the chaser, and all timestamps.

## Reset

None. E2E-04 continues from **With Adjuster**.
