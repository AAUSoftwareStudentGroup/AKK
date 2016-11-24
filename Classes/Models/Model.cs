using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKK.Classes.Models.Repository;

namespace AKK.Classes.Models
{
    public class Model : IIdentifyable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid Id  { get; set; }
    }
}