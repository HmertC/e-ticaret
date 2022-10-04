using Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eticaret.WebUI.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        //[Display(Name="Name",Prompt ="Enter product name")]
        //[Required(ErrorMessage ="Name Zorunlu Bir Alan.")]
        //[StringLength(60,MinimumLength =5,ErrorMessage ="Ürün İsmi 5-10 karakter arasında olmalıdır")]
        public string Name { get; set; }
       
        //[Required(ErrorMessage = "Url Zorunlu Bir Alan.")]
        public string Url { get; set; }
       
        [Required(ErrorMessage = "Price Zorunlu Bir Alan.")]
        [Range(1,100000,ErrorMessage = "Price İçin 1-100000 değer girmelisiniz")]
        public double? Price { get; set; }
       
        [Required(ErrorMessage = "Description Zorunlu Bir Alan.")]
        [StringLength(100,MinimumLength =5,ErrorMessage ="Description 5-100 karakter arasında olmalıdır")]

        public string Description { get; set; }
       
        [Required(ErrorMessage = "Imageurl Zorunlu Bir Alan.")]
        public string ImageUrl { get; set; }
        public bool IsApproved { get; set; }
        public bool IsHome { get; set; }
        public List<Category> SelectedCategories { get; set; }
    }
}
