# E2E-02 — Further documents are filed against a claim already open

**Proves:** that a second email about a loss already on the books is attached to the existing
claim rather than opening a duplicate, and that the application actively resists duplication.

**Applications:** Outlook → Claims Manager
**Role:** Claims Handler (`jbaker`)
**About:** 5 minutes

## Preconditions

- Signed in as `jbaker`.
- A real policy number taken from the seeded book. Get one: `Claims > All claims`, untick
  **Open claims only**, and note the `Policy No` and `Insured` of any claim whose status is
  not Closed. Call these `TARGET-POLICY` and `TARGET-INSURED`.
- Fixture **FIX-02** sent to the test mailbox, with `EM/2211907` replaced by `TARGET-POLICY`.

## Steps

### 1. Outlook — read the message
**Do:** Open *Receipts as requested*. Note that the body quotes `TARGET-POLICY`.
**Do:** Save `invoice-4471.pdf` to disk.

### 2. Claims Manager — capture it
**Do:** `Intake > Record inbound item...`. Complete from the email. Set *File this next as*
to **Invoice** and add the PDF.
**Do:** **Save and find claim...**

### 3. Claims Manager — the application should find the claim for you
**Expect:** On the match screen, the **Policy no** box is **already filled in** with
`TARGET-POLICY`, and the hints line names it, saying it was pre-filled from the item. This is
the point of the test: the operator did not have to search.
**Expect:** The candidates grid lists at least the target claim. The result line reads
`One possible match. Check it before attaching.` or names a higher number.

### 4. Claims Manager — try the wrong thing first
**Do:** With matches on screen, click **Create new claim from this item**.
**Expect:** A warning appears, stating how many possible matches are listed and asking
whether to open a new claim anyway.
**Do:** Answer **No**.
**Expect:** You are returned to the match screen. No claim was created.

### 5. Claims Manager — attach it to the right claim
**Do:** Select the target claim in the grid. **Attach to selected claim**.
**Expect:** A confirmation naming the claim reference and warning that attachments will be
moved into that claim's folder. Confirm.
**Expect:** A message confirming it was filed, offering to open the claim. Accept.

### 6. Claims Manager — verify the filing
**Expect:** On the target claim, the **Correspondence** tab lists the new item with a `Docs`
count of 1, and the **Documents** tab lists `invoice-4471.pdf` as type `Invoice`.

### 7. Explorer — verify the document moved
**Do:** Look in `Documents\<target claim reference>` beside the executable.
**Expect:** `invoice-4471.pdf` is there. It is **not** in `Documents\Unfiled`.

### 8. Claims Manager — verify the audit trail on both records
**Do:** Open the target claim's **History** tab.
**Expect:** The audit grid carries an entry recording that correspondence was filed against
the claim, and another recording the documents being filed from intake.

## Pass criteria

- [ ] The policy number was pre-filled on the match screen without the operator typing it
- [ ] Creating a new claim while matches were listed produced a warning, and answering No
      created nothing
- [ ] The item is on the target claim's Correspondence tab
- [ ] The PDF is on the Documents tab and has physically moved into the claim's folder
- [ ] The audit trail records the filing against the claim
- [ ] Total claim count is unchanged from before the test

## Negative checks

- [ ] No new claim exists. Compare the claim count on `All claims` with *Open claims only*
      unticked, before and after.
- [ ] `Documents\Unfiled` does not contain `invoice-4471.pdf`.

## Legitimate variation

Which claim you targeted, and therefore its reference, insured and policy number. The number
of candidates the loose search returns — matching on policy number, insured name and a date
window will sometimes surface more than one, which is intended.

## Reset

None. The extra document on the target claim is harmless.
