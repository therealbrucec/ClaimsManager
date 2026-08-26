# Northbridge Assurance - Claims Manager

A broker claims-handling desk for the fictional **Northbridge Assurance**, built as a VB.NET
WinForms application against an Access
(Jet) database. It models the workflow it was described from: claim data arrives by email
and fax, a handler decides whether it belongs to a **new** or an **existing** claim, a loss
adjuster approves or rejects, and the claim then goes to underwriters who decide whether to
pay.

The look is deliberately period: MDI parent window, Tahoma 8.25pt, grey 3D controls, sunken
borders, menu and toolbar and status bar. It compiles with current tooling.

---

## Before you build

**Read this bit — it is the one thing that will otherwise waste your afternoon.**

The database is a `.mdb` reached through `Microsoft.Jet.OLEDB.4.0`. Jet ships with Windows,
so there is nothing to install, but **it is 32-bit only**. The project is therefore set to
`AnyCPU` with `Prefer32Bit = true`, which makes the process 32-bit on 64-bit Windows.

If you change that setting, the app will start and then fail on the first database call with
"provider is not registered". `Db.OpenConnection` catches exactly that case and says so.

The alternative (`Microsoft.ACE.OLEDB.12.0`) was deliberately avoided — it needs the Access
Database Engine redistributable downloaded onto every machine.

## Start-up

A splash screen carries the Northbridge Assurance mark while the application starts, and
reports what it is doing: building tables, adding users, generating claims, filling the
intake queue. On a first run that takes several seconds, so the progress matters; on later
runs it is held on screen for a moment so it does not flash past.

The logo is **compiled into the executable** as an embedded resource, so there is no loose
image file to lose. `frmSplash` finds it by matching the end of the resource name rather than
by its full manifest name, which depends on the root namespace and folder and is easy to get
subtly wrong. If the resource cannot be loaded the splash falls back to text and start-up
carries on.

To change the artwork, replace `Resources\NorthbridgeAssurance.png` and rebuild.

## Building

There are two outputs. The plain executable is unchanged; the installer is additional.

**The executable**

1. Open `ClaimsManager.sln` in Visual Studio 2022.
2. Target framework is .NET Framework 4.8. Build.
3. Run `bin\Debug\ClaimsManager.exe`. No installer, no configuration step.

**The installer**

`Installer\build-msi.cmd` builds Release and then packages it. It needs the WiX build
tool, installed once per machine:

```
dotnet tool install --global wix
```

Then, from a **Developer Command Prompt for VS 2022** so that msbuild is on the path:

```
Installer\build-msi.cmd
```

That produces `Installer\ClaimsManager.msi`. The authoring is a single readable file,
`Installer\ClaimsManager.wxs`, if you want to change the product name, version or paths.
It needs **no WiX extensions** — everything it uses is in the core schema — so there is
nothing to add beyond the `wix` tool itself.

**If `wix --version` reports a different version than you installed**, you have more than
one copy. The build script now lists every `wix.exe` it can find on PATH, in the order
Windows resolves them, so the culprit is visible. `dotnet tool list --global` shows what the
.NET tooling thinks is installed; a copy put there by Scoop, winget or an MSI will sit
earlier on PATH and win regardless.

**If the build fails with `WIX7015`**, WiX is waiting for you to accept the Open Source
Maintenance Fee EULA. WiX v6 introduced the fee and **v7 enforces it**, blocking every
command until the EULA is accepted. The build script does not accept it on your behalf,
because whether the fee applies is a licensing question rather than a build setting. Three
ways forward:

1. **Accept it for the build.** Set `ACCEPT_WIX_EULA=1` before running the script and it
   passes `-acceptEula wix7` to `wix.exe`. The switch takes the EULA id as its value; if a
   later WiX version uses a different one, set `WIX_EULA_ID` to override it.
   ```
   set ACCEPT_WIX_EULA=1
   Installer\build-msi.cmd
   ```
2. **Accept it once per user, per machine.** The OSMF page has a section covering this, for
   people who run `wix.exe` directly and would rather not pass the switch every time.
3. **Use WiX v5**, which predates the fee entirely and accepts the same authoring:
   ```
   dotnet tool uninstall --global wix
   dotnet tool install --global wix --version 5.*
   ```

If you drive the build from a `.wixproj` rather than this script, the equivalent is an
MSBuild property:

```xml
<PropertyGroup>
  <AcceptEula>wix7</AcceptEula>
</PropertyGroup>
```

Worth knowing before you choose: the fee applies to organisations generating revenue from
products built with WiX, with an exemption below $10,000 a year. Whether that covers using
it to package a demo application is not for this README to decide. The terms and a way to
check your organisation's payment status are at
[docs.firegiant.com/wix/osmf](https://docs.firegiant.com/wix/osmf/). Option 3 sidesteps the
question, which for a demo build is often the path of least friction.

**If the script says WiX was not found**, having just installed it: a global dotnet tool is
placed in `%USERPROFILE%\.dotnet\tools`, and that folder is added to the *user* PATH at
install time. A command prompt that was already open keeps its old copy of PATH and will
not see it. Either open a new prompt, or let the script find it — it now looks in the
standard global-tool locations as well as on PATH. `dotnet tool list --global` confirms
what is installed.

## End-to-end tests

`Tests/` holds nine multi-application workflow tests, versioned with the code. Every one spans
at least two applications and most span three or four — Outlook, Claims Manager, File Explorer,
Excel, Teams — because that is the shape of the actual job: the desk is the integration between
systems that do not talk to each other.

| Test | Workflow |
|---|---|
| E2E-01 | A new loss arrives by email and becomes a claim |
| E2E-02 | Further documents filed against a claim already open, without duplicating it |
| E2E-03 | The insured is chased for what is missing |
| E2E-04 | An adjuster reviews, refers back, and the handler responds |
| E2E-05 | A submission pack goes to an underwriter and the reply comes back |
| E2E-06 | Payment is made, advice is sent, the file is closed |
| E2E-07 | The claim value report goes to the supervisor by email |
| E2E-08 | The frequency report is posted to a Teams channel |
| E2E-09 | One claim from email to closed, then reported to management both ways |

They are **written as specifications rather than coded automation**, deliberately. A script
that drives Outlook, then a WinForms window, then Excel, then Teams breaks the first time any
one of the four redraws a dialog. These files state intent, preconditions and pass criteria
precisely enough to be executed by a person or interpreted by an agent, and to be read as the
specification of what the application should do.

Each test separates **pass criteria** from **legitimate variation**, which matters more than it
sounds: claim references, timestamps and generated figures differ between runs, while statuses,
permissions and arithmetic do not. Anything not listed as variation is expected to be identical
every time, which is what the fixed data seed buys.

`Tests/README.md` covers the reset procedure and the accounts. `Tests/inbound-fixtures.md`
gives the messages to send into a test mailbox, since the application has no mail integration
and the Outlook side of these workflows needs mail that genuinely exists. Note that all seeded
addresses end in `.example`, which does not route — real test mailboxes must be substituted
before any test that sends mail.

## Deploying with Intune

The MSI is **per-machine** (`ALLUSERS=1`), **32-bit**, **self-contained** (the cab is embedded,
so it is one file) and has **no UI authored**, which makes it silent-capable as it stands. It
installs to `C:\Program Files (x86)\Northbridge Assurance\Claims Manager\`. 32-bit is not an
oversight: the Jet database provider exists only as a 32-bit component, so the executable runs
as a 32-bit process and belongs there.

### Settings

| Setting | Value |
|---|---|
| Install command | `msiexec /i "ClaimsManager.msi" /qn /norestart` |
| Uninstall command | `msiexec /x "{8AC2ECD5-CEC0-5248-A807-E9B326123C8A}" /qn /norestart` |
| Install behaviour | **System** (device context) |
| Detection rule | MSI product code `{8AC2ECD5-CEC0-5248-A807-E9B326123C8A}` |
| Applicable OS architecture | **64-bit** — the package is 32-bit, which is intended and not a mismatch |
| Minimum OS | Windows 10 1903 or later, for .NET Framework 4.8 |
| Device restart behaviour | No specific action; nothing here asks for a reboot |

`build-msi.cmd` prints these at the end of a successful build so they can be copied straight
across.

### Line-of-business app, or Win32 app?

**Either works.** Uploading the `.msi` as a **line-of-business app** is the least work, because
Intune reads the name, version and product code out of the package itself, and this MSI meets
the requirements for it: a single file, per-machine, no external cab.

Wrapping it as a **Win32 app** gives more control and is the better-supported route. Package it
with the Microsoft Win32 Content Prep Tool:

```
IntuneWinAppUtil.exe -c <folder containing the msi> -s ClaimsManager.msi -o <output folder>
```

then use the commands from the table above.

### Windows 365 for Agents Cloud PCs

Cloud PCs for Agents are Microsoft Entra ID backed and Intune managed like any other Windows
device, so this MSI needs no changes and no special switches. What differs is that management
is **pool based rather than device based**: you group Cloud PCs with a provisioning policy, and
an agent **checks one out**, works, then returns it to the pool.

That model has three practical consequences for this application.

**Target devices, not users.** There is no interactive person to receive a user assignment, so
assign the app as **Required** to a **device group**, with install behaviour **System**. Build
a dynamic device group scoped to the Cloud PCs your agent provisioning policy creates, and
assign to that. The per machine MSI already suits this; nothing needs changing.

**Decide whether the app should be in the image instead.** Intune installs an app after the
device has enrolled and checked in. On a pooled Cloud PC that a agent may check out shortly
after provisioning, there is a window where the machine exists but the app is not on it yet.
If your agent runs are short and frequent, putting the application into a **custom device image**
for the provisioning policy is more dependable than racing the Intune agent. Deploying through
Intune is still right when you want the app updated independently of the image. Whichever you
choose, confirm the behaviour on a test pool before you rely on it, because the ordering is a
property of the platform rather than of this package.

**The first run builds the database, on every fresh machine.** The application generates 140
claims the first time it runs, which takes a few seconds behind a splash screen. On a recycled
Cloud PC that happens at the start of every session. Two ways to look at it:

- **Leave it.** The generator uses a fixed seed, so every machine produces a byte for byte
  identical book. For repeatable validation that is a feature, not a cost: each run starts from
  exactly the same state without anybody having to reset anything.
- **Pre-seed it.** Run the app once, copy the resulting `Claims.mdb` next to
  `ClaimsManager.wxs`, and uncomment the `PreSeededDatabase` component in that file. Start up
  then skips generation. Instructions are in the file.

Two smaller things worth knowing for agent driven runs:

- **The Start menu entry is the entry point.** The installer puts a single **Claims Manager**
  item in the All apps list, so an agent navigating the Start menu finds it by name rather than
  needing a path.
- **The sign in details are discoverable on the machine.** `demo-accounts.csv` sits beside the
  executable and lists the user names, the shared password and what each role may do, so an
  agent can read its own credentials rather than being told them out of band. That is only
  reasonable because the accounts are invented.
- **Reports need Excel on the Cloud PC.** Both reports drive Excel through COM to draw their
  charts. Without Excel installed, the export offers CSV instead and no chart is produced. If a
  demo is meant to cover reporting, check the image includes Excel.

Windows 365 for Agents was in public preview when this was written, so verify the specifics
against the current Microsoft documentation before depending on them.

### Points specific to this application

- **The product code is pinned** in `ClaimsManager.wxs`. Left to WiX, it would be regenerated on
  every build, and since both the uninstall command and the detection rule name it, you would be
  re-entering them in Intune after every rebuild. Change it when you change `ProductVersion` —
  that is what makes a new build a major upgrade, and `MajorUpgrade` then removes the old one.
  The `UpgradeCode` stays fixed forever.
- **The install folder is made writable, and you should know why.** The application keeps its
  database, documents and accounts file in its own folder, so the `Users` group is granted write
  access to it during installation, via the core `PermissionEx` element and an SDDL string. Be
  clear-eyed about this: granting users write access inside Program Files is something you would
  never do for a real product, because anyone who can write next to an executable can influence
  what it loads. It is acceptable here only because this is a demonstration application holding
  invented data.

  If that trade is unwelcome, point the installer somewhere users can already write to — change
  `ProgramFilesFolder` in `ClaimsManager.wxs` — and delete the `InstallFolderWritable` component.
- **Installation creates no data.** The database is built the first time somebody runs the
  application, in the install folder. All users of a machine therefore share one book, which for
  a demo is usually what you want. Nothing happens at install time beyond copying the executable
  and creating the shortcut, so a system-context install has nothing to get wrong.
- **Uninstalling removes the program, not the data.** `Claims.mdb`, `Documents\` and
  `demo-accounts.csv` were created by running the application rather than by the installer, so
  MSI does not know about them and leaves them behind. Delete the folder afterwards if you want
  the machine clean.
- **.NET Framework 4.8 is a prerequisite.** It is part of Windows 10 1903 and later and of
  Windows 11, so on any supported build it is already there. The installer checks for 4.x and
  stops with a readable message if it finds nothing, which Intune would report as a failure
  rather than a silent no-op.
- **Exit codes are the standard MSI ones.** 0 for success, 3010 for "success, reboot queued".
  Intune treats both as success by default, and nothing in this package requests a reboot anyway.

## The Start menu entry and the icon

The installer adds a single **Claims Manager** entry to the Windows All apps list, carrying
the Northbridge Assurance mark. It sits directly in the All Users Start menu rather than in
a folder, so it appears as one entry rather than something to expand.

The same icon appears in the title bar of every window, on the taskbar, in Explorer and on
the Apps and Features entry. It is built from the bridge device of the logo — the wordmark
is illegible below about 64 pixels — set on a white rounded tile, because the Windows 11
Start menu and taskbar are dark by default and navy on dark reads poorly. It ships at 16,
24, 32, 48, 64, 128 and 256 pixels.

To change it, replace `Resources\ClaimsManager.ico` and rebuild. It is referenced twice, as
the project's `ApplicationIcon` and as an embedded resource that `Core\AppIcon.vb` loads at
run time for the window icons.

On first run the app creates, **in the same folder as `ClaimsManager.exe`**:

| Path | What it is |
|---|---|
| `Claims.mdb` | the database, created via ADOX and populated with schema and seed data |
| `Documents\` | the document store; attachments are copied here, one folder per claim ref |
| `demo-accounts.csv` | sign-in details and the permissions each role has |

Keeping everything beside the executable means a demo is one folder: copy it, inspect it,
zip it up, throw it away. It also means each build configuration has its own database, so
`bin\Debug` and `bin\Release` do not share state.

**Nothing is written to the user profile.** There is no fallback location: if the folder the
executable sits in cannot be written to, the application says so at start-up and stops,
naming the folder and suggesting you copy it somewhere writable. That is deliberate — a
silent relocation would mean the database is not where the demo says it is.

The consequence is that **the folder has to be writable by whoever runs it**, which Program
Files is not for a standard user. The installer therefore grants the `Users` group write
access to the install folder. See the note under Deploying with Intune below, because it is
a trade worth understanding.

**If you have run an earlier build**, delete the leftover `%LOCALAPPDATA%\ClaimsManager`
folder by hand. Older versions kept their data there and nothing cleans it up.

Attachments are kept **on disk, not as BLOBs** — Jet databases bloat badly once you push
photographs into them, and it matches how these systems actually stored their post. The
database holds a path relative to `Documents\`, so the whole folder can be moved or backed
up independently.

Delete `Claims.mdb` and restart to get a clean database.

## Signing in

The user name is **typed**, not picked from a list, so the accounts are not on display.
Every account uses the same password: **`Pa55w0rd`** (capital P, zero for the o, digits for
the a and s). The role attached to the account is what actually drives permissions.

Because the list is no longer on the form, the first run writes **`demo-accounts.csv`** beside
the executable. It carries every user name, full name, role and the password, and then a
Yes/No column for each of the nine things a role can do, plus a one-line summary of the role:

```
UserName,FullName,Role,Password,Record inbound item,Match item to claim,Create claim,
Edit claim details,Refer to adjuster,"Approve, reject or refer back",Send to underwriter,
Reopen or reverse a decision,Add notes,Role summary
```

The workflow columns are not maintained by hand. `Core\RolePermissions.vb` asks
`ClaimWorkflow` what each role may actually do, so the file cannot drift from what the
application enforces. Only the screen-level rights — recording post, creating and editing
claims — are stated directly, because those live in the forms rather than in the state machine.

The sign-in dialog points at the file and `Help > About` shows its full path. Delete it and it
is rewritten on the next start; delete the `lblHint` line from `frmLogin.Designer.vb` to stop
the dialog mentioning it.

| User | Name | Role |
|---|---|---|
| `jbaker` | Jane Baker | Handler |
| `mokafor` | Michael Okafor | Handler |
| `rpatel` | Rita Patel | Adjuster |
| `dcampbell` | Douglas Campbell | Adjuster |
| `shollis` | Susan Hollis | Supervisor |

What each role can do:

- **Handler** — records inbound items, matches them to claims, creates and edits claims,
  refers to an adjuster, sends to underwriters, records payments.
- **Adjuster** — sees the review queue and lands on it at sign-in. Approves, rejects or
  refers back for more information. Cannot create claims, file post, or edit claim details;
  the Details tab is read-only for them. Can still add notes.
- **Supervisor** — everything a handler can do, plus the override transitions: sending an
  approved claim back to the adjuster, and reopening a rejected or declined claim.

## Demo data

A fresh database is populated with **140 claims spread over the twelve months ending on the
day the database is created**, plus
their payments, status history and correspondence, and seven items waiting in the intake
queue. The generator is seeded with a fixed number, so every rebuild produces exactly the
same book — handy when you want to compare two runs of a report.

It is shaped rather than uniform, because a flat spread makes for useless reports:

- **Perils are weighted** like a real UK commercial book. Escape of water dominates by
  frequency; fire, subsidence and liability dominate by value.
- **Values are skewed** towards the bottom of each peril's band, so there are many small
  claims and a few large ones rather than an even scatter.
- **Status follows age.** Claims from seven or more months ago are mostly settled or closed;
  those from the last month or two are still at notification, awaiting documents, or with an
  adjuster. Older claims carry a concluding history entry dated some weeks after
  notification, which is what the frequency report reads to count claims leaving the book.
- Settled claims have real payments against them, sometimes an interim followed by a final.

Because most of a year-old book has finished, **roughly half the claims are settled, closed
or withdrawn**. Those are excluded by the `Open claims only` tick on the claim list, so untick
it to see the full book — the count line below the grid tells you how many are being held
back. `Claims > All claims` is the unfiltered master list.

The reporting period defaults to the same full year, so nothing in the database falls outside
it. First run takes a few seconds while several hundred rows are written to the Jet file.
Delete `Claims.mdb` to regenerate.

## The workflow

`Core/ClaimWorkflow.vb` is the only place that decides what a claim may do next. Forms ask
it what to offer and never move a claim themselves.

```
              ┌─────────────┐
              │  Notified   │
              └──────┬──────┘
        ┌────────────┼────────────────┐
        v            v                v
┌───────────────┐  ┌───────────────┐  ┌───────────┐
│Documents      │  │With Adjuster  │  │ Withdrawn │ (terminal)
│Pending        ├─>│               │  └───────────┘
└───────────────┘  └───┬───┬───┬───┘
                       │   │   └──────────────┐
              ┌────────┘   └────────┐         v
              v                     v   ┌──────────────────┐
     ┌─────────────────┐  ┌─────────────┴───┐ │Referred For Info│
     │Adjuster Approved│  │Adjuster Rejected│ └────────┬────────┘
     └────────┬────────┘  └────────┬────────┘          │
              v                    v          back to adjuster
   ┌────────────────────┐    ┌──────────┐
   │Sent To Underwriter │    │  Closed  │ (terminal)
   └─────┬─────────┬────┘    └──────────┘
         v         v
┌─────────────┐ ┌──────────────────┐
│Underwriter  │ │Underwriter       │
│Agreed       │ │Declined          │
└──────┬──────┘ └────────┬─────────┘
       v                 v
  ┌─────────┐      ┌──────────┐
  │ Settled ├─────>│  Closed  │
  └─────────┘      └──────────┘
```

Some transitions require a written reason, which lands in the claim's status history:
requesting documents, withdrawing, every adjuster decision, an underwriter decline, and all
supervisor overrides. `frmTransition` enforces that.

**Guards** stop a claim moving on before it is ready (`ClaimWorkflow.Validate`):

- *To With Adjuster* — needs a date of loss, a peril, an estimated value above zero, an
  assigned adjuster, and at least one document or recorded item. An adjuster needs something
  to review.
- *To Sent To Underwriter* — needs a chosen underwriter and a reserve above zero.
- *To Settled* — needs at least one recorded payment.

## The flow the app was built around

1. **Intake** (`Intake > Record inbound item`). Manual capture, as specified: how it arrived
   (email / fax / post / phone), when, from whom, subject, the text, and any files. Files are
   staged and copied into the document store on save. There is no MAPI and no folder
   watching — you mock the inbox yourself.
2. It lands in the **intake queue** (`frmUnmatched`) with no claim against it. The status bar
   carries a running count.
3. **The decision** (`frmMatchClaim`, titled *"New claim or existing claim?"*). This is the
   pivot. `Core/TextHints.vb` scrapes claim references, policy numbers and en-GB dates out of
   the message text and pre-fills the search; a grey line tells you what it found and where
   from. It is a hint, never a decision.
   - **Attach to selected claim** files the item and *moves its attachments into that claim's
     folder*.
   - **Create new claim from this item** warns you first if there are possible matches on
     screen, then opens a new claim pre-filled from the item, and files the item against it
     once saved.
4. Duplicate detection is deliberately **loose** — an OR search across policy number, insured
   name and a date-of-loss window (default ±7 days), including a `REPLACE` variant so
   `NGC884213` still finds `NGC/884213`. Better to show a handler four possibles than to
   silently miss one and open a duplicate.
5. **The claim** (`frmClaim`) — Details, Correspondence, Documents, Notes, Payments, History.
   The header holds the reference, the current status, and the only route to changing it.

## Reports

`Reports > Export to Excel...` produces either of two workbooks. Each writes the figures to a
worksheet and draws a **line chart** underneath them.

**Claim value over time** — one row per month, showing total estimated value, reserve and paid
to date for claims *notified* in that month, with a claim count alongside. The status
tick-list filters it: tick only `Settled` to chart what the book actually cost, or use the
**Open claims** button to see what is still exposed. The count column deliberately sits
outside the charted range so it cannot flatten the money axis.

**Claim frequency by month** — claims notified against claims concluded, plus the resulting
number still open at each month end. Inflow against outflow. The concluded figure comes from
the status history, so it reflects when claims actually left the book rather than when they
arrived. The running total starts from however many claims were already open when the period
began, not from zero.

Both fill every month in the range, including empty ones, so the line has no gaps.

Charting is done by **Excel itself, through COM automation** — the same late-binding approach
used for creating the database, so nothing is added to the project references. It is also how
this kind of report was always produced on a broker's desk. **Excel must be installed.** If it
is not, the export offers to save the figures as CSV instead, without a chart.

## Layout

```
ClaimsManager.sln
ClaimsManager.vbproj
Program.vb                     Sub Main: create DB, sign in, run the MDI shell
Core/
  Config.vb                    paths and the Jet connection string
  Session.vb                   who is signed in; role checks
  ClaimWorkflow.vb             the state machine, guards, and Apply
  DocumentStore.vb             copies, relocates and opens attachments
  TextHints.vb                 scrapes refs / policy numbers / dates from prose
  UiStyle.vb                   the period look, brand colours, grid styling
  ExcelReport.vb               late-bound Excel automation, line charts, CSV fallback
  AppIcon.vb                   loads the embedded icon for window title bars
  RolePermissions.vb           the role capability matrix, derived from ClaimWorkflow
Data/
  Db.vb                        OleDb wrapper, positional "?" params, typed nulls
  DbInitializer.vb             creates the .mdb (ADOX), full DDL, reference data
  DemoData.vb                  generates twelve months of shaped demo claims
Models/
  Enums.vb                     statuses, roles, perils, doc types as constants
  Entities.vb                  Claim, Correspondence, Attachment, Note, Payment...
Repositories/
  ClaimRepository.vb           CRUD, search, duplicate detection, ref allocation
  CorrespondenceRepository.vb  intake queue and MatchToClaim
  AttachmentRepository.vb      document store links
  SupportRepositories.vb       notes, payments, status history, audit, users
  ReportRepository.vb          monthly aggregates for the two reports
Forms/                         10 forms, each with a hand-written .Designer.vb
Resources/                     the logo and the application icon, both embedded
Installer/
  ClaimsManager.wxs            WiX authoring for the MSI
  build-msi.cmd                builds Release, then packages it
```

Grid column headings come from SQL `AS [Friendly Name]` aliases rather than designer
configuration, which is why the repositories read the way they do.

---

## Known limitations — read before trusting it

**None of this has been compiled.** It was written in a Linux container with no VB compiler
available and no network route to obtain one, so it has never been through a build. Two real
bugs were found and fixed by inspection (a local named `path` shadowing `System.IO.Path` —
VB is case-insensitive — and a `Validate()` method colliding with the inherited
`Control.Validate()`), and a static check confirms that every control reference resolves to
its designer, every cross-module call resolves to a real member, and every grid column read
back matches a SQL alias. **Expect a first-build round of compiler errors anyway.** Typos in
80-odd designer properties are exactly what a compiler is for.

Beyond that, and by design rather than by accident:

- **No email or fax integration.** Manual capture only, as specified.
- **`NextClaimRef` has a benign race.** It reads the counter, then updates it, without a
  transaction. Two people creating a claim in the same instant on a shared `.mdb` could
  collide; the `UNIQUE` index on `ClaimRef` would reject the second one. Fine for a desktop
  Jet application, wrong for anything concurrent.
- **Deleted attachments leave the file on disk.** Removing a document deletes the database
  row only, so a mis-click destroys nothing. Housekeeping the store is a separate,
  deliberate job.
- **The audit log is append-only.** Nothing in the application updates or deletes an audit
  row, which is the entire point of having one.
- **Relocation failures degrade quietly.** If a file cannot be moved into a claim's folder
  when an item is matched, the database keeps pointing at the old location rather than
  breaking the link.
- **Reports need Excel installed.** Charting goes through COM automation, so a machine
  without Excel gets CSV and no chart. Nothing else in the application depends on Office.
- **Reports group by date of notification**, not by date of loss or by accounting period, so
  they answer "what came in when" rather than "what was earned when".
- **No underwriter submission pack.** The claim is marked as sent; producing the actual
  document to send is not built.
- **No soft delete for claims.** Withdraw or close them instead; both are terminal states.
- **The password is a single hardcoded constant**, held in plain text in `Session.vb` and
  compared directly. There is no hashing, no per-user credential and no lockout. It exists to
  demonstrate the sign-in gate, and a real deployment would do none of it this way.
- **`demo-accounts.csv` is a plain text file containing a password.** That is a deliberate
  convenience for a demo whose credentials are fictional and already in the source. No real
  system should write such a file.
- **Demo data is generated, not real.** It is shaped to look plausible and to make the
  reports interesting, but the numbers carry no meaning. Delete `Claims.mdb` to start empty
  — though note the reference data (users, underwriters) is seeded in the same step, so an
  empty book still gets its five sign-in users.
