# End-to-end workflow tests

Nine multi-application workflows covering Claims Manager from first notification to closed
file, including the parts that happen outside it: mail arriving in Outlook, correspondence
going back out, and reports reaching management through Teams and email.

## Why these are written, not coded

Every workflow here spans at least two applications, and most span three or four: Outlook,
Claims Manager, File Explorer, Excel, Teams. That is deliberate, because it is the shape of
the real job — the desk is the integration between systems that do not talk to each other.

It is also why these are **specifications rather than scripts**. A scripted test that drives
Outlook, then a WinForms window, then Excel, then Teams breaks the first time any one of
those four redraws a dialog, and the maintenance falls on whoever is least able to refuse it.
These files describe intent, preconditions and pass criteria precisely enough to be executed
by a person or interpreted by an agent, and to be read as the specification of what the
application is supposed to do.

They live in the repository next to the code so they are versioned with it. When behaviour
changes, the test changes in the same commit.

## The suite

| Test | Workflow | Applications |
|---|---|---|
| [E2E-01](E2E-01-new-claim-from-email.md) | A new loss arrives by email and becomes a claim | Outlook, Explorer, Claims Manager |
| [E2E-02](E2E-02-documents-onto-existing-claim.md) | Further documents are filed against a claim already open, without duplicating it | Outlook, Claims Manager |
| [E2E-03](E2E-03-chase-the-insured.md) | The insured is chased for what is missing | Claims Manager, Outlook |
| [E2E-04](E2E-04-adjuster-review.md) | An adjuster reviews, refers back, and the handler responds | Claims Manager (three roles), Outlook |
| [E2E-05](E2E-05-underwriter-submission.md) | A submission pack goes to an underwriter and the reply comes back | Claims Manager, Explorer, Outlook |
| [E2E-06](E2E-06-settlement-and-closure.md) | Payment is made, advice is sent, the file is closed | Claims Manager, Outlook |
| [E2E-07](E2E-07-value-report-by-email.md) | The claim value report goes to the supervisor by email | Claims Manager, Excel, Outlook |
| [E2E-08](E2E-08-frequency-report-to-teams.md) | The frequency report is posted to a Teams channel | Claims Manager, Excel, Teams |
| [E2E-09](E2E-09-full-lifecycle.md) | One claim from email to closed, then reported to management both ways | All five |

Run them in order for a first pass: 01 creates the claim that 02 to 06 carry forward. After
that any of them stands alone, given the preconditions it names.

## Before you start

**Reset to a known state.** Delete `Claims.mdb` from the folder holding `ClaimsManager.exe`
and start the application. It rebuilds 140 claims across the last twelve months, plus seven
items in the intake queue, from a fixed seed — so every reset produces an identical book and
the figures quoted in these tests hold. Delete the `Documents` folder too if a previous run
left attachments behind.

**Sign-in.** User names are typed, not picked from a list. All five accounts share the
password `Pa55w0rd`, and `demo-accounts.csv` sits beside the executable listing them with
what each role may do. The accounts these tests use:

| Account | Role | Used for |
|---|---|---|
| `jbaker` | Handler | intake, claim creation, submission, payment |
| `rpatel` | Adjuster | review decisions |
| `shollis` | Supervisor | reversals, and the recipient of reports |

**Mail addresses need substituting.** Every address in the seeded data ends in `.example`,
which is reserved and does not route anywhere. Before running any test that sends mail,
decide on real test mailboxes and use those instead. See
[inbound-fixtures.md](inbound-fixtures.md), which also gives the messages to send into the
test mailbox so the Outlook side of these workflows has something to work with.

**Teams needs a channel.** Tests 08 and 09 post to a channel. Any channel the operator can
write to will do; the tests call it *the management channel*.

**Excel is required for the report tests.** Reports are drawn by driving Excel through COM.
Without Excel the export offers CSV instead and no chart is produced, which fails the pass
criteria of 07, 08 and 09.

## How a test is laid out

Each file has the same sections:

- **Proves** — the one thing the test exists to establish
- **Applications** and **Role** — what is involved and who is signed in
- **Preconditions** — the state that must hold before step 1
- **Steps** — numbered, each naming the application, the action, and what should happen
- **Pass criteria** — checkable statements; all must hold
- **Negative checks** — things that must *not* happen, where that is the point
- **Legitimate variation** — what may differ between runs without being a failure
- **Reset** — how to put the environment back

The distinction between *pass criteria* and *legitimate variation* matters most. Claim
references, timestamps and generated figures differ from machine to machine; statuses,
permissions and arithmetic do not. Anything not listed as legitimate variation is expected
to be identical every run.
