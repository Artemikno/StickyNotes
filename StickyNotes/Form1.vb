Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Size = My.Settings.Resize
        RichTextBox1.Text = My.Settings.StickyNote
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If MsgBox("Do you want to save?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question) = MsgBoxResult.Yes Then
            My.Settings.Resize = Me.Size
            My.Settings.StickyNote = RichTextBox1.Text
        End If
    End Sub

    Private Sub RichTextBox1_MouseUp(sender As Object, e As MouseEventArgs) Handles RichTextBox1.MouseUp
        If RichTextBox1.SelectedText = "/reset/" Then
            If MsgBox("Are you sure you want to reset?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question) = MsgBoxResult.Yes Then
                My.Settings.Resize = New Size(300, 300)
                My.Settings.StickyNote = "Text here"
                Form1_Load(Nothing, Nothing)
            End If
        ElseIf RichTextBox1.SelectedText.Trim().StartsWith("/") And RichTextBox1.SelectedText.Trim().EndsWith("/") Then
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
