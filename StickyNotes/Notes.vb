Public Class Notes
    Public Class NoteSerializer
        Public Shared ReadOnly SeparatorSmall As String = "␞␟"
        Public Shared ReadOnly SeparatorBig As String = "><␟␞|><␟␞|"
        Public Shared Function NoteToString(Note As Note) As String
            Return Note.Name & SeparatorSmall & Note.Text & SeparatorSmall & Note.Size.Width & SeparatorSmall & Note.Size.Height
        End Function
        Private Shared Function Split(s As String, c As String) As List(Of String)
            Dim out As New List(Of String)
            Dim preout As New List(Of Char)
            Dim h = 0
            While h <= s.Length - 1
                If h + c.Length <= s.Length AndAlso s(h) = c(0) AndAlso s.Substring(h, c.Length) = c Then
                    out.Add(New String(preout.ToArray()))
                    preout.Clear()
                    h += c.Length - 1
                Else
                    preout.Add(s(h))
                End If
                h += 1
            End While
            out.Add(New String(preout.ToArray()))
            Return out
        End Function
        Public Shared Function StringToNote(Note As String) As Note
            Dim sp As String() = Split(Note, SeparatorSmall).ToArray()
            Return New Note(sp(0), sp(1), New Size(CInt(sp(2)), CInt(sp(3))))
        End Function
        Public Shared Function NoteCollectionToString(Collection As NoteCollection) As String
            Dim out As String = ""
            Dim used As Boolean = False
            For Each Note As Note In Collection.Notes
                If used Then
                    out &= SeparatorBig
                Else
                    used = True
                End If
                out &= NoteToString(Note)
            Next
            Return out
        End Function
        Public Shared Function StringToNoteCollection(Collection As String) As NoteCollection
            If Collection.Length < 1 Then
                Return New NoteCollection(New List(Of Note))
            End If
            Dim sp As String() = Split(Collection, SeparatorBig).ToArray()
            Dim preout As New List(Of Note)
            For Each s As String In sp
                preout.Add(StringToNote(s))
            Next
            Return New NoteCollection(preout)
        End Function
    End Class
    Public Class NoteCollection
        Property Notes As List(Of Note)
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(Notes As List(Of Note))
            MyBase.New()
            Me.Notes = Notes
        End Sub
        Public Sub New(Notes As Note())
            MyBase.New()
            Me.Notes = Notes.ToList()
        End Sub
    End Class
    Public Class Note
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(Name As String, Text As String, Size As Size)
            MyBase.New()
            Me.Name = Name
            Me.Text = Text
            Me.Size = Size
        End Sub

        Property Name As String
        Property Text As String
        Property Size As Size
    End Class
End Class
