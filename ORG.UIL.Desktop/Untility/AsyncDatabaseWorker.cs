using ORGEntity;
using ORGRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ORG.UIL.Desktop.Untility
{
    public class AsyncDatabaseWorker
    {
        public static UsersApp User { get; set; }
        public static bool IsRuning = false;
        public static bool Stop = false;
        public static System.Timers.Timer timer = new System.Timers.Timer(3000);

        public void StartProcessing(UsersApp _User)
        {
            User = _User;
            timer.Enabled = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
        }

        static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!IsRuning && User != null && User.Id > 0 && Helper.CheckInternetConnection())
            {
                IsRuning = true;
                new Thread(() =>
                {
                    try
                    {
                        if (!Stop)
                        {
                            // Get tasks from offline
                            var NewOfflineTasks = new TasksLogRepo(true).GetAllByFilter(x => x.OnLineId == 0);
                            if (Application.OpenForms["frm_Main"] != null && NewOfflineTasks != null && NewOfflineTasks.Count > 0)
                                ((frm_Main)Application.OpenForms["frm_Main"]).ShowLoadData = true;

                            foreach (var item in NewOfflineTasks.OrderBy(t=>t.TaskId).ToList())
                            {
                                if (Stop)
                                    break;
                                switch (item.TaskId)
                                {
                                    case 1:
                                        AddToOnLine(item);
                                        break;
                                    case 2:
                                        UpdateOnLine(item);
                                        break;
                                    case 3:
                                        DeleteOnLine(item);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

                        if (!Stop)
                        {
                            // Get tasks from online                        
                            List<int> ids = new TasksLogRepo(true).GetAllByFilter(x => x.OnLineId > 0)?.Select(x => x.OnLineId).ToList();
                            var NewOnlineTasks = new TasksLogRepo(false).GetAllByFilter(x => !ids.Contains(x.Id));
                            if (Application.OpenForms["frm_Main"] != null && NewOnlineTasks != null && NewOnlineTasks.Count > 0)
                                ((frm_Main)Application.OpenForms["frm_Main"]).ShowLoadData = true;

                            foreach (var item in NewOnlineTasks.OrderBy(t => t.TaskId).ToList())
                            {
                                if (Stop)
                                    break;
                                switch (item.TaskId)
                                {
                                    case 1:
                                        AddToOffLine(item);
                                        break;
                                    case 2:
                                        UpdateOffLine(item);
                                        break;
                                    case 3:
                                        DeleteOffLine(item);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    if (Application.OpenForms["frm_Main"] != null)
                        ((frm_Main)Application.OpenForms["frm_Main"]).ShowLoadData = false;
                    ShowData("", "", "");
                    Stop = false;
                    IsRuning = false;
                }).Start();
            }
        }

        public static bool AddToOffLine(TasksLog ob)
        {
            switch (ob.ReferenceType)
            {
                case "unit":
                    AddOffUnitTask(ob);
                    break;
                case "category":
                    AddOffCategoryTask(ob);
                    break;
                case "item":
                    AddOffItemTask(ob);
                    break;
                case "user":
                    AddOffUserTask(ob);
                    break;
                case "invoice":
                    AddOffInvoiceTask(ob);
                    break;
                default:
                    break;
            }
            return true;
        }

        public static bool AddToOnLine(TasksLog ob)
        {
            switch (ob.ReferenceType)
            {
                case "unit":
                    AddOnUnitTask(ob);
                    break;
                case "category":
                    AddOnCategoryTask(ob);
                    break;
                case "item":
                    AddOnItemTask(ob);
                    break;
                case "user":
                    AddOnUserTask(ob);
                    break;
                case "invoice":
                    AddOnInvoiceTask(ob);
                    break;
                default:
                    break;
            }
            return true;
        }

        public static bool UpdateOffLine(TasksLog ob)
        {
            switch (ob.ReferenceType)
            {
                case "unit":
                    UpdateOffUnitTask(ob);
                    break;
                case "category":
                    UpdateOffCategoryTask(ob);
                    break;
                case "item":
                    UpdateOffItemTask(ob);
                    break;
                case "user":
                    UpdateOffUserTask(ob);
                    break;
                case "invoice":
                    UpdateOffInvoiceTask(ob);
                    break;
                default:
                    break;
            }
            return true;
        }

        public static bool UpdateOnLine(TasksLog ob)
        {
            switch (ob.ReferenceType)
            {
                case "unit":
                    UpdateOnUnitTask(ob);
                    break;
                case "category":
                    UpdateOnCategoryTask(ob);
                    break;
                case "item":
                    UpdateOnItemTask(ob);
                    break;
                case "user":
                    UpdateOnUserTask(ob);
                    break;
                case "invoice":
                    UpdateOnInvoiceTask(ob);
                    break;
                default:
                    break;
            }
            return true;
        }

        public static bool DeleteOffLine(TasksLog ob)
        {
            switch (ob.ReferenceType)
            {
                case "unit":
                    DeleteOffUnitTask(ob);
                    break;
                case "category":
                    DeleteOffCategoryTask(ob);
                    break;
                case "item":
                    DeleteOffItemTask(ob);
                    break;
                case "user":
                    DeleteOffUserTask(ob);
                    break;
                case "invoice":
                    DeleteOffInvoiceTask(ob);
                    break;
                default:
                    break;
            }
            return true;
        }

        public static bool DeleteOnLine(TasksLog ob)
        {
            switch (ob.ReferenceType)
            {
                case "unit":
                    DeleteOnUnitTask(ob);
                    break;
                case "category":
                    DeleteOnCategoryTask(ob);
                    break;
                case "item":
                    DeleteOnItemTask(ob);
                    break;
                case "user":
                    DeleteOnUserTask(ob);
                    break;
                case "invoice":
                    DeleteOnInvoiceTask(ob);
                    break;
                default:
                    break;
            }
            return true;
        }

        #region Units
        public static string AddOffUnitTask(TasksLog ob)
        {
            try
            {
                int id = int.Parse("0" + ob.ReferenceId);
                Unit un = new Unit();
                var unit = new UnitRepo(false).getById(id);

                if (unit != null && unit.Id > 0 && !new UnitRepo(true).Any(e => e.Name == unit.Name))
                {
                    ShowData("Add unit", unit.Name, "Receive");
                    un = new UnitRepo(true).Save(
                        new Unit
                        {
                            Name = unit.Name,
                            LastStatus = unit.LastStatus
                        }, true);

                    new TasksLogRepo(true).Save(new TasksLog
                    {
                        KeyCode = ob.KeyCode,
                        OnLineId = ob.Id,
                        ReferenceType = "unit",
                        TaskId = 1,
                        ReferenceId = "" + un?.Id,
                        Sent = true
                    }, true);
                }
                else
                {
                    ShowData("Add unit", unit.Name, "Receive");
                    un = new UnitRepo(true).GetByFilter(e => e.Name == unit.Name);
                    var taskOnLine = new TasksLogRepo(true).GetByFilter(e => e.ReferenceType == "unit" && e.ReferenceId == "" + un.Id && e.ReferenceType == "1");
                    if (taskOnLine == null)
                        taskOnLine = new TasksLog();
                    taskOnLine.ReferenceType = "unit";
                    taskOnLine.TaskId = 1;
                    taskOnLine.ReferenceId = "" + un?.Id;
                    taskOnLine.KeyCode = ob.KeyCode;
                    taskOnLine.OnLineId = ob.Id;
                    taskOnLine.Sent = true;
                    new TasksLogRepo(true).Save(taskOnLine, true);
                }
                return "Add new unit id: " + un?.Id + " , name: " + un?.Name;
            }
            catch (Exception ex)
            {
                return "Error : can't add unit reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }

        public static string AddOnUnitTask(TasksLog ob)
        {
            try
            {
                int id = int.Parse("0" + ob.ReferenceId);
                var KeyCode = new TasksLogRepo(false).GetMax(e => e.KeyCode);
                KeyCode++;

                Unit un = new Unit();
                var unit = new UnitRepo(true).getById(id);
                if (unit != null && unit.Id > 0 && !new UnitRepo(false).Any(e => e.Name == unit.Name))
                {
                    ShowData("Add unit", unit.Name, "Sends");
                    un = new UnitRepo(false).Save(
                        new Unit
                        {
                            Name = unit.Name,
                            LastStatus = unit.LastStatus
                        }, true);

                    var taskOnLine = new TasksLogRepo(false).Save(new TasksLog
                    {
                        KeyCode = KeyCode,
                        ReferenceType = "unit",
                        TaskId = 1,
                        ReferenceId = "" + un?.Id
                    }, true);

                    ob.KeyCode = taskOnLine.KeyCode;
                    ob.OnLineId = taskOnLine.Id;
                    ob.Sent = true;
                    new TasksLogRepo(true).Save(ob, true);
                }
                else
                {
                    un = new UnitRepo(false).GetByFilter(e => e.Name == unit.Name);
                    var taskOnLine = new TasksLogRepo(false).GetByFilter(e => e.ReferenceType == "unit" && e.ReferenceId == "" + un.Id);
                    ob.KeyCode = taskOnLine.KeyCode;
                    ob.OnLineId = taskOnLine.Id;
                    ob.Sent = true;
                    new TasksLogRepo(true).Save(ob, true);
                }

                return "Add sent new unit id: " + un?.Id + " , name: " + un?.Name;
            }
            catch (Exception ex)
            {
                return "Error : can't send new unit reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }

        public static string UpdateOffUnitTask(TasksLog ob)
        {
            try
            {
                var unit = new UnitRepo(false).getById(int.Parse("0" + ob.ReferenceId));
                Unit un = new UnitRepo(true).GetByKeyCode(ob.KeyCode);
                if (unit != null && unit.Id > 0)
                {
                    ShowData("Update unit", unit.Name, "Receive");
                    un.Name = unit.Name;
                    un.LastStatus = unit.LastStatus;
                    new UnitRepo(true).Save(un, true);
                }

                new TasksLogRepo(true).Save(new TasksLog
                {
                    KeyCode = ob.KeyCode,
                    OnLineId = ob.Id,
                    ReferenceType = "unit",
                    TaskId = 2,
                    ReferenceId = "" + un?.Id,
                    Sent = true
                }, true);
                return "update unit id: " + un?.Id + " , name: " + un?.Name;
            }
            catch (Exception ex)
            {
                return "Error : can't update unit reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }

        public static string UpdateOnUnitTask(TasksLog ob)
        {
            try
            {
                var unit = new UnitRepo(true).getById(int.Parse("0" + ob.ReferenceId));
                Unit un = new UnitRepo(false).GetByKeyCode(ob.KeyCode);
                if (unit != null && unit.Id > 0)
                {
                    ShowData("Update unit", unit.Name, "Sends");
                    un.Name = unit.Name;
                    un.LastStatus = unit.LastStatus;
                    new UnitRepo(false).Save(un, true);
                }

                var taskOnLine = new TasksLogRepo(false).Save(new TasksLog
                {
                    KeyCode = ob.KeyCode,
                    ReferenceType = "unit",
                    TaskId = 2,
                    ReferenceId = "" + un?.Id
                }, true);

                ob.OnLineId = taskOnLine.Id;
                ob.Sent = true;
                new TasksLogRepo(true).Save(ob, true);
                return "update unit id: " + un?.Id + " , name: " + un?.Name;
            }
            catch (Exception ex)
            {
                return "Error : can't update unit reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }

        public static string DeleteOffUnitTask(TasksLog ob)
        {
            try
            {
                var unit = new UnitRepo(true).GetByKeyCode(ob.KeyCode);
                if (unit != null && unit.Id > 0)
                {
                    ShowData("Delete unit", unit.Name, "Receive");
                    new UnitRepo(true).Remove(unit.Id, true);
                }
                new TasksLogRepo(true).Save(new TasksLog
                {
                    KeyCode = ob.KeyCode,
                    OnLineId = ob.Id,
                    ReferenceType = "unit",
                    TaskId = 3,
                    ReferenceId = "" + unit?.Id,
                    Sent = true
                }, true);

                return "Delete unit id: " + unit?.Id + " , name: " + unit?.Name;
            }
            catch (Exception ex)
            {
                return "Error : can't delete unit reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }

        public static string DeleteOnUnitTask(TasksLog ob)
        {
            try
            {
                var unit = new UnitRepo(false).GetByKeyCode(ob.KeyCode);
                if (unit != null && unit.Id > 0)
                {
                    ShowData("Delete unit", unit.Name, "Sends");
                    new UnitRepo(false).Remove(unit.Id, true);
                }

                var taskOnLine = new TasksLogRepo(false).Save(new TasksLog
                {
                    KeyCode = ob.KeyCode,
                    ReferenceType = "unit",
                    TaskId = 3,
                    ReferenceId = "" + unit?.Id
                }, true);

                ob.OnLineId = taskOnLine.Id;
                ob.Sent = true;
                new TasksLogRepo(true).Save(ob, true);
                return "Delete unit id: " + unit?.Id + " , name: " + unit?.Name;
            }
            catch (Exception ex)
            {
                return "Error : can't delete unit reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }
        #endregion

        #region Categories
        public static string AddOffCategoryTask(TasksLog ob)
        {
            try
            {
                int id = int.Parse("0" + ob.ReferenceId);
                Category ct = new Category();
                var category = new CategoryRepo(false).getById(id);

                if (category != null && category.Id > 0 && !new CategoryRepo(true).Any(e => e.Name == category.Name))
                {
                    ShowData("Add category", category.Name, "Receive");
                    ct = new CategoryRepo(true).Save(
                        new Category
                        {
                            Name = category.Name,
                            LastStatus = category.LastStatus,
                            Code = category.Code,
                            ImageFile = category.ImageFile,
                            ImagePath = category.ImagePath,
                            ParentId = category.ParentId
                        }, true);

                    new TasksLogRepo(true).Save(new TasksLog
                    {
                        KeyCode = ob.KeyCode,
                        OnLineId = ob.Id,
                        ReferenceType = "category",
                        TaskId = 1,
                        ReferenceId = "" + ct?.Id,
                        Sent = true
                    }, true);
                }
                else
                {
                    ShowData("Add category", category.Name, "Receive");
                    ct = new CategoryRepo(true).GetByFilter(e => e.Name == category.Name);
                    var taskOnLine = new TasksLogRepo(true).GetByFilter(e => e.ReferenceType == "category" && e.ReferenceId == "" + ct.Id && e.ReferenceType == "1");
                    if (taskOnLine == null)
                        taskOnLine = new TasksLog();
                    taskOnLine.ReferenceType = "category";
                    taskOnLine.TaskId = 1;
                    taskOnLine.ReferenceId = "" + ct?.Id;
                    taskOnLine.KeyCode = ob.KeyCode;
                    taskOnLine.OnLineId = ob.Id;
                    taskOnLine.Sent = true;
                    new TasksLogRepo(true).Save(taskOnLine, true);
                }

                return "Add new category id: " + ct?.Id + " , name: " + ct?.Name;
            }
            catch (Exception ex)
            {
                return "Error : can't add category reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }

        public static string AddOnCategoryTask(TasksLog ob)
        {
            try
            {
                int id = int.Parse("0" + ob.ReferenceId);
                var KeyCode = new TasksLogRepo(false).GetMax(e => e.KeyCode);
                KeyCode++;

                Category ct = new Category();
                var category = new CategoryRepo(true).getById(id);
                if (category != null && category.Id > 0 && !new CategoryRepo(false).Any(e => e.Name == category.Name))
                {
                    ShowData("Add category", category.Name, "Sends");
                    ct = new CategoryRepo(false).Save(
                        new Category
                        {
                            Name = category.Name,
                            LastStatus = category.LastStatus,
                            Code = category.Code,
                            ImageFile = category.ImageFile,
                            ImagePath = category.ImagePath,
                            ParentId = category.ParentId
                        }, true);

                    var taskOnLine = new TasksLogRepo(false).Save(new TasksLog
                    {
                        KeyCode = KeyCode,
                        ReferenceType = "category",
                        TaskId = 1,
                        ReferenceId = "" + ct?.Id
                    }, true);

                    ob.KeyCode = taskOnLine.KeyCode;
                    ob.OnLineId = taskOnLine.Id;
                    ob.Sent = true;
                    new TasksLogRepo(true).Save(ob, true);
                }
                else
                {
                    ct = new CategoryRepo(false).GetByFilter(e => e.Name == category.Name);
                    var taskOnLine = new TasksLogRepo(false).GetByFilter(e => e.ReferenceType == "category" && e.ReferenceId == "" + ct.Id);
                    ob.KeyCode = taskOnLine.KeyCode;
                    ob.OnLineId = taskOnLine.Id;
                    ob.Sent = true;
                    new TasksLogRepo(true).Save(ob, true);
                }
                return "Add sent new category id: " + ct?.Id + " , name: " + ct?.Name;
            }
            catch (Exception ex)
            {
                return "Error : can't send new category reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }

        public static string UpdateOffCategoryTask(TasksLog ob)
        {
            try
            {
                var category = new CategoryRepo(false).getById(int.Parse("0" + ob.ReferenceId));
                Category ct = new CategoryRepo(true).GetByKeyCode(ob.KeyCode);
                if (category != null && category.Id > 0 && ct != null)
                {
                    ShowData("Update category", category.Name, "Receive");
                    ct.Name = category.Name;
                    ct.LastStatus = category.LastStatus;
                    ct.Code = category.Code;
                    ct.ImageFile = category.ImageFile;
                    ct.ImagePath = category.ImagePath;
                    ct.ParentId = category.ParentId;

                    new CategoryRepo(true).Save(ct, true);
                }

                new TasksLogRepo(true).Save(new TasksLog
                {
                    KeyCode = ob.KeyCode,
                    OnLineId = ob.Id,
                    ReferenceType = "category",
                    TaskId = 2,
                    ReferenceId = "" + ct?.Id,
                    Sent = true
                }, true);
                return "update category id: " + ct?.Id + " , name: " + ct?.Name;
            }
            catch (Exception ex)
            {
                return "Error : can't update category reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }

        public static string UpdateOnCategoryTask(TasksLog ob)
        {
            try
            {
                var category = new CategoryRepo(true).getById(int.Parse("0" + ob.ReferenceId));
                Category ct = new CategoryRepo(false).GetByKeyCode(ob.KeyCode);
                if (category != null && category.Id > 0)
                {
                    ShowData("Update category", category.Name, "Sends");
                    ct.Name = category.Name;
                    ct.LastStatus = category.LastStatus; ct.Code = category.Code;
                    ct.ImageFile = category.ImageFile;
                    ct.ImagePath = category.ImagePath;
                    ct.ParentId = category.ParentId;

                    new CategoryRepo(false).Save(ct, true);
                }

                var taskOnLine = new TasksLogRepo(false).Save(new TasksLog
                {
                    KeyCode = ob.KeyCode,
                    ReferenceType = "category",
                    TaskId = 2,
                    ReferenceId = "" + ct?.Id
                }, true);

                ob.OnLineId = taskOnLine.Id;
                ob.Sent = true;
                new TasksLogRepo(true).Save(ob, true);
                return "update category id: " + ct?.Id + " , name: " + ct?.Name;
            }
            catch (Exception ex)
            {
                return "Error : can't update category reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }

        public static string DeleteOffCategoryTask(TasksLog ob)
        {
            try
            {
                var category = new CategoryRepo(true).GetByKeyCode(ob.KeyCode);
                if (category != null && category.Id > 0)
                {
                    ShowData("Delete category", category.Name, "Receive");
                    new CategoryRepo(true).Remove(category.Id, true);
                }

                new TasksLogRepo(true).Save(new TasksLog
                {
                    KeyCode = ob.KeyCode,
                    OnLineId = ob.Id,
                    ReferenceType = "category",
                    TaskId = 3,
                    ReferenceId = "" + category?.Id,
                    Sent = true
                }, true);

                return "Delete category id: " + category?.Id + " , name: " + category?.Name;
            }
            catch (Exception ex)
            {
                return "Error : can't delete category reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }

        public static string DeleteOnCategoryTask(TasksLog ob)
        {
            try
            {
                var category = new CategoryRepo(false).GetByKeyCode(ob.KeyCode);
                if (category != null && category.Id > 0)
                {
                    ShowData("Delete category", category.Name, "Sends");
                    new CategoryRepo(false).Remove(category.Id, true);
                }

                var taskOnLine = new TasksLogRepo(false).Save(new TasksLog
                {
                    KeyCode = ob.KeyCode,
                    ReferenceType = "category",
                    TaskId = 3,
                    ReferenceId = "" + category?.Id
                }, true);

                ob.OnLineId = taskOnLine.Id;
                ob.Sent = true;
                new TasksLogRepo(true).Save(ob, true);
                return "Delete category id: " + category?.Id + " , name: " + category?.Name;
            }
            catch (Exception ex)
            {
                return "Error : can't delete category reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }
        #endregion

        #region Items
        public static string AddOffItemTask(TasksLog ob)
        {
            try
            {
                int id = int.Parse("0" + ob.ReferenceId);
                Item it = new Item();
                var item = new ItemRepo(false).getById(id);
                if (item != null && item.Id > 0 && new ItemRepo(true).Any(e => e.Name == item.Name) == false)
                {
                    ShowData("Add item", item.Name, "Receive");

                    var cton = new TasksLogRepo(false).GetByFilter(e => e.ReferenceId == "" + item.CategoryId && e.ReferenceType == "category");
                    var uson = new TasksLogRepo(false).GetByFilter(e => e.ReferenceId == "" + item.DefaultUnitId && e.ReferenceType == "unit");
                    var ct = new CategoryRepo(true).GetByKeyCode(cton.KeyCode);
                    var us = new UnitRepo(true).GetByKeyCode(uson.KeyCode);

                    it = new ItemRepo(true).Save(
                        new Item
                        {
                            Name = item.Name,
                            LastStatus = item.LastStatus,
                            Code = item.Code,
                            ImageFile = item.ImageFile,
                            ImagePath = item.ImagePath,
                            Barcode = item.Barcode,
                            CategoryId = ct.Id,
                            DefaultQuantity = item.DefaultQuantity,
                            DefaultUnitId = us.Id,
                            Description = item.Description,
                            IsSerialized = item.IsSerialized,
                            LocationId = item.LocationId,
                            PurchasePrice = item.PurchasePrice,
                            ReorderPoint = item.ReorderPoint,
                            SellingPrice = item.SellingPrice,
                            TaxPercent = item.TaxPercent
                        }, true);

                    new TasksLogRepo(true).Save(new TasksLog
                    {
                        KeyCode = ob.KeyCode,
                        OnLineId = ob.Id,
                        ReferenceType = "item",
                        TaskId = 1,
                        ReferenceId = "" + it?.Id,
                        Sent = true
                    }, true);
                }
                else
                {
                    ShowData("Add item", item.Name, "Receive");
                    it = new ItemRepo(true).GetByFilter(e => e.Name == item.Name);
                    var taskOnLine = new TasksLogRepo(true).GetByFilter(e => e.ReferenceType == "item" && e.ReferenceId == "" + it.Id && e.ReferenceType == "1");
                    if (taskOnLine == null)
                        taskOnLine = new TasksLog();

                    taskOnLine.ReferenceType = "item";
                    taskOnLine.TaskId = 1;
                    taskOnLine.ReferenceId = "" + it?.Id;
                    taskOnLine.KeyCode = ob.KeyCode;
                    taskOnLine.OnLineId = ob.Id;
                    taskOnLine.Sent = true;
                    new TasksLogRepo(true).Save(taskOnLine, true);
                }

                return "Add new item id: " + it?.Id + " , name: " + it?.Name;
            }
            catch (Exception ex)
            {
                return "Error : can't add item reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }

        public static string AddOnItemTask(TasksLog ob)
        {
            try
            {
                int id = int.Parse("0" + ob.ReferenceId);
                var KeyCode = new TasksLogRepo(false).GetMax(e => e.KeyCode);
                KeyCode++;

                Item it = new Item();
                var item = new ItemRepo(true).getById(id);
                if (item != null && item.Id > 0 && new ItemRepo(false).Any(e => e.Name == item.Name) == false)
                {
                    ShowData("Add item", item.Name, "Sends");
                    var cton = new TasksLogRepo(true).GetByFilter(e => e.ReferenceId == "" + item.CategoryId && e.ReferenceType == "category");
                    var uson = new TasksLogRepo(true).GetByFilter(e => e.ReferenceId == "" + item.DefaultUnitId && e.ReferenceType == "unit");
                    var ct = new CategoryRepo(false).GetByKeyCode(cton.KeyCode);
                    var us = new UnitRepo(false).GetByKeyCode(uson.KeyCode);

                    it = new ItemRepo(false).Save(
                        new Item
                        {
                            Name = item.Name,
                            LastStatus = item.LastStatus,
                            Code = item.Code,
                            ImageFile = item.ImageFile,
                            ImagePath = item.ImagePath,
                            Barcode = item.Barcode,
                            CategoryId = ct.Id,
                            DefaultQuantity = item.DefaultQuantity,
                            DefaultUnitId = us.Id,
                            Description = item.Description,
                            IsSerialized = item.IsSerialized,
                            LocationId = item.LocationId,
                            PurchasePrice = item.PurchasePrice,
                            ReorderPoint = item.ReorderPoint,
                            SellingPrice = item.SellingPrice,
                            TaxPercent = item.TaxPercent
                        }, true);

                    var taskOnLine = new TasksLogRepo(false).Save(new TasksLog
                    {
                        KeyCode = KeyCode,
                        ReferenceType = "item",
                        TaskId = 1,
                        ReferenceId = "" + it?.Id
                    }, true);

                    ob.KeyCode = taskOnLine.KeyCode;
                    ob.OnLineId = taskOnLine.Id;
                    ob.Sent = true;
                    new TasksLogRepo(true).Save(ob, true);
                }
                else
                {
                    it = new ItemRepo(false).GetByFilter(e => e.Name == item.Name);
                    var taskOnLine = new TasksLogRepo(false).GetByFilter(e => e.ReferenceType == "item" && e.ReferenceId == "" + it.Id);
                    ob.KeyCode = taskOnLine.KeyCode;
                    ob.OnLineId = taskOnLine.Id;
                    ob.Sent = true;
                    new TasksLogRepo(true).Save(ob, true);
                }

                return "Add sent new item id: " + it?.Id + " , name: " + it?.Name;
            }
            catch (Exception ex)
            {
                return "Error : can't send new item reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }

        public static string UpdateOffItemTask(TasksLog ob)
        {
            try
            {
                var item = new ItemRepo(false).getById(int.Parse("0" + ob.ReferenceId));
                Item it = new ItemRepo(true).GetByKeyCode(ob.KeyCode);
                if (item != null && item.Id > 0)
                {
                    ShowData("Update item", item.Name, "Receive");
                    var cton = new TasksLogRepo(false).GetByFilter(e => e.ReferenceId == "" + item.CategoryId && e.ReferenceType == "category");
                    var uson = new TasksLogRepo(false).GetByFilter(e => e.ReferenceId == "" + item.DefaultUnitId && e.ReferenceType == "unit");
                    var ct = new CategoryRepo(true).GetByKeyCode(cton.KeyCode);
                    var us = new UnitRepo(true).GetByKeyCode(uson.KeyCode);

                    it.Name = item.Name;
                    it.LastStatus = item.LastStatus;
                    it.Code = item.Code;
                    it.ImageFile = item.ImageFile;
                    it.ImagePath = item.ImagePath;
                    it.Barcode = item.Barcode;
                    it.CategoryId = ct.Id;
                    it.DefaultQuantity = item.DefaultQuantity;
                    it.DefaultUnitId = us.Id;
                    it.Description = item.Description;
                    it.IsSerialized = item.IsSerialized;
                    it.LocationId = item.LocationId;
                    it.PurchasePrice = item.PurchasePrice;
                    it.ReorderPoint = item.ReorderPoint;
                    it.SellingPrice = item.SellingPrice;
                    it.TaxPercent = item.TaxPercent;

                    new ItemRepo(true).Save(it, true);
                }

                new TasksLogRepo(true).Save(new TasksLog
                {
                    KeyCode = ob.KeyCode,
                    OnLineId = ob.Id,
                    ReferenceType = "item",
                    TaskId = 2,
                    ReferenceId = "" + it?.Id,
                    Sent = true
                }, true);
                return "update item id: " + it?.Id + " , name: " + it?.Name;
            }
            catch (Exception ex)
            {
                return "Error : can't update item reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }

        public static string UpdateOnItemTask(TasksLog ob)
        {
            try
            {
                var item = new ItemRepo(true).getById(int.Parse("0" + ob.ReferenceId));
                Item it = new ItemRepo(false).GetByKeyCode(ob.KeyCode);
                if (item != null && item.Id > 0)
                {
                    ShowData("Update item", item.Name, "Sends");
                    var cton = new TasksLogRepo(true).GetByFilter(e => e.ReferenceId == "" + item.CategoryId && e.ReferenceType == "category");
                    var uson = new TasksLogRepo(true).GetByFilter(e => e.ReferenceId == "" + item.DefaultUnitId && e.ReferenceType == "unit");
                    var ct = new CategoryRepo(false).GetByKeyCode(cton.KeyCode);
                    var us = new UnitRepo(false).GetByKeyCode(uson.KeyCode);

                    it.Name = item.Name;
                    it.LastStatus = item.LastStatus;
                    it.Code = item.Code;
                    it.ImageFile = item.ImageFile;
                    it.ImagePath = item.ImagePath;
                    it.Barcode = item.Barcode;
                    it.CategoryId = ct.Id;
                    it.DefaultQuantity = item.DefaultQuantity;
                    it.DefaultUnitId = us.Id;
                    it.Description = item.Description;
                    it.IsSerialized = item.IsSerialized;
                    it.LocationId = item.LocationId;
                    it.PurchasePrice = item.PurchasePrice;
                    it.ReorderPoint = item.ReorderPoint;
                    it.SellingPrice = item.SellingPrice;
                    it.TaxPercent = item.TaxPercent;

                    new ItemRepo(false).Save(it, true);
                }

                var taskOnLine = new TasksLogRepo(false).Save(new TasksLog
                {
                    KeyCode = ob.KeyCode,
                    ReferenceType = "item",
                    TaskId = 2,
                    ReferenceId = "" + it?.Id
                }, true);

                ob.OnLineId = taskOnLine.Id;
                ob.Sent = true;
                new TasksLogRepo(true).Save(ob, true);
                return "update item id: " + it?.Id + " , name: " + it?.Name;
            }
            catch (Exception ex)
            {
                return "Error : can't update item reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }

        public static string DeleteOffItemTask(TasksLog ob)
        {
            try
            {
                var item = new ItemRepo(true).GetByKeyCode(ob.KeyCode);
                if (item != null && item.Id > 0)
                {
                    ShowData("Delete item", item.Name, "Receive");
                    new ItemRepo(true).Remove(item.Id, true);
                }

                new TasksLogRepo(true).Save(new TasksLog
                {
                    KeyCode = ob.KeyCode,
                    OnLineId = ob.Id,
                    ReferenceType = "item",
                    TaskId = 3,
                    ReferenceId = "" + item?.Id,
                    Sent = true
                }, true);

                return "Delete item id: " + item?.Id + " , name: " + item?.Name;
            }
            catch (Exception ex)
            {
                return "Error : can't delete item reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }

        public static string DeleteOnItemTask(TasksLog ob)
        {
            try
            {
                var item = new ItemRepo(false).GetByKeyCode(ob.KeyCode);
                if (item != null && item.Id > 0)
                {
                    ShowData("Delete item", item.Name, "Sends");
                    new ItemRepo(false).Remove(item.Id, true);
                }

                var taskOnLine = new TasksLogRepo(false).Save(new TasksLog
                {
                    KeyCode = ob.KeyCode,
                    ReferenceType = "item",
                    TaskId = 3,
                    ReferenceId = "" + item?.Id
                }, true);

                ob.OnLineId = taskOnLine.Id;
                ob.Sent = true;
                new TasksLogRepo(true).Save(ob, true);
                return "Delete item id: " + item?.Id + " , name: " + item?.Name;
            }
            catch (Exception ex)
            {
                return "Error : can't delete item reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }
        #endregion

        #region UsersApp
        public static string AddOffUserTask(TasksLog ob)
        {
            try
            {
                int id = int.Parse("0" + ob.ReferenceId);
                UsersApp us = new UsersApp();
                var usersApp = new UserRepo(false).getById(id);
                if (usersApp != null && usersApp.Id > 0 && !new UserRepo(true).Any(e => e.Email == usersApp.Email))
                {
                    ShowData("Add user", usersApp.UserName, "Receive");
                    us = new UserRepo(true).Save(
                        new UsersApp
                        {
                            CompanyName = usersApp.CompanyName,
                            ButtonSound = usersApp.ButtonSound,
                            Address = usersApp.Address,
                            DbVersion = usersApp.DbVersion,
                            ElectronicScaleCode = usersApp.ElectronicScaleCode,
                            Email = usersApp.Email,
                            MusicSound = usersApp.MusicSound,
                            Password = usersApp.Password,
                            PricingId = usersApp.PricingId,
                            PrintLang = usersApp.PrintLang,
                            RoleId = usersApp.RoleId,
                            SchemaName = usersApp.SchemaName,
                            Service = usersApp.Service,
                            SysVersion = usersApp.SysVersion,
                            Tax = usersApp.Tax,
                            Telephone = usersApp.Telephone,
                            TypeSystem = usersApp.TypeSystem,
                            UserName = usersApp.UserName,
                            Volume = usersApp.Volume
                        }, true);

                    new TasksLogRepo(true).Save(new TasksLog
                    {
                        KeyCode = ob.KeyCode,
                        OnLineId = ob.Id,
                        ReferenceType = "user",
                        TaskId = 1,
                        ReferenceId = "" + us?.Id,
                        Sent = true
                    }, true);
                }
                else
                {
                    ShowData("Add user", usersApp.UserName, "Receive");
                    us = new UserRepo(true).GetByFilter(e => e.Email == usersApp.Email);
                    var taskOnLine = new TasksLogRepo(true).GetByFilter(e => e.ReferenceType == "user" && e.ReferenceId == "" + us.Id && e.ReferenceType == "1");
                    if (taskOnLine == null)
                        taskOnLine = new TasksLog();
                    taskOnLine.ReferenceType = "user";
                    taskOnLine.TaskId = 1;
                    taskOnLine.ReferenceId = "" + us?.Id;
                    taskOnLine.KeyCode = ob.KeyCode;
                    taskOnLine.OnLineId = ob.Id;
                    taskOnLine.Sent = true;
                    new TasksLogRepo(true).Save(taskOnLine, true);
                }
                return "Add new user id: " + us?.Id + " , email: " + us?.Email;
            }
            catch (Exception ex)
            {
                return "Error : can't add user reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }

        public static string AddOnUserTask(TasksLog ob)
        {
            try
            {
                int id = int.Parse("0" + ob.ReferenceId);



                int? KeyCode = new TasksLogRepo(false).GetMax(e => e.KeyCode);
                KeyCode++;

                UsersApp us = new UsersApp();
                var usersApp = new UserRepo(true).getById(id);
                if (usersApp != null && usersApp.Id > 0 && !new UserRepo(false).Any(e => e.Email == usersApp.Email && e.SchemaName == usersApp.SchemaName))
                {
                    if (new UserRepo(false).Any(e => e.Email == usersApp.Email))
                    {
                        int x = 0;
                        while (true)
                        {
                            x++;
                            var em = usersApp.Email.Split('@').ToArray();
                            string email = em[0] + x + "@" + em[1];

                            us = new UserRepo(false).GetByFilter(e => e.Email == email);
                            if (us == null || us.Id == 0)
                            {
                                usersApp.Email = email;
                                new UserRepo(true).Save(usersApp  , true);
                                break;
                            }                          
                        }
                    }

                    ShowData("Add user", usersApp.UserName, "Sends");
                    us = new UserRepo(false).Save(
                        new UsersApp
                        {
                            CompanyName = usersApp.CompanyName,
                            ButtonSound = usersApp.ButtonSound,
                            Address = usersApp.Address,
                            DbVersion = usersApp.DbVersion,
                            ElectronicScaleCode = usersApp.ElectronicScaleCode,
                            Email = usersApp.Email,
                            MusicSound = usersApp.MusicSound,
                            Password = usersApp.Password,
                            PricingId = usersApp.PricingId,
                            PrintLang = usersApp.PrintLang,
                            RoleId = usersApp.RoleId,
                            SchemaName = usersApp.SchemaName,
                            Service = usersApp.Service,
                            SysVersion = usersApp.SysVersion,
                            Tax = usersApp.Tax,
                            Telephone = usersApp.Telephone,
                            TypeSystem = usersApp.TypeSystem,
                            UserName = usersApp.UserName,
                            Volume = usersApp.Volume
                        }, true);

                    var taskOnLine = new TasksLogRepo(false).Save(new TasksLog
                    {
                        KeyCode = KeyCode ?? 0,
                        ReferenceType = "user",
                        TaskId = 1,
                        ReferenceId = "" + us?.Id
                    }, true);

                    ob.KeyCode = taskOnLine.KeyCode;
                    ob.OnLineId = taskOnLine.Id;
                    ob.Sent = true;
                    new TasksLogRepo(true).Save(ob, true);
                }
                else
                {
                    us = new UserRepo(false).GetByFilter(e => e.Email == usersApp.Email && e.SchemaName == usersApp.SchemaName);
                    var taskOnLine = new TasksLogRepo(false).GetByFilter(e => e.ReferenceType == "user" && e.ReferenceId == "" + us.Id);
                    ob.KeyCode = taskOnLine.KeyCode;
                    ob.OnLineId = taskOnLine.Id;
                    ob.Sent = true;
                    new TasksLogRepo(true).Save(ob, true);
                }

                return "Add sent new user id: " + us?.Id + " , email: " + us?.Email;
            }
            catch (Exception ex)
            {
                return "Error : can't send new user reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }

        public static string UpdateOffUserTask(TasksLog ob)
        {
            try
            {
                var usersApp = new UserRepo(false).getById(int.Parse("0" + ob.ReferenceId));
                UsersApp us = new UserRepo(true).GetByKeyCode(ob.KeyCode);
                if (usersApp != null && usersApp.Id > 0)
                {
                    ShowData("Update user", usersApp.UserName, "Receive");
                    us.CompanyName = usersApp.CompanyName;
                    us.ButtonSound = usersApp.ButtonSound;
                    us.Address = usersApp.Address;
                    us.DbVersion = usersApp.DbVersion;
                    us.ElectronicScaleCode = usersApp.ElectronicScaleCode;
                    us.Email = usersApp.Email;
                    us.MusicSound = usersApp.MusicSound;
                    us.Password = usersApp.Password;
                    us.PricingId = usersApp.PricingId;
                    us.PrintLang = usersApp.PrintLang;
                    us.RoleId = usersApp.RoleId;
                    us.SchemaName = usersApp.SchemaName;
                    us.Service = usersApp.Service;
                    us.SysVersion = usersApp.SysVersion;
                    us.Tax = usersApp.Tax;
                    us.Telephone = usersApp.Telephone;
                    us.TypeSystem = usersApp.TypeSystem;
                    us.UserName = usersApp.UserName;
                    us.Volume = usersApp.Volume;

                    new UserRepo(true).Save(us, true);
                }

                if (usersApp.Id == GeneralMembers.User.Id)
                    GeneralMembers.User = GeneralMembersRepo.User = us;

                new TasksLogRepo(true).Save(new TasksLog
                {
                    KeyCode = ob.KeyCode,
                    OnLineId = ob.Id,
                    ReferenceType = "user",
                    TaskId = 2,
                    ReferenceId = "" + us?.Id,
                    Sent = true
                }, true);
                return "update user id: " + us?.Id + " , email: " + us?.Email;
            }
            catch (Exception ex)
            {
                return "Error : can't update user reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }

        public static string UpdateOnUserTask(TasksLog ob)
        {
            try
            {
                var usersApp = new UserRepo(true).getById(int.Parse("0" + ob.ReferenceId));
                UsersApp us = new UserRepo(false).GetByKeyCode(ob.KeyCode, false);
                if (usersApp != null && usersApp.Id > 0)
                {
                    ShowData("Update user", usersApp.UserName, "Sends");
                    us.CompanyName = usersApp.CompanyName;
                    us.ButtonSound = usersApp.ButtonSound;
                    us.Address = usersApp.Address;
                    us.DbVersion = usersApp.DbVersion;
                    us.ElectronicScaleCode = usersApp.ElectronicScaleCode;
                    us.Email = usersApp.Email;
                    us.MusicSound = usersApp.MusicSound;
                    us.Password = usersApp.Password;
                    us.PricingId = usersApp.PricingId;
                    us.PrintLang = usersApp.PrintLang;
                    us.RoleId = usersApp.RoleId;
                    us.SchemaName = usersApp.SchemaName;
                    us.Service = usersApp.Service;
                    us.SysVersion = usersApp.SysVersion;
                    us.Tax = usersApp.Tax;
                    us.Telephone = usersApp.Telephone;
                    us.TypeSystem = usersApp.TypeSystem;
                    us.UserName = usersApp.UserName;
                    us.Volume = usersApp.Volume;

                    new UserRepo(false).Save(us, true);
                }

                var taskOnLine = new TasksLogRepo(false).Save(new TasksLog
                {
                    KeyCode = ob.KeyCode,
                    ReferenceType = "user",
                    TaskId = 2,
                    ReferenceId = "" + us?.Id
                }, true);

                ob.OnLineId = taskOnLine.Id;
                ob.Sent = true;
                new TasksLogRepo(true).Save(ob, true);
                return "update user id: " + us?.Id + " , Email: " + us?.Email;
            }
            catch (Exception ex)
            {
                return "Error : can't update user reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }

        public static string DeleteOffUserTask(TasksLog ob)
        {
            try
            {
                var user = new UserRepo(true).GetByKeyCode(ob.KeyCode, true);
                if (user != null && user.Id > 0)
                {
                    ShowData("Delete user", user.UserName, "Receive");
                    new UserRepo(true).Remove(user.Id, true);
                }


                new TasksLogRepo(true).Save(new TasksLog
                {
                    KeyCode = ob.KeyCode,
                    OnLineId = ob.Id,
                    ReferenceType = "user",
                    TaskId = 3,
                    ReferenceId = "" + user?.Id,
                    Sent = true
                }, true);
                if (user.Id == GeneralMembers.User.Id)
                    ((frm_Main)Application.OpenForms["frm_Main"]).LogOff(null, null);
                return "Delete user id: " + user?.Id + " , email: " + user?.Email;
            }
            catch (Exception ex)
            {
                return "Error : can't delete user reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }

        public static string DeleteOnUserTask(TasksLog ob)
        {
            try
            {
                var user = new UserRepo(false).GetByKeyCode(ob.KeyCode, false);
                if (user != null && user.Id > 0)
                {
                    ShowData("Delete user", user.UserName, "Sends");
                    new UserRepo(false).Remove(user.Id, true);
                }


                var taskOnLine = new TasksLogRepo(false).Save(new TasksLog
                {
                    KeyCode = ob.KeyCode,
                    ReferenceType = "user",
                    TaskId = 3,
                    ReferenceId = "" + user?.Id
                }, true);

                ob.OnLineId = taskOnLine.Id;
                ob.Sent = true;
                new TasksLogRepo(true).Save(ob, true);
                return "Delete user id: " + user?.Id + " , email: " + user?.Email;
            }
            catch (Exception ex)
            {
                return "Error : can't delete user reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }
        #endregion

        #region Invoices
        public static string AddOffInvoiceTask(TasksLog ob)
        {
            try
            {
                int id = int.Parse("0" + ob.ReferenceId);
                Invoice inv = new Invoice();
                var invoice = new InvoiceRepo(false).getById(id);
                if (invoice != null && invoice.Id > 0 && !new InvoiceRepo(true).Any(e => e.Code == invoice.Code && e.InvoiceDate == invoice.InvoiceDate))
                {
                    string title = "Add invoice";

                    ShowData(title, invoice.Code + " / " + string.Format("{0:yyyy-MM-dd}", invoice.InvoiceDate), "Receive");
                    var uton = new TasksLogRepo(false).GetByFilter(e => e.ReferenceId == "" + invoice.UserId && e.ReferenceType == "user");
                    var us = new UserRepo(true).GetByKeyCode(uton.KeyCode);

                    var newInv = new Invoice
                    {
                        Code = invoice.Code,
                        Comment = invoice.Comment,
                        CreateDate = invoice.CreateDate,
                        DealerId = invoice.DealerId,
                        Discount = invoice.Discount,
                        GrandTotal = invoice.GrandTotal,
                        InvoiceBaseId = invoice.InvoiceBaseId,
                        InvoiceDate = invoice.InvoiceDate,
                        NetTotal = invoice.NetTotal,
                        Printed = invoice.Printed,
                        Service = invoice.Service,
                        StoreId = invoice.StoreId,
                        Tax = invoice.Tax,
                        TypeId = invoice.TypeId,
                        UserId = us.Id,
                        LastStatus = invoice.LastStatus,
                    };

                    newInv.InvoiceItem = new List<InvoiceItem>();
                    foreach (var item in invoice.InvoiceItem)
                    {
                        var iton = new TasksLogRepo(false).GetByFilter(e => e.ReferenceId == "" + item.ItemId && e.ReferenceType == "item");
                        var it = new ItemRepo(true).GetByKeyCode(iton.KeyCode);

                        var uon = new TasksLogRepo(false).GetByFilter(e => e.ReferenceId == "" + item.UnitId && e.ReferenceType == "unit");
                        var un = new UnitRepo(true).GetByKeyCode(uon.KeyCode);

                        var itemInv = new InvoiceItem
                        {
                            Code = item.Code,
                            Discount = item.Discount,
                            LastStatus = item.LastStatus,
                            NetTotal = item.NetTotal,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            Serialnumber = item.Serialnumber,
                            Tax = item.Tax,
                            GrandTotal = item.GrandTotal,
                            ItemId = it.Id,
                            UnitId = un.Id,
                            StoreId = item.StoreId
                        };

                        newInv.InvoiceItem.Add(itemInv);
                    }

                    inv = new InvoiceRepo(true).Save(newInv, true);

                    new TasksLogRepo(true).Save(new TasksLog
                    {
                        KeyCode = ob.KeyCode,
                        OnLineId = ob.Id,
                        ReferenceType = "invoice",
                        TaskId = 1,
                        ReferenceId = "" + inv?.Id,
                        Sent = true
                    }, true);
                }
                else
                {
                    ShowData("Add invoice", invoice.Code + " / " + string.Format("{0:yyyy-MM-dd}", invoice.InvoiceDate), "Receive");
                    inv = new InvoiceRepo(true).GetByFilter(e => e.Code == invoice.Code && e.InvoiceDate == invoice.InvoiceDate);
                    inv.InvoiceItem = new List<InvoiceItem>();
                    foreach (var item in invoice.InvoiceItem)
                    {
                        var iton = new TasksLogRepo(false).GetByFilter(e => e.ReferenceId == "" + item.ItemId && e.ReferenceType == "item");
                        var it = new ItemRepo(true).GetByKeyCode(iton.KeyCode);

                        var uon = new TasksLogRepo(false).GetByFilter(e => e.ReferenceId == "" + item.UnitId && e.ReferenceType == "unit");
                        var un = new UnitRepo(true).GetByKeyCode(uon.KeyCode);

                        var itemInv = new InvoiceItem
                        {
                            Code = item.Code,
                            Discount = item.Discount,
                            LastStatus = item.LastStatus,
                            NetTotal = item.NetTotal,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            Serialnumber = item.Serialnumber,
                            Tax = item.Tax,
                            GrandTotal = item.GrandTotal,
                            ItemId = it.Id,
                            UnitId = un.Id,
                            StoreId = item.StoreId
                        };

                        inv.InvoiceItem.Add(itemInv);
                    }
                    inv = new InvoiceRepo(true).Save(inv, true);

                    var taskOnLine = new TasksLogRepo(true).GetByFilter(e => e.ReferenceType == "item" && e.ReferenceId == "" + inv.Id && e.ReferenceType == "1");
                    if (taskOnLine == null)
                        taskOnLine = new TasksLog();
                    taskOnLine.ReferenceType = "invoice";
                    taskOnLine.TaskId = 1;
                    taskOnLine.ReferenceId = "" + inv?.Id;
                    taskOnLine.KeyCode = ob.KeyCode;
                    taskOnLine.OnLineId = ob.Id;
                    taskOnLine.Sent = true;
                    new TasksLogRepo(true).Save(taskOnLine, true);
                }
                return "Add new invoice id: " + inv?.Id + " , code: " + inv?.Code;
            }
            catch (Exception ex)
            {
                return "Error : can't add invoice reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }

        public static string AddOnInvoiceTask(TasksLog ob)
        {
            try
            {
                int id = int.Parse("0" + ob.ReferenceId);
                int KeyCode = new TasksLogRepo(false).GetMax(e => e.KeyCode);
                KeyCode++;

                Invoice inv = new Invoice();
                var invoice = new InvoiceRepo(true).getById(id);
                if (invoice != null && invoice.Id > 0)
                {
                    var inv_ = new InvoiceRepo(false).GetByFilter(e => e.Code == invoice.Code && e.InvoiceDate == invoice.InvoiceDate);
                    if (inv_ != null && inv_.Id > 0)
                    {
                        var maxCode = new InvoiceRepo(false).GetMaxCode(inv_.TypeId, inv_.UserId);
                        invoice.Code = maxCode;
                        new InvoiceRepo(true).Save(invoice, true);
                    }

                    string title = "Add invoice";

                    ShowData(title, invoice.Code + " / " + string.Format("{0:yyyy-MM-dd}", invoice.InvoiceDate), "Sends");
                    var uton = new TasksLogRepo(true).GetByFilter(e => e.ReferenceId == "" + invoice.UserId && e.ReferenceType == "user");
                    var us = new UserRepo(false).GetByKeyCode(uton.KeyCode, false);

                    var newInv = new Invoice
                    {
                        Code = invoice.Code,
                        Comment = invoice.Comment,
                        CreateDate = invoice.CreateDate,
                        DealerId = invoice.DealerId,
                        Discount = invoice.Discount,
                        GrandTotal = invoice.GrandTotal,
                        InvoiceBaseId = invoice.InvoiceBaseId,
                        InvoiceDate = invoice.InvoiceDate,
                        NetTotal = invoice.NetTotal,
                        Printed = invoice.Printed,
                        Service = invoice.Service,
                        StoreId = invoice.StoreId,
                        Tax = invoice.Tax,
                        TypeId = invoice.TypeId,
                        UserId = us.Id,
                        LastStatus = invoice.LastStatus,
                    };

                    newInv.InvoiceItem = new List<InvoiceItem>();
                    foreach (var item in invoice.InvoiceItem)
                    {
                        var iton = new TasksLogRepo(true).GetByFilter(e => e.ReferenceId == "" + item.ItemId && e.ReferenceType == "item");
                        var it = new ItemRepo(false).GetByKeyCode(iton.KeyCode);

                        var un = new Unit();
                        if (item.UnitId == 0)
                        {
                            un = new UnitRepo().getAll().FirstOrDefault();
                        }
                        else
                        {
                            var uon = new TasksLogRepo(true).GetByFilter(e => e.ReferenceId == "" + item.UnitId && e.ReferenceType == "unit");
                            un = new UnitRepo(false).GetByKeyCode(uon.KeyCode);
                        }

                        var itemInv = new InvoiceItem
                        {
                            Code = item.Code,
                            Discount = item.Discount,
                            LastStatus = item.LastStatus,
                            NetTotal = item.NetTotal,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            Serialnumber = item.Serialnumber,
                            Tax = item.Tax,
                            GrandTotal = item.GrandTotal,
                            ItemId = it.Id,
                            UnitId = un.Id,
                            StoreId = item.StoreId
                        };

                        newInv.InvoiceItem.Add(itemInv);
                    }

                    inv = new InvoiceRepo(false).Save(newInv, true);

                    var taskOnLine = new TasksLogRepo(false).Save(new TasksLog
                    {
                        KeyCode = KeyCode,
                        ReferenceType = "invoice",
                        TaskId = 1,
                        ReferenceId = "" + inv?.Id
                    }, true);

                    ob.KeyCode = taskOnLine.KeyCode;
                    ob.OnLineId = taskOnLine.Id;
                    ob.Sent = true;
                    new TasksLogRepo(true).Save(ob, true);
                }               

                return "Add sent new invoice id: " + inv?.Id + " , code: " + inv?.Code;
            }
            catch (Exception ex)
            {
                return "Error : can't send new invoice reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }

        public static string UpdateOffInvoiceTask(TasksLog ob)
        {
            try
            {
                var invoice = new InvoiceRepo(false).getById(int.Parse("0" + ob.ReferenceId));
                Invoice inv = new InvoiceRepo(true).GetByKeyCode(ob.KeyCode);
                if (invoice != null && invoice.Id > 0)
                {
                    string title = "Update invoice";

                    ShowData(title, invoice.Code + " / " + string.Format("{0:yyyy-MM-dd}", invoice.InvoiceDate), "Receive");
                    var uton = new TasksLogRepo(false).GetByFilter(e => e.ReferenceId == "" + invoice.UserId && e.ReferenceType == "user");
                    var us = new UserRepo(true).GetByKeyCode(uton.KeyCode);

                    inv.Code = invoice.Code;
                    inv.Comment = invoice.Comment;
                    inv.CreateDate = invoice.CreateDate;
                    inv.DealerId = invoice.DealerId;
                    inv.Discount = invoice.Discount;
                    inv.GrandTotal = invoice.GrandTotal;
                    inv.InvoiceBaseId = invoice.InvoiceBaseId;
                    inv.InvoiceDate = invoice.InvoiceDate;
                    inv.NetTotal = invoice.NetTotal;
                    inv.Printed = invoice.Printed;
                    inv.Service = invoice.Service;
                    inv.StoreId = invoice.StoreId;
                    inv.Tax = invoice.Tax;
                    inv.TypeId = invoice.TypeId;
                    inv.UserId = us.Id;
                    inv.LastStatus = invoice.LastStatus;
                    inv.InvoiceItem = new List<InvoiceItem>();

                    foreach (var item in invoice.InvoiceItem)
                    {
                        var iton = new TasksLogRepo(false).GetByFilter(e => e.ReferenceId == "" + item.ItemId && e.ReferenceType == "item");
                        var it = new ItemRepo(true).GetByKeyCode(iton.KeyCode);

                        var uon = new TasksLogRepo(false).GetByFilter(e => e.ReferenceId == "" + item.UnitId && e.ReferenceType == "unit");
                        var un = new UnitRepo(true).GetByKeyCode(uon.KeyCode);

                        inv.InvoiceItem.Add(new InvoiceItem
                        {
                            Code = item.Code,
                            Discount = item.Discount,
                            LastStatus = item.LastStatus,
                            NetTotal = item.NetTotal,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            Serialnumber = item.Serialnumber,
                            Tax = item.Tax,
                            GrandTotal = item.GrandTotal,
                            ItemId = it.Id,
                            UnitId = un.Id,
                            StoreId = item.StoreId
                        });
                    }

                    new InvoiceRepo(true).Save(inv, true);
                }

                new TasksLogRepo(true).Save(new TasksLog
                {
                    KeyCode = ob.KeyCode,
                    OnLineId = ob.Id,
                    ReferenceType = "invoice",
                    TaskId = 2,
                    ReferenceId = "" + inv?.Id,
                    Sent = true
                }, true);
                return "update invoice id: " + inv?.Id + " , code: " + inv?.Code;
            }
            catch (Exception ex)
            {
                return "Error : can't update invoice reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }

        public static string UpdateOnInvoiceTask(TasksLog ob)
        {
            try
            {
                var invoice = new InvoiceRepo(true).getById(int.Parse("0" + ob.ReferenceId));
                Invoice inv = new InvoiceRepo(false).GetByKeyCode(ob.KeyCode);
                if (invoice != null && invoice.Id > 0)
                {
                    string title = "Update invoice";

                    ShowData(title, invoice.Code + " / " + string.Format("{0:yyyy-MM-dd}", invoice.InvoiceDate), "Sends");
                    var uton = new TasksLogRepo(true).GetByFilter(e => e.ReferenceId == "" + invoice.UserId && e.ReferenceType == "user");
                    var us = new UserRepo(false).GetByKeyCode(uton.KeyCode, false);

                    inv.Code = invoice.Code;
                    inv.Comment = invoice.Comment;
                    inv.CreateDate = invoice.CreateDate;
                    inv.DealerId = invoice.DealerId;
                    inv.Discount = invoice.Discount;
                    inv.GrandTotal = invoice.GrandTotal;
                    inv.InvoiceBaseId = invoice.InvoiceBaseId;
                    inv.InvoiceDate = invoice.InvoiceDate;
                    inv.NetTotal = invoice.NetTotal;
                    inv.Printed = invoice.Printed;
                    inv.Service = invoice.Service;
                    inv.StoreId = invoice.StoreId;
                    inv.Tax = invoice.Tax;
                    inv.TypeId = invoice.TypeId;
                    inv.UserId = us.Id;
                    inv.LastStatus = invoice.LastStatus;
                    inv.InvoiceItem = new List<InvoiceItem>();

                    foreach (var item in invoice.InvoiceItem)
                    {
                        var iton = new TasksLogRepo(true).GetByFilter(e => e.ReferenceId == "" + item.ItemId && e.ReferenceType == "item");
                        var it = new ItemRepo(false).GetByKeyCode(iton.KeyCode);

                        var uon = new TasksLogRepo(true).GetByFilter(e => e.ReferenceId == "" + item.UnitId && e.ReferenceType == "unit");
                        var un = new UnitRepo(false).GetByKeyCode(uon.KeyCode);

                        inv.InvoiceItem.Add(new InvoiceItem
                        {
                            Code = item.Code,
                            Discount = item.Discount,
                            LastStatus = item.LastStatus,
                            NetTotal = item.NetTotal,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            Serialnumber = item.Serialnumber,
                            Tax = item.Tax,
                            GrandTotal = item.GrandTotal,
                            ItemId = it.Id,
                            UnitId = un.Id,
                            StoreId = item.StoreId
                        });
                    }

                    new InvoiceRepo(false).Save(inv, true);
                }

                var taskOnLine = new TasksLogRepo(false).Save(new TasksLog
                {
                    KeyCode = ob.KeyCode,
                    ReferenceType = "invoice",
                    TaskId = 2,
                    ReferenceId = "" + inv?.Id
                }, true);

                ob.OnLineId = taskOnLine.Id;
                ob.Sent = true;
                new TasksLogRepo(true).Save(ob, true);
                return "update invoice id: " + inv?.Id + " , code: " + inv?.Code;
            }
            catch (Exception ex)
            {
                return "Error : can't update invoice reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }

        public static string DeleteOffInvoiceTask(TasksLog ob)
        {
            try
            {
                var invoice = new InvoiceRepo(true).GetByKeyCode(ob.KeyCode);
                if (invoice != null && invoice.Id > 0)
                {
                    string title = "Delete invoice";

                    ShowData(title, invoice.Code + " / " + string.Format("{0:yyyy-MM-dd}", invoice.InvoiceDate), "Receive");
                    new InvoiceRepo(true).Remove(invoice.Id, true);
                }


                new TasksLogRepo(true).Save(new TasksLog
                {
                    KeyCode = ob.KeyCode,
                    OnLineId = ob.Id,
                    ReferenceType = "invoice",
                    TaskId = 3,
                    ReferenceId = "" + invoice?.Id,
                    Sent = true
                }, true);

                return "Delete invoice id: " + invoice?.Id + " , code: " + invoice?.Code;
            }
            catch (Exception ex)
            {
                return "Error : can't delete unit reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }

        public static string DeleteOnInvoiceTask(TasksLog ob)
        {
            try
            {
                var invoice = new InvoiceRepo(false).GetByKeyCode(ob.KeyCode);
                if (invoice != null && invoice.Id > 0)
                {
                    string title = "Delete invoice";

                    ShowData(title, invoice.Code + " / " + string.Format("{0:yyyy-MM-dd}", invoice.InvoiceDate), "Receive");
                    new InvoiceRepo(false).Remove(invoice.Id, true);
                }

                var taskOnLine = new TasksLogRepo(false).Save(new TasksLog
                {
                    KeyCode = ob.KeyCode,
                    ReferenceType = "invoice",
                    TaskId = 3,
                    ReferenceId = "" + invoice?.Id
                }, true);

                ob.OnLineId = taskOnLine.Id;
                ob.Sent = true;
                new TasksLogRepo(true).Save(ob, true);
                return "Delete invoice id: " + invoice?.Id + " , code: " + invoice?.Code;
            }
            catch (Exception ex)
            {
                return "Error : can't delete invoice reference id : " + ob.ReferenceId + " , message error : " + ex.Message;
            }
        }
        #endregion

        public static void ShowData(string Title, string FileName, string Status)
        {
            if (Application.OpenForms["frm_Main"] != null)
            {
                string txt = "";
                if(Title != "")
                    txt = Trans(Title) + " : " + FileName;
                ((frm_Main)Application.OpenForms["frm_Main"]).TextData = txt;
            }
        }

        public static string Trans(string text)
        {
            if (GeneralMembers.Lang != "ar")
                return text;

            switch (text)
            {
                case "Add item":
                    return "اضافة صنف";
                case "Add unit":
                    return "اضافة وحدة";
                case "Add category":
                    return "اضافة مجموع صنف";
                case "Add user":
                    return "اضافة مستخدم";
                case "Add invoice":
                    return "اضافة فاتورة";
                case "Update item":
                    return "تعديل صنف";
                case "Update unit":
                    return "تعديل وحدة";
                case "Update category":
                    return "تعديل مجموع صنف";
                case "Update user":
                    return "تعديل مستخدم";
                case "Update invoice":
                    return "تعديل فاتورة";
                case "Delete item":
                    return "حذف صنف";
                case "Delete unit":
                    return "حذف وحدة";
                case "Delete category":
                    return "حذف مجموع صنف";
                case "Delete user":
                    return "حذف مستخدم";
                case "Delete invoice":
                    return "حذف فاتورة";
                default:
                    break;
            }

            return text;
        }
    }
}