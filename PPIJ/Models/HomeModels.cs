using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using System.Runtime.Serialization.Json;
using System.Text;
using System.IO;

namespace PPIJ.Models
{
    public class MessageModel
    {
        [Display(Name = "Name")]
        [StringLength(100, ErrorMessage = "Ime mora biti najmanje {2} znaka dugo.", MinimumLength = 4)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email adresa je potrebna")]
        [EmailAddress(ErrorMessage = "Email adresa nije u dobrom formatu")]
        [Display(Name = "Email")]
        public string Email{ get; set; }

        [StringLength(100, ErrorMessage = "Naslov mora biti najmanje {2} znaka dug.", MinimumLength = 4)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Molimo napišite neku poruku")]
        [Display(Name = "Message")]
        public string Message { get; set; }

        [Display(Name = "TestEmail")]
        public string TestEmail { get; set; }

    }
    public class JsonHelper
    {
        /// <summary>
        /// JSON Serialization
        /// </summary>
        public static string JsonSerializer<T>(T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }
        /// <summary>
        /// JSON Deserialization
        /// </summary>
        public static T JsonDeserialize<T>(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }
    }
}