using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Punchout.Web.Controllers
{
    public class CheckOutController : Controller
    {
        // GET: CheckOut
        public ActionResult CheckOut()
        {
            string queryString = "SELECT * FROM [cmis_portal_uat].[dbo].[ViewCart] WHERE CartID = '" + Session["PunchOut_CartID"] + "';";
            string queryString3 = "SELECT TOP(1) orderno, orderdt, orderuser FROM [cmis_portal_uat].[dbo].[PunchOrders] ORDER BY orderno DESC;";
            string hiddenHTML = "";
            int x = 0;
            bool hasFreight = false;
            bool hasFuel = false;

            string unitPrice = "";
            double dunitPrice = 0;
            double totalItemPrice = 0;
            string stotalItemPrice = "";
            double totPrice = 0;
            string stotPrice = "";
            //string emailBody = "";
            //string stoAddress = "";
            string orderNo = "";
            int iorderNo = 0;
            //using (SqlConnection sqlcon = new SqlConnection("Data Source=massql\\massql;Initial Catalog=cmis_portal_uat;Persist Security Info=True;User ID=CMIS;Password=Chemico;MultipleActiveResultSets=True"))
            using (SqlConnection sqlcon = new SqlConnection("Data Source=PUNE-LAPTOP-136;Initial Catalog=cmis_portal_uat;Persist Security Info=True;User ID=sa;Password=pass123!@#;MultipleActiveResultSets=True"))
            {
                SqlCommand orcom = new SqlCommand(queryString3, sqlcon);
                sqlcon.Open();

                SqlDataReader orreader = orcom.ExecuteReader();

                orreader.Read();

                if (orreader.HasRows)
                {
                    iorderNo = Convert.ToInt16(orreader[0]);
                    iorderNo++;
                    orderNo = Convert.ToString(iorderNo);
                    orderNo = orderNo.PadLeft(10, '0');
                    orderNo = "PNCH-" + orderNo;
                }
            }
            
            using (SqlConnection con = new SqlConnection("Data Source=PUNE-LAPTOP-136;Initial Catalog=cmis_portal_uat;Persist Security Info=True;User ID=sa;Password=pass123!@#;MultipleActiveResultSets=True"))
            {
                SqlCommand com = new SqlCommand(queryString, con);
                con.Open();

                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    x++;
                    unitPrice = String.Format("{0:0.00}", reader[3]);
                    
                    totalItemPrice = Convert.ToDouble(reader[3]);
                    // Check first if the product has a freight charge
                    if (Convert.ToDouble(reader[11]) > 0 && hasFreight == false)
                    {
                        hasFreight = true;
                        totalItemPrice = totalItemPrice + (Convert.ToDouble(reader[11]) / Convert.ToDouble(reader[4]));
                    }

                    // now check if the product has a fuel charge
                    if (Convert.ToDouble(reader[12]) > 0 && hasFuel == false)
                    {
                        hasFuel = true;
                        totalItemPrice = totalItemPrice + (Convert.ToDouble(reader[12]) / Convert.ToDouble(reader[4]));
                    }

                    // Now we can just add the others in as they are per ite, per quantity
                    totalItemPrice = totalItemPrice + Convert.ToDouble(reader[10]) + Convert.ToDouble(reader[13]) + Convert.ToDouble(reader[14]);
                    stotalItemPrice = String.Format("{0:0.00}", totalItemPrice);

                    // Convert the StandardLeadTime into an equivalent with checking if we need to account for Honeywell
                    // counting weekends as business days

                    int newDays = int.Parse(((reader[6])==null || (reader[6]).ToString()==string.Empty)?"0": (reader[6]).ToString());
                    DateTime cur = System.DateTime.Now;

                    switch (cur.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            if (newDays > 4 && newDays <= 9)
                            {
                                newDays += 2;
                            }
                            else if (newDays > 9 && newDays <= 14)
                            {
                                newDays += 4;
                            }

                            else if (newDays > 14 && newDays <= 19)
                            {
                                newDays += 6;
                            }
                            else if (newDays > 19 && newDays <= 24)
                            {
                                newDays += 8;
                            }
                            break;
                        case DayOfWeek.Tuesday:
                            if (newDays > 3 && newDays <= 8)
                            {
                                newDays += 2;
                            }
                            else if (newDays > 8 && newDays <= 13)
                            {
                                newDays += 4;
                            }

                            else if (newDays > 13 && newDays <= 18)
                            {
                                newDays += 6;
                            }
                            else if (newDays > 18 && newDays <= 23)
                            {
                                newDays += 8;
                            }
                            break;
                        case DayOfWeek.Wednesday:
                            if (newDays > 2 && newDays <= 7)
                            {
                                newDays += 2;
                            }
                            else if (newDays > 7 && newDays <= 12)
                            {
                                newDays += 4;
                            }

                            else if (newDays > 12 && newDays <= 17)
                            {
                                newDays += 6;
                            }
                            else if (newDays > 17 && newDays <= 22)
                            {
                                newDays += 8;
                            }
                            break;
                        case DayOfWeek.Thursday:
                            if (newDays > 1 && newDays <= 6)
                            {
                                newDays += 2;
                            }
                            else if (newDays > 6 && newDays <= 11)
                            {
                                newDays += 4;
                            }

                            else if (newDays > 11 && newDays <= 16)
                            {
                                newDays += 6;
                            }
                            else if (newDays > 16 && newDays <= 21)
                            {
                                newDays += 8;
                            }
                            break;
                        case DayOfWeek.Friday:
                            if (newDays > 6 && newDays <= 11)
                            {
                                newDays += 2;
                            }
                            else if (newDays > 11 && newDays <= 16)
                            {
                                newDays += 4;
                            }

                            else if (newDays > 16 && newDays <= 21)
                            {
                                newDays += 6;
                            }
                            else if (newDays > 21 && newDays <= 26)
                            {
                                newDays += 8;
                            }
                            newDays += 2;
                            break;
                        default:
                            if (newDays > 6 && newDays <= 11)
                            {
                                newDays += 2;
                            }
                            else if (newDays > 11 && newDays <= 16)
                            {
                                newDays += 4;
                            }

                            else if (newDays > 16 && newDays <= 21)
                            {
                                newDays += 6;
                            }
                            else if (newDays > 21 && newDays <= 26)
                            {
                                newDays += 8;
                            }
                            newDays += 2;
                            break;
                    }

                    hiddenHTML += @"<input type=""hidden"" name=""NEW_ITEM-DESCRIPTION[" + x + @"]"" value=""" + reader[2] + @""">";
                    //hiddenHTML += @"<input type=""hidden"" name=""NEW_ITEM-MATNR[" + x + @"]"" value="""">";
                    hiddenHTML += @"<input type=""hidden"" name=""NEW_ITEM-MATGROUP[" + x + @"]"" value=""" + reader[15] + @""">";
                    hiddenHTML += @"<input type=""hidden"" name=""NEW_ITEM-QUANTITY[" + x + @"]"" value=""" + reader[4] + @""">";
                    hiddenHTML += @"<input type=""hidden"" name=""NEW_ITEM-UNIT[" + x + @"]"" value=""" + reader[5] + @""">";
                    hiddenHTML += @"<input type=""hidden"" name=""NEW_ITEM-PRICE[" + x + @"]"" value=""" + stotalItemPrice + @""">";
                    hiddenHTML += @"<input type=""hidden"" name=""NEW_ITEM-PRICEUNIT[" + x + @"]"" value=""1"">";
                    hiddenHTML += @"<input type=""hidden"" name=""NEW_ITEM-CURRENCY[" + x + @"]"" value=""USD"">";
                    hiddenHTML += @"<input type=""hidden"" name=""NEW_ITEM-LEADTIME[" + x + @"]"" value=""" + newDays + @""">";
                    hiddenHTML += @"<input type=""hidden"" name=""NEW_ITEM-VENDOR[" + x + @"]"" value=""116034"">";
                    hiddenHTML += @"<input type=""hidden"" name=""NEW_ITEM-VENDORMAT[" + x + @"]"" value=""" + reader[0] + @""">";
                    hiddenHTML += @"<input type=""hidden"" name=""NEW_ITEM-MANUFACTCODE[" + x + @"]"" value="""">";
                    hiddenHTML += @"<input type=""hidden"" name=""NEW_ITEM-MANUFACTMAT[" + x + @"]"" value=""" + reader[0] + @""">";
                    hiddenHTML += @"<input type=""hidden"" name=""NEW_ITEM-SERVICE[" + x + @"]"" value="""">";
                    hiddenHTML += @"<input type=""hidden"" name=""NEW_ITEM-EXT_PRODUCT_ID[" + x + @"]"" value="""">";
                    hiddenHTML += @"<input type=""hidden"" name=""NEW_ITEM-CUSTFIELD1[" + x + @"]"" value="""">";
                    hiddenHTML += @"<input type=""hidden"" name=""NEW_ITEM-CUSTFIELD2[" + x + @"]"" value="""">";
                    hiddenHTML += @"<input type=""hidden"" name=""NEW_ITEM-CUSTFIELD3[" + x + @"]"" value="""">";
                    hiddenHTML += @"<input type=""hidden"" name=""NEW_ITEM-CUSTFIELD4[" + x + @"]"" value="""">";
                    hiddenHTML += @"<input type=""hidden"" name=""NEW_ITEM-CUSTFIELD5[" + x + @"]"" value="""">";
                    totPrice = totPrice + totalItemPrice;  
                }
                reader.Close();
                con.Close();
            }
            stotPrice = String.Format("{0:0.00}", totPrice);
           
            // the form submission to HubWoo
            Response.Write(hiddenHTML);
            
            // Now that we are all done, we need to clear out this user's shopping cart
            string deleteString = "DELETE FROM [cmis_portal_uat].[dbo].[ShoppingCart] WHERE CartID = '" + Session["CartId"] + "';";
            String Date = DateTime.Now.ToString("dd/MM/yyyy");
            string orderString = "INSERT INTO [cmis_portal_uat].[dbo].[PunchOrders] VALUES(getdate(), '" + Session["UserName"] + "');";
            int numRows1 = 0;
            int numRows2 = 0;
            using (SqlConnection condel = new SqlConnection("Data Source=PUNE-LAPTOP-136;Initial Catalog=cmis_portal_uat;Persist Security Info=True;User ID=sa;Password=pass123!@#;MultipleActiveResultSets=True"))
            {
                SqlCommand com3 = new SqlCommand(deleteString, condel);
                SqlCommand com4 = new SqlCommand(orderString, condel);
                condel.Open();
                numRows1 = com3.ExecuteNonQuery();
                numRows2 = com4.ExecuteNonQuery();

                condel.Close();
            }

            return View();
        }
    }
}