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

    }
    public class Loading
    {
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
    }

}

