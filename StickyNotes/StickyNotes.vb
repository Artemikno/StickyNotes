Imports System.IO

Module StickyNotes
    Sub Main()
        If (Not Dialog1.PrevInstance(1)) AndAlso CBool(My.Settings.Options And Options.SHOW_NOTE_SUGGESTIONS) Then
            If Not File.Exists(Environ("appdata") & "\Microsoft\Windows\Start Menu\Programs\Startup\" & Application.ProductName & ".lnk") Then
                Dim oShell As Object
                Dim oLink As Object
                oShell = CreateObject("WScript.Shell")
                oLink = oShell.CreateShortcut(Environ("appdata") & "\Microsoft\Windows\Start Menu\Programs\Startup\" & Application.ProductName & ".lnk")

                oLink.TargetPath = Application.ExecutablePath
                oLink.WindowStyle = 1
                oLink.Save()
            End If
            AskUser.TopMost = True
                Application.Run(AskUser)
            Else
                Application.Run(Form1)
        End If
    End Sub
End Module
