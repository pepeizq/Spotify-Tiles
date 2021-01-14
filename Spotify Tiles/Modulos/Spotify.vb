Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports SpotifyAPI.Web
Imports Windows.UI.Xaml.Media.Animation

Module Spotify

    'https://developer.spotify.com/dashboard/applications/ec4978f924524f1cabe6394c2ad44329
    'https://johnnycrazy.github.io/SpotifyAPI-NET/

    Dim cliente As SpotifyClient

    Public Sub Cargar()

        Dim config As SpotifyClientConfig = SpotifyClientConfig.CreateDefault.WithAuthenticator(New ClientCredentialsAuthenticator("ec4978f924524f1cabe6394c2ad44329", "660fb9bee11048ecb7f2673e78d69e7b"))
        cliente = New SpotifyClient(config)

        Dim recursos As New Resources.ResourceLoader

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gridAlbums As Grid = pagina.FindName("gridAlbums")
        Interfaz.Pestañas.Visibilidad(gridAlbums, recursos.GetString("Albums"), Nothing)

    End Sub

    Public Async Function BuscadorAlbums(texto As String) As Task(Of SearchResponse)

        Dim peticion As New SearchRequest(SearchRequest.Types.Album, texto) With {
            .Limit = 50
        }

        Dim resultados As SearchResponse = Await cliente.Search.Item(peticion)

        Return resultados

    End Function

    Public Async Function BuscadorPlaylists(texto As String) As Task(Of Paging(Of SimplePlaylist))

        Dim resultados As Paging(Of SimplePlaylist) = Await cliente.Playlists.GetUsers(texto)

        Return resultados

    End Function

    Public Sub BotonTile_Click(sender As Object, e As RoutedEventArgs)

        Trial.Detectar()
        Interfaz.AñadirTile.ResetearValores()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim botonJuego As Button = e.OriginalSource
        Dim juego As Tile = botonJuego.Tag

        Dim botonAñadirTile As Button = pagina.FindName("botonAñadirTile")
        botonAñadirTile.Tag = juego

        Dim imagenJuegoSeleccionado As ImageEx = pagina.FindName("imagenJuegoSeleccionado")
        imagenJuegoSeleccionado.Source = juego.ImagenAncha

        Dim tbJuegoSeleccionado As TextBlock = pagina.FindName("tbJuegoSeleccionado")
        tbJuegoSeleccionado.Text = juego.Titulo

        Dim gridAñadirTile As Grid = pagina.FindName("gridAñadirTile")
        Interfaz.Pestañas.Visibilidad(gridAñadirTile, juego.Titulo, Nothing)

        '---------------------------------------------

        ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("animacionJuego", botonJuego)
        Dim animacion As ConnectedAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("animacionJuego")

        If Not animacion Is Nothing Then
            animacion.Configuration = New BasicConnectedAnimationConfiguration
            animacion.TryStart(gridAñadirTile)
        End If

        '---------------------------------------------

        Dim tbImagenTituloTextoTileAncha As TextBox = pagina.FindName("tbImagenTituloTextoTileAncha")
        tbImagenTituloTextoTileAncha.Text = juego.Titulo
        tbImagenTituloTextoTileAncha.Tag = juego.Titulo

        Dim tbImagenTituloTextoTileGrande As TextBox = pagina.FindName("tbImagenTituloTextoTileGrande")
        tbImagenTituloTextoTileGrande.Text = juego.Titulo
        tbImagenTituloTextoTileGrande.Tag = juego.Titulo

        '---------------------------------------------

        Dim imagenPequeña As ImageEx = pagina.FindName("imagenTilePequeña")
        imagenPequeña.Source = Nothing

        Dim imagenMediana As ImageEx = pagina.FindName("imagenTileMediana")
        imagenMediana.Source = Nothing

        Dim imagenAncha As ImageEx = pagina.FindName("imagenTileAncha")
        imagenAncha.Source = Nothing

        Dim imagenGrande As ImageEx = pagina.FindName("imagenTileGrande")
        imagenGrande.Source = Nothing

        If Not juego.ImagenPequeña = Nothing Then
            imagenPequeña.Source = juego.ImagenPequeña
            imagenPequeña.Tag = juego.ImagenPequeña
        End If

        If Not juego.ImagenAncha = Nothing Then
            If Not juego.ImagenMediana = Nothing Then
                imagenMediana.Source = juego.ImagenMediana
                imagenMediana.Tag = juego.ImagenMediana
            Else
                imagenMediana.Source = juego.ImagenAncha
                imagenMediana.Tag = juego.ImagenAncha
            End If

            imagenAncha.Source = juego.ImagenAncha
            imagenAncha.Tag = juego.ImagenAncha
        End If

        If Not juego.ImagenGrande = Nothing Then
            imagenGrande.Source = juego.ImagenGrande
            imagenGrande.Tag = juego.ImagenGrande
        End If

    End Sub

End Module
