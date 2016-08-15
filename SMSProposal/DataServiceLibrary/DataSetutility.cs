using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using DataModelLibrary;

namespace DataServiceLibrary
{
    public static class DataSetutility
    {
        public static Tuple<bool, string> ValidateStudentTemplate(this DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0)
            {
                var lstcolumns = new List<string> { "RollNo", "Name", "Class", "Section", "Mobile", "Blood Group" };
                bool iscolumnExist = IsAllHeaderColumnExist(ds.Tables[0], lstcolumns);
                if (iscolumnExist)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataSet dsremovedemptyrowSet = RemovedNotFilledRows(ds);
                        if (!IsAllARequiredFieldsFilled(dsremovedemptyrowSet))
                        {
                            return Tuple.Create(false, "RollNo, Name,Class and Mobile should not be blank");
                        }
                        var tupleunique = IsAllUniqueRollNo(dsremovedemptyrowSet);
                        if (!tupleunique.Item1)
                        {
                            return Tuple.Create(false,  tupleunique.Item2);
                        }
                        var isValidMobile = IsValidMobile(dsremovedemptyrowSet);
                        if (!isValidMobile.Item1)
                        {
                            return Tuple.Create(false,
                                string.Format("Invalid Mobile - {0} ",
                                    isValidMobile.Item2));
                        }
                        return Tuple.Create(true, "");
                    }
                    else
                    {
                        return Tuple.Create(false, "Please enter a student info");
                    }
                }
                else
                {
                    return Tuple.Create(false,
                        "Excel Input header format => RollNo	, Name,Class, Section ,	Mobile,Blood Group");
                }
            }
            else
            {
                return Tuple.Create(false, "Excel sheet is empty");
            }
        }

        private static Tuple<bool, string> IsAllUniqueRollNo(DataSet ds)
        {
            var duplicatcheck =
                ds.Tables[0].AsEnumerable() //.Where(r=>r["Section"]==DBNull.Value)
                    .GroupBy(
                        r =>
                            new
                            {
                                RollNo = r["RollNo"],
                                Class = r["Class"],
                                Section = r["Section"] == DBNull.Value ? string.Empty : r["Section"]
                            })
                    .SingleOrDefault(dr => dr.Count() > 1);
            if (duplicatcheck != null)
            {
                return Tuple.Create(false, string.Format("Duplicate found in  Roll No {0} Class {1} Section {2} ",duplicatcheck.Key.RollNo.ToString(),
                    duplicatcheck.Key.Class.ToString(),duplicatcheck.Key.Section.ToString()));
            }
            return Tuple.Create(true, string.Empty);
        }
        private static Tuple<bool, string> IsValidMobile(DataSet ds)
        {

            Regex mobileRegex = new Regex(@"^[789]\d{9}$");
            var mobilenocheck =
                ds.Tables[0].AsEnumerable()
                    .FirstOrDefault(dr=>mobileRegex.IsMatch(dr["Mobile"].ToString())==false);
            if (mobilenocheck != null)
            {
                return Tuple.Create(false, mobilenocheck["Mobile"].ToString());
            }
            return Tuple.Create(true, string.Empty);
        }

        private static DataSet RemovedNotFilledRows(DataSet ds)
        {
            var notfilledrows = ds.Tables[0].AsEnumerable().Where(r => r["RollNo"] == DBNull.Value &&
                                                                       r["Name"] == null && r["Class"] == DBNull.Value &&
                                                                       r["Mobile"] == DBNull.Value).ToList();

            notfilledrows.ForEach(dr=>
            {
                ds.Tables[0].Rows.Remove(dr);
            });
            ds.Tables[0].AcceptChanges();
            return ds;
        }

        private static bool IsAllARequiredFieldsFilled(DataSet ds)
        {
            var emptyrowcheck = ds.Tables[0].AsEnumerable().SingleOrDefault(r => r["RollNo"] == DBNull.Value ||
                                                                       r["Name"] == null || r["Class"] == DBNull.Value ||
                                                                       r["Mobile"] == DBNull.Value);

            if (emptyrowcheck != null)
            {
                return false;
            }
            return true;
        }

        private static bool IsAllHeaderColumnExist(DataTable tableNameToCheck, List<string> columnsNames)
        {
            bool iscolumnExist = true;
            foreach (string columnName in columnsNames)
            {
                if (!tableNameToCheck.Columns.Contains(columnName))
                {
                    iscolumnExist = false;
                    break;
                }
            }
            return iscolumnExist;
        }

    }
}
