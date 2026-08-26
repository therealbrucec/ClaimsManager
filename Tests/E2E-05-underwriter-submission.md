# E2E-05 — A submission pack goes to an underwriter and the reply comes back

**Proves:** that the application refuses to send a claim onward until it is fit to send, that
assembling and transmitting the pack is the operator's job outside the application, and that
the reply is captured as a decision with a date.

**Applications:** Claims Manager → File Explorer → Outlook → Claims Manager
**Role:** Claims Handler (`jbaker`)
**About:** 8 minutes

## Preconditions

- The claim from E2E-01 is at **Adjuster Approved**.
- Signed in as `jbaker`.
- Able to send to and receive from `UNDERWRITER-MAILBOX`.

## Steps

### 1. Claims Manager — try to send it before it is ready
**Do:** Open the claim. On the **Details** tab, confirm **Underwriter** reads
`(not chosen)`. Set **Reserve** to `0`. **Save claim**.
**Do:** **Move claim on...** then **Send to underwriter**.
**Expect:** Blocked before any dialog appears, with a message naming what is missing —
choosing an underwriter. The status is unchanged.

### 2. Claims Manager — half-fix it, and be refused again
**Do:** Set **Underwriter** to **Northgate Syndicate**. Leave **Reserve** at `0`. **Save claim**.
**Do:** **Move claim on...** then **Send to underwriter**.
**Expect:** Blocked again, this time naming the reserve. The gate checks both conditions
independently.

### 3. Claims Manager — make it fit to send
**Do:** Set **Reserve** to `7000`. **Save claim**.

### 4. Claims Manager — gather the pack
**Do:** **Documents** tab. Note every document listed.
**Do:** **Details** tab. Note the policy number, insured, date of loss, peril, estimate and
reserve. Note the underwriter's contact address shown for **Northgate Syndicate** — this is
the address the pack must go to.

### 5. Explorer — collect the files
**Do:** Open `Documents\<claim reference>` beside the executable.
**Expect:** Every document listed on the Documents tab is present as a file.

### 6. Outlook — send the submission
**Do:** Compose to `UNDERWRITER-MAILBOX`:
- Subject: `Claim submission - <claim reference> - <insured name>`
- Body: policy number, insured, date of loss, peril, a summary of the loss, the estimate and
  the reserve, and the adjuster's approval reason from the History tab.
- Attach: every file from step 5.

**Do:** Send.
**Expect:** The message is in Sent Items with all attachments.

### 7. Claims Manager — record that it went
**Do:** **Move claim on...** then **Send to underwriter**. No reason is required for this one,
but enter `Pack emailed to Northgate Syndicate with photographs, invoice and assessor report.`
**Do:** **Confirm**.
**Expect:** Status **Sent To Underwriter**. The History tab shows the change stamped with the
date, the time and `jbaker`.

### 8. Claims Manager — record the outbound correspondence
**Do:** **Record an item against this claim...** capturing the submission email.
**Expect:** It appears on the Correspondence tab.

### 9. Outlook — the underwriter replies
**Do:** Send fixture **FIX-04** from `UNDERWRITER-MAILBOX`, with the claim reference filled in.

### 10. Claims Manager — capture the decision
**Do:** **Record an item against this claim...** capturing the reply.
**Do:** **Move claim on...** then **Record underwriter agreement**. Confirm.
**Expect:** Status **Underwriter Agreed**.

### 11. Claims Manager — check what a decline would have demanded
**Do:** **Move claim on...** and read the options.
**Expect:** Only **Record settlement** is offered from Underwriter Agreed. The decline route
was available only from Sent To Underwriter, and would have required a written reason.

## Pass criteria

- [ ] Sending was blocked with no underwriter chosen, naming that specific gap
- [ ] Sending was blocked again with a zero reserve, naming that gap
- [ ] Once both were set, the claim moved to **Sent To Underwriter**
- [ ] The submission email exists in Sent Items with every document from the claim folder
- [ ] The address used matches the one the application holds for Northgate Syndicate
- [ ] Status history shows Sent To Underwriter then Underwriter Agreed, both timestamped
      against `jbaker`
- [ ] Both the submission and the reply are on the Correspondence tab

## Negative checks

- [ ] The application did not generate the pack. The operator assembled it from the Documents
      tab and the claim folder, and Outlook sent it. Claims Manager holds only the record.
- [ ] The application did not transmit anything, and has no Sent Items of its own.

## Legitimate variation

Which underwriter you chose, the wording of the submission, and all timestamps.

## Reset

None. E2E-06 continues from **Underwriter Agreed**.
