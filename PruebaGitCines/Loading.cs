using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Android.Graphics;
using Android.Webkit;
using Android.Widget;

namespace PruebaGitCines
{
    public class CineInfo
    {
        public string nombrePeliculas;
        public string url;
        public List<string> horariosPeliculas;
        public Bitmap imagenesPeliculas;
        public ImageView image;
        public string prueba = "";
        public string cine;
    }
    public class Loading
    {
        List<string> loadedMovieNames;
        List<string> loadedTimesForMovies;
        public WebClient webClient = new WebClient();
        Byte[] raw;

        public string ReturnEspecificString(string Completo,string TextoInicial, string TextoFinal)
        {
            int indexInicio = Completo.IndexOf(TextoInicial);
            int indexFinal = Completo.IndexOf(TextoFinal);
            
            return indexFinal > indexInicio ? Completo.Substring(indexInicio, (indexFinal + TextoFinal.Length) - indexInicio) : Completo.Substring(indexInicio, Completo.Length - indexInicio);

        }
        async void DownloadPage(string urlString)
        {
            Uri uri = new Uri(urlString);
            raw = webClient.DownloadData(uri);
        }
        public List<CineInfo> LoadCinepolis(){
            List<CineInfo> lista = new List<CineInfo>();
           // lista.Add(new CineInfo());
            DownloadPage("https://www.cinepolis.com.gt/cartelera/guatemala-guatemala/");
            if (raw == null)
                return lista;
            string str =  System.Text.Encoding.UTF8.GetString(raw);//webClient.DownloadString(uri.ToString());

            string meroTexto = "";
            string imagenesCinepolis = "https://static.cinepolis.com/img/peliculas/";
            string dataCinepolis = "class=\"datalayer-movie ng-binding";

            int CantidadImagenes = 0;
            foreach (var myString in str.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                //Cargando imagenes
                if (myString.Contains(imagenesCinepolis))
                {
                    CineInfo cine = new CineInfo();
                    meroTexto =ReturnEspecificString(myString, imagenesCinepolis, "jpg");
                    cine.url = meroTexto;
                    downloadAsync(meroTexto, cine.imagenesPeliculas, cine.image);
                    lista.Add(cine);
                    CantidadImagenes++;
                }
                if(myString.Contains(dataCinepolis))
                {
                    //meroTexto += "-" + ReturnEspecificString(myString, dataCinepolis, "\" ");
                }
            }
            //if(line.Contains("https://static.cinepolis.com/img/peliculas/"))
            //{
            //meroTexto += line.Contains("div") ? "si-" : "no-";

            //}

            lista[0].prueba += meroTexto;
                
            //}
            //catch
            //{
            //    return lista;
            //}


            return lista;
        }
        async void downloadAsync(string urlImage, Bitmap Bitimage,ImageView image)
        {
            var url = new Uri(urlImage);
            byte[] imageBytes = null;
            try
            {
                imageBytes = await webClient.DownloadDataTaskAsync(url);
                //Saving bitmap locally
                if (imageBytes != null)
                {
                    string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                    string localFilename = "imagen.png";
                    string localPath = System.IO.Path.Combine(documentsPath, localFilename);

                    //Save the Image using writeAsync
                    FileStream fs = new FileStream(localPath, FileMode.OpenOrCreate);
                    await fs.WriteAsync(imageBytes, 0, imageBytes.Length);

                    //Close file connection
                    fs.Close();

                    BitmapFactory.Options options = new BitmapFactory.Options();
                    options.InJustDecodeBounds = true;
                    await BitmapFactory.DecodeFileAsync(localPath, options);

                    //Resizing bitmap image
                    options.InSampleSize = options.OutWidth > options.OutHeight ? options.OutHeight / image.Height : options.OutWidth / image.Width;
                    options.InJustDecodeBounds = false;

                    Bitimage = await BitmapFactory.DecodeFileAsync(localPath, options);
                }
            }

            catch
            {
                return;
            }

           

        }
        public List<CineInfo> LoadCinemark()
        {
            loadedMovieNames = new List<string>();
            loadedTimesForMovies = new List<string>();
            return LoadingCinemark(0);
        }

        List<CineInfo> LoadingCinemark(int cin) { 
            string pagina = "";
            string cineactual = "";
            switch (cin)
            {
                case 0:
                    pagina = "https://www.cinemarkca.com/es/theatres/guatemala-arkadia-los-proceres/billboard?tag=gt-ap";
                    cineactual = "Arkadia";
                    break;
                case 1:
                    pagina = "https://www.cinemarkca.com/es/theatres/guatemala-eskala-roosevelt?tag=gt-er";
                    cineactual = "Eskala";
                    break;
                case 2:
                    pagina = "https://www.cinemarkca.com/es/theatres/guatemala-metrocentro/billboard?tag=gt-me";
                    cineactual = "Metrocentro Villanueva";
                    break;
            }
            List<CineInfo> lista = new List<CineInfo>();
            DownloadPage(pagina);
            if (raw == null)
                return lista;
            string str = System.Text.Encoding.UTF8.GetString(raw);

            string meroTexto = "";
            string imagenesCinepolis = "https://cinemarkpy.modyocdn.com/uploads/";
            string dataCinemark = "movie-info-cover";
            var allPageString = str.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            int CantidadImagenes = 0;
            int lastline = -1;
            foreach (var myString in allPageString)
            {
                lastline++;
                //Cargando imagenes
                if (myString.Contains(imagenesCinepolis) && myString.Contains(dataCinemark))
                {
                    string nombreEcontrado = "It Is Not This Name";
                    for(int i = lastline+1; i < allPageString.Length; i++)
                    {
                        if (allPageString[i].Contains(""))
                        {
                            //aqui sigo trabajando
                        }
                    }
                    if (!loadedMovieNames.Contains(nombreEcontrado))
                    {
                        CineInfo cine = new CineInfo();
                        meroTexto = ReturnEspecificString2(myString, imagenesCinepolis);
                        cine.url = meroTexto;
                        cine.cine = cineactual;
                        downloadAsync(meroTexto, cine.imagenesPeliculas, cine.image);
                        lista.Add(cine);
                        CantidadImagenes++;
                    }
                }
            }
            /*public string nombrePeliculas;
        public string url;
        public List<string> horariosPeliculas;
        public Bitmap imagenesPeliculas;
        public ImageView image;
        public string prueba = "";*/
            if (cin < 2)
            {
                lista.AddRange(LoadingCinemark(cin+1));
            }
            return lista;
        }


        public string ReturnEspecificString2(string Completo, string TextoInicial)
        {
            int indexInicio =0;
            int indexFinal =0;
            string TextoFinal = "";
            if (Completo.Contains(TextoInicial))
            {
                indexInicio = Completo.IndexOf(TextoInicial);
                if (Completo.Contains("jpg"))
                {
                    TextoFinal = "jpg";
                }
                else if (Completo.Contains("png"))
                {
                    TextoFinal = "png";
                }
                indexFinal = Completo.IndexOf(TextoFinal);
            }
            else
            {
                return "https://www.1and1.es/digitalguide/fileadmin/DigitalGuide/Teaser/not-found-t.jpg";
            }
            

            return indexFinal > indexInicio ? Completo.Substring(indexInicio, (indexFinal + TextoFinal.Length) - indexInicio) : Completo.Substring(indexInicio, Completo.Length - indexInicio);

        }
    }

}

