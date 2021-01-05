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
            AddHandler botonBuscarAlbums.PointerEntered, AddressOf EfectosHover.Entra_Boton_IconoTexto
            AddHandler botonBuscarAlbums.PointerExited, AddressOf EfectosHover.Sale_Boton_IconoTexto

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

        Private Async Sub BuscarAlbumsClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            boton.IsEnabled = False

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim pbBuscadorAlbums As ProgressBar = pagina.FindName("pbBuscadorAlbums")
            pbBuscadorAlbums.Visibility = Visibility.Visible

            Dim tbBuscadorAlbums As TextBox = pagina.FindName("tbBuscadorAlbums")
            tbBuscadorAlbums.IsEnabled = False

            Dim resultados As SearchResponse = Await Spotify.Buscador(tbBuscadorAlbums.Text.Trim)

            If Not resultados Is Nothing Then
                Dim gv As AdaptiveGridView = pagina.FindName("gvTiles")
                gv.DesiredWidth = anchoColumna
                gv.Items.Clear()

                Dim listaAlbums As New List(Of Tile)

                For Each album In resultados.Albums.Items
                    Dim album2 As New Tile(album.Name, album.Id, album.Uri, album.Images(0).Url, album.Images(0).Url, album.Images(0).Url, album.Images(0).Url)
                    listaAlbums.Add(album2)
                Next

                For Each album In listaAlbums
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
                        .Source = album.ImagenGrande,
                        .IsCacheEnabled = True,
                        .Stretch = Stretch.UniformToFill,
                        .Padding = New Thickness(0, 0, 0, 0),
                        .HorizontalAlignment = HorizontalAlignment.Center,
                        .VerticalAlignment = VerticalAlignment.Center,
                        .EnableLazyLoading = True
                    }

                    botonAlbum.Tag = album
                    botonAlbum.Content = imagen
                    botonAlbum.Padding = New Thickness(0, 0, 0, 0)
                    botonAlbum.Background = New SolidColorBrush(Colors.Transparent)

                    panel.Content = botonAlbum

                    Dim tbToolTip As TextBlock = New TextBlock With {
                        .Text = album.Titulo,
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
            End If

            pbBuscadorAlbums.Visibility = Visibility.Collapsed
            boton.IsEnabled = True
            tbBuscadorAlbums.IsEnabled = True

        End Sub

    End Module

End Namespace