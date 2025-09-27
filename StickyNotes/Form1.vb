Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim d As New Dialog1
        If Not d.ShowDialog() = DialogResult.OK Then
            End
        End If
        Me.Text = d.SelectedItem
        RichTextBox1.Text = My.Settings.StickyNote.Item(My.Settings.NoteNames.IndexOf(Me.Text))
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        My.Settings.StickyNote.Item(My.Settings.NoteNames.IndexOf(Me.Text)) = RichTextBox1.Text
    End Sub

    Private Sub RichTextBox1_MouseUp(sender As Object, e As MouseEventArgs) Handles RichTextBox1.MouseUp
        If RichTextBox1.SelectedText.Trim().StartsWith("/") And RichTextBox1.SelectedText.Trim().EndsWith("/") Then
            Process.Start(RichTextBox1.SelectedText.Trim().Remove(0, 1).Remove(RichTextBox1.SelectedText.Trim().Remove(0, 1).Length - 1, 1))
        End If
    End Sub

    Private Sub Form1_DragEnter(sender As Object, e As DragEventArgs) Handles MyBase.DragEnter
        e.Effect = DragDropEffects.Link
    End Sub

    Private Sub Form1_DragDrop(sender As Object, e As DragEventArgs) Handles MyBase.DragDrop
        If e.Data.GetFormats().Contains(DataFormats.StringFormat) Then
            RichTextBox1.AppendText(Environment.NewLine + e.Data.GetData(DataFormats.StringFormat))
        ElseIf e.Data.GetFormats().Contains(DataFormats.FileDrop) Then
            RichTextBox1.AppendText(Environment.NewLine + "/" + e.Data.GetData(DataFormats.FileDrop)(0).ToString() + "/")
        End If
    End Sub
End Class
