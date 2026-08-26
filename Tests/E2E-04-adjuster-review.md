# E2E-04 — An adjuster reviews, refers back, and the handler responds

**Proves:** that the same claim presents differently to three roles, that an adjuster's
decision is theirs alone to make, and that a referral back travels out to a third party and
returns.

**Applications:** Claims Manager (three sign-ins) → Outlook
**Role:** Adjuster (`rpatel`), then Handler (`jbaker`), then Supervisor (`shollis`)
**About:** 8 minutes

## Preconditions

- The claim from E2E-01 is at **With Adjuster**, with **Rita Patel** as its loss adjuster.
- Fixture **FIX-03** available to send from `ASSESSOR-MAILBOX`.

## Steps

### 1. Claims Manager — what the handler cannot do
**Do:** Signed in as `jbaker`, open the claim. Click **Move claim on...**
**Expect:** The button is **disabled** and reads **No action available**. A handler has no
route out of With Adjuster.

### 2. Claims Manager — sign in as the adjuster
**Do:** `File > Sign out`, confirm, then sign in as `rpatel` with `Pa55w0rd`.
**Expect:** The application opens on the **Adjuster review queue**, not on a claim list. The
title bar reads `... - Rita Patel (Adjuster)`.

### 3. Claims Manager — confirm the adjuster cannot rewrite the file
**Do:** Open the claim from the queue. Go to the **Details** tab.
**Expect:** Every text field is read only. The peril, handler, adjuster and underwriter
combos are disabled. **Save claim** is disabled.
**Expect:** `Intake` menu items for recording post are disabled, and so is **New claim**.

### 4. Claims Manager — read the file, then refer it back
**Do:** Read the **Correspondence** and **Documents** tabs. Note that the plumber's report
promised in E2E-03 has not arrived.
**Do:** **Move claim on...**
**Expect:** Exactly three options: **Approve claim**, **Reject claim**, **Refer back for more
information**.
**Do:** Choose **Refer back for more information**. Enter:
`Cause of loss is not established. Need the plumber's report on the failed pipe before I can
say whether this is sudden failure or gradual seepage.`
**Do:** **Confirm**.
**Expect:** Status becomes **Referred For Info**.

### 5. Claims Manager — add a note for whoever picks it up
**Do:** **Notes** tab. Type `Referred back pending plumber's report. Will approve if it
confirms sudden failure.` Leave **Internal only** ticked. **Add note**.
**Expect:** The note appears with visibility `Internal`, against `rpatel`.

### 6. Claims Manager — back to the handler
**Do:** Sign out, sign in as `jbaker`.
**Do:** Open the claim.
**Expect:** Status **Referred For Info**. **Move claim on...** now offers **Return to
adjuster** and **Withdraw claim** — the handler's options, not the adjuster's.

### 7. Outlook — instruct the assessor
**Do:** Compose to `ASSESSOR-MAILBOX`, subject `Claim <reference> - inspection required`,
asking for an inspection and quoting the adjuster's reason from step 4.
**Do:** Send.

### 8. Outlook — the report comes back
**Do:** Send fixture **FIX-03** from `ASSESSOR-MAILBOX` to the test mailbox. Save
`inspection-report.pdf` to disk.

### 9. Claims Manager — file it and return the claim
**Do:** **Record an item against this claim...**, arrived by **Email**, from the assessor,
attach the PDF as type **Report**. **Save to claim**.
**Do:** **Move claim on...** then **Return to adjuster**. Confirm.
**Expect:** Status back to **With Adjuster**.

### 10. Claims Manager — the adjuster approves
**Do:** Sign in as `rpatel`, open the claim from the review queue.
**Expect:** The assessor's report is on the Documents tab, and the internal note from step 5
is still on the Notes tab.
**Do:** **Move claim on...** then **Approve claim**, with reason
`Report confirms sudden failure of the mains feed. Covered. Figures are reasonable.`
**Expect:** Status **Adjuster Approved**.

### 11. Claims Manager — the supervisor's override exists, and only theirs
**Do:** Sign in as `shollis`. Open the claim.
**Expect:** **Move claim on...** offers **Send to underwriter** *and* **Send back to
adjuster**. The second is the supervisor-only reversal.
**Do:** Do not use it. Sign out.

## Pass criteria

- [ ] As handler at With Adjuster, the action button was disabled and read *No action available*
- [ ] The adjuster landed on the review queue at sign-in, not on a claim list
- [ ] The Details tab was read only for the adjuster, and Save was disabled
- [ ] The adjuster was offered exactly three decisions, all requiring a reason
- [ ] Status history shows the full chain: Notified, Documents Pending, With Adjuster,
      Referred For Info, With Adjuster, Adjuster Approved — each with its reason and its user
- [ ] The internal note survived the round trip and is attributed to `rpatel`
- [ ] The supervisor, and only the supervisor, was offered **Send back to adjuster**

## Negative checks

- [ ] The handler was never able to approve, reject or refer the claim
- [ ] The adjuster was never able to record post, create a claim, or edit the Details tab
- [ ] No reason field accepted an empty value on any of the three adjuster decisions

## Legitimate variation

Timestamps, and the wording of every reason and note.

## Reset

None. E2E-05 continues from **Adjuster Approved**.
