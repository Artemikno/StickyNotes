Imports System.Windows.Forms

Public Class Dialog1
    Public SelectedItem As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.SelectedItem = ListView1.SelectedItems.Item(0).Name
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Dialog1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.StickyNote Is Nothing Then
            My.Settings.StickyNote = New Specialized.StringCollection()
        End If
        For Each s As String In My.Settings.StickyNote
            ListView1.Items.Add(New ListViewItem(s))
        Next
    End Sub

    Private Sub ListView1_DoubleClick(sender As Object, e As EventArgs) Handles ListView1.DoubleClick
        ListView1.Items.Remove(ListView1.SelectedItems.Item(0))
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ListView1.Items.Add(New ListViewItem(InputBox("Name")))
    End Sub
End Class
