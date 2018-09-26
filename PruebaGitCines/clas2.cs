
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.Animation;
using Android.Util;
using Android.Content.Res;
using Android.Graphics;
using System.Net;
using System.IO;

namespace PruebaGitCines
{
    public class clas2 : BaseAdapter<CineInfo>
    {
        Activity context;
        List<CineInfo> list;

        public clas2(Activity _context, List<CineInfo> _list) : base()
        {
            this.context = _context;
            this.list = _list;
        }

        public override int Count
        {
            get { return list.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override CineInfo this[int index]
        {
            get { return list[index]; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            ImageView boton;
            ImageView imagenBasePelicula;
            ImageView portada;
            TextView text;
            RelativeLayout tarjetaPelicula;
            bool flyinUp = true;
            // re-use an existing view, if one is available
            // otherwise create a new one
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.TarjetaPeliculas_Small, parent, false);

            CineInfo item = this[position];
            //view.FindViewById<TextView>(Resource.Id.tituloPelicula).Text = item.Name;
            Loading ls = new Loading();
            imagenBasePelicula = view.FindViewById<ImageView>(Resource.Id.imageView1);
            text = view.FindViewById<TextView>(Resource.Id.tituloPelicula);
            portada = view.FindViewById<ImageView>(Resource.Id.peliculaPortada);
            tarjetaPelicula = view.FindViewById<RelativeLayout>(Resource.Id.layoutPelicula);
            boton = view.FindViewById<ImageView>(Resource.Id.Arrows);
            imagenBasePelicula.SetColorFilter(Color.Rgb(14, 56, 177));
            text.Text = item.url;
            downloadAsync(ls.webClient, portada, item.url);

            boton.Click += (o, e) =>
            {
                /*SpringAnimation spring = new SpringAnimation(imagenPelicula, DynamicAnimation.TranslationY, 0);
                spring.Spring.SetStiffness(SpringForce.StiffnessLow);
                spring.Spring.SetDampingRatio(SpringForce.DampingRatioHighBouncy);
                spring.SetStartVelocity(DpToPx(-2000));
                spring.Start();*/
                /*FlingAnimation fling = new FlingAnimation(tarjetaPelicula, DynamicAnimation.TranslationY);
                fling.SetStartVelocity(DpToPx(flyinUp ? -250 : 250));
                fling.SetFriction(1);
                fling.Start();
                flyinUp = !flyinUp;*/

            };

            return view;
        }
        private float DpToPx(float dp)
        {
            return TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, Resources.System.DisplayMetrics);
        }
        async void downloadAsync(WebClient webClient ,ImageView image, string urlImage)
        {
            if (urlImage == null)
                return;
            var url = new Uri(urlImage);
            byte[] imageBytes = null;

            //Show loading progress


            try
            {
                imageBytes = await webClient.DownloadDataTaskAsync(url);
            }

            catch
            {

                return;
            }

            //Saving bitmap locally
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string localFilename = "imagen.png";
            string localPath = System.IO.Path.Combine(documentsPath, localFilename);

            //Save the Image using writeAsync
            FileStream fs = new FileStream(localPath, FileMode.OpenOrCreate);
            await fs.WriteAsync(imageBytes, 0, imageBytes.Length);
            Console.WriteLine("Saving image in local path: " + localPath);

            //Close file connection
            fs.Close();

            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InJustDecodeBounds = true;
            await BitmapFactory.DecodeFileAsync(localPath, options);

            //Resizing bitmap image
            options.InSampleSize = options.OutWidth > options.OutHeight ? options.OutHeight / image.Height : options.OutWidth / image.Width;
            options.InJustDecodeBounds = false;

            Bitmap bitmap = await BitmapFactory.DecodeFileAsync(localPath, options);
            image.SetImageBitmap(bitmap);
            //Hide progress bar layout

            //Toggle button click listener

        }
    }
}