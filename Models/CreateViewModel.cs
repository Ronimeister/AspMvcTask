using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpamAspMvc.Models
{
    public class CreateViewModel
    {
        public Picture PictureInner { get; set; }
        public string PicturePath { get; set; }
    }
}