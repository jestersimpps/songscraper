Imports System.Net
Imports System
Imports System.Threading
Imports System.IO
Imports System.Text
Imports System.Diagnostics
Public Class form2
    Dim wikitext As String
    Dim wikisource As String
    Dim songsource As String
    Dim complete As Integer
    Dim setindex As Integer
    Dim artistname As String


    Private Sub me_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If UI.wmplayer.currentMedia.getItemInfo("Artist").ToString() <> "" Then 'if artist info can be retrieved, use that
            'Dim ifs As New Thread(Sub() info_search(UI.wmplayer.currentMedia.getItemInfo("Artist").ToString()))
            'ifs.Start()
            info_search(UI.wmplayer.currentMedia.getItemInfo("Artist").ToString())
        Else 'if artist info cannot be retrieved, use title
            'Dim ifs As New Thread(Sub() info_search(UI.nametext.Text))
            'ifs.Start()
            info_search(UI.nametext.Text)
        End If

        WebBrowser1.DocumentText = "<html><body bgcolor = ""black"" link=""white""><font size=""2""><font color=""cyan""><font face=""arial"">" & "Retrieving information... " & "</body></html>"
        Me.WindowState = FormWindowState.Normal
        'Me.TopMost = True
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.Location = New Point(SystemInformation.PrimaryMonitorSize.Width / 9, 0)
        Me.Width = 7 * SystemInformation.PrimaryMonitorSize.Width / 9
        Me.Height = SystemInformation.PrimaryMonitorSize.Height
        DataGridView3.RowsDefaultCellStyle.BackColor = Color.Black : DataGridView4.RowsDefaultCellStyle.BackColor = Color.Black
        DataGridView3.RowsDefaultCellStyle.ForeColor = Color.Cyan : DataGridView4.RowsDefaultCellStyle.ForeColor = Color.Cyan
        DataGridView3.RowsDefaultCellStyle.SelectionBackColor = Color.DarkSlateGray : DataGridView4.RowsDefaultCellStyle.SelectionBackColor = Color.DarkSlateGray
        DataGridView3.RowsDefaultCellStyle.SelectionForeColor = Color.White : DataGridView4.RowsDefaultCellStyle.SelectionForeColor = Color.White
        PictureBox1.Top = 42
        PictureBox1.Left = 10
        PictureBox1.Width = 1 / 4 * Me.Width
        WebBrowser1.Height = 3 * SystemInformation.PrimaryMonitorSize.Height / 5
        WebBrowser1.Top = 42
        WebBrowser1.Left = 10 + PictureBox1.Width + 10
        WebBrowser1.Width = Me.Width - 30 - PictureBox1.Width
        PictureBox1.Height = WebBrowser1.Height / 2
        DataGridView3.Top = 42 + WebBrowser1.Height + 5
        DataGridView3.Left = 10
        DataGridView3.Width = Me.Width / 2 - 15
        DataGridView3.Height = Me.Height - 3 * SystemInformation.PrimaryMonitorSize.Height / 5 - 10
        DataGridView4.Top = 42 + WebBrowser1.Height + 5
        DataGridView4.Left = 15 + DataGridView3.Width
        DataGridView4.Width = Me.Width / 2 - 10
        DataGridView4.Height = Me.Height - 3 * SystemInformation.PrimaryMonitorSize.Height / 5 - 10

        Button3.Left = WebBrowser1.Left
        song.Left = Button3.Left + Button3.Width + 5
        song.Text = UI.DataGridView1.SelectedCells(2).Value
        Button2.Left = song.Left + song.Width + 5
        UI.downloadbar.Value = 0
        Me.TopMost = True

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub
    Public Sub clear_track_datagrid()
        Me.DataGridView4.Rows.Clear()
    End Sub
    Public Sub clear_similar_artists_datagrid()
        Me.DataGridView3.Rows.Clear()
    End Sub
    Public Sub artist_track_add(ByVal track As String, ByVal popularity As String)
        Me.DataGridView4.Rows.Add(track, popularity)
    End Sub
    Public Sub similar_artists_add(ByVal artist As String, ByVal picture As Image)
        Me.DataGridView3.Rows.Add(artist, picture)
    End Sub
    Public Sub browser_source_refresh(ByVal source As String)
        Me.WebBrowser1.DocumentText = source
    End Sub
    Public Sub new_image(ByVal pic As Image)
        Me.PictureBox1.Image = pic
        Me.PictureBox1.Refresh()
    End Sub

    Public Sub info_name(ByVal artist As String)
        Me.artistnamelabel.Text = artist
    End Sub
    Public Sub permutations(ByVal permutation As String)
        song.Text = permutation
    End Sub
    Public Sub info_search(ByVal searchstring As String)

        Dim url As String
        Dim part As String
        Dim partindex As Integer
        Dim countresults As Integer
        Dim source As String
        Dim listeners As Integer = 0
        Dim newcompare As Integer
        Dim nextindex As Integer



        ' the following has to be rewritten to include all combinations of space delimited substrings

        searchstring = searchstring.Replace(",", "")
        searchstring = searchstring.Replace(".", "").Replace("-", " ")
        searchstring = searchstring.Replace("&bnsp;", " ").Replace("  ", " ")
        'part = searchstring.Split(" ")(0)

        artistname = searchstring
        For nextindex = 0 To searchstring.Split(" ").Length - 1
            part = searchstring.Split(" ")(nextindex)

            For partindex = nextindex To searchstring.Split(" ").Length - 1

                url = "http://www.last.fm/music/" & part
                source = getsourcestring(url)
                countresults = Count_substrings_in_string(source, "cover-image-image")

                Try
                    If countresults > 0 Then
                        newcompare = CInt(Int(multilinextract(source, 0, "class=""chartbar""><span>", 0, "</span>", 0).Replace(",", "")))
                        'send temporary namestring

                    End If
                    If listeners < newcompare Then
                        listeners = newcompare
                        artistname = part
                        If listeners > 1000 Then Exit For
                    End If
                Catch ex As Exception
                    Exit For
                End Try
                If Not partindex = searchstring.Split(" ").Length - 1 Then part = part & " " & searchstring.Split(" ")(partindex + 1)
            Next
            If listeners > 1000 Then Exit For
        Next

        If artistname = Me.artistnamelabel.Text Then
            Exit Sub
        End If
        Me.artistnamelabel.Invoke(Sub() Me.info_name(artistname))


        'get artist page source string

        url = "http://www.last.fm/music/" & artistname.Replace("-", "").Replace("  ", " ")
        source = getsourcestring(url)

        'get artist image

        Dim gai As New Thread(Sub() get_artist_image(source))
        gai.Start()

        'get artist wiki/bio page

        Dim gawp As New Thread(Sub() get_artist_wiki_page(artistname))
        gawp.Start()

        'get artist track list

        Dim gatl As New Thread(Sub() get_artist_track_list(source))

        gatl.Start()

        'get similar artists

        Dim gsa As New Thread(Sub() get_similar_artists(source))
        gsa.Start()






    End Sub

    Public Sub get_artist_image(ByVal sourcestring As String)

        Dim artistimagesource As String
        Dim tClient As WebClient = New WebClient
        Dim pic As Image

        Try

            artistimagesource = linextract(sourcestring, "                <img itemprop=""image"" src=""", """ alt=", 0)

            pic = Image.FromStream(New MemoryStream(tClient.DownloadData(artistimagesource)))
        Catch ex As Exception
            pic = Me.PictureBox1.ErrorImage
        End Try

        'invoke return image from thread
        Me.PictureBox1.Invoke(Sub() Me.new_image(pic))

    End Sub
    Public Sub get_artist_wiki_page(ByVal artistname As String)

        Dim url As String

        url = "http://www.last.fm/music/" & artistname & "/+wiki"
        wikisource = getsourcestring(url)
        If Not wikisource.StartsWith("?") Then

            wikitext = "<html><body bgcolor = ""black"" link=""white""><font size=""2""><font color=""cyan""><font face=""arial"">" & multilinextract(wikisource, 0, "<div id=""wiki"">", 0, "</div>", 0) & "</body></html>"

            ' wikitext = "<html><body bgcolor = ""black"" link=""white""><font size=""2""><font color=""cyan""><font face=""arial"">" & wikisource & "</body></html>"

        End If

        'invoke return browsertext from thread
        Me.WebBrowser1.Invoke(Sub() Me.browser_source_refresh(wikitext))

    End Sub
    Public Sub get_artist_track_list(ByVal sourcestring As String)

        Dim countresults As Integer
        Dim counter As Integer
        Dim track As String
        Dim popularity As String

        'invoke clear tracklist
        Me.DataGridView4.Invoke(Sub() Me.clear_track_datagrid())

        countresults = Count_substrings_in_string(sourcestring, "<span itemprop=""name"">")

        Try
            For counter = 0 To countresults - 1
                track = multilinextract(sourcestring, counter, "><span itemprop=""name"">", 0, "</span>", 0).Replace("&amp;", " ")
                popularity = multilinextract(sourcestring, counter, "class=""chartbar""><span>", 0, "</span>", 0)

                If Not track.StartsWith("?") Then
                    'invoke add new track to datagrid
                    Me.DataGridView4.Invoke(Sub() Me.artist_track_add(track, popularity))
                End If
            Next
        Catch ex As Exception
            MsgBox("cannot load tracklist")
        End Try

    End Sub
    Public Sub get_similar_artists(ByVal sourcestring As String)

        Dim counter As Integer
        Dim artistname As String
        Dim picturelink As String
        Dim pic As Image
        Dim tClient As WebClient = New WebClient

        'invoke clear similar artists list
        Me.DataGridView3.Invoke(Sub() Me.clear_similar_artists_datagrid())

        For counter = 0 To 5
            Try
                artistname = multilinextract(sourcestring, counter, "text-over-image-text", 2, "</span>", 2).Replace("&bnsp;", " ").Replace("  ", " ").Replace("&amp;", "")
                picturelink = multilinextract(sourcestring, counter, "cover-image-image", 7, "alt=", 9)
                If Not artistname.StartsWith("?") Then
                    pic = Image.FromStream(New MemoryStream(tClient.DownloadData(picturelink)))
                    'invoke add similar artist with picture
                    Me.DataGridView3.Invoke(Sub() Me.similar_artists_add(artistname, pic))
                End If
            Catch ex As Exception
                MsgBox("cannot load similar artists")
            End Try
        Next

    End Sub


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim url As String
        Try
            url = ("http://www.last.fm/music/" & artistname)
            System.Diagnostics.Process.Start(url)
        Catch ex As Exception
            MsgBox("Cannot open page")
        End Try


    End Sub

  

    Private Sub DataGridView3_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView3.CellDoubleClick
        UI.nametext.Text = DataGridView3.Rows(e.RowIndex).Cells(0).Value
        UI.DataGridView1.Rows.Clear()
        UI.search_from_info()
        Me.Close()

    End Sub

 

    Private Sub DataGridView4_CellMouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles DataGridView4.CellMouseDoubleClick
        UI.nametext.Text = artistnamelabel.Text & " " & DataGridView4.Rows(e.RowIndex).Cells(0).Value
        UI.DataGridView1.Rows.Clear()
        UI.search_from_info()
        Me.Close()
    End Sub

    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try


            setindex = UI.DataGridView1.SelectedRows(0).Index + 1
            UI.DataGridView1.ClearSelection()
            UI.DataGridView1.Rows(setindex).Selected = True
            UI.play(UI.DataGridView1.Rows(UI.DataGridView1.SelectedRows(0).Index).Cells(0).Value)
            song.Text = UI.DataGridView1.SelectedRows(0).Cells(2).Value
            Button2.Left = song.Left + song.Width + 5
        Catch ex As Exception
            UI.log.Text = "No more songs found"

        End Try
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Try


            setindex = UI.DataGridView1.SelectedRows(0).Index - 1
            UI.DataGridView1.ClearSelection()
            UI.DataGridView1.Rows(setindex).Selected = True
            UI.play(UI.DataGridView1.Rows(UI.DataGridView1.SelectedRows(0).Index).Cells(0).Value)
            song.Text = UI.DataGridView1.SelectedRows(0).Cells(2).Value
            Button2.Left = song.Left + song.Width + 5
        Catch ex As Exception
            UI.log.Text = "No more songs found"

        End Try
    End Sub
End Class