
using ExwhyzeeEDI.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExwhyzeeEDI.Web.Models.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Comment is Required")]
        public string Content { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public DateTime? CommentDate { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }





    }
}