Public Class Form1
    Dim LastI As Integer
    Dim exitnow As Boolean = False
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim d As New Dialog1
        If Not d.ShowDialog() = DialogResult.OK Then
            exitnow = True
            Application.Exit()
            Return
        End If
        LastI = d.SelectedItemIndex
        Me.Text = d.SelectedItem.Name
        RichTextBox1.Text = d.SelectedItem.Text.Replace("\\n", Environment.NewLine)
        Me.Size = d.SelectedItem.Size
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If exitnow Then
            Return
        End If
        Dim b = Notes.NoteSerializer.StringToNoteCollection(My.Settings.Notes)
        If LastI < 0 Or LastI >= b.Notes.Count Then
            Throw New Exception("Invalid selection for note!")
        End If
        b.Notes(LastI) = New Notes.Note(Me.Text, RichTextBox1.Text.Replace(Environment.NewLine, "\\n"), Me.Size)
        My.Settings.Notes = Notes.NoteSerializer.NoteCollectionToString(b)
        My.Settings.Save()
    End Sub

    Private Sub RichTextBox1_MouseUp(sender As Object, e As MouseEventArgs) Handles RichTextBox1.MouseUp
        rtbismousedown = False
        sbismousedown = False
        If e.Button = MouseButtons.Right OrElse e.Button = MouseButtons.Middle Then
            Return
        End If
        If RichTextBox1.SelectedText.TrimStart().StartsWith("/") And RichTextBox1.SelectedText.TrimEnd().EndsWith("/") Then
            If MsgBox("!WARNING! You are about to execute potentially malicious code! Press ""Yes"" to continue. Your info might be at risk if you choose to continue.", MsgBoxStyle.Exclamation Or MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2) = MsgBoxResult.Yes Then
                Shell(RichTextBox1.SelectedText.Trim().Remove(0, 1).Remove(RichTextBox1.SelectedText.Trim().Remove(0, 1).Length - 1, 1), AppWinStyle.NormalFocus, False)
            End If
        End If
    End Sub

    Private Sub Form1_DragEnter(sender As Object, e As DragEventArgs) Handles MyBase.DragEnter
        If e.Data.GetFormats(True).Contains(DataFormats.StringFormat) OrElse e.Data.GetFormats().Contains(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Link
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub Form1_DragDrop(sender As Object, e As DragEventArgs) Handles MyBase.DragDrop
        If e.Data.GetFormats().Contains(DataFormats.FileDrop) Then
            RichTextBox1.AppendText(Environment.NewLine + "/cmd /c """ + e.Data.GetData(DataFormats.FileDrop)(0).ToString() + """/")
        ElseIf e.Data.GetFormats(True).Contains(DataFormats.StringFormat) Then
            RichTextBox1.AppendText(Environment.NewLine + e.Data.GetData(DataFormats.StringFormat))
        End If
    End Sub

    Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click
        Dim b = Notes.NoteSerializer.StringToNoteCollection(My.Settings.Notes)
        If LastI < 0 Or LastI >= b.Notes.Count Then
            Throw New Exception("Invalid selection for note!")
        End If
        b.Notes(LastI) = New Notes.Note(Me.Text, RichTextBox1.Text.Replace(Environment.NewLine, "\\n"), Me.Size)
        My.Settings.Notes = Notes.NoteSerializer.NoteCollectionToString(b)
        My.Settings.Save()
    End Sub

    Private Sub ExecuteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExecuteToolStripMenuItem.Click
        If RichTextBox1.SelectedText.TrimStart().StartsWith("/") And RichTextBox1.SelectedText.TrimEnd().EndsWith("/") Then
            If MsgBox("!WARNING! You are about to execute potentially malicious code! Press ""Yes"" to continue. Your info might be at risk if you choose to continue.", MsgBoxStyle.Exclamation Or MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2) = MsgBoxResult.Yes Then
                Shell(RichTextBox1.SelectedText.Trim().Remove(0, 1).Remove(RichTextBox1.SelectedText.Trim().Remove(0, 1).Length - 1, 1), AppWinStyle.NormalFocus, False)
            End If
        End If
    End Sub

    Dim rtbismousedown As Boolean = False
    Dim sbismousedown As Boolean = False
    Dim mouseoffset As Point

    Private Sub RichTextBox1_MouseDown(sender As Object, e As MouseEventArgs) Handles RichTextBox1.MouseDown
        If e.Button = MouseButtons.Middle Then
            rtbismousedown = True
            mouseoffset = e.Location
        End If
    End Sub

    Private Sub StatusStrip1_MouseDown(sender As Object, e As MouseEventArgs) Handles StatusStrip1.MouseDown
        If e.Location.X > Size.Width - 22 Then
            sbismousedown = True
            mouseoffset = Size.op_Explicit(Size) - Cursor.Position + Location
        Else
            rtbismousedown = True
            mouseoffset = Cursor.Position - Location
        End If
    End Sub

    Private Sub StatusStrip1_MouseUp(sender As Object, e As MouseEventArgs) Handles StatusStrip1.MouseUp
        rtbismousedown = False
        sbismousedown = False
    End Sub

    Public Sub DrawGrip(g As Graphics, p As Point)
        g.FillRectangles(SystemBrushes.ControlLightLight, {New Rectangle(p + New Point(9, 1), New Size(2, 2)), New Rectangle(p + New Point(9, 5), New Size(2, 2)), New Rectangle(p + New Point(9, 9), New Size(2, 2)), New Rectangle(p + New Point(5, 5), New Size(2, 2)), New Rectangle(p + New Point(5, 9), New Size(2, 2)), New Rectangle(p + New Point(1, 9), New Size(2, 2))})
        g.FillRectangles(SystemBrushes.ControlDark, {New Rectangle(p + New Point(8, 0), New Size(2, 2)), New Rectangle(p + New Point(8, 4), New Size(2, 2)), New Rectangle(p + New Point(8, 8), New Size(2, 2)), New Rectangle(p + New Point(4, 4), New Size(2, 2)), New Rectangle(p + New Point(4, 8), New Size(2, 2)), New Rectangle(p + New Point(0, 8), New Size(2, 2))})
    End Sub

    Private Sub StatusStrip1_Paint(sender As Object, e As PaintEventArgs) Handles StatusStrip1.Paint
        'e.Graphics.FillRectangles(SystemBrushes.ControlDark, {New Rectangle(Size.op_Explicit(Size) - New Point(3, 3), New Size(2, 2))})
        DrawGrip(e.Graphics, Size.op_Explicit(StatusStrip1.Size) - New Point(13, 13))
    End Sub

    Private Sub ToolStripDropDownButton1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Close()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If sbismousedown Then
            Size += Point.op_Explicit(Cursor.Position - Location - Size.op_Explicit(Size) + mouseoffset)
            StatusStrip1.Invalidate()
        End If
        If rtbismousedown Then
            Location += Cursor.Position - Location - mouseoffset
        End If
    End Sub
End Class
