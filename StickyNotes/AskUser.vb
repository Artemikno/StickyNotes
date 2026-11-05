Imports System.Runtime.InteropServices

Public Class AskUser
    Private anim As Byte = 0
    Private pen As Pen = New Pen(Color.Orange, 4)
    Private Sub AskUser_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
        e.Graphics.DrawEllipse(pen, 202 - CByte(anim / 255 * 202), 140 - CByte(anim / 255 * 140), CUShort(anim / 255 * 404), CUShort(anim / 255 * 280))
    End Sub

    Private Sub losad() Handles MyBase.Load
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        anim = (anim + 15) Mod 255
        Me.Invalidate()
    End Sub

    Private enteredstring As String = ""
    Private openingnote As String = ""
    Private WithEvents LLKB As New LowLevelKeyBoardhook()

    Private Sub AskUser_KeyDown(keyCode As Keys) Handles LLKB.F12
        If TimeOfDay.Ticks < My.Settings.SuggestionsPausedUntil Then
            Return
        End If
        enteredstring += keyCode.ToString()
        Dim ret As Boolean = False
        For Each s As String In My.Settings.Keywords.Split("␞␟")
            Dim aas As String = s.Split("|")(0).ToUpper()
            If aas.StartsWith(enteredstring) Then
                ret = True
            End If
            If aas.Equals(enteredstring) Then
                enteredstring = ""
                openingnote = s.Split("|")(1)
                WindowState = FormWindowState.Normal
                Me.Show()
            End If
            aas = Nothing
        Next
        If ret Then
            Return
        End If
        ret = Nothing
        enteredstring = ""
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Hide()
        If RadioButton3.Checked Then
            My.Settings.SuggestionsPausedUntil = TimeOfDay.Ticks + New TimeSpan(Math.Floor(NumericUpDown1.Value / 60) * 60, Math.Floor(NumericUpDown1.Value) Mod 60, (NumericUpDown1.Value - Math.Floor(NumericUpDown1.Value)) * 60).Ticks
        End If
        If RadioButton1.Checked Then
            Process.Start(Application.ExecutablePath, openingnote)
        End If
    End Sub

    Private Sub losad(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Hide()
        Timer2.Enabled = False
    End Sub

    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        My.Settings.Reload()
    End Sub
End Class

' Modified version of https://www.experts-exchange.com/questions/24693164/Vb-net-KeyPress-outside-the-form.html
Public Class LowLevelKeyBoardhook

    Private Const HC_ACTION As Integer = 0
    Private Const WH_KEYBOARD_LL As Integer = 13
    Private Const WM_KEYDOWN As Integer = &H100
    Private Const WM_SYSKEYDOWN As Integer = &H104

    Public Structure KBDLLHOOKSTRUCT
        Public vkCode As Integer
        Public scancode As Integer
        Public flags As Integer
        Public time As Integer
        Public dwExtraInfo As Integer
    End Structure

    Private Declare Function SetWindowsHookEx Lib "user32" _
        Alias "SetWindowsHookExA" (
        ByVal idHook As Integer,
        ByVal lpfn As LowLevelKeyboardProcDelegate,
        ByVal hmod As Integer,
        ByVal dwThreadId As Integer) As Integer

    Private Declare Function CallNextHookEx Lib "user32" (
        ByVal hHook As Integer,
        ByVal nCode As Integer,
        ByVal wParam As Integer,
        ByVal lParam As KBDLLHOOKSTRUCT) As Integer

    Private Declare Function UnhookWindowsHookEx Lib "user32" (
        ByVal hHook As Integer) As Integer

    Private Delegate Function LowLevelKeyboardProcDelegate(
        ByVal nCode As Integer,
        ByVal wParam As Integer,
        ByRef lParam As KBDLLHOOKSTRUCT) As Integer

    Public Event F12(keyCode As Keys)

    Private hhkLowLevelKeyboard As Integer
    Private keyboardDelegate As LowLevelKeyboardProcDelegate

    Public Sub New()
        keyboardDelegate = New LowLevelKeyboardProcDelegate(AddressOf Me.LowLevelKeyboardProc)
        hhkLowLevelKeyboard = SetWindowsHookEx(WH_KEYBOARD_LL, keyboardDelegate,
            Marshal.GetHINSTANCE(System.Reflection.Assembly.GetExecutingAssembly.GetModules()(0)).ToInt32, 0)
    End Sub

    Private Function LowLevelKeyboardProc(
        ByVal nCode As Integer,
        ByVal wParam As Integer,
        ByRef lParam As KBDLLHOOKSTRUCT) As Integer

        If (nCode = HC_ACTION) Then
            Select Case wParam
                Case WM_KEYDOWN, WM_SYSKEYDOWN
                    Try
                        RaiseEvent F12(lParam.vkCode)
                    Catch e As IndexOutOfRangeException

                    End Try

            End Select
        End If

        Return CallNextHookEx(hhkLowLevelKeyboard, nCode, wParam, lParam)
    End Function

    Protected Overrides Sub Finalize()
        UnhookWindowsHookEx(hhkLowLevelKeyboard)
        MyBase.Finalize()
    End Sub

End Class