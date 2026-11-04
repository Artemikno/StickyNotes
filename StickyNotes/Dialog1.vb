Imports System.Windows.Forms

Public Class Dialog1
    Public SelectedItem As Notes.Note
    Public SelectedItemIndex As Integer?
    Public ParsedData As Notes.NoteCollection

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.SelectedItem = ParsedData.Notes.Item(ListView1.SelectedItems.Item(0).Index)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Shared m As Threading.Mutex

    Public Function PrevInstance() As Boolean
        Return Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > (IIf(CBool(My.Settings.Options And Options.SHOW_NOTE_SUGGESTIONS), 2, 1))
    End Function
    Public Function PrevInstance(amount As Integer) As Boolean
        Return Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > amount
    End Function

    Private Sub Dialog1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not PrevInstance() Then
            Button1.Enabled = True
            Button2.Enabled = True
        Else
            Button1.Enabled = False
            Button2.Enabled = False
        End If
        ' Source - https://stackoverflow.com/a/534335
        ' Posted by Markus Olsson, modified by community. See post 'Timeline' for change history
        ' Retrieved 2026-11-03, License - CC BY-SA 2.5

        If (My.Settings.UpgradeRequired) Then
            My.Settings.Upgrade()
            My.Settings.UpgradeRequired = False
            My.Settings.Save()
        End If

        If My.Settings.Notes Is Nothing Then
            My.Settings.Notes = ""
        End If
        ParsedData = Notes.NoteSerializer.StringToNoteCollection(My.Settings.Notes)
        'If My.Settings.NoteNames IsNot Nothing AndAlso My.Settings.StickyNote IsNot Nothing Then
        '    For s As Integer = 0 To My.Settings.NoteNames.Count - 1
        '        ParsedData.Notes.Add(New Notes.Note(My.Settings.NoteNames.Item(s), My.Settings.StickyNote.Item(s), New Drawing.Size(316, 339)))
        '    Next
        '    My.Settings.NoteNames = Nothing
        '    My.Settings.StickyNote = Nothing
        'End If
        For Each s As Notes.Note In ParsedData.Notes
            ListView1.Items.Add(New ListViewItem(s.Name, 0))
        Next
        If Environment.GetCommandLineArgs().Length = 2 Then
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.SelectedItemIndex = ParsedData.Notes.FindIndex(Function(ByVal n As Notes.Note)
                                                                  Return n.Name = Environment.GetCommandLineArgs()(1)
                                                              End Function)
            Me.SelectedItem = ParsedData.Notes.Item(SelectedItemIndex)
            Me.Close()
        End If
    End Sub

    Private Sub ListView1_DoubleClick(sender As Object, e As EventArgs) Handles ListView1.DoubleClick
        OK_Button_Click(sender, e)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If PrevInstance() Then
            Button1.Enabled = False
            Button2.Enabled = False
            Return
        End If
        Dim i = InputBox("Name")
        ParsedData.Notes.Add(New Notes.Note(i, "Text here", New Drawing.Size(300, 322))) 'New Drawing.Size(316, 339)))
        ListView1.Items.Add(New ListViewItem(i) With {.ImageIndex = 0})
        My.Settings.Notes = Notes.NoteSerializer.NoteCollectionToString(ParsedData)
        My.Settings.Save()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If PrevInstance() Then
            Button1.Enabled = False
            Button2.Enabled = False
            Return
        End If
        If SelectedItemIndex Is Nothing Then
            Return
        End If
        ParsedData.Notes.RemoveAt(SelectedItemIndex)
        ListView1.Items.RemoveAt(SelectedItemIndex)
        My.Settings.Notes = Notes.NoteSerializer.NoteCollectionToString(ParsedData)
        My.Settings.Save()
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count < 1 Then
            Return
        End If
        SelectedItemIndex = ListView1.SelectedItems.Item(0).Index
    End Sub

    Private Sub Dialog1_MouseEnter(sender As Object, e As EventArgs) Handles MyBase.MouseEnter
        If Not PrevInstance() Then
            Button1.Enabled = True
            Button2.Enabled = True
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dialog2.ShowDialog(Me)
    End Sub
End Class
