using ClosedXML.Excel;
using Microsoft.AspNet.Identity;
using PhoneBook.Common;
using PhoneBook.Models;
using PhoneBook.Models.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace PhoneBooks.Controllers
{
    [Authorize]
    public class BusinessCardController : Controller
    {

        private readonly IRepository<BusinessCard> repository;
        private readonly PhoneBookDBEntities _dbContext;

        public BusinessCardController(IRepository<BusinessCard> _repository)
        {
            this.repository = _repository;
            _dbContext = new PhoneBookDBEntities();
        }


        // GET: BusinessCard
        public ActionResult Index()
        {
            var Cards = repository.List();
            return View(Cards);
        }

        // GET: BusinessCard/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BusinessCard/Create
        [HttpPost]
        public ActionResult Create(BusinessCard model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    model.UserId = User.Identity.GetUserId();
                    //model.Photo = SaveImageFile(model.ImageFile);

                    model.Photo = base64_Creater(model.ImageFile);


                    var Result = repository.Add(model);

                    if (Result == SavingStatus.ExistsEmail)
                    {
                        ViewBag.message = "Email address or phone number is exists ..!";
                        var Obj = model;
                        Obj.ImageFile = model.ImageFile;
                        return View(model);
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }

                }
                return View();

            }
            catch
            {
                return View();
            }
        }

        // GET: BusinessCard/Edit/5

        public ActionResult Delete(int id)
        {
            var cards = repository.Find(id);
            return View(cards);
        }

        // POST: BusinessCard/Delete/5
        [HttpPost]
        public ActionResult ConfirmDelete(int id)
        {
            try
            {
                if (!id.Equals(null))
                {
                    repository.Delete(id);
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Delete", id);
            }
            catch
            {
                return View();
            }
        }


        public string base64_Creater(HttpPostedFileBase file)
        {
            string filename = Path.Combine(Server.MapPath("~/Uploads/Images"), file.FileName);
            file.SaveAs(filename);

            byte[] fileByte = new byte[file.ContentLength];
            file.InputStream.Read(fileByte, 0, file.ContentLength);

            String Base64String = Convert.ToBase64String(fileByte);
            byte[] newByreFile = Convert.FromBase64String(Base64String);


            return Base64String;
        }



        /// <summary>
        /// Import Xml methods
        /// </summary>
        /// <returns></returns>

        public ActionResult xmlAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase xmlFile)
        {
            if (xmlFile.ContentType.Equals("application/xml") || xmlFile.ContentType.Equals("text/xml"))
            {
                var xmlPath = Server.MapPath("~/Uploads/XMLFileUpload" + xmlFile.FileName);
                xmlFile.SaveAs(xmlPath);

                XDocument xDoc = XDocument.Load(xmlPath);
                List<BusinessCard> BusinessCardList = xDoc.Descendants("BusinessCard").Select
                    (Card => new BusinessCard
                    {
                        //Id = Convert.ToInt32(Card.Element("Id").Value),
                        Name = Card.Element("Name").Value,
                        Gender = Card.Element("Gender").Value,
                        DateOfBirth = DateTime.ParseExact(Card.Element("DOB").Value, "dd/MM/yyyy", null),
                        Email = Card.Element("Email").Value,
                        Phone = Card.Element("Phone").Value,
                        Photo = Card.Element("Photo").Value,
                        Address = Card.Element("Address").Value,
                        UserId = User.Identity.GetUserId()


                    }).ToList();


                TempData["showxml"] = BusinessCardList;

                XMLConnection(BusinessCardList);


                ViewBag.Success = "File uploaded successfully..";
            }
            else
            {
                ViewBag.Error = "Invalid file(Upload xml file only)";
            }

            var Cards = repository.List();
            return View("Index", Cards);
        }

        public void XMLConnection(List<BusinessCard> businessCard)
        {
            using (PhoneBookDBEntities db = new PhoneBookDBEntities())
            {

                foreach (var i in businessCard)
                {
                    var v = db.BusinessCards.Where(a => a.Id.Equals(i.Id)).FirstOrDefault();


                    if (v != null)
                    {
                        v.Id = i.Id;
                        v.Name = i.Name;
                        v.Gender = i.Gender;
                        v.DateOfBirth = i.DateOfBirth;
                        v.Email = i.Email;
                        v.Phone = i.Phone;
                        v.Photo = i.Photo;
                        v.Address = i.Address;
                        v.UserId = User.Identity.GetUserId();
                    }
                    else
                    {
                        repository.Add(i);
                    }
                    db.SaveChanges();

                }
            }
        }







        /// <summary>
        /// Import Csv methods
        /// </summary>
        /// <returns></returns>
        public ActionResult csvAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult csvAdd(HttpPostedFileBase postedFile)
        {
            string filePath = string.Empty;
            if (postedFile != null)
            {
                string path = Server.MapPath("~/Uploads/CSVFileUpload/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(filePath);


                //Create a DataTable.
                CSVConnection(filePath);



            }

            return View();
        }

        public System.Data.DataTable DataTable(string filePath = "")
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.AddRange(new DataColumn[7] { new DataColumn("Id", typeof(int)),
                                new DataColumn("Name", typeof(string)),
                                 new DataColumn("Gender", typeof(string)),
                                  new DataColumn("Email", typeof(string)),
                                   new DataColumn("Phone", typeof(int)),
                                    new DataColumn("Photo", typeof(string)),
                                     new DataColumn("Address",typeof(string)) });

            //Read the contents of CSV file.
            string csvData = System.IO.File.ReadAllText(filePath);

            //Execute a loop over the rows.
            foreach (string row in csvData.Split('\n'))
            {
                if (!string.IsNullOrEmpty(row))
                {
                    dt.Rows.Add();
                    int i = 0;

                    //Execute a loop over the columns.
                    foreach (string cell in row.Split(','))
                    {
                        dt.Rows[dt.Rows.Count - 1][i] = cell;
                        i++;
                    }
                }
            }

            return dt;

        }

        public void CSVConnection(string filePath)
        {
            var dt = DataTable(filePath);

            string conString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                {

                    // database table .

                    sqlBulkCopy.DestinationTableName = "dbo.BusinessCard";


                    // Map the DataTable with database table
                    sqlBulkCopy.ColumnMappings.Add("Id", "Id");
                    sqlBulkCopy.ColumnMappings.Add("Name", "Name");
                    sqlBulkCopy.ColumnMappings.Add("Gender", "Gender");
                    sqlBulkCopy.ColumnMappings.Add("Email", "Email");
                    sqlBulkCopy.ColumnMappings.Add("Phone", "Phone");
                    sqlBulkCopy.ColumnMappings.Add("Photo", "Photo");
                    sqlBulkCopy.ColumnMappings.Add("Address", "Address");

                    //////
                    ////
                    ///


                    string UiseriID = User.Identity.GetUserId(); // here i have a problem which is inability to add current user identity to F.K field in BusinessCard table ??????


                    //
                    //
                    ////
                    ///////


                    con.Open();

                    sqlBulkCopy.WriteToServer(dt);
                    con.Close();
                }
            }

        }









        /// <summary>
        /// Export To CSV File
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public FileResult ExportCSV()
        {
            using (PhoneBookDBEntities db = new PhoneBookDBEntities())
            {
                System.Data.DataTable dt = new System.Data.DataTable("Grid");
                dt.Columns.AddRange(new DataColumn[8] { new DataColumn("Id"),
                                            new DataColumn("Name"),
                                            new DataColumn("Gender"),
                                             new DataColumn("DateOfBirth"),
                                              new DataColumn("Email"),
                                            new DataColumn("Phone"),
                                              new DataColumn("Photo"),
                                            new DataColumn("Address") });


                var BusinessCard = from obj in db.BusinessCards.ToList()
                                   select obj;

                foreach (var BusinessCards in BusinessCard)
                {
                    dt.Rows.Add(BusinessCards.Id, BusinessCards.Name, BusinessCards.Gender, BusinessCards.DateOfBirth, BusinessCards.Email, BusinessCards.Phone, BusinessCards.Photo, BusinessCards.Address);
                }

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Phone_Book.csv");
                    }
                }
            }

        }




        /// <summary>
        /// Export To XML File
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public ActionResult ExportToXML()
        {

            var products = repository.List();

            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("BusinessCards");
            xml.AppendChild(root);
            foreach (var product in products)
            {
                XmlElement child = xml.CreateElement("BusinessCard");
                child.SetAttribute("ID", product.Id.ToString());
                child.SetAttribute("Name", product.Name);
                child.SetAttribute("Gender", product.Gender);
                child.SetAttribute("DateOfBirth", Convert.ToString(product.DateOfBirth));
                child.SetAttribute("Email", product.Email);
                child.SetAttribute("Phone", product.Phone.ToString());
                child.SetAttribute("Photo", product.Photo);
                root.AppendChild(child);
            }
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.ContentType = "application/xml";
            Response.AddHeader("Content-Disposition", "attachment; filename=ProductDetails.xml;");
            Response.Output.Write(xml.OuterXml.ToString());
            Response.Flush();
            Response.End();

            xml = null;
            return View();
        }

    }
}
