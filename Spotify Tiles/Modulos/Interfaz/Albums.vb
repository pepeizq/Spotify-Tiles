Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports SpotifyAPI.Web
Imports Windows.UI

Namespace Interfaz

    Module Albums

        Public anchoColumna As Integer = 250

        Public Sub Cargar()

            Dim recursos As New Resources.ResourceLoader

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim tbBuscadorAlbums As TextBox = pagina.FindName("tbBuscadorAlbums")

            AddHandler tbBuscadorAlbums.TextChanged, AddressOf BuscadorAlbumsTextoCambia

            Dim botonBuscarAlbums As Button = pagina.FindName("botonBuscarAlbums")
            botonBuscarAlbums.IsEnabled = False

            AddHandler botonBuscarAlbums.Click, AddressOf BuscarAlbumsClick
            AddHandler botonBuscarAlbums.PointerEntered, AddressOf EfectosHover.Entra_Boton_1_05
            AddHandler botonBuscarAlbums.PointerExited, AddressOf EfectosHover.Sale_Boton_1_05

            Dim tbBuscadorPlaylists As TextBox = pagina.FindName("tbBuscadorPlaylists")

            AddHandler tbBuscadorPlaylists.TextChanged, AddressOf BuscadorPlaylistsTextoCambia

            Dim botonBuscarPlaylists As Button = pagina.FindName("botonBuscarPlaylists")
            botonBuscarPlaylists.IsEnabled = False

            AddHandler botonBuscarPlaylists.Click, AddressOf BuscarPlaylistsClick
            AddHandler botonBuscarPlaylists.PointerEntered, AddressOf EfectosHover.Entra_Boton_1_05
            AddHandler botonBuscarPlaylists.PointerExited, AddressOf EfectosHover.Sale_Boton_1_05

            Dim botonAbrirUsuarioID As Button = pagina.FindName("botonAbrirUsuarioID")

            AddHandler botonAbrirUsuarioID.Click, AddressOf ComoAveriguarUsuarioIDClick
            AddHandler botonAbrirUsuarioID.PointerEntered, AddressOf EfectosHover.Entra_Boton_1_05
            AddHandler botonAbrirUsuarioID.PointerExited, AddressOf EfectosHover.Sale_Boton_1_05

            Dim botonUsuarioIDVolver As Button = pagina.FindName("botonUsuarioIDVolver")

            AddHandler botonUsuarioIDVolver.Click, AddressOf VolverUsuarioIDClick
            AddHandler botonUsuarioIDVolver.PointerEntered, AddressOf EfectosHover.Entra_Boton_1_05
            AddHandler botonUsuarioIDVolver.PointerExited, AddressOf EfectosHover.Sale_Boton_1_05

        End Sub

        Private Sub BuscadorAlbumsTextoCambia(sender As Object, e As TextChangedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim botonBuscarAlbums As Button = pagina.FindName("botonBuscarAlbums")

            Dim tb As TextBox = sender

            If tb.Text.Trim.Length > 2 Then
                botonBuscarAlbums.IsEnabled = True
            Else
                botonBuscarAlbums.IsEnabled = False
            End If

        End Sub

        Private Sub BuscadorPlaylistsTextoCambia(sender As Object, e As TextChangedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim botonBuscarPlaylists As Button = pagina.FindName("botonBuscarPlaylists")

            Dim tb As TextBox = sender

            If tb.Text.Trim.Length > 2 Then
                botonBuscarPlaylists.IsEnabled = True
            Else
                botonBuscarPlaylists.IsEnabled = False
            End If

        End Sub

        Private Async Sub BuscarAlbumsClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            boton.IsEnabled = False

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim pbBuscadorAlbums As ProgressBar = pagina.FindName("pbBuscadorAlbums")
            pbBuscadorAlbums.Visibility = Visibility.Visible

            Dim tbBuscadorAlbums As TextBox = pagina.FindName("tbBuscadorAlbums")
            tbBuscadorAlbums.IsEnabled = False

            Dim resultados As SearchResponse = Await Spotify.BuscadorAlbums(tbBuscadorAlbums.Text.Trim)

            If Not resultados Is Nothing Then
                Dim gv As AdaptiveGridView = pagina.FindName("gvTiles")
                gv.DesiredWidth = anchoColumna
                gv.Items.Clear()

                Dim listaAlbums As New List(Of Tile)

                For Each album In resultados.Albums.Items
                    Dim album2 As New Tile(album.Name, album.Id, album.Uri, album.Images(0).Url, album.Images(0).Url, album.Images(0).Url, album.Images(0).Url)
                    listaAlbums.Add(album2)
                Next

                GenerarTiles(gv, listaAlbums)
            End If

            pbBuscadorAlbums.Visibility = Visibility.Collapsed
            boton.IsEnabled = True
            tbBuscadorAlbums.IsEnabled = True

        End Sub

        Private Async Sub BuscarPlaylistsClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            boton.IsEnabled = False

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim pbBuscadorPlaylists As ProgressBar = pagina.FindName("pbBuscadorPlaylists")
            pbBuscadorPlaylists.Visibility = Visibility.Visible

            Dim tbBuscadorPlaylists As TextBox = pagina.FindName("tbBuscadorPlaylists")
            tbBuscadorPlaylists.IsEnabled = False

            Dim resultados As Paging(Of SimplePlaylist) = Await Spotify.BuscadorPlaylists(tbBuscadorPlaylists.Text.Trim)

            If Not resultados Is Nothing Then
                Dim gv As AdaptiveGridView = pagina.FindName("gvTiles")
                gv.DesiredWidth = anchoColumna
                gv.Items.Clear()

                Dim listaPlaylists As New List(Of Tile)

                For Each playlist In resultados.Items
                    Dim playlist2 As New Tile(playlist.Name, playlist.Id, playlist.Uri, playlist.Images(0).Url, playlist.Images(0).Url, playlist.Images(0).Url, playlist.Images(0).Url)
                    listaPlaylists.Add(playlist2)
                Next

                GenerarTiles(gv, listaPlaylists)
            End If

            pbBuscadorPlaylists.Visibility = Visibility.Collapsed
            boton.IsEnabled = True
            tbBuscadorPlaylists.IsEnabled = True

        End Sub

        Private Sub GenerarTiles(gv As AdaptiveGridView, lista As List(Of Tile))

            For Each objeto In lista
                Dim panel As New DropShadowPanel With {
                    .Margin = New Thickness(10, 10, 10, 10),
                    .ShadowOpacity = 0.9,
                    .BlurRadius = 10,
                    .MaxWidth = anchoColumna + 20,
                    .HorizontalAlignment = HorizontalAlignment.Center,
                    .VerticalAlignment = VerticalAlignment.Center
                }

                Dim botonAlbum As New Button

                Dim imagen As New ImageEx With {
                    .Source = objeto.ImagenGrande,
                    .IsCacheEnabled = True,
                    .Stretch = Stretch.UniformToFill,
                    .Padding = New Thickness(0, 0, 0, 0),
                    .HorizontalAlignment = HorizontalAlignment.Center,
                    .VerticalAlignment = VerticalAlignment.Center,
                    .EnableLazyLoading = True
                }

                botonAlbum.Tag = objeto
                botonAlbum.Content = imagen
                botonAlbum.Padding = New Thickness(0, 0, 0, 0)
                botonAlbum.Background = New SolidColorBrush(Colors.Transparent)

                panel.Content = botonAlbum

                Dim tbToolTip As TextBlock = New TextBlock With {
                    .Text = objeto.Titulo,
                    .FontSize = 16,
                    .TextWrapping = TextWrapping.Wrap
                }

                ToolTipService.SetToolTip(botonAlbum, tbToolTip)
                ToolTipService.SetPlacement(botonAlbum, PlacementMode.Mouse)

                AddHandler botonAlbum.Click, AddressOf Spotify.BotonTile_Click
                AddHandler botonAlbum.PointerEntered, AddressOf Entra_Boton_Imagen
                AddHandler botonAlbum.PointerExited, AddressOf Sale_Boton_Imagen

                gv.Items.Add(panel)
            Next

        End Sub

        Private Sub ComoAveriguarUsuarioIDClick(sender As Object, e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim gridUsuarioID As Grid = pagina.FindName("gridUsuarioID")
            Pestañas.Visibilidad(gridUsuarioID, Nothing, Nothing)

        End Sub

        Private Sub VolverUsuarioIDClick(sender As Object, e As RoutedEventArgs)

            Dim recursos As New Resources.ResourceLoader

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim gridAlbums As Grid = pagina.FindName("gridAlbums")
            Pestañas.Visibilidad(gridAlbums, recursos.GetString("Spotify_AlbumsPlaylists"), Nothing)

        End Sub

    End Module

End Namespace