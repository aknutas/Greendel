using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.Xml.Serialization;
using System.IO.IsolatedStorage;

namespace CloverMobile
{
    public class XmlSerilizierHelper
    {
        public static void Serialize(string filename, object objectForSerialization)
        {
            var appStorage = IsolatedStorageFile.GetUserStoreForApplication();
            Stream streamObject = new MemoryStream();
            streamObject = appStorage.OpenFile(filename, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            if (objectForSerialization == null || streamObject == null)
                return;
            XmlSerializer serializer = new XmlSerializer(objectForSerialization.GetType());
            serializer.Serialize(streamObject, objectForSerialization);
            streamObject.Position = 0;
            streamObject.Close();
        }
        public static object Deserialize(string filename, Type serializedObjectType)
        {
            var appStorage = IsolatedStorageFile.GetUserStoreForApplication();
            Stream streamObject = new MemoryStream();
            streamObject = appStorage.OpenFile(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            if (serializedObjectType == null || streamObject == null)
                return null;
            XmlSerializer serializer = new XmlSerializer(serializedObjectType);           
            return serializer.Deserialize(streamObject);
            //streamObject.Position = 0;
            //streamObject.Close();
        }
    }
}
