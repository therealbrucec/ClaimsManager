Option Strict On
Imports System.Windows.Forms

''' <summary>
''' One dialog for every workflow decision. The reason typed here lands in the claim's
''' status history, which is the record an auditor or a complaint will be read against.
''' </summary>
Public Class frmTransition

    Private ReadOnly _fromStatus As String
    Private ReadOnly _transition As WorkflowTransition

    Public ReadOnly Property Rationale As String
        Get
            Return txtRationale.Text.Trim()
        End Get
    End Property

    Public Sub New()
        Me.New(ClaimStatus.Notified, New WorkflowTransition())
    End Sub

    Public Sub New(fromStatus As String, transition As WorkflowTransition)
        InitializeComponent()
        UiStyle.ApplyTo(Me)
        AppHelp.Attach(Me, AppHelp.MovingAClaim)

        _fromStatus = If(fromStatus, String.Empty)
        _transition = If(transition, New WorkflowTransition())
    End Sub

    Private Sub frmTransition_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblQuestion.Text = _transition.Caption
        lblFromTo.Text = "Status will change from " & _fromStatus & " to " & _transition.ToStatus & "."

        If _transition.RequiresRationale Then
            lblRequired.Text = "A reason is required for this decision."
            Text = "Confirm decision - reason required"
        Else
            lblRequired.Text = "A reason is optional here, but it helps whoever picks this up next."
        End If
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        If _transition.RequiresRationale AndAlso Rationale.Length = 0 Then
            UiStyle.ShowProblem(Me, "Type the reason for this decision before confirming.")
            txtRationale.Focus()
            Return
        End If

        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

End Class
