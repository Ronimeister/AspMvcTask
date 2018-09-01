using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpamAspMvc.Models
{
    public class Picture
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public byte[] PictureBytes { get; set; }
        public string PictureDescription { get; set; }
    }
}