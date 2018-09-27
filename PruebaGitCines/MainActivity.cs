using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Android.Graphics;
using System;

namespace PruebaGitCines
{
    public class Tarjeta
    {
        public string Name { get; set; }

        public Tarjeta(string str)
        {
            Name = str;
        }

    }
    [Activity(Label = "PruebaGitCines", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        ListView mainList;
        Loading cargaDeDatos = new Loading();
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            List<Tarjeta> lis = new List<Tarjeta>();
            List<CineInfo> ls = cargaDeDatos.LoadCinemark();
            Tarjeta t = new Tarjeta("JArrol");
            Tarjeta t1 = new Tarjeta("Oblivion");
            Tarjeta t2 = new Tarjeta("Carlos");

            lis.Add(t);
            lis.Add(t1);
            //lis.Add(t2);

            mainList = FindViewById<ListView>(Resource.Id.mainlistview);
            ImageView image = FindViewById<ImageView>(Resource.Id.imageView1);
            TextView texto = FindViewById<TextView>(Resource.Id.textView1);
            //texto.Text = ls[0].prueba;
            //downloadAsync(image);
            clas2 c = new clas2(this, ls);
            mainList.Adapter = c;

        }

    }
}

