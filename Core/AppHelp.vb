Option Strict On
Imports System.IO
Imports System.Windows.Forms

''' <summary>
''' F1 help, done the way line-of-business applications did it: a compiled HTML Help file
''' beside the executable, opened at the topic for whichever window has focus.
'''
''' Each form names its topic once, in its constructor. Pressing F1 on any control raises
''' HelpRequested, which WinForms passes up through the control's parents until something
''' handles it, so the form's handler answers for everything on it. An MDI child that
''' handles it stops it there; anything that reaches the main window gets the main window's
''' topic.
'''
''' The topics are the file names in Help\. Renaming one there means renaming it here.
''' </summary>
Public Module AppHelp

    Public Const Welcome As String = "welcome.htm"
    Public Const SigningIn As String = "signing-in.htm"
    Public Const MainWindow As String = "main-window.htm"
    Public Const RecordInbound As String = "record-inbound.htm"
    Public Const IntakeQueue As String = "intake-queue.htm"
    Public Const NewOrExisting As String = "new-or-existing.htm"
    Public Const ClaimLists As String = "claim-lists.htm"
    Public Const AdjusterReview As String = "adjuster-review.htm"
    Public Const ClaimDetails As String = "claim-details.htm"
    Public Const ClaimCorrespondence As String = "claim-correspondence.htm"
    Public Const ClaimDocuments As String = "claim-documents.htm"
    Public Const ClaimNotes As String = "claim-notes.htm"
    Public Const ClaimPayments As String = "claim-payments.htm"
    Public Const ClaimHistory As String = "claim-history.htm"
    Public Const MovingAClaim As String = "moving-a-claim.htm"
    Public Const Reports As String = "reports.htm"

    ''' <summary>F1 anywhere on the form opens this topic.</summary>
    Public Sub Attach(target As Form, topic As String)
        Attach(target, Function() topic)
    End Sub

    ''' <summary>F1 anywhere on the form opens whichever topic this returns at the time,
    ''' for a form whose subject changes, such as a claim's tabs.</summary>
    Public Sub Attach(target As Form, topic As Func(Of String))
        AddHandler target.HelpRequested,
            Sub(sender As Object, e As HelpEventArgs)
                e.Handled = True
                ShowTopic(target, topic())
            End Sub
    End Sub

    Public Sub ShowTopic(owner As Control, topic As String)
        If Not HelpFileIsPresent(owner) Then Return
        Help.ShowHelp(owner, Config.HelpFilePath, HelpNavigator.Topic, topic)
    End Sub

    Public Sub ShowContents(owner As Control)
        If Not HelpFileIsPresent(owner) Then Return
        Help.ShowHelp(owner, Config.HelpFilePath, HelpNavigator.TableOfContents)
    End Sub

    Public Sub ShowIndex(owner As Control)
        If Not HelpFileIsPresent(owner) Then Return
        Help.ShowHelpIndex(owner, Config.HelpFilePath)
    End Sub

    ''' <summary>Left to itself the help viewer reports a missing file in terms nobody on a
    ''' claims desk would follow, so this says it first.</summary>
    Private Function HelpFileIsPresent(owner As Control) As Boolean
        If File.Exists(Config.HelpFilePath) Then Return True

        UiStyle.ShowProblem(owner, "The help file is missing. It should be here:" & vbCrLf & vbCrLf &
                                   Config.HelpFilePath & vbCrLf & vbCrLf &
                                   "Reinstall Claims Manager to put it back.")
        Return False
    End Function

End Module
