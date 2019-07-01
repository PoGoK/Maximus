using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using MySql.Data.MySqlClient;
using System.IO;
using System.Net;
using EzrealtyToRia.Classes;

namespace EzrealtyToRia
{
    public partial class Form1 : Form
    {
        List<string> corruptObjects = new List<string>();
        public Form1()
        {
            InitializeComponent();
            LoadWorkers();
        }

        private MySqlConnection _connection;
        private MySqlConnection Connection { 
            get 
            { 
                if(_connection == null)
                {
                    string MySQL_host = @"db7.freehost.com.ua";
                    string MySQL_port = "3306";
                    string MySQL_uid = "maximuszt_pocc";
                    string MySQL_pw = "Bh272424";

                    _connection =
                        new MySqlConnection("Data Source=" + MySQL_host + ";Port=" + MySQL_port + ";User Id=" + MySQL_uid +
                                            ";Password=" + MySQL_pw + ";" + "Database=" + "maximuszt_maximus;  Allow Zero Datetime=true;");
                }
                return _connection;
            }
            set
            {
                _connection = value;
            }
        }

        private DataTable GetExportTable(bool exportAll)
        {
            
            // Создаем соединение. Формат строки соединения подробно описан в прилагающейся документации.
            MySqlCommand Query = new MySqlCommand(); // С помощью этого объекта выполняются запросы к БД
            Query.Connection = Connection; // Присвоим объекту только что созданное соединение

            try
            {
                Console.WriteLine("Соединяюсь с сервером базы данных...");
                if(Connection.State == ConnectionState.Closed)
                    Connection.Open();// Соединяемся
            }
            catch (MySqlException SSDB_Exception)
            {
                // Ошибка - выходим
                Console.WriteLine("Проверьте настройки соединения, не могу соединиться с базой данных!\nОшибка: " + SSDB_Exception.Message);
            }
            DataTable dt = new DataTable("jos_ezrealty");
            try
            {

                MySqlDataAdapter da = new MySqlDataAdapter();
                string query = "select * from jos_ezrealty WHERE published = 1 " + (exportAll ? "" : "AND doexport = 1");
                da.SelectCommand = new MySqlCommand(query, Connection);
                da.Fill(dt);

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                Connection = null;
            }
            return dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            WriteXmlRia(GetExportTable(cbRiaExportAll.Checked));
            
            groupBox1.Enabled = true;
            toolStripStatusLabel1.Text = "Файл для Дом Риа сформирован";
        }

        private const string StartElement = "realties";
        private const string ObjectStartElement = "realty";
        private List<Worker> Workers = new List<Worker>();

        private void LoadWorkers()
        {
            MySqlCommand Query = new MySqlCommand(); // С помощью этого объекта выполняются запросы к БД
            Query.Connection = Connection; // Присвоим объекту только что созданное соединение

            try
            {
                Console.WriteLine("Соединяюсь с сервером базы данных...");
                if (Connection.State == ConnectionState.Closed)
                    Connection.Open();// Соединяемся
            }
            catch (MySqlException SSDB_Exception)
            {
                // Ошибка - выходим
                Console.WriteLine("Проверьте настройки соединения, не могу соединиться с базой данных!\nОшибка: " + SSDB_Exception.Message);
            }
            DataTable dt = new DataTable("jos_ezrealty_profile");
            try
            {

                MySqlDataAdapter da = new MySqlDataAdapter();
                string query = "SELECT * FROM jos_ezrealty_profile WHERE published = 1 ";
                da.SelectCommand = new MySqlCommand(query, Connection);
                da.Fill(dt);
            }
            catch (Exception)
            {
                throw;
            }

            foreach(DataRow row in dt.Rows)
            {
                Workers.Add(new Worker
                {
                    Email = row["dealer_email"].ToString(),
                    Id = (int)row["mid"],
                    Name = row["dealer_name"].ToString(),
                    Phones = new List<string> { (string)row["dealer_phone"], (string)row["dealer_fax"], (string)row["dealer_mobile"] }
                });
            }
        }


        private void WriteXmlRia(DataTable dataTable)
        {
            /*type 3 - аренда
                   1 - продажа
                   */
            using (XmlWriter _xmlWriter = XmlWriter.Create(cbRiaExportAll.Checked ? "maximusFull.xml" : "maximus.xml"))
            {
                _xmlWriter.WriteStartDocument();
                _xmlWriter.WriteStartElement(StartElement, "http://dom.ria.ua/xml/xsd/");
                //_xmlWriter.WriteAttributeString("xmlns", null, "http://dom.ria.ua/xml/xsd/");
                _xmlWriter.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                _xmlWriter.WriteAttributeString("xsi", "schemaLocation", null, "http://dom.ria.ua/xml/xsd/ http://dom.ria.ua/xml/xsd/dom.xsd");
                _xmlWriter.WriteElementString("generation_date", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss+03:00"));
                foreach (DataRow row in dataTable.Rows)
                {
                    try
                    {
                        string realty_type = string.Empty;
                        string obj_type = string.Empty;
                        switch (int.Parse(row["cid"].ToString()))
                        {
                            case 1:
                                obj_type = realty_type = "квартира";
                                break;
                            case 2:
                                obj_type = realty_type = "дом";
                                break;
                            case 3:
                                obj_type = realty_type = "часть дома";
                                break;
                            case 4:
                                obj_type = realty_type = "дача";
                                break;
                            case 6:
                                obj_type = realty_type = "участок под жилую застройку";
                                break;
                            case 11:
                                obj_type = realty_type = "комната";
                                break;
                            case 13:
                                realty_type = "офисное помещение";
                                obj_type = "торгово-офисный центр";
                                break;
                            case 14:
                                realty_type = "торговые площади";
                                obj_type = "здание / комплекс / павильон";
                                break;
                            case 15:
                                realty_type = "производственные помещения";
                                obj_type = "здание / комплекс";
                                break;
                            case 16:
                                realty_type = "складские помещения";
                                obj_type = "здание / комплекс";
                                break;
                            case 17:
                                obj_type = realty_type = "кафе, бар, ресторан";
                                break;
                            case 18:
                                obj_type = realty_type = "земля коммерческого назначения";
                                break;
                            default: break;
                        }
                        if (realty_type == string.Empty)
                            continue;
                        var worker = Workers.Find(w => w.Id == int.Parse(row["owner"].ToString()));
                        if (worker == null)
                            MessageBox.Show("Объект с кодом = " + row["office_id"]+" назначен на агента с ошибкой");
                        _xmlWriter.WriteStartElement(ObjectStartElement);
                        _xmlWriter.WriteElementString("local_realty_id", row["office_id"].ToString());


                        _xmlWriter.WriteElementString("email", worker.Email);
                        if (!row["stid"].Equals(4))
                            _xmlWriter.WriteElementString("district", row["locality"].ToString());
                        _xmlWriter.WriteElementString("street", row["address2"].ToString());
                        _xmlWriter.WriteElementString("realty_type", realty_type);

                        _xmlWriter.WriteElementString("advert_type", int.Parse(row["type"].ToString()) == 1 ? "продажа" : "долгосрочная аренда");
                        _xmlWriter.WriteElementString("state", "Житомирская");
                        if (row["stid"].Equals(4))
                            _xmlWriter.WriteElementString("city", row["locality"].ToString());
                        else
                            _xmlWriter.WriteElementString("city", "Житомир");
                        _xmlWriter.WriteElementString("description", string.Format("{0} <br /> {1} <br /> Код объекта в базе Максимус: {2}", row["smalldesc"].ToString(), row["propdesc"].ToString(), row["office_id"].ToString()));
                        _xmlWriter.WriteStartElement("characteristics");
                        _xmlWriter.WriteElementString("rooms_count", row["bedrooms"].ToString());
                        _xmlWriter.WriteElementString("object_type", obj_type);
                        string wall_type = string.Empty;
                        switch (int.Parse(row["furnished"].ToString()))
                        {
                            case 1:
                                wall_type = "кирпич";
                                break;
                            case 2:
                                wall_type = "панель";
                                break;
                            case 3:
                                wall_type = "брус";
                                break;
                            case 4:
                                wall_type = "ракушечник (ракушняк)";
                                break;
                            case 5:
                                wall_type = "дерево и кирпич";
                                break;
                            case 6:
                                wall_type = "пеноблок";
                                break;
                            case 7:
                                wall_type = "";
                                break;
                            default: break;
                        }
                        _xmlWriter.WriteElementString("wall_type", wall_type);
                        _xmlWriter.WriteElementString("street", row["address2"].ToString());

                        _xmlWriter.WriteElementString("total_area", row["squarefeet"].ToString());
                        _xmlWriter.WriteElementString("floor", row["custom1"].ToString());
                        _xmlWriter.WriteElementString("floors", row["custom2"].ToString());
                        _xmlWriter.WriteElementString("currency", "$");
                        _xmlWriter.WriteElementString("price", row["price"].ToString());

                        if (int.Parse(row["cid"].ToString()) == 6 || int.Parse(row["cid"].ToString()) == 18)
                            _xmlWriter.WriteElementString("price_type", "за участок");
                        else
                            _xmlWriter.WriteElementString("price_type", "за объект");

                        _xmlWriter.WriteElementString("plot_area", row["schooldist"].ToString());
                        _xmlWriter.WriteElementString("plot_area_unit", "сотка");

                        _xmlWriter.WriteEndElement();

                        bool isImg = false;
                        _xmlWriter.WriteStartElement("photos_urls");

                        for (int i = 1; i <= 12; i++)
                        {
                            if (row["image" + i].ToString() != "")
                            {
                                isImg = true;
                                _xmlWriter.WriteElementString("loc", "http://maximuszt.com/components/com_ezrealty/ezrealty/" + row["image" + i].ToString());
                            }
                        }
                        _xmlWriter.WriteEndElement();

                        _xmlWriter.WriteEndElement();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Объект с кодом " + row["office_id"].ToString() + "содержит ошибки и не будет выгружен.");
                        continue;
                    }
                    _xmlWriter.Flush();

                }
                _xmlWriter.WriteEndDocument();
                _xmlWriter.Flush();
                _xmlWriter.Close();
            }
        }

        private void btSendRiaToFtp_Click(object sender, EventArgs e)
        {
            SendFileToFtp(cbRiaExportAll.Checked ? "maximusFull.xml" : "maximus.xml");
        }

        private void WriteXmlPliz(DataTable dataTable)
        {
            /*type 3 - аренда
                   1 - продажа
                   */
            XmlWriter _xmlWriter = XmlWriter.Create(cbGetAllPliz.Checked ? "maximusPlizFull.xml" : "maximusPliz.xml");
            _xmlWriter.WriteStartDocument();
            _xmlWriter.WriteStartElement("import");
            _xmlWriter.WriteElementString("feed_version_format", "0.2");
            _xmlWriter.WriteElementString("generation_date", DateTime.UtcNow.ToString("yyyy-MM-dd"));
            _xmlWriter.WriteStartElement("realties");

            foreach (DataRow row in dataTable.Rows)
            {
                try
                {
                    string realty_type = string.Empty;
                    string obj_type = string.Empty;
                    switch (int.Parse(row["cid"].ToString()))
                    {
                        case 1:
                            obj_type = "Квартира";
                            realty_type = "Квартиры";
                            break;
                        case 2:
                            obj_type = "Дом";
                            realty_type = "Дома";
                            break;
                        case 3:
                            obj_type = "Часть дома";
                            realty_type = "Дома";
                            break;
                        case 4:
                            obj_type = "Дача";
                            realty_type = "Дома";
                            break;
                        case 6:
                            obj_type = "Под жилую застройку";
                            realty_type = "Земельные участки";
                            break;
                        case 11:
                            obj_type = "Комната в общежитии";
                            realty_type = "Комнаты";
                            break;
                        case 13:
                            realty_type = "Офисное помещение";
                            obj_type = "Коммерческая недвижимость";
                            break;
                        case 14:
                            realty_type = "Торговые площади";
                            obj_type = "Коммерческая недвижимость";
                            break;
                        case 15:
                            realty_type = "Производств. помещения";
                            obj_type = "Коммерческая недвижимость";
                            break;
                        case 16:
                            realty_type = "Складские помещения";
                            obj_type = "Коммерческая недвижимость";
                            break;
                        case 17:
                            obj_type = "Кафе, ресторан";
                            realty_type = "Коммерческая недвижимость";
                            break;
                        case 18:
                            obj_type = "Коммерческого назначения";
                            realty_type = "Земельные участки";
                            break;
                        default: break;
                    }
                    if (realty_type == string.Empty)
                        continue;

                    _xmlWriter.WriteStartElement("realty");
                    _xmlWriter.WriteElementString("id", row["office_id"].ToString());

                    var worker = Workers.Find(w => w.Id == int.Parse(row["owner"].ToString()));
                    /*string ownerEmail = "mrc.maximus@mail.ru";
                    List<string> ownerPhones = new List<string>();
                    string ownerName;

                    switch (int.Parse(row["owner"].ToString()))
                    {
                        case 258:
                            ownerEmail = "aptsale@ukr.net";
                            ownerPhones = new List<string> { "+380673776037", "+380734668059", "+380970178868" };
                            ownerName = "Кособуцкий Андрей";
                            break;
                        case 117:
                            ownerEmail = "pemaximus@ukr.net";
                            ownerPhones = new List<string> { "+380976913509" };
                            ownerName = "Пясковская Евгения  Станиславовна";
                            break;
                        case 106:
                            ownerEmail = "firsan08@i.ua";
                            ownerPhones = new List<string> { "+380672173007", "+380933333299" };
                            ownerName = "Фирстов Андрей Георгиевич";
                            break;
                        case 99:
                            ownerEmail = "nick_1981@mail.ru";
                            ownerPhones = new List<string> { "+380974488743" };
                            ownerName = "Порядин Николай  Степанович";
                            break;
                        case 85:
                            ownerEmail = "forestin@yandex.ru";
                            ownerPhones = new List<string> { "+380673601930" };
                            ownerName = "Алушин Иван Викторович";
                            break;
                        case 82:
                            ownerEmail = "pavlichenko@maximuszt.com";
                            ownerPhones = new List<string> { "+380680813697" };
                            ownerName = "Павличенко Игорь Николаевич";
                            break;
                        case 80:
                            ownerEmail = "kosyha@maximuszt.com";
                            ownerPhones = new List<string> { "+380973546305" };
                            ownerName = "Косуха Светлана Леонидовна";
                            break;
                        case 77:
                            ownerEmail = "minaka@maximuszt.com";
                            ownerPhones = new List<string> { "+380934501325", "+380975705610" };
                            ownerName = "Минака Тамара Владимировна";
                            break;
                        case 70:
                            ownerEmail = "v_maximus@ukr.net";
                            ownerPhones = new List<string> { "+380674126593" };
                            ownerName = "Полищук Виктор Сергеевич";
                            break;
                        case 96:
                            ownerEmail = "agent.zt@ukr.net";
                            ownerPhones = new List<string> { "+380939909829", "+380953229552", "+380969860005" };
                            ownerName = "Витюк Андрей Петрович";
                            break;
                        case 112:
                            ownerEmail = "kvartiruzt@ukr.net";
                            ownerPhones = new List<string> { "+380970178868" };
                            ownerName = "Андрей";
                            break;
                        case 83:
                            ownerEmail = "kovalchukzt@ukr.net";
                            ownerPhones = new List<string> { "+380932052680", "+380977423995" };
                            ownerName = "Ольга";
                            break;
                        default:
                            ownerEmail = "mrc.maximus@mail.ru";
                            ownerPhones = new List<string> { "+380988914923" };
                            ownerName = "Полищук Светлана Юрьевна";
                            break;
                    }*/

                    _xmlWriter.WriteStartElement("user");
                    _xmlWriter.WriteElementString("email", worker.Email);
                    _xmlWriter.WriteStartElement("phones");
                    foreach(var phone in worker.Phones)
                        _xmlWriter.WriteElementString("phn", phone);
                    _xmlWriter.WriteEndElement();
                    _xmlWriter.WriteEndElement();

                    _xmlWriter.WriteElementString("motion", int.Parse(row["type"].ToString()) == 1 ? "Продать" : "Сдать");
                    if (int.Parse(row["type"].ToString()) != 1)
                        _xmlWriter.WriteElementString("rent_type", "Долгосрочно");

                    _xmlWriter.WriteElementString("catalog", realty_type);
                    _xmlWriter.WriteElementString("sub_catalog", obj_type);
                    _xmlWriter.WriteElementString("region", "6");

                    if (!row["stid"].Equals(4))
                        _xmlWriter.WriteElementString("sub_region", row["locality"].ToString());
                    if (row["stid"].Equals(4))
                        _xmlWriter.WriteElementString("location", row["locality"].ToString());
                    else
                        _xmlWriter.WriteElementString("location", "Житомир");
                    
                    _xmlWriter.WriteElementString("street", row["address2"].ToString());

                    _xmlWriter.WriteElementString("cur", "Доллар");
                    _xmlWriter.WriteElementString("price", row["price"].ToString());
                    _xmlWriter.WriteElementString("price_type", "за все");
                    _xmlWriter.WriteElementString("proposal", "От посредника");

                    

                    _xmlWriter.WriteElementString("description", string.Format("{0} <br /> {1} <br /> Код объекта в базе Максимус: {2}", row["smalldesc"].ToString(), row["propdesc"].ToString(), row["office_id"].ToString()));
                    _xmlWriter.WriteElementString("rooms", row["bedrooms"].ToString());
                    _xmlWriter.WriteElementString("floor", row["custom1"].ToString());
                    _xmlWriter.WriteElementString("all_floors", row["custom2"].ToString());

                    string wall_type = string.Empty;
                    switch (int.Parse(row["furnished"].ToString()))
                    {
                        case 1:
                            wall_type = "кирпич";
                            break;
                        case 2:
                            wall_type = "панель";
                            break;
                        case 3:
                            wall_type = "брус";
                            break;
                        case 4:
                            wall_type = "ракушняк";
                            break;
                        case 5:
                            wall_type = "дерево и кирпич";
                            break;
                        case 6:
                            wall_type = "пеноблок";
                            break;
                        case 7:
                            wall_type = "другое";
                            break;
                        default: break;
                    }
                    _xmlWriter.WriteElementString("walls_type", wall_type);

                    _xmlWriter.WriteElementString("all_sq", row["squarefeet"].ToString());

                    if (!string.IsNullOrEmpty(row["schooldist"].ToString()))
                    {
                        _xmlWriter.WriteElementString("plot_area", row["schooldist"].ToString());
                        _xmlWriter.WriteElementString("meas", "сотка");
                    }

                    bool isImg = false;
                    _xmlWriter.WriteStartElement("photos");

                    for (int i = 1; i <= 12; i++)
                    {
                        if (row["image" + i].ToString() != "")
                        {
                            isImg = true;
                            _xmlWriter.WriteElementString("url", "http://maximuszt.com/components/com_ezrealty/ezrealty/" + row["image" + i].ToString());
                        }
                    }
                    _xmlWriter.WriteEndElement();
                    _xmlWriter.WriteEndElement();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Объект с кодом " + row["office_id"].ToString() + "содержит ошибки и не будет выгружен.");
                    continue;
                }
                _xmlWriter.Flush();

            }
            _xmlWriter.WriteEndElement();

            _xmlWriter.WriteEndDocument();
            _xmlWriter.Flush();
            _xmlWriter.Close();
        }

        private void btSendPlizToFtp_Click(object sender, EventArgs e)
        {
            SendFileToFtp(cbGetAllPliz.Checked ? "maximusPlizFull.xml" : "maximusPliz.xml");
        }


        private void WriteXmlMesto(DataTable dataTable)
        {
            /*type 3 - аренда
                   1 - продажа
                   */
            XmlWriter _xmlWriter = XmlWriter.Create(cbMestoExportAll.Checked ? "maximusMestoFull.xml" : "maximusMesto.xml");
            _xmlWriter.WriteStartDocument();
            _xmlWriter.WriteStartElement("properties");
            foreach (DataRow row in dataTable.Rows)
            {
                try
                {
                    string realty_type = string.Empty;
                    string obj_type = string.Empty;
                    switch (int.Parse(row["cid"].ToString()))
                    {
                        case 1:
                            obj_type = realty_type = "flat";
                            break;
                        case 2:
                        case 3:
                        case 4:
                            obj_type = realty_type = "house";
                            break;
                        case 6:
                            obj_type = realty_type = "plot";
                            break;
                        case 11:
                            obj_type = realty_type = "room";
                            break;
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                            realty_type = "commercial";
                            obj_type = "торгово-офисный центр";
                            break;
                        default: break;
                    }
                    if (realty_type == string.Empty)
                        continue;

                    var worker = Workers.Find(w => w.Id == int.Parse(row["owner"].ToString()));
                    /*string ownerEmail = "mrc.maximus@mail.ru";
                    List<string> ownerPhones = new List<string>();
                    string ownerName;

                    switch (int.Parse(row["owner"].ToString()))
                    {
                        case 258:
                            ownerEmail = "aptsale@ukr.net";
                            ownerPhones = new List<string> { "+380673776037", "+380734668059", "+380970178868" };
                            ownerName = "Кособуцкий Андрей";
                            break;
                        case 117:
                            ownerEmail = "pemaximus@ukr.net";
                            ownerPhones = new List<string> { "+380976913509" };
                            ownerName = "Пясковская Евгения  Станиславовна";
                            break;
                        case 106:
                            ownerEmail = "firsan08@i.ua";
                            ownerPhones = new List<string> { "+380672173007", "+380933333299" };
                            ownerName = "Фирстов Андрей Георгиевич";
                            break;
                        case 99:
                            ownerEmail = "nick_1981@mail.ru";
                            ownerPhones = new List<string> { "+380974488743" };
                            ownerName = "Порядин Николай  Степанович";
                            break;
                        case 85:
                            ownerEmail = "forestin@yandex.ru";
                            ownerPhones = new List<string> { "+380673601930" };
                            ownerName = "Алушин Иван Викторович";
                            break;
                        case 82:
                            ownerEmail = "pavlichenko@maximuszt.com";
                            ownerPhones = new List<string> { "+380680813697" };
                            ownerName = "Павличенко Игорь Николаевич";
                            break;
                        case 80:
                            ownerEmail = "kosyha@maximuszt.com";
                            ownerPhones = new List<string> { "+380973546305" };
                            ownerName = "Косуха Светлана Леонидовна";
                            break;
                        case 77:
                            ownerEmail = "minaka@maximuszt.com";
                            ownerPhones = new List<string> { "+380934501325", "+380975705610" };
                            ownerName = "Минака Тамара Владимировна";
                            break;
                        case 70:
                            ownerEmail = "v_maximus@ukr.net";
                            ownerPhones = new List<string> { "+380674126593" };
                            ownerName = "Полищук Виктор Сергеевич";
                            break;
                        case 96:
                            ownerEmail = "agent.zt@ukr.net";
                            ownerPhones = new List<string> { "+380939909829", "+380953229552", "+380969860005" };
                            ownerName = "Витюк Андрей Петрович";
                            break;
                        case 112:
                            ownerEmail = "kvartiruzt@ukr.net";
                            ownerPhones = new List<string> { "+380970178868" };
                            ownerName = "Андрей";
                            break;
                        case 83:
                            ownerEmail = "kovalchukzt@ukr.net";
                            ownerPhones = new List<string> { "+380932052680", "+380977423995" };
                            ownerName = "Ольга";
                            break;
                        default:
                            ownerEmail = "mrc.maximus@mail.ru";
                            ownerPhones = new List<string> { "+380988914923" };
                            ownerName = "Полищук Светлана Юрьевна";
                            break;
                    }*/

                    _xmlWriter.WriteStartElement("property");
                    _xmlWriter.WriteElementString("xml_id", row["office_id"].ToString());
                    _xmlWriter.WriteElementString("deal_type", int.Parse(row["type"].ToString()) == 1 ? "sale" : "rent");
                    _xmlWriter.WriteElementString("property_type", realty_type);
                    string town;
                    if (row["stid"].Equals(4))
                        town =  row["locality"].ToString();
                    else
                        town =  "Житомир";
                    _xmlWriter.WriteElementString("addr_city", "Житомирская область, " + town);
                    _xmlWriter.WriteElementString("addr_street", row["address2"].ToString());
                    _xmlWriter.WriteElementString("rooms", row["bedrooms"].ToString());
                    _xmlWriter.WriteElementString("area", row["squarefeet"].ToString());
                    _xmlWriter.WriteElementString("description", string.Format("{0}  {1}  Код объекта в базе Максимус: {2}", row["smalldesc"].ToString(), row["propdesc"].ToString(), row["office_id"].ToString()));
                    _xmlWriter.WriteElementString("price", row["price"].ToString().Split(new []{','})[0]);
                    _xmlWriter.WriteElementString("currency", "USD");
                    _xmlWriter.WriteElementString("source_url", "http://maximuszt.com/index.php?option=com_ezrealty&task=detail&id=" + row["id"].ToString());
                    _xmlWriter.WriteElementString("contact_person", worker.Name);
                    _xmlWriter.WriteStartElement("phones");
                        foreach(string person in worker.Phones)
                            _xmlWriter.WriteElementString("phone", person);
                    _xmlWriter.WriteEndElement();
                    _xmlWriter.WriteStartElement("photos");

                    for (int i = 1; i <= 12; i++)
                    {
                        if (row["image" + i].ToString() != "")
                        {
                            _xmlWriter.WriteElementString("photo_url", "http://maximuszt.com/components/com_ezrealty/ezrealty/" + row["image" + i].ToString());
                        }
                    }
                    _xmlWriter.WriteEndElement();

                    _xmlWriter.WriteEndElement();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Объект с кодом " + row["office_id"].ToString() + "содержит ошибки и не будет выгружен.");
                    continue;
                }
                _xmlWriter.Flush();

            }
            _xmlWriter.WriteEndDocument();
            _xmlWriter.Flush();
            _xmlWriter.Close();
        }

        private void btMakeXmlMesto_Click(object sender, EventArgs e)
        {
            groupBox2.Enabled = false;
            WriteXmlMesto(GetExportTable(cbMestoExportAll.Checked));

            groupBox2.Enabled = true;
            toolStripStatusLabel1.Text = "Файл для Mesto.ua сформирован";
        }

        private void btSendMestoToFtp_Click(object sender, EventArgs e)
        {
            SendFileToFtp(cbMestoExportAll.Checked ? "maximusMestoFull.xml" : "maximusMesto.xml");
        }

        private void SendFileToFtp(string fileName)
        {
            FileInfo fileInf = new FileInfo(fileName);
            string uri = "ftp://ftp.s16.freehost.com.ua/www.maximuszt.com" + "/" + fileInf.Name;
            FtpWebRequest reqFTP;
            // Создаем объект FtpWebRequest
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
            // Учетная запись
            reqFTP.Credentials = new NetworkCredential("maximuszt_pocc", "NUeJYF5RH1");
            reqFTP.KeepAlive = false;
            // Задаем команду на закачку
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            // Тип передачи файла
            reqFTP.UseBinary = true;
            // Сообщаем серверу о размере файла
            reqFTP.ContentLength = fileInf.Length;
            // Буффер в 2 кбайт
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
            // Файловый поток
            FileStream fs = fileInf.OpenRead();
            try
            {
                Stream strm = reqFTP.GetRequestStream();
                // Читаем из потока по 2 кбайт
                contentLen = fs.Read(buff, 0, buffLength);
                // Пока файл не кончится
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                // Закрываем потоки
                strm.Close();
                fs.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Ошибка");

            }
            toolStripStatusLabel1.Text = "Файл успешно выложен на сервер.";

        }
        
        private void btLunMakeFile_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = groupBox2.Enabled = gbLun.Enabled = false;

            WriteXmlLun(GetExportTable(cbLunSendAll.Checked));
            toolStripStatusLabel1.Text = "Файл для LUN.ua сформирован";
            groupBox1.Enabled = groupBox2.Enabled = gbLun.Enabled = true;
        }
        private void WriteXmlLun(DataTable dataTable)
        {
            /*type 3 - аренда
                   1 - продажа
                   */
            XmlWriter _xmlWriter = XmlWriter.Create(cbLunSendAll.Checked ? "maximusLUNFull.xml" : "maximusLUN.xml");
            _xmlWriter.WriteStartDocument();
            _xmlWriter.WriteStartElement("page");
            _xmlWriter.WriteElementString("generation_time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            _xmlWriter.WriteStartElement("announcements");

            foreach (DataRow row in dataTable.Rows)
            {
                try
                {
                    string realty_type = string.Empty;
                    string obj_type = string.Empty;
                    switch (int.Parse(row["cid"].ToString()))
                    {
                        case 1:
                            obj_type = realty_type = "квартира";
                            break;
                        case 2:
                            obj_type = realty_type = "дом";
                            break;
                        case 3:
                            obj_type = realty_type = "часть дома";
                            break;
                        case 4:
                            obj_type = realty_type = "дача";
                            break;
                        case 6:
                            obj_type = realty_type = "участок под жилую застройку";
                            break;
                        case 11:
                            obj_type = realty_type = "комната";
                            break;
                        case 13:
                            realty_type = "офисное помещение";
                            obj_type = "торгово-офисный центр";
                            break;
                        case 14:
                            realty_type = "торговые площади";
                            obj_type = "здание / комплекс / павильон";
                            break;
                        case 15:
                            realty_type = "производственные помещения";
                            obj_type = "здание / комплекс";
                            break;
                        case 16:
                            realty_type = "складские помещения";
                            obj_type = "здание / комплекс";
                            break;
                        case 17:
                            obj_type = realty_type = "кафе, бар, ресторан";
                            break;
                        case 18:
                            obj_type = realty_type = "земля коммерческого назначения";
                            break;
                        default: break;
                    }
                    if (realty_type == string.Empty)
                        continue;

                    _xmlWriter.WriteStartElement("announcement");
                    _xmlWriter.WriteElementString("agency_code", row["office_id"].ToString());
                    _xmlWriter.WriteElementString("contract_type", int.Parse(row["type"].ToString()) == 1 ? "продажа" : "долгосрочная аренда");


                    var worker = Workers.Find(w => w.Id == int.Parse(row["owner"].ToString()));
                    /*string ownerEmail = "mrc.maximus@mail.ru";
                    List<string> ownerPhones = new List<string>();
                    string ownerName;

                    switch (int.Parse(row["owner"].ToString()))
                    {
                        case 258:
                            ownerEmail = "aptsale@ukr.net";
                            ownerPhones = new List<string> { "+380673776037", "+380734668059", "+380970178868" };
                            ownerName = "Кособуцкий Андрей";
                            break;
                        case 117:
                            ownerEmail = "pemaximus@ukr.net";
                            ownerPhones = new List<string> { "+380976913509" };
                            ownerName = "Пясковская Евгения  Станиславовна";
                            break;
                        case 106:
                            ownerEmail = "firsan08@i.ua";
                            ownerPhones = new List<string> { "+380672173007", "+380933333299" };
                            ownerName = "Фирстов Андрей Георгиевич";
                            break;
                        case 99:
                            ownerEmail = "nick_1981@mail.ru";
                            ownerPhones = new List<string> { "+380974488743" };
                            ownerName = "Порядин Николай  Степанович";
                            break;
                        case 85:
                            ownerEmail = "forestin@yandex.ru";
                            ownerPhones = new List<string> { "+380673601930" };
                            ownerName = "Алушин Иван Викторович";
                            break;
                        case 82:
                            ownerEmail = "pavlichenko@maximuszt.com";
                            ownerPhones = new List<string> { "+380680813697" };
                            ownerName = "Павличенко Игорь Николаевич";
                            break;
                        case 80:
                            ownerEmail = "kosyha@maximuszt.com";
                            ownerPhones = new List<string> { "+380973546305" };
                            ownerName = "Косуха Светлана Леонидовна";
                            break;
                        case 77:
                            ownerEmail = "minaka@maximuszt.com";
                            ownerPhones = new List<string> { "+380934501325", "+380975705610" };
                            ownerName = "Минака Тамара Владимировна";
                            break;
                        case 70:
                            ownerEmail = "v_maximus@ukr.net";
                            ownerPhones = new List<string> { "+380674126593" };
                            ownerName = "Полищук Виктор Сергеевич";
                            break;
                        case 96:
                            ownerEmail = "agent.zt@ukr.net";
                            ownerPhones = new List<string> { "+380939909829", "+380953229552", "+380969860005" };
                            ownerName = "Витюк Андрей Петрович";
                            break;
                        case 112:
                            ownerEmail = "kvartiruzt@ukr.net";
                            ownerPhones = new List<string> { "+380970178868" };
                            ownerName = "Андрей";
                            break;
                        case 83:
                            ownerEmail = "kovalchukzt@ukr.net";
                            ownerPhones = new List<string> { "+380932052680", "+380977423995" };
                            ownerName = "Ольга";
                            break;
                        default:
                            ownerEmail = "mrc.maximus@mail.ru";
                            ownerPhones = new List<string> { "+380988914923" };
                            ownerName = "Полищук Светлана Юрьевна";
                            break;
                    }*/

                    if (realty_type != string.Empty)
                        _xmlWriter.WriteElementString("realty_type", realty_type);
                    _xmlWriter.WriteElementString("region", "Житомирская область");
                    if (!row["stid"].Equals(4))
                        _xmlWriter.WriteElementString("rajon", row["locality"].ToString());
                    if (row["stid"].Equals(4))
                        _xmlWriter.WriteElementString("city", row["locality"].ToString());
                    else
                        _xmlWriter.WriteElementString("city", "Житомир");

                    if (!string.IsNullOrEmpty(row["address2"].ToString()))
                        _xmlWriter.WriteElementString("street", row["address2"].ToString());

                    if (!string.IsNullOrEmpty(row["bedrooms"].ToString()))
                        _xmlWriter.WriteElementString("room_count", row["bedrooms"].ToString());

                    if (!string.IsNullOrEmpty(row["custom1"].ToString()))
                        _xmlWriter.WriteElementString("floor", row["custom1"].ToString());

                    if (!string.IsNullOrEmpty(row["custom2"].ToString()))
                        _xmlWriter.WriteElementString("floor_count", row["custom2"].ToString());

                    if (!string.IsNullOrEmpty(row["squarefeet"].ToString()))
                        _xmlWriter.WriteElementString("total_area", row["squarefeet"].ToString());


                    _xmlWriter.WriteElementString("currency", "$");

                    if (!string.IsNullOrEmpty(row["price"].ToString()))
                        _xmlWriter.WriteElementString("price", row["price"].ToString());

                    _xmlWriter.WriteElementString("url", "http://maximuszt.com/index.php?option=com_ezrealty&task=detail&id=" + row["id"].ToString());

                    string telNumbers = string.Empty;
                    foreach (string person in worker.Phones)
                        telNumbers = telNumbers + person + ",";
                    _xmlWriter.WriteElementString("contacts", telNumbers + worker.Email);
                    _xmlWriter.WriteElementString("contact_name", worker.Name);

                    try
                    {
                        row["smalldesc"] = ClearString(row["smalldesc"].ToString(), row["office_id"].ToString());
                        row["propdesc"] = ClearString(row["propdesc"].ToString(), row["office_id"].ToString());
                    }
                    catch { MessageBox.Show("Объект с кодом " + row["office_id"].ToString() + " содержит вредоносный код! Попытка очистки не удалась!"); }

                    _xmlWriter.WriteElementString("text", string.Format("{0}   {1}   Код объекта в базе Максимус: {2}", row["smalldesc"].ToString(), row["propdesc"].ToString(), row["office_id"].ToString()));
                    _xmlWriter.WriteElementString("realty_type", obj_type);
                    string wall_type = string.Empty;
                    switch (int.Parse(row["furnished"].ToString()))
                    {
                        case 1:
                            wall_type = "кирпич";
                            break;
                        case 2:
                            wall_type = "панель";
                            break;
                        case 3:
                            wall_type = "брус";
                            break;
                        case 4:
                            wall_type = "ракушечник (ракушняк)";
                            break;
                        case 5:
                            wall_type = "дерево и кирпич";
                            break;
                        case 6:
                            wall_type = "пеноблок";
                            break;
                        case 7:
                            wall_type = "";
                            break;
                        default: break;
                    }

                    if (!string.IsNullOrEmpty(wall_type))
                        _xmlWriter.WriteElementString("house_type", wall_type);

                    bool isImg = false;
                    _xmlWriter.WriteStartElement("images");

                    for (int i = 1; i <= 12; i++)
                    {
                        if (row["image" + i].ToString() != "")
                        {
                            isImg = true;
                            _xmlWriter.WriteElementString("image", "http://maximuszt.com/components/com_ezrealty/ezrealty/" + row["image" + i].ToString());
                        }
                    }
                    _xmlWriter.WriteEndElement();

                    _xmlWriter.WriteEndElement();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Объект с кодом " + row["office_id"].ToString() + "содержит ошибки и не будет выгружен.");
                    continue;
                }
                _xmlWriter.Flush();

            }
            _xmlWriter.WriteEndElement();
            _xmlWriter.WriteEndDocument();
            _xmlWriter.Flush();
            _xmlWriter.Close();

            string corruptObj = string.Join(",", corruptObjects.ToArray());
            MessageBox.Show("Рекомендуется очистить описание объектов от мусора. Перечень кодов объектов: " + corruptObj);


        }

        private void btLunSendFile_Click(object sender, EventArgs e)
        {
            SendFileToFtp(cbLunSendAll.Checked ? "maximusLUNFull.xml" : "maximusLUN.xml");
        }

        private string ClearString(string sDesc, string objId)
        {
            var cleaned = false;
            if (sDesc.Contains("<script>"))
            {
                cleaned = true;
                sDesc = sDesc.Remove(sDesc.IndexOf("<script>"), sDesc.IndexOf("</script>") - sDesc.IndexOf("<script>")+9);
            }

            if (sDesc.Contains("<!--"))
            {
                cleaned = true;
                sDesc = sDesc.Remove(sDesc.IndexOf("<!--"), sDesc.IndexOf("-->") - sDesc.IndexOf("<!--") + 3);
            }
            if (sDesc.Contains(@"&lt;p&gt;"))
            {
                cleaned = true;
                sDesc = sDesc.Replace(@"&lt;p&gt;", "");
                sDesc = sDesc.Replace(@"&lt;/p&gt;", "");
            }
            if (sDesc.Contains(@"&lt;script&gt;"))
            {
                cleaned = true;
                sDesc = sDesc.Remove(sDesc.IndexOf(@"&lt;script&gt;"), sDesc.IndexOf(@"&lt;/script&gt;") - sDesc.IndexOf(@"&lt;script&gt;") + 14);
            }
            if (sDesc.Contains(@"&lt;!--"))
            {
                cleaned = true;
                sDesc = sDesc.Remove(sDesc.IndexOf(@"&lt;!--"), sDesc.IndexOf(@"--&gt;") - sDesc.IndexOf(@"&lt;!--") + 5);
            }
            if (sDesc.Contains(@"br"))
            {
                cleaned = true;
                sDesc = sDesc.Replace(@"<br/>", "");
                sDesc = sDesc.Replace(@"<br />", "");
            }
            if (sDesc.Contains(@"<p>"))
            {
                cleaned = true;
                sDesc = sDesc.Replace(@"<p>", "");
                sDesc = sDesc.Replace(@"</p>", "");
            }

            if (sDesc.Contains("<script>") || sDesc.Contains("<!--") || sDesc.Contains(@"&lt;!--") || sDesc.Contains(@"&lt;script&gt;"))
                ClearString(sDesc, objId);

            if(!corruptObjects.Contains(objId) && cleaned)
                corruptObjects.Add(objId);
            return sDesc;
        }

        private void btMakePliz_Click(object sender, EventArgs e)
        {
            groupBox3.Enabled = false;
            WriteXmlPliz(GetExportTable(cbGetAllPliz.Checked));

            groupBox3.Enabled = true;
            toolStripStatusLabel1.Text = "Файл для dom.pliz.info сформирован";
        }

    }
}
