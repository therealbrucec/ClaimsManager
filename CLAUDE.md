# Claims Manager — notes for Claude Code

A VB.NET WinForms claims-handling desk for the fictional **Northbridge Assurance**, built as a
**demonstration subject for ATP360** (WorkspaceDNA's autonomous validation product). It is not
a product and holds no real data. Several things that look like shortcomings are deliberate
properties of a demo; check here before "fixing" them.

Read `README.md` for how to build, deploy and run it. This file covers the things that will
bite you if you don't know them.

---

## The one that catches everyone

**This is a classic, non-SDK `.vbproj`.** There is no globbing. **Every new source file needs
an explicit `<Compile Include="..." />` entry**, and every form needs its designer paired to it:

```xml
<Compile Include="Forms\frmThing.vb">
  <SubType>Form</SubType>
</Compile>
<Compile Include="Forms\frmThing.Designer.vb">
  <DependentUpon>frmThing.vb</DependentUpon>
</Compile>
```

Add a file without this and it silently isn't compiled. The format was chosen over SDK-style
to avoid ambiguous implicit/explicit reference conflicts, and it stays.

## Hard constraints — do not change these

**32-bit only.** The Access database is reached through `Microsoft.Jet.OLEDB.4.0`, which exists
only as a 32-bit component. The project is `AnyCPU` with `Prefer32Bit`, and the MSI is built
`-arch x86`. Change either and the app starts, then dies on its first database call. ACE was
rejected deliberately: it needs a redistributable, Jet ships with Windows.

**Data lives beside the executable.** `Config.vb` resolves everything relative to
`AppDomain.CurrentDomain.BaseDirectory`. There is **no fallback to the user profile** — if the
folder is unwritable, `Program.Main` says so and exits. Do not reintroduce a fallback; the
installer grants write access to the install folder instead.

**`Option Strict On` everywhere, with two exceptions.** `Data\DbInitializer.vb` (ADOX late
binding to create the .mdb) and `Core\ExcelReport.vb` (Excel COM late binding). Both are
`Option Strict Off` at file scope on purpose, so no interop assemblies need shipping.

## Where the rules live

**`Core\ClaimWorkflow.vb` is the single authority on what a claim may do next.** Forms ask it
what to offer and never move a claim themselves. The twelve statuses, the roles permitted for
each transition, which transitions demand a written reason, and the three guard conditions all
live there. Change behaviour there, not in a form.

**`Core\RolePermissions.vb` derives its answers from `ClaimWorkflow`** rather than restating
them, so the permissions written into `demo-accounts.csv` cannot drift from what is enforced.
Only screen-level rights (recording post, creating and editing claims) are stated directly,
because those live in the forms as `Session.IsInRole` checks.

## Jet and OleDb: the sharp edges

These were all found the hard way. The database layer is `Data\Db.vb`.

- **Parameters are positional.** Every `?` is filled from the values array in order. Names are
  irrelevant.
- **Types are set explicitly in `Db.Bind`, not inferred.** OleDb maps a .NET `Decimal` to
  `DBTYPE_DECIMAL`, which Jet refuses to coerce into a `CURRENCY` column — and reports as
  "Data type mismatch in criteria expression", even on an INSERT. Dates bind as `OleDbType.Date`
  because Jet stores DATETIME as a double.
- **Jet's `LONG` is 32-bit.** An `Int64` cannot go straight in; `Bind` narrows it.
- **Strings over 255 characters bind as `LongVarWChar`** for the MEMO columns.
- **Not every Access function is available through OleDb.** `REPLACE()` and `FORMAT()` are not.
  This is why `ReportRepository` reads rows and aggregates by month **in VB** rather than in SQL.
  Do not "optimise" that into a `GROUP BY` with date functions.
- **Avoid subqueries in `UPDATE`.** Jet often rejects them as non-updateable. `DemoData` writes
  history rows with the right timestamp first time rather than inserting then correcting.
- **Every failure is re-thrown with the statement and its bound parameters attached**, because
  Jet's own messages name neither. Keep that.

## VB gotchas that have already caused bugs

- **VB is case-insensitive**, so a local named `path` shadows `System.IO.Path` and every
  `Path.Combine` in that method fails to compile. Name locals `sourcePath`, `targetPath`.
- **`Validate()` collides with `Control.Validate()`** on a Form. Validation methods are named
  `ValidateItem`, `CollectFromForm` and so on.

## Forms

Designer files are **hand-written and paired**, and there are no `.resx` files. Forms carry
`DesignerGeneratedAttribute` so the compiler synthesises the constructor; where a form needs
parameters it declares them explicitly plus a parameterless overload so the VS designer can
still render it.

`UiStyle.ApplyTo(Me)` in a form's constructor sets the period font (Tahoma 8.25pt), the grey
face and the window icon. Call it from any new form. `frmSplash` deliberately does not, because
it needs a white background.

Grid column headings come from **SQL `AS [Friendly Name]` aliases**, not designer configuration.
That is why the repositories' SELECT lists read the way they do, and why renaming an alias
breaks the code that reads `Cells("...")`.

## The help file

F1 opens `ClaimsManager.chm`, compiled from the HTML Help Workshop project in `Help\` by
`Help\BuildChm.vb`, an MSBuild inline task (RoslynCodeTaskFactory) that the vbproj runs after
every build. **`BuildChm.vb` is build tooling and must never get a `<Compile>` entry.** It
compiles without the VB runtime, so no `ChrW`, and no `=` or `Select Case` on strings: use
`String.Equals`.

- **Each form calls `AppHelp.Attach(Me, topic)`** beside `UiStyle.ApplyTo(Me)`. Topic names
  are the `.htm` file names, held as constants in `Core\AppHelp.vb`. Do the same for any new form.
- **The topics restate the rules.** `transitions.htm`, `roles.htm` and `guards.htm` mirror
  `ClaimWorkflow`; change them together.
- **Sitemaps (`.hhc`, `.hhk`) must keep Workshop's multi-line layout and stay ASCII.** The
  viewer showed an empty Contents tree for the same sitemap written on one line.
- **The container quirks were found against the real viewer**, and each is commented in
  `BuildChm.vb`: the last LZX frame is padded to a full 32K (otherwise hh.exe and 7-Zip both
  fail on the final frame), and `#WINDOWS` entries are 0xC4 bytes (0x188 makes the viewer
  refuse the whole file).
- To check a build: `7z t bin\Release\ClaimsManager.chm` decodes every stream, and
  `hh.exe -decompile <folder> <chm>` extracts through Windows' own reader.

## The installer

`Installer\ClaimsManager.wxs`, built by `Installer\build-msi.cmd`. **No WiX extensions are
required** — everything used is in the core schema.

- **`UpgradeCode` is fixed forever.** Changing it breaks upgrade detection.
- **`ProductCode` is pinned**, so Intune's detection rule and uninstall command survive a
  rebuild. Generate a new one whenever `ProductVersion` changes; that is what makes it a major
  upgrade.
- **The `InstallFolderWritable` component needs its explicit `Guid`.** Its key path is a
  directory, and WiX will not auto-generate a GUID for those. Do not remove it.
- WiX v6 and later gate every command behind the Open Source Maintenance Fee EULA. The build
  script does **not** accept it automatically; that is a licensing decision, opt in with
  `ACCEPT_WIX_EULA=1`.

## Tests

`Tests\` holds nine multi-application workflow tests, written as **specifications rather than
code**. That is deliberate twice over: a script spanning Outlook, a WinForms window, Excel and
Teams breaks whenever any of the four redraws a dialog, and coded automation would be the exact
thing ATP360 exists to replace, which would undercut the demo. **Do not convert them to
scripted automation** without being asked.

Each test separates **pass criteria** from **legitimate variation**. References, timestamps and
generated figures differ per run; statuses, permissions and arithmetic do not.

## Demo data

`Data\DemoData.vb` generates 140 claims across the twelve months ending on the database's
creation date, plus payments, history, correspondence and seven intake items. **The random
generator is seeded with a fixed number**, so every rebuild produces an identical book — the
figures quoted in the tests depend on that. Do not remove the seed.

To reset: delete `Claims.mdb` and the `Documents` folder from beside the executable.

## Deliberate, not unfinished

Do not "fix" these without being asked:

- **No mailbox, fax or Teams integration.** Every inbound item is typed in by a person. The
  operator is the integration, and that is the workflow being demonstrated.
- **No submission pack is generated.** The app records that a claim went to an underwriter and
  when; assembling and sending it happens in Outlook.
- **One shared password**, held in plain text in `Session.vb`, and written to
  `demo-accounts.csv` beside the executable. Demoware.
- **Deleted attachments leave the file on disk.** Only the database row goes, so a mis-click
  destroys nothing.
- **The audit log is append-only.** Nothing updates or deletes an audit row.
- **`NextClaimRef` has a benign race** under genuine concurrency. Fine for a desktop Jet app.

## Verification

There is **no test framework and no CI**. Nothing here has been compiled outside Visual Studio
on Windows. Verification is: build in VS, then run the workflows in `Tests\`.

Useful static checks, if you are changing things in bulk: every `<Compile>` entry resolves to a
real file; every control referenced in a form's code is declared in its designer; every grid
column read via `Cells("...")` matches a SQL alias; and `RolePermissions` still reproduces the
matrix in `Tests\` and in the deck.
