using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKK.Models.Repositories;
using Newtonsoft.Json;

namespace AKK.Models
{
    public class Model : IIdentifyable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid Id  { get; set; }
    }
}