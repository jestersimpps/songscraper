Module scraping
    Public Function getsourcestring(ByVal url As String) As String
        Try
            Dim request As System.Net.HttpWebRequest = System.Net.HttpWebRequest.Create(url)
            Dim response As System.Net.HttpWebResponse = request.GetResponse()
            Dim sr As System.IO.StreamReader = New System.IO.StreamReader(response.GetResponseStream())
            Dim sourcestring As String = sr.ReadToEnd()
            Return sourcestring
        Catch ex As Exception
            Return "?"
        End Try
    End Function
    Function Count_substrings_in_string(ByVal StringToSearch As String, ByVal StringToFind As String) As Integer
        Dim ic As Integer = 0
        If Len(StringToFind) Then ic = UBound(Split(StringToSearch, StringToFind))
        Return ic
    End Function
    Public Function multilinextract(ByVal SourceString As String, ByVal StringIndex As Integer, ByVal PreString As String, ByVal PreDist As Integer, ByVal EndString As String, ByVal EndDist As Integer) As String

        Dim beginofstring As Integer
        Dim longstring As String
        Dim shortstring As String
        Dim a As Integer

        Try
            For i = 0 To StringIndex
                a = a + 1
                beginofstring = SourceString.IndexOf(PreString, a)
                a = SourceString.IndexOf(PreString, a + 1)
            Next
            StringIndex = a
            beginofstring = SourceString.IndexOf(PreString, StringIndex)
            longstring = SourceString.Substring(beginofstring + PreString.Length, SourceString.Length - beginofstring - PreString.Length)
            shortstring = longstring.Substring(PreDist, longstring.IndexOf(EndString) - EndDist)
        Catch ex As Exception
            Return "?"
        End Try
        shortstring = shortstring.Replace("<td>", "")
        shortstring = shortstring.Replace("</td>", "")
        Return shortstring

    End Function
    Public Function linextract(ByVal sourcestring As String, ByVal prestring As String, ByVal poststring As String, ByVal stringindex As Integer) As String

        Dim extracted As String
        Dim textobject As New RichTextBox
        Dim counter As Integer = 0

        textobject.Text = sourcestring
        For Each Line As String In textobject.Lines

            If Line.StartsWith(prestring) Then
                counter = counter + 1
                If counter > stringindex Then
                    Try
                        extracted = Line.Substring(prestring.Length, (Line.Length - prestring.Length) - (Line.Length - Line.IndexOf(poststring)))
                        textobject.Dispose()
                        Return extracted
                    Catch ex As Exception
                        Return Line.Substring(prestring.Length, (Line.Length - prestring.Length))
                    End Try
                End If
            End If
        Next
        Return "?"
        textobject.Dispose()
    End Function
End Module
