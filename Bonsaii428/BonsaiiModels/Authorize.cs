namespace BonsaiiModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Authorize")]
    public partial class Authorize
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ModuleName { get; set; }

        public string UnitId { get; set; }
        [Required]
        [StringLength(50)]
        public string UnitName { get; set; }

        [StringLength(50)]
        public string ActionName { get; set; }

        [StringLength(50)]
        public string ActionValue { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }


    public class AuthorizeViewModel{
        public string ActionName{get;set;}
        public string ActionValue{get;set;}
        public string Name{get;set;}
    }


    public class UnitViewModel
    {
        public string UnitName { get; set; }
        public string UnitId { get; set; }
        public List<AuthorizeViewModel> Actions { get; set; }
    }

    public class ModuleViewModel
    {
        public string ModuleName { get; set; }
        public int UnitCount { get; set; }
        public List<UnitViewModel> Units { get; set; }
    }
    //封装针对View的一行的对象
    public class AuthorizeRowModel
    {
        public string ModuleName{get;set;}
        public int UnitCount { get; set; }

        public string UnitName { get; set; }
        public List<AuthorizeViewModel> Actions { get; set; }
    }
}
