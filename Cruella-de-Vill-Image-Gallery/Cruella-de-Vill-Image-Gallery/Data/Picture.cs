//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CruellaDeVillImageGallery.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Picture
    {
        public Picture()
        {
            this.Comments = new HashSet<Comment>();
        }
    
        public int Id { get; set; }
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public string BaseUrl { get; set; }
        public string ThumbUrl { get; set; }
    
        public virtual Album Album { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
