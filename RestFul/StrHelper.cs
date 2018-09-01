using MySql.Data.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;

namespace RestFul
{
    public static class StrHelper
    {
        private static string Key = "OGL76}9ii@36F>9x";

        #region AESEncrypt 加密并返回字符串
        /// <summary>
        /// AES加密算法
        /// </summary>
        /// <param name="plainText">明文字符串</param>
        /// <returns>将加密后的密文转换为Base64编码，以便显示</returns>
        public static string AESEncrypt(string plainText)
        {

            //分组加密算法
            SymmetricAlgorithm des = Rijndael.Create();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(plainText);//得到需要加密的字节数组 
            //设置密钥及密钥向量
            des.Key = Encoding.UTF8.GetBytes(Key);
            des.IV = Encoding.UTF8.GetBytes(Key);
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.PKCS7;
            byte[] cipherBytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cipherBytes = ms.ToArray();//得到加密后的字节数组
                    cs.Close();
                    ms.Close();
                }
            }

            return Convert.ToBase64String(cipherBytes);
            //return Encoding.UTF8.GetString(cipherBytes);
        }
        #endregion

        #region Encrypt 加密并返回字节数组
        /// <summary>
        /// AES加密算法
        /// </summary>
        /// <param name="plainText">明文字符串</param>
        /// <returns>返回加密后的字节数据以便压缩</returns>
        public static byte[] Encrypt(string plainText)
        {

            //分组加密算法
            SymmetricAlgorithm des = Rijndael.Create();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(plainText);//得到需要加密的字节数组 
            //设置密钥及密钥向量
            des.Key = Encoding.UTF8.GetBytes(Key);
            des.IV = Encoding.UTF8.GetBytes(Key);
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.PKCS7;
            byte[] cipherBytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cipherBytes = ms.ToArray();//得到加密后的字节数组
                    cs.Close();
                    ms.Close();
                }
            }

            return cipherBytes;
            //return Encoding.UTF8.GetString(cipherBytes);
        }
        #endregion 

        #region AESDecrypt解密并获得字符串
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="cipherText">密文字符串</param>
        /// <returns>返回解密后的明文字符串</returns>
        public static string AESDecrypt(string showText)
        {

            byte[] cipherText = Convert.FromBase64String(showText);
            SymmetricAlgorithm des = Rijndael.Create();
            des.Key = Encoding.UTF8.GetBytes(Key);
            des.IV = Encoding.UTF8.GetBytes(Key); ;
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.PKCS7;
            byte[] decryptBytes = new byte[cipherText.Length];
            using (MemoryStream ms = new MemoryStream(cipherText))
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    cs.Read(decryptBytes, 0, decryptBytes.Length);
                    cs.Close();
                    ms.Close();
                }
            }
            return Encoding.UTF8.GetString(decryptBytes).Replace("\0", "");   ///将字符串后尾的'\0'去掉

        }
        #endregion 

        #region Decrypt解密并获得字节
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="cipherText">密文字符串</param>
        /// <returns>返回解密后的明文字符串</returns>
        public static string Decrypt(byte[] cipherText)
        {

            SymmetricAlgorithm des = Rijndael.Create();
            des.Key = Encoding.UTF8.GetBytes(Key);
            des.IV = Encoding.UTF8.GetBytes(Key); ;
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.PKCS7;
            byte[] decryptBytes = new byte[cipherText.Length];
            using (MemoryStream ms = new MemoryStream(cipherText))
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    cs.Read(decryptBytes, 0, decryptBytes.Length);
                    cs.Close();
                    ms.Close();
                }
            }
            return Encoding.UTF8.GetString(decryptBytes).Replace("\0", "");   ///将字符串后尾的'\0'去掉

        }
        #endregion 

        #region Compress算法压缩字节数组
        public static byte[] Compress(byte[] encStr)
        {

            using (MemoryStream ms = new MemoryStream())
            {
                GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true);
                zip.Write(encStr, 0, encStr.Length);
                zip.Close();
                byte[] zipStr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(zipStr, 0, zipStr.Length);
                ms.Close();
                return zipStr;
            }
        }
        #endregion

        #region Decompress 解压缩字节数组
        public static byte[] Decompress(byte[] decStr)
        {
            using (MemoryStream dms = new MemoryStream())
            {
                using (MemoryStream ms = new MemoryStream(decStr))
                {
                    GZipStream uzip = new GZipStream(ms, CompressionMode.Decompress);
                    byte[] bytes = new byte[4096];
                    int n;
                    while ((n = uzip.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        dms.Write(bytes, 0, n);
                    }
                    uzip.Close();
                }
                return dms.ToArray();
            }
            
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>json字符串</returns>
        private static string String2Json(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 格式化字符型、日期型、布尔型
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string StringFormat(string str, Type type)
        {
            str = str.Replace("\"","'");
            str = str.Replace("\\", "");
            if (type == typeof(string))
            {
                str = String2Json(str);
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime))
            {
                str = "\"" + str + "\"";
            }
            else if (type == typeof(MySqlDateTime))
            {
                str = str.Replace("/", "-");
                str = "\"" + str + "\"";
            }
            else if (type == typeof(bool))
            {
                str = str.ToLower();
            }
            else if (type != typeof(string) && string.IsNullOrEmpty(str))
            {
                str = "\"" + str + "\"";
            }
            return str;
        }

        #endregion

        #region list转换成JSON
        /// <summary>
        /// list转换为Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ListToJson<T>(IList<T> list)
        {
            object obj = list[0];
            return ListToJson<T>(list, obj.GetType().Name);
        }
        /// <summary>
        /// list转换为json
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="list"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private static string ListToJson<T>(IList<T> list, string JsonName)
        {
            StringBuilder Json = new StringBuilder();
            if (string.IsNullOrEmpty(JsonName))
                JsonName = list[0].GetType().Name;
            Json.Append("{\"" + JsonName + "\":[");
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    T obj = Activator.CreateInstance<T>();
                    PropertyInfo[] pi = obj.GetType().GetProperties();
                    Json.Append("{");
                    for (int j = 0; j < pi.Length; j++)
                    {
                        Type type = pi[j].GetValue(list[i], null).GetType();
                        Json.Append("\"" + pi[j].Name.ToString() + "\":" + StringFormat(pi[j].GetValue(list[i], null).ToString(), type));
                        if (j < pi.Length - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < list.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }
        #endregion

        #region 对象转换为Json
        /// <summary>
        /// 对象转换为json
        /// </summary>
        /// <param name="jsonObject">json对象</param>
        /// <returns>json字符串</returns>
        public static string ToJson(object jsonObject)
        {
            string jsonString = "{";
            PropertyInfo[] propertyInfo = jsonObject.GetType().GetProperties();
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                object objectValue = propertyInfo[i].GetGetMethod().Invoke(jsonObject, null);
                string value = string.Empty;
                if (objectValue is DateTime || objectValue is Guid || objectValue is TimeSpan)
                {
                    value = "'" + objectValue.ToString() + "'";
                }
                else if (objectValue is string)
                {
                    value = "'" + ToJson(objectValue.ToString()) + "'";
                }
                else if (objectValue is IEnumerable)
                {
                    value = ToJson((IEnumerable)objectValue);
                }
                else
                {
                    value = ToJson(objectValue.ToString());
                }
                jsonString += "\"" + ToJson(propertyInfo[i].Name) + "\":" + value + ",";
            }
            jsonString.Remove(jsonString.Length - 1, jsonString.Length);
            return jsonString + "}";
        }

        #endregion

        #region 对象集合转换为json
        /// <summary>
        /// 对象集合转换为json
        /// </summary>
        /// <param name="array">对象集合</param>
        /// <returns>json字符串</returns>
        public static string ToJson(IEnumerable array)
        {
            string jsonString = "{";
            foreach (object item in array)
            {
                jsonString += ToJson(item) + ",";
            }
            jsonString.Remove(jsonString.Length - 1, jsonString.Length);
            return jsonString + "]";
        }
        #endregion

        #region 普通集合转换Json
        /// <summary>    
        /// 普通集合转换Json   
        /// </summary>   
        /// <param name="array">集合对象</param> 
        /// <returns>Json字符串</returns>  
        public static string ToArrayString(IEnumerable array)
        {
            string jsonString = "[";
            foreach (object item in array)
            {
                jsonString = ToJson(item.ToString()) + ",";
            }
            jsonString.Remove(jsonString.Length - 1, jsonString.Length);
            return jsonString + "]";
        }
        #endregion

        #region  DataSet转换为Json
        /// <summary>    
        /// DataSet转换为Json   
        /// </summary>    
        /// <param name="dataSet">DataSet对象</param>   
        /// <returns>Json字符串</returns>    
        public static string ToJson(DataSet dataSet)
        {
            string jsonString = "{";
            foreach (DataTable table in dataSet.Tables)
            {
                jsonString += "\"" + table.TableName + "\":" + ToJson(table) + ",";
            }
            jsonString = jsonString.TrimEnd(',');
            return jsonString + "}";
        }
        #endregion

        #region Datatable转换为Json
        /// <summary>     
        /// Datatable转换为Json     
        /// </summary>    
        /// <param name="table">Datatable对象</param>     
        /// <returns>Json字符串</returns>     
        public static string ToJson(DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName;
                    string strValue = drc[i][j].ToString();
                    Type type = dt.Columns[j].DataType;
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(strValue, type);
                    if (j < dt.Columns.Count - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            jsonString.Remove(jsonString.Length - 1, 1);
            if (!"".Equals(jsonString.ToString())) jsonString.Append("]");
            return jsonString.ToString();
        }
        /// <summary>    
        /// DataTable转换为Json     
        /// </summary>    
        public static string ToJson(DataTable dt, string jsonName)
        {
            StringBuilder Json = new StringBuilder();
            if (string.IsNullOrEmpty(jsonName))
                jsonName = dt.TableName;
            Json.Append("{\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Type type = dt.Rows[i][j].GetType();
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + StringFormat(dt.Rows[i][j].ToString(), type));
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }
        public static string DataTableToJsonWithJavaScriptSerializer(DataTable table)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;
            foreach (DataRow row in table.Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    childRow.Add(col.ColumnName, row[col]);
                }
                parentRow.Add(childRow);
            }
            return jsSerializer.Serialize(parentRow);
        }
        #endregion

        #region DataReader转换为Json
        /// <summary>     
        /// DataReader转换为Json     
        /// </summary>     
        /// <param name="dataReader">DataReader对象</param>     
        /// <returns>Json字符串</returns>  
        public static string ToJson(SqlDataReader dataReader)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            while (dataReader.Read())
            {
                jsonString.Append("{");
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    Type type = dataReader.GetFieldType(i);
                    string strKey = dataReader.GetName(i);
                    string strValue = dataReader[i].ToString();
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(strValue, type);
                    if (i < dataReader.FieldCount - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            dataReader.Close();
            jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }
        #endregion

        #region choose_net检查是否手机浏览
        public static bool choose_net(String userAgent)
        {
            if (userAgent.IndexOf("Noki") > -1 || // Nokia phones and emulators     
                     userAgent.IndexOf("Eric") > -1 || // Ericsson WAP phones and emulators     
                     userAgent.IndexOf("WapI") > -1 || // Ericsson WapIDE 2.0     
                     userAgent.IndexOf("MC21") > -1 || // Ericsson MC218     
                     userAgent.IndexOf("AUR") > -1 || // Ericsson R320     
                     userAgent.IndexOf("R380") > -1 || // Ericsson R380     
                     userAgent.IndexOf("UP.B") > -1 || // UP.Browser     
                     userAgent.IndexOf("WinW") > -1 || // WinWAP browser     
                     userAgent.IndexOf("UPG1") > -1 || // UP.SDK 4.0     
                     userAgent.IndexOf("upsi") > -1 || //another kind of UP.Browser     
                     userAgent.IndexOf("QWAP") > -1 || // unknown QWAPPER browser     
                     userAgent.IndexOf("Jigs") > -1 || // unknown JigSaw browser     
                     userAgent.IndexOf("Java") > -1 || // unknown Java based browser     
                     userAgent.IndexOf("Alca") > -1 || // unknown Alcatel-BE3 browser (UP based)     
                     userAgent.IndexOf("MITS") > -1 || // unknown Mitsubishi browser     
                     userAgent.IndexOf("MOT-") > -1 || // unknown browser (UP based)     
                     userAgent.IndexOf("My S") > -1 ||//  unknown Ericsson devkit browser      
                     userAgent.IndexOf("WAPJ") > -1 ||//Virtual WAPJAG www.wapjag.de     
                     userAgent.IndexOf("fetc") > -1 ||//fetchpage.cgi Perl script from www.wapcab.de     
                     userAgent.IndexOf("ALAV") > -1 || //yet another unknown UP based browser     
                     userAgent.IndexOf("Wapa") > -1 || //another unknown browser (Web based "Wapalyzer")    
                     userAgent.IndexOf("UCWEB") > -1 || //another unknown browser (Web based "Wapalyzer")    
                     userAgent.IndexOf("BlackBerry") > -1 || //another unknown browser (Web based "Wapalyzer")                     
                     userAgent.IndexOf("J2ME") > -1 || //another unknown browser (Web based "Wapalyzer")              
                     userAgent.IndexOf("Oper") > -1 ||
                     userAgent.IndexOf("Android") > -1 ||
                userAgent.IndexOf("mozilla") > -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 返回一个无-号并且是大写形式的GUID
        public static string GetNewGuid()
        {
            string gid = Guid.NewGuid().ToString();
            gid = gid.Replace("-", "").ToUpper();
            return gid;
        }
        #endregion

        #region GetMd5Str32获取MD5加密串
        public static string GetMd5Str32(string str)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            // Convert the input string to a byte array and compute the hash.  
            char[] temp = str.ToCharArray();
            byte[] buf = new byte[temp.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                buf[i] = (byte)temp[i];
            }
            byte[] data = md5Hasher.ComputeHash(buf);
            // Create a new Stringbuilder to collect the bytes  
            // and create a string.  
            StringBuilder sBuilder = new StringBuilder();
            // Loop through each byte of the hashed data   
            // and format each one as a hexadecimal string.  
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            // Return the hexadecimal string.  
            return sBuilder.ToString();
        }
        #endregion


    }
}
