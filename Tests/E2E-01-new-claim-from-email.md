# E2E-01 — A new loss arrives by email and becomes a claim

**Proves:** that an unstructured email with attachments can be captured, correctly judged to
be a new loss rather than an existing one, and turned into a claim with its documents filed
against it.

**Applications:** Outlook → File Explorer → Claims Manager
**Role:** Claims Handler (`jbaker`)
**About:** 5 minutes

## Preconditions

- Database freshly rebuilt. The status bar reads `7 item(s) waiting to be matched`.
- Fixture **FIX-01** has been sent to the test mailbox and is unread in the inbox.
- Signed out of Claims Manager.

## Steps

### 1. Outlook — read what arrived
**Do:** Open the message *Water damage at our unit - urgent*. Read it.
**Expect:** Two image attachments. No policy number anywhere in the body, and no explicit
date of loss beyond "over the weekend".

### 2. Explorer — get the attachments onto disk
**Do:** Save both attachments to a folder you can find again, e.g. `Documents\Downloads`.
**Expect:** `damage-01.jpg` and `damage-02.jpg` on disk.

### 3. Claims Manager — sign in
**Do:** Start Claims Manager from the **Claims Manager** entry in the Start menu. Sign in as
`jbaker` with `Pa55w0rd`.
**Expect:** Splash screen carrying the Northbridge Assurance mark, then the MDI window titled
`Northbridge Assurance - Claims Manager - Jane Baker (Handler)`, opening on **My claims**.
The title bar carries the Northbridge icon.

### 4. Claims Manager — record what arrived
**Do:** `Intake > Record inbound item...`. Fill in from the email, not from memory:
- Arrived by: **Email**
- Received: the date and time Outlook shows on the message
- From: `Douglas Fairbairn`
- Email or fax no: the sending address
- Subject: copy the subject line
- Text of item: paste the body

**Do:** Set *File this next as* to **Photo**, then **Add files...** and select both JPEGs.
**Expect:** Both appear in the list with type `Photo`.

### 5. Claims Manager — make the new-or-existing decision
**Do:** **Save and find claim...**
**Expect:** The window titled **New claim or existing claim?** opens. The grey hints line
reports what it could extract — the sender surname `Fairbairn` — and says nothing about a
policy number, because the email contained none. The candidates grid is empty or lists
nothing plausible, and the result line reads
`No existing claim matches. This looks like a new loss.`

### 6. Claims Manager — create the claim
**Do:** **Create new claim from this item**.
**Expect:** A new claim window opens, pre-filled from the email: insured name `Douglas
Fairbairn`, the contact email populated, date notified set to the received date, and the loss
description containing the pasted body.

**Do:** Complete what the email did not give you:
- Policy number: `NGC/884213`
- Peril: **Escape of Water**
- Estimated value: `6400`
- Reserve: `7000`
- Date of loss: the Saturday before the email; leave *Date is known* ticked
- Loss adjuster: **Rita Patel**

**Do:** **Save claim**.
**Expect:** A reference is allocated in the form `CLM-2026-000NN`. **Write it down** — later
tests need it. The header shows status **Notified**.

### 7. Claims Manager — confirm the item and its documents followed the claim
**Do:** Open the **Correspondence** tab, then the **Documents** tab.
**Expect:** Correspondence lists the email, arrived by Email, with a `Docs` count of 2.
Documents lists both JPEGs as type `Photo`.

### 8. Explorer — confirm the document store
**Do:** Open the folder holding `ClaimsManager.exe`, then `Documents\<the claim reference>`.
**Expect:** Both JPEGs are there. They were moved out of `Unfiled` when the item was matched.

### 9. Claims Manager — confirm the queue shrank
**Expect:** The status bar now reads `7 item(s) waiting to be matched` — unchanged, because
this item was captured and matched in one pass and never sat in the queue.

## Pass criteria

- [ ] The match screen reported no existing claim, and said so in those terms
- [ ] A claim reference was allocated in the form `CLM-yyyy-NNNNN`
- [ ] Status is **Notified**
- [ ] Both photographs are listed on the Documents tab and present on disk under the claim's
      own folder, not under `Unfiled`
- [ ] The Correspondence tab shows one inbound Email item with a `Docs` count of 2
- [ ] The **History** tab shows one entry, to **Notified**, with reason text mentioning the
      claim was created
- [ ] The email body text is legible in the claim's *What happened* field

## Negative checks

- [ ] The application did not reach into Outlook. Every field was typed or pasted by the
      operator. If anything appeared without being entered, that is a defect.
- [ ] No second claim was created for the same email.

## Legitimate variation

The claim reference number, all timestamps, and the exact wording of the hints line.

## Reset

None needed; E2E-02 onwards build on this claim. To start over, delete `Claims.mdb` and the
`Documents` folder from beside the executable.
