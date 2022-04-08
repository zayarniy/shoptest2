﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;
using System.Reflection;
using Microsoft.Office.Interop.Word;
using ShopVik;
using System.Data.OleDb;

namespace ShopVik
{
    class Product
    {
        public string Name;
        public int Price;
        public int Count;

        public Product(string Name, int Price, int Count)
        {
            this.Name = Name;
            this.Price = Price;
            this.Count = Count;
        }
    }
    class Service
    {


        static public void CreateDocument(object filename, Receipt receipt)
        {
            try
            {
                //Create an instance for word app  
                Microsoft.Office.Interop.Word.Application winword = new Microsoft.Office.Interop.Word.Application();

                //Set animation status for word application  
                winword.ShowAnimation = false;

                //Set status for word application is to be visible or not.  
                winword.Visible = false;

                //Create a missing variable for missing value  
                object missing = System.Reflection.Missing.Value;

                //Create a new document  
                Microsoft.Office.Interop.Word.Document document = winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);

                //Add header into the document  
                foreach (Microsoft.Office.Interop.Word.Section section in document.Sections)
                {
                    //Get the header range and add the header details.  
                    Microsoft.Office.Interop.Word.Range headerRange = section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    headerRange.Fields.Add(headerRange, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage);
                    headerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    headerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdBlue;
                    headerRange.Font.Size = 10;
                    headerRange.Text = "Header text goes here";
                }

                ////Add the footers into the document  
                //foreach (Microsoft.Office.Interop.Word.Section wordSection in document.Sections)
                //{
                //    //Get the footer range and add the footer details.  
                //    Microsoft.Office.Interop.Word.Range footerRange = wordSection.Footers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                //    footerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdDarkRed;
                //    footerRange.Font.Size = 10;
                //    footerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                //    footerRange.Text = "Footer text goes here";
                //}

                //adding text to document  
                document.Content.SetRange(0, 0);
                document.Content.Text = receipt.Str;


                ////Add paragraph with Heading 1 style  
                //Microsoft.Office.Interop.Word.Paragraph para1 = document.Content.Paragraphs.Add(ref missing);
                //object styleHeading1 = "Heading 1";
                //para1.Range.set_Style(ref styleHeading1);
                //para1.Range.Text = "Para 1 text";
                //para1.Range.InsertParagraphAfter();

                ////Add paragraph with Heading 2 style  
                //Microsoft.Office.Interop.Word.Paragraph para2 = document.Content.Paragraphs.Add(ref missing);
                //object styleHeading2 = "Heading 2";
                //para2.Range.set_Style(ref styleHeading2);
                //para2.Range.Text = "Para 2 text";
                //para2.Range.InsertParagraphAfter();

                //Create a 5X5 table and insert some dummy record  
                //Table firstTable = document.Tables.Add(para1.Range, 5, 5, ref missing, ref missing);

                //firstTable.Borders.Enable = 1;
                //foreach (Row row in firstTable.Rows)
                //{
                //    foreach (Cell cell in row.Cells)
                //    {
                //        //Header row  
                //        if (cell.RowIndex == 1)
                //        {
                //            cell.Range.Text = "Column " + cell.ColumnIndex.ToString();
                //            cell.Range.Font.Bold = 1;
                //            //other format properties goes here  
                //            cell.Range.Font.Name = "Arial";
                //            cell.Range.Font.Size = 10;
                //            //cell.Range.Font.ColorIndex = WdColorIndex.wdGray25;                              
                //            cell.Shading.BackgroundPatternColor = WdColor.wdColorGray25;
                //            //Center alignment for the Header cells  
                //            cell.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                //            cell.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

                //        }
                //        //Data row  
                //        else
                //        {
                //            cell.Range.Text = (cell.RowIndex - 2 + cell.ColumnIndex).ToString();
                //        }
                //    }
                //}

                //Save the document  
                // object filename = @"D:\temp\temp1.docx";
                document.SaveAs2(ref filename);
                document.Close(ref missing, ref missing, ref missing);
                document = null;
                winword.Quit(ref missing, ref missing, ref missing);
                winword = null;
                System.Diagnostics.Debug.WriteLine("Document created successfully !");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }


        static string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=..\..\..\shop.accdb";

        public static int? Check(string login, string password, out User user)
        {
            OleDbConnection connection = null;
            Nullable<int> status = null;
            user = null;
            try
            {
                connection = new OleDbConnection(connectionString); //создание подключения к бд
                connection.Open();
                OleDbCommand command = new OleDbCommand("Select * from Accounts where login='" + login + "' and pass='" + password + "'", connection);//Create command
                var n = command.ExecuteScalar();
                if (n != null)
                {
                    var reader = command.ExecuteReader();
                    reader.Read();
                    string lastName = (reader["Lastname"] is DBNull) ? "" : (string)reader["Lastname"];
                    string firstName = (reader["Firstname"] is DBNull) ? "" : (string)reader["Firstname"];
                    string phone = (reader["PhoneNumber"] is DBNull) ? "" : (string)reader["PhoneNumber"];
                    string email = (reader["Email"] is DBNull) ? "" : (string)reader["Email"];
                    user = new User(lastName, firstName, phone, email);

                    status = (int)reader["Status"];

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection?.Close();
            }
            return (status);
        }

        public static bool Registration(string login, string password, string firstName, string lastName, string phone, string email, string city, out string result, out User user)
        {
            user = null;
            OleDbConnection connection = new OleDbConnection(connectionString); //создание подключения к бд
            connection.Open();
            OleDbCommand command = new OleDbCommand("Select * from Accounts where login='" + login + "'", connection);//Create command
            var n = command.ExecuteScalar();
            if (n != null)
            {
                result = "Пользователь " + login + " уже существует";
                return false;
                ;
            }
            string query = @"Insert into Accounts (login, pass, firstname, lastname, phonenumber, email, city) values ('" + login + "','" + password + "','" + firstName + "','" + lastName + "','" + phone + "','" + email + "','" + city + "')";
            System.Diagnostics.Debug.WriteLine(query);
            command = new OleDbCommand(query, connection);//Create command
            n = command.ExecuteNonQuery();
            connection.Close();
            result = "Пользователь " + login + " добавлен";
            user = new User(lastName, firstName, phone, email);
            return true;
        }




        public class User
        {
            public string LastName { get; private set; }
            public string FirstName { get; private set; }
            public string Phone { get; private set; }

            public string Email { get; private set; }

            public User(string LastName, string FirstName, string Phone, string Email)
            {
                this.LastName = LastName; ;
                this.FirstName = FirstName;
                this.Phone = Phone;
                this.Email = Email;
            }

        }

        public class Receipt
        {

            public User User { get; private set; }
            public List<Product> Cart;
            public int Sum;

            public void Init(User user)
            {
                User = user;
                Cart = new List<Product>();
                Cart.Add(new Product("Item", 1000, 5));
                Cart.Add(new Product("Item", 1000, 5));
                Cart.Add(new Product("Item", 1000, 5));
                Sum = 0;
                foreach (var item in Cart)
                    Sum += item.Count * item.Price;
            }

            public string Str
            {
                get
                {
                    string str = "";
                    str += "Имя:" + User.FirstName + Environment.NewLine +
                        "Фамилия:" + User.LastName + Environment.NewLine +
                        "Телефон:" + User.Phone + Environment.NewLine;
                    foreach (var item in Cart)
                        str += item.Name + " " + item.Count + "x" + item.Price + " " + item.Count * item.Price + Environment.NewLine;
                    str += "Итог:" + Sum + Environment.NewLine;
                    str += "Время:" + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
                    return str;
                }
            }

            //public void Init(string LastName, string FirstName, string Phone)
            //{
            //    this.LastName = "LastName";
            //    this.FirstName = "FirstName";
            //    this.Phone = "Phone";
            //}
        }


    }
}


