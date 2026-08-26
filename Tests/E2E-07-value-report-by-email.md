# E2E-07 — The claim value report goes to the supervisor by email

**Proves:** that the value report is produced by driving Excel, that its filter changes the
figures in a way that can be reasoned about, that the chart is real, and that the workbook
reaches a named recipient.

**Applications:** Claims Manager → Excel → Outlook
**Role:** Claims Handler (`jbaker`)
**About:** 7 minutes

## Preconditions

- Excel installed on the machine. Without it the export offers CSV and this test cannot pass.
- Database rebuilt from the fixed seed, so the figures are stable.
- Signed in as `jbaker`.
- Able to send to `SUPERVISOR-MAILBOX`.

## Steps

### 1. Claims Manager — open the reports dialog
**Do:** `Reports > Export to Excel...`
**Expect:** The **Period** dates default to one year back from today, through to today. Every
claim status is ticked in the list.

### 2. Claims Manager — take a baseline across all statuses
**Do:** **Export claim value over time...** Save as `value-all-statuses.xlsx`. Accept the
offer to open it.
**Expect:** Excel opens the workbook. A sheet named **Claim value** with a header row in bold
on a grey fill, and one row per month for the whole period, including months with no claims.
**Expect:** A **line chart with markers** sits below the figures, titled
`Claim value by month of notification (all statuses)`, with three series — Estimated value,
Reserve and Paid to date — a Month category axis and a `Value (GBP)` value axis.
**Do:** Note the total of the **Claims** column. Call it `TOTAL-CLAIMS`.

### 3. Excel — check the chart is a chart
**Do:** Click the chart. Look at the formula bar or the chart's source data.
**Expect:** It is a native Excel chart object bound to the cells on the sheet, not a pasted
image. Editing a figure on the sheet moves the line.
**Expect:** The **Claims** column sits in column E, **outside** the plotted range A to D, so
the small counts do not flatten the money axis.

### 4. Claims Manager — narrow to settled claims only
**Do:** Back in the reports dialog, **Clear all**, then tick only **Settled**.
**Do:** **Export claim value over time...** Save as `value-settled.xlsx`. Open it.
**Expect:** The chart title now reads `... (1 of 12 statuses)`.
**Expect:** The **Claims** total is lower than `TOTAL-CLAIMS`, and **Paid to date** is
substantial rather than near zero — settled claims are the ones that paid out.

### 5. Claims Manager — narrow to open claims
**Do:** **Open claims** button, then **Export claim value over time...** Save as
`value-open.xlsx`. Open it.
**Expect:** **Paid to date** is at or near zero across every month, because open claims have
not paid. **Reserve** is substantial. This is the outstanding exposure.

### 6. Excel — reconcile the three
**Do:** Compare the **Claims** totals of the settled and open workbooks against
`TOTAL-CLAIMS`.
**Expect:** Settled plus open is **less than or equal to** the all-statuses total, because
withdrawn, rejected and declined claims are in neither. If settled plus open exceeded the
total, something is double counting.

### 7. Outlook — send it to the boss
**Do:** Compose to `SUPERVISOR-MAILBOX`:
- Subject: `Claim value by month - <the period, e.g. Aug 2025 to Aug 2026>`
- Body: state the period, that the figures are by month of notification, the number of claims
  in the all-statuses run, and one sentence on what the settled-versus-open comparison shows.
- Attach: all three workbooks.

**Do:** Send.
**Expect:** In Sent Items with three attachments.

### 8. Outlook — verify what the recipient receives
**Do:** Open the sent message and the attachment `value-settled.xlsx` from within Outlook.
**Expect:** It opens in Excel with the chart intact. The workbook survived the round trip.

## Pass criteria

- [ ] All three exports produced `.xlsx` files that open in Excel without a repair prompt
- [ ] Each contains a native line chart with markers, three series, and both axes titled
- [ ] Every month in the period has a row, including empty ones, so no gaps in the line
- [ ] The Claims column is outside the charted range
- [ ] The chart title names the status filter, and changes between the three runs
- [ ] Paid to date is near zero on the open-claims run and substantial on the settled run
- [ ] Settled claims plus open claims does not exceed the all-statuses claim count
- [ ] The email reached Sent Items with all three workbooks, and one opens from Outlook

## Negative checks

- [ ] No CSV fallback was offered. If it was, Excel is not installed and the test is void
      rather than failed.
- [ ] Claims Manager did not send the mail. It produced the workbooks; the operator attached
      and sent them.

## Legitimate variation

Every figure, because the demo book depends on today's date — the twelve month window moves.
The claim counts, month labels and monetary totals are all expected to differ from any
previous run. What must hold is the **relationships** asserted above, not the numbers.

## Reset

Delete the three workbooks. Nothing in the application changed; reports only read.
