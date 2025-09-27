Imports System.Windows.Forms

Public Class Dialog1
    Public SelectedItem As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.SelectedItem = ListView1.SelectedItems.Item(0).Text
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Dialog1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.NoteNames Is Nothing Then
            My.Settings.NoteNames = New Specialized.StringCollection()
            My.Settings.StickyNote = New Specialized.StringCollection()
        End If
        If My.Settings.StickyNote Is Nothing Then
            My.Settings.StickyNote = New Specialized.StringCollection()
        End If
        For Each s As String In My.Settings.NoteNames
            ListView1.Items.Add(New ListViewItem(s, 0))
        Next
    End Sub

    Private Sub ListView1_DoubleClick(sender As Object, e As EventArgs) Handles ListView1.DoubleClick
        OK_Button_Click(sender, e)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim i = InputBox("Name")
        My.Settings.StickyNote.Add("Text here")
        My.Settings.NoteNames.Add(i)
        ListView1.Items.Add(New ListViewItem(i, 0))
        My.Settings.Save()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        My.Settings.StickyNote.RemoveAt(My.Settings.NoteNames.IndexOf(ListView1.SelectedItems.Item(0).Text))
        My.Settings.NoteNames.Remove(ListView1.SelectedItems.Item(0).Text)
        ListView1.Items.Remove(ListView1.SelectedItems.Item(0))
        My.Settings.Save()
    End Sub
End Class
