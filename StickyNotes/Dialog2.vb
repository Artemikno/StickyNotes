Imports System.Windows.Forms

Public Class Dialog2

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Dim kwd As String = ""
        For Each itm As Object In ListBox1.Items
            kwd += itm + "␞␟"
        Next
        My.Settings.Keywords = kwd
        My.Settings.Options = CUShort(IIf(CheckedListBox1.CheckedIndices.Contains(0), Options.CONTINUE_AFTER_RESTART, 0) Or IIf(CheckedListBox1.CheckedIndices.Contains(1), Options.SHOW_NOTE_SUGGESTIONS, 0))
        My.Settings.Save()
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ListBox1.Items.Add("Change|None")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If ListBox1.SelectedItems.Count > 0 Then
            ListBox1.Items.Remove(ListBox1.SelectedItem)
        End If
    End Sub
    Private changing As Boolean = False

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If ListBox1.SelectedItems.Count > 0 AndAlso Not changing Then
            ListBox1.Items(ListBox1.SelectedIndex) = TextBox1.Text & "|" & TextBox2.Text
        End If
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        If ListBox1.SelectedItems.Count > 0 AndAlso Not changing Then
            ListBox1.Items(ListBox1.SelectedIndex) = TextBox1.Text & "|" & TextBox2.Text
        End If
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        If ListBox1.SelectedItems.Count > 0 Then
            changing = True
            TextBox1.Text = ListBox1.Text.Split("|")(0)
            TextBox2.Text = ListBox1.Text.Split("|")(1)
            changing = False
        End If
    End Sub

    Private Sub Dialog2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ListBox1.Items.Clear()
        For Each s As String In My.Settings.Keywords.Split("␞␟")
            If s.Contains("|") Then
                ListBox1.Items.Add(s)
            End If
        Next
        For i = 0 To CheckedListBox1.Items.Count - 1
            CheckedListBox1.SetItemChecked(i, My.Settings.Options And 2 ^ i)
        Next

    End Sub
End Class
