using System.Collections.Generic;
using UnityEngine;

namespace App.Helpers
{
    public static class Extension
    {
        public static Color With(this Color origin, float? r = null, float? g = null, float? b = null, float? a = null)
        {
            return new Color(r ?? origin.r, g ?? origin.g, b ?? origin.b, a ?? origin.a);
        }

        public static Vector3 With(this Vector3 origin, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? origin.x, y ?? origin.y, z ?? origin.z);
        }

        public static Vector2 With(this Vector2 origin, float? x = null, float? y = null)
        {
            return new Vector3(x ?? origin.x, y ?? origin.y);
        }
    
        public static bool ToBool(this int origin)
        {
            return origin > 0;
        }

        public static int ToInt(this bool origin)
        {
            return origin ? 1 : 0;
        }

        public static float ToPercent(this float origin, float percent)
        {
            return origin * percent / 100;
        }

        public static List<Transform> GetChildren(this Transform parent)
        {
            List<Transform> list = new List<Transform>();

            for (int i = 0; i < parent.childCount; i++)
            {
                list.Add(parent.GetChild(i));
            }

            return list;
        }

   

        public static float ToRadian(this float origin)
        {
            return origin / 57.2957795f;
        }

        public static float ToDegree(this float origin)
        {
            return origin * 57.2957795f;
        }
    
        //Bu degree origin trigonometrik deger olmalı yani sag tarafı merkez alıp, saat yonunun tersine olan acı.
        public static Vector2 GetPoint(this float degree)
        {
            //Unity uzayında rotation ile aynı degerler olması için
            degree = 360 - degree; //Saat yönünün tersine
            degree += 90; //Baslangıc noktasını top'a tası

            //Bu radian degerine göre alırsam 0 noktası birim cemberin en sagı oluyor.
            float radian = degree.ToRadian();

            float x = Mathf.Cos(radian);
            float y = Mathf.Sin(radian);

            return new Vector2(x, y);
        }

        public static float GetDegree(this Vector2 coordinate)
        {
            float x = coordinate.x;
            float y = coordinate.y;

            float radianX = Mathf.Acos(x);
            float degreeX = radianX.ToDegree();

            float radianY = Mathf.Asin(y);
            float degreeY = radianY.ToDegree();

            float degree ;

            //++ sorunsuz
            degree = degreeX;

            //-+ 
            if (x < 0 && y >= 0)
                degree = degreeX;

            //+-
            if (x >= 0 && y < 0)
                degree = 360 + degreeY;

            //--
            if (x < 0 && y < 0)
                degree = (180 - degreeX) * 2 + degreeX;


            //Unity uzayına convert işlemi => Merkez Top'ta ve Saat yönüne dogru.
            degree = 360 - degree;
            degree += 90;

            return degree;
        }
    }
}