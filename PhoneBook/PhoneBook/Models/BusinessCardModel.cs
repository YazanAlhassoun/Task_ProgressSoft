using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace PhoneBook.Models
{
    [MetadataType(typeof(BusinessCardMetaData))]
    public partial class BusinessCard
    {
        public HttpPostedFileBase ImageFile { set; get; }

    }

    [Serializable]
    [XmlRoot("BusinessCard")]
    public class BusinessCardMetaData
    {
    
        public int Id { get; set; }

        [Required]
        [XmlElement("Name")]     
        [Display(Name ="Name")]       
        public string Name { get; set; }

        [Required]
        [XmlElement("Gender")]      
        [Display(Name = "Gender")]
        public string Gender { get; set; }

     
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        [XmlElement("DOB")]      
        [Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [EmailAddress]
        [XmlElement("Email")]      
        [Display(Name = "Email Adress")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "Not a valid phone number")]
        [XmlElement("Phone")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }


        [XmlElement("Photo")]
        [Display(Name = "Photo")]
        public string Photo { get; set; }

        [Required]
        [XmlElement("Address")]
        [Display(Name = "Address")]
        public string Address { get; set; }
       
        public string UserId { get; set; }
    }
}