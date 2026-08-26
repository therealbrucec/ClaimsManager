# E2E-08 — The frequency report is posted to a Teams channel

**Proves:** that the frequency report reflects claims leaving the book as well as arriving,
that its running total is internally consistent, and that the workbook can be shared to a
Teams channel and opened from there.

**Applications:** Claims Manager → Excel → Teams
**Role:** Claims Handler (`jbaker`)
**About:** 7 minutes

## Preconditions

- Excel installed.
- Teams open, with write access to a channel. These steps call it *the management channel*.
- Database rebuilt from the fixed seed.
- Signed in as `jbaker`.

## Steps

### 1. Claims Manager — export the frequency report
**Do:** `Reports > Export to Excel...` then **Export claim frequency by month...**
Save as `claim-frequency.xlsx`. Open it.
**Expect:** A sheet named **Claim frequency**, one row per month, with four columns: Month,
Claims notified, Claims concluded, Open at month end.
**Expect:** A line chart titled `Claim frequency by month`, three series, value axis titled
`Number of claims`.

### 2. Claims Manager — confirm the status filter is irrelevant here
**Do:** Back in the dialog, **Clear all** so no status is ticked. Export the frequency report
again to a different name. Open it.
**Expect:** **Identical figures.** The frequency report counts every claim regardless of
status, which is what the grey note on the dialog says. Only the value report is filtered.

### 3. Excel — check the running total is consistent
**Do:** For each row, compute the previous row's *Open at month end*, plus this row's *Claims
notified*, minus this row's *Claims concluded*.
**Expect:** It equals this row's *Open at month end*, every time. The first row starts from
however many claims were already open when the period began, so it need not start at its own
notified figure.
**Expect:** *Open at month end* is never negative.

### 4. Excel — sanity-check the shape
**Expect:** *Claims notified* is non-zero in most months. *Claims concluded* is low or zero in
the most recent month or two and higher in older months, because recent claims have not had
time to conclude. If concluded exceeded notified in every month, the book would be emptying,
which the seeded data is not.

### 5. Teams — post it to the channel
**Do:** In the management channel, start a new post.
- Subject: `Claim frequency - <period>`
- Body: the period covered, total notified, total concluded, and the closing *Open at month
  end* figure. Add one sentence on whether the book grew or shrank over the period.

**Do:** Attach `claim-frequency.xlsx` to the post.
**Do:** Post it.
**Expect:** The post appears in the channel with the file attached.

### 6. Teams — verify from the recipient's side
**Do:** Open the attached workbook from the Teams post.
**Expect:** It opens, either in Teams or in Excel, with the chart intact.
**Do:** Check the **Files** tab of the channel.
**Expect:** `claim-frequency.xlsx` is listed there, because a channel post attachment lands in
the channel's document library.

### 7. Teams — reply with the figure that matters
**Do:** Reply to your own post with just the closing open-claims figure and how it compares to
twelve months earlier.
**Expect:** The reply threads under the post rather than starting a new one.

## Pass criteria

- [ ] The export produced an `.xlsx` that opens without a repair prompt
- [ ] It contains a native line chart with three series and a titled value axis
- [ ] Every month in the period has a row
- [ ] Clearing all statuses did not change the frequency figures
- [ ] The running total reconciles on every row: previous open, plus notified, minus concluded
- [ ] Open at month end is never negative
- [ ] The post appears in the channel with the workbook attached
- [ ] The workbook opens from Teams with its chart intact and appears in the channel Files tab

## Negative checks

- [ ] The status tick list had no effect on this report, unlike the value report
- [ ] Claims Manager did not post to Teams. It produced the workbook; the operator posted it.

## Legitimate variation

All figures, month labels and the shape of the lines, since the window moves with today's
date. The reconciliation in step 3 must hold regardless.

## Reset

Delete the workbooks. Optionally delete the Teams post. Nothing in the application changed.
