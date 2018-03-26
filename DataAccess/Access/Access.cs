
using AppLog4Net;
using DataAccess.Models;
using DataAccess.Servers;
using IBatisNet.DataMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Access
{
    public class Access
    {
        static AppLog applog = new AppLog("Access");
        private Access() { }

        #region LoginWindow

        /// <summary>
        /// 返回用户信息
        /// </summary>
        /// <param name="cardword">账号</param>
        /// <returns>是否成功</returns>
        public static bool GetUserByCard(string username, ref UserInfo userInfo)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();

            //查看账号是否存在
            try
            {
                mapper.BeginTransaction();
                userInfo = mapper.QueryForObject<UserInfo>("GetUserByCard", username);
                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                applog.InfoFormat("Access.GetUserByCard() Error:{0}", ex.Message); 
                mapper.RollBackTransaction();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 账号是否存在
        /// </summary>
        /// <param name="cardword">账号</param>
        /// <returns>记录数</returns>
        public static int CardIsExist(string username)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int result = 0;

            try
            {
                mapper.BeginTransaction();
                result = mapper.QueryForObject<int>("User_IsExist", username);
                mapper.CommitTransaction();
            }
            catch
            {
                mapper.RollBackTransaction();
            }

            return result;
        }

        /// <summary>
        /// 插入用户信息
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns>object</returns>
        public static object InsertUser(UserInfo userInfo)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            object oj = null;

            try
            {
                mapper.BeginTransaction();
                oj = mapper.Insert("InsertUser", userInfo);
                mapper.CommitTransaction();
            }
            catch
            {
                mapper.RollBackTransaction();
            }

            return oj;
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="userInfo">用户信息类</param>
        /// <returns>影响行数</returns>
        public static int UpdateUser(UserInfo userInfo)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int result = 0;

            try
            {
                mapper.BeginTransaction();
                result = mapper.Update("UpdateUser", userInfo);
                mapper.CommitTransaction();
            }
            catch
            {
                mapper.RollBackTransaction();
            }

            return result;
        }

        /// <summary>
        /// 更新用户状态Y
        /// </summary>
        /// <param name="cardword">账号</param>
        /// <returns>是否成功</returns>
        public static bool UpdateStatus_Y(string username)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int result = 0;

            //更新状态
            try
            {
                mapper.BeginTransaction();
                result = mapper.Update("UpdateStatusY", username);
                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                applog.InfoFormat("Access.UpdateStatus_Y() Error:{0}", ex.Message); 
                mapper.RollBackTransaction();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 更新状态为N
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <returns></returns>
        public static int UpdateStatus_N(int userid)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int result = 0;

            try
            {
                mapper.BeginTransaction();
                result = mapper.Update("UpdateStatusN", userid);
                mapper.CommitTransaction();
            }
            catch
            {
                mapper.RollBackTransaction();
            }

            return result;
        }

        /// <summary>
        /// 重置登录密码
        /// </summary>
        /// <param name="cardword"></param>
        /// <param name="PrePass"></param>
        /// <param name="NewPass"></param>
        /// <returns></returns>
        public static int ResetLoginPassword(string cardword, string NewPass)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int result = 0;
            Hashtable hashTable = new Hashtable();

            hashTable.Add("CardWord", cardword); 
            hashTable.Add("NewPassWord", NewPass);

            try
            {
                mapper.BeginTransaction();
                result = mapper.Update("UpdateLoginPassword", hashTable);
                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                applog.InfoFormat("Access.ResetLoginPassword() Error:{0}", ex.Message);
                mapper.RollBackTransaction();
                return 0;
            }

            return result;
        }

        /// <summary>
        /// 重置功能密码
        /// </summary>
        /// <param name="cardword"></param>
        /// <param name="PrePass"></param>
        /// <param name="NewPass"></param>
        /// <returns></returns>
        public static int ResetFuncPassword(string cardword,string NewPass)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int result = 0;
            Hashtable hashTable = new Hashtable();

            hashTable.Add("CardWord", cardword); 
            hashTable.Add("NewPassWord", NewPass);

            try
            {
                mapper.BeginTransaction();
                result = mapper.Update("UpdateFuncPassword", hashTable);
                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                applog.InfoFormat("Access.ResetFuncPassword() Error:{0}", ex.Message);
                mapper.RollBackTransaction();
                return 0;
            }

            return result;
        }

        #endregion

        #region 单

        #region 单

        /// <summary>
        /// 返回所有Sheet信息
        /// </summary> 
        /// <returns>Sheet信息</returns>
        public static List<SheetInfo> GetSheetList()
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            List<SheetInfo> sheetList = null;

            try
            {
                mapper.BeginTransaction();
                sheetList = mapper.QueryForList<SheetInfo>("GetAllSheet", null).OrderBy(info=>info.ID).ToList();
                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                applog.InfoFormat("Access.GetSheetList() Error:{0}", ex.Message);
                mapper.RollBackTransaction();
                return new List<SheetInfo>();
            }

            return sheetList;
        }

        /// <summary>
        /// 更新Sheet信息
        /// </summary> 
        public static int UpdateSheetInfo(SheetInfo sheetInfo)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int result = 0;

            try
            {
                mapper.BeginTransaction();
                result = mapper.Update("UpdateSheetInfo", sheetInfo);
                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                applog.InfoFormat("Access.UpdateSheetInfo() Error:{0}", ex.Message);
                mapper.RollBackTransaction();
            }

            return result;
        }

        /// <summary>
        /// 插入Sheet信息
        /// </summary>
        /// <returns>最新插入插入的ID</returns>
        public static int InsertSheet(SheetInfo sheetInfo)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int id = 0;

            try
            {
                mapper.BeginTransaction();
                id = (int)(mapper.Insert("InsertSheet", sheetInfo));
                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                applog.InfoFormat("Access.InsertSheet() Error:{0}", ex.Message);
                mapper.RollBackTransaction();
            }

            return id;
        }

        /// <summary>
        /// 删除Sheet信息
        /// </summary>
        public static int DeleteSheet(int sheetID)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int result = 0;

            try
            {
                mapper.BeginTransaction();
                result = mapper.Delete("DeleteSheet", sheetID);
                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                applog.InfoFormat("Access.DeleteSheet() Error:{0}", ex.Message);
                mapper.RollBackTransaction();
            }

            return result;
        }

        #endregion

        #region 业务

        /// <summary>
        /// 返回所有业务类型信息
        /// </summary> 
        /// <returns>Sheet信息</returns>
        public static List<BsTypeInfo> GetBsTypeList()
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            List<BsTypeInfo> typeList = null;

            try
            {
                mapper.BeginTransaction();
                typeList = mapper.QueryForList<BsTypeInfo>("GetAllType", null).ToList();
                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                applog.InfoFormat("Access.GetBsTypeList() Error:{0}", ex.Message);
                mapper.RollBackTransaction();
                return new List<BsTypeInfo>();
            }

            return typeList;
        }

        #endregion  

        #region 流程

        /// <summary>
        /// 返回Sheet所对应的流程信息
        /// </summary>  
        public static string GetProcedureMap(int sheetID)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            string procedureMap = null;

            try
            {
                mapper.BeginTransaction();
                procedureMap = mapper.QueryForObject<string>("GetProcedureMapBySheet", sheetID);
                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                applog.InfoFormat("Access.GetProcedureMap() Error:{0}", ex.Message);
                mapper.RollBackTransaction();
            }

            return procedureMap;
        }

        /// <summary>
        /// 更新Sheet对应的流程信息
        /// </summary> 
        public static int UpdateProcedureMap(int sheetID, string procedureMap)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int result = 0;

            try
            {
                mapper.BeginTransaction();
                result = mapper.QueryForObject<int>("SheetIsExistInProcedure", sheetID);
                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                applog.InfoFormat("Access.UpdateProcedureMap()--Search Error:{0}", ex.Message);
                mapper.RollBackTransaction();
                return result;
            }

            Hashtable hashTable = new Hashtable();
            hashTable.Add("SheetID", sheetID);
            hashTable.Add("ProcedureMap", procedureMap);
            if (result == 0)
            {
                try
                {
                    mapper.BeginTransaction();
                    mapper.Insert("InsertProcedureMap", hashTable);
                    mapper.CommitTransaction();

                    result = 1;
                }
                catch (Exception ex)
                {
                    applog.InfoFormat("Access.UpdateProcedureMap()--Insert Error:{0}", ex.Message);
                    mapper.RollBackTransaction();
                }
            }
            else
            {
                try
                {
                    mapper.BeginTransaction();
                    mapper.Update("UpdateProcedureMapBySheet", hashTable);
                    mapper.CommitTransaction();

                    result = 1;
                }
                catch (Exception ex)
                {
                    applog.InfoFormat("Access.UpdateProcedureMap()--Update Error:{0}", ex.Message);
                    mapper.RollBackTransaction();
                }
            } 

            return result;
        }

        /// <summary>
        /// 插入Sheet对应的流程信息
        /// </summary> 
        public static int InsertProcedureMap(int sheetID, string procedureMap)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int insertID = 0;
            Hashtable hashTable = new Hashtable();

            hashTable.Add("SheetID", sheetID);
            hashTable.Add("ProcedureMap", procedureMap);

            try
            {
                mapper.BeginTransaction();
                insertID = (int)(mapper.Insert("InsertProcedureMap", hashTable));
                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                applog.InfoFormat("Access.InsertProcedureMap() Error:{0}", ex.Message);
                mapper.RollBackTransaction();
            }

            return insertID;
        }

        #endregion

        #region 收支

        /// <summary>
        /// 返回Sheet所对应收支信息
        /// </summary> 
        public static string GetSheetBOPMap(int sheetID)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            string bopMap = null;

            try
            {
                mapper.BeginTransaction();
                bopMap = mapper.QueryForObject<string>("GetSheetBOPMapBySheet", sheetID);
                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                applog.InfoFormat("Access.GetSheetBOPMap() Error:{0}", ex.Message);
                mapper.RollBackTransaction();
            }

            return bopMap;
        }

        /// <summary>
        /// 更新Sheet对应的收支信息
        /// </summary> 
        public static int UpdateSheetBOPMap(int sheetID, string bopMap)
        {
            //查看记录是否存在
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int result = 0;

            try
            {
                mapper.BeginTransaction();
                result = mapper.QueryForObject<int>("SheetIsExistInBOP", sheetID);
                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                applog.InfoFormat("Access.UpdateSheetBOPMap()--Search Error:{0}", ex.Message);
                mapper.RollBackTransaction();
                return result;
            }

            Hashtable hashTable = new Hashtable();
            hashTable.Add("SheetID", sheetID);
            hashTable.Add("BOPMap", bopMap);
            if (result == 0)
            {
                try
                {
                    mapper.BeginTransaction();
                    mapper.Insert("InsertSheetBOPMap", hashTable);
                    mapper.CommitTransaction();

                    result = 1;
                }
                catch (Exception ex)
                {
                    applog.InfoFormat("Access.UpdateSheetBOPMap()--Insert Error:{0}", ex.Message);
                    mapper.RollBackTransaction();
                }
            }
            else
            {
                try
                {
                    mapper.BeginTransaction();
                    result = mapper.Update("UpdateSheetBOPMapBySheet", hashTable);
                    mapper.CommitTransaction();
                }
                catch (Exception ex)
                {
                    applog.InfoFormat("Access.UpdateSheetBOPMap()--Update Error:{0}", ex.Message);
                    mapper.RollBackTransaction();
                }
            } 

            return result;
        }

        /// <summary>
        /// 插入Sheet对应的收支信息
        /// </summary> 
        public static int InsertSheetBOPMap(int sheetID, string bopMap)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int insertID = 0;
            Hashtable hashTable = new Hashtable();

            hashTable.Add("SheetID", sheetID);
            hashTable.Add("BOPMap", bopMap);

            try
            {
                mapper.BeginTransaction();
                insertID = (int)(mapper.Insert("InsertSheetBOPMap", hashTable));
                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                applog.InfoFormat("Access.InsertSheetBOPMap() Error:{0}", ex.Message);
                mapper.RollBackTransaction();
            }

            return insertID;
        }

        #endregion

        #endregion

        #region 企业收支

        /// <summary>
        /// 返回所有BOP信息
        /// </summary> 
        public static List<BOPInfo> GetBOPList()
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            List<BOPInfo> bopList = null;

            try
            {
                mapper.BeginTransaction();
                bopList = mapper.QueryForList<BOPInfo>("GetAllBOP", null).ToList();
                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                applog.InfoFormat("Access.GetBOPList() Error:{0}", ex.Message);
                mapper.RollBackTransaction();
                return new List<BOPInfo>();
            }

            return bopList;
        }

        /// <summary>
        /// 更新BOP信息
        /// </summary> 
        public static int UpdateBOPInfo(BOPInfo bopInfo)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int result = 0;

            try
            {
                mapper.BeginTransaction();
                result = mapper.Update("UpdateBOPInfo", bopInfo);
                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                applog.InfoFormat("Access.UpdateBOPInfo() Error:{0}", ex.Message);
                mapper.RollBackTransaction();
            }

            return result;
        }

        /// <summary>
        /// 插入BOP信息
        /// </summary>
        public static int InsertBOP(BOPInfo bopInfo)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int id = 0;

            try
            {
                mapper.BeginTransaction();
                id = (int)(mapper.Insert("InsertBOP", bopInfo));
                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                applog.InfoFormat("Access.InsertBOP() Error:{0}", ex.Message);
                mapper.RollBackTransaction();
            }

            return id;
        }

        /// <summary>
        /// 删除BOP信息
        /// </summary>
        public static int DeleteBOP(int bopID)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int result = 0;

            try
            {
                mapper.BeginTransaction();
                result = mapper.Delete("DeleteBOP", bopID);
                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                applog.InfoFormat("Access.DeleteBOP() Error:{0}", ex.Message);
                mapper.RollBackTransaction();
            }

            return result;
        }

        #endregion

        #region 雇员

        /// <summary>
        /// 返回所有雇员信息
        /// </summary> 
        /// <returns>Employee信息</returns>
        public static List<EmployeeInfo> GetEmpluyeeInfoList()
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            List<EmployeeInfo> employeeList = null;

            try
            {
                mapper.BeginTransaction();
                employeeList = mapper.QueryForList<EmployeeInfo>("GetAllEmployee", null).ToList();
                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                applog.InfoFormat("Access.GetEmpluyeeInfoList() Error:{0}", ex.Message);
                mapper.RollBackTransaction();
                return new List<EmployeeInfo>();
            }

            return employeeList;
        }

        /// <summary>
        /// 更新Employee信息
        /// </summary> 
        public static int UpdateEmployeeInfo(EmployeeInfo employeeInfo)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int result = 0;

            try
            {
                mapper.BeginTransaction();
                result = mapper.Update("UpdateEmployeeInfo", employeeInfo);
                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                applog.InfoFormat("Access.UpdateEmployeeInfo() Error:{0}", ex.Message);
                mapper.RollBackTransaction();
            }

            return result;
        }

        /// <summary>
        /// 插入Employee信息
        /// </summary>
        public static int InsertEmployee(EmployeeInfo employeeInfo)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int id = 0;

            try
            {
                mapper.BeginTransaction();
                id = (int)(mapper.Insert("InsertEmployee", employeeInfo));
                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                applog.InfoFormat("Access.InsertEmployee() Error:{0}", ex.Message);
                mapper.RollBackTransaction();
            }

            return id;
        }

        /// <summary>
        /// 删除Employee信息
        /// </summary>
        public static int DeleteEmployee(int employeeID)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int result = 0;

            try
            {
                mapper.BeginTransaction();
                result = mapper.Delete("DeleteEmployee", employeeID);
                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                applog.InfoFormat("Access.DeleteEmployee() Error:{0}", ex.Message);
                mapper.RollBackTransaction();
            }

            return result;
        }

        #endregion

        #region 部门

        /// <summary>
        /// 返回所有部门信息
        /// </summary> 
        /// <returns>Department信息</returns>
        public static List<DepartmentInfo> GetDepartmentInfoList()
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            List<DepartmentInfo> departmentList = null;

            try
            {
                mapper.BeginTransaction();
                departmentList = mapper.QueryForList<DepartmentInfo>("GetAllDepartment", null).ToList();
                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                applog.InfoFormat("Access.GetDepartmentInfoList() Error:{0}", ex.Message);
                mapper.RollBackTransaction();
                return new List<DepartmentInfo>();
            }

            return departmentList;
        }

        #endregion

        #region 职务

        /// <summary>
        /// 返回所有职务信息
        /// </summary> 
        /// <returns>Department信息</returns>
        public static List<PostInfo> GetPostInfoList()
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            List<PostInfo> postList = null;

            try
            {
                mapper.BeginTransaction();
                postList = mapper.QueryForList<PostInfo>("GetAllPost", null).ToList();
                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                applog.InfoFormat("Access.GetPostInfoList() Error:{0}", ex.Message);
                mapper.RollBackTransaction();
                return new List<PostInfo>();
            }

            return postList;
        }

        #endregion

        #region 工资

        /// <summary>
        /// 返回所有工资信息
        /// </summary> 
        /// <returns>Salary信息</returns>
        public static List<SalaryInfo> GetSalaryInfoList()
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            List<SalaryInfo> salaryList = null;

            try
            {
                mapper.BeginTransaction();
                salaryList = mapper.QueryForList<SalaryInfo>("GetAllSalary", null).ToList();
                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                applog.InfoFormat("Access.GetSalaryInfoList() Error:{0}", ex.Message);
                mapper.RollBackTransaction();
                return new List<SalaryInfo>();
            }

            return salaryList;
        }

        /// <summary>
        /// 更新Salary信息
        /// </summary> 
        public static int UpdateSalaryInfo(SalaryInfo salaryInfo)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int result = 0;

            try
            {
                mapper.BeginTransaction();
                result = mapper.Update("UpdateSalaryInfo", salaryInfo);
                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                applog.InfoFormat("Access.UpdateSalaryInfo() Error:{0}", ex.Message);
                mapper.RollBackTransaction();
            }

            return result;
        }

        /// <summary>
        /// 插入Salary信息
        /// </summary>
        public static int InsertSalary(SalaryInfo salaryInfo)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int id = 0;

            try
            {
                mapper.BeginTransaction();
                id = (int)(mapper.Insert("InsertSalary", salaryInfo));
                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                applog.InfoFormat("Access.InsertSalary() Error:{0}", ex.Message);
                mapper.RollBackTransaction();
            }

            return id;
        }

        /// <summary>
        /// 删除Salary信息
        /// </summary>
        public static int DeleteSalary(int salaryID)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int result = 0;

            try
            {
                mapper.BeginTransaction();
                result = mapper.Delete("DeleteSalary", salaryID);
                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                applog.InfoFormat("Access.DeleteSalary() Error:{0}", ex.Message);
                mapper.RollBackTransaction();
            }

            return result;
        }

        #endregion

        /*

        #region MainWindow

        /// <summary>
        /// 根据账号查看状态
        /// </summary>
        /// <param name="cardword">账号</param>
        /// <returns>用户信息</returns>
        public static UserInfo StatusIsY(string cardword)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            UserInfo userInfo = null;

            try
            {
                mapper.BeginTransaction();
                userInfo = mapper.QueryForObject<UserInfo>("Status_IsY", cardword);
                mapper.CommitTransaction();
            }
            catch { }

            return userInfo;
        }

        /// <summary>
        /// 根据用户ID返回好友记录行集合
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <returns>满足记录集合</returns>
        public static List<FriendInfo> GetAllFriendByID(int userid)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            List<FriendInfo> friendList = null;

            try
            {
                mapper.BeginTransaction();
                friendList = mapper.QueryForList<FriendInfo>("GetAllFriendByID", userid).ToList();
                mapper.CommitTransaction();
            }
            catch { }

            return friendList;
        }

        /// <summary>
        /// 根据用户id返回用户信息
        /// </summary>
        /// <param name="listFriend">存放用户id与组id的集合</param>
        /// <returns>包含（用户信息+所在组id）的集合</returns>
        public static List<UserInfo> GetAllUserByID(List<Friend> listFriend)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            List<UserInfo> userList = null;

            try
            {
                mapper.BeginTransaction();
                userList = mapper.QueryForList<UserInfo>("GetAllUserByID", listFriend).ToList();
                mapper.CommitTransaction();
            }
            catch { }

            return userList;
        }

        /// <summary>
        /// 根据用户id返回所玩游戏信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static List<GameInfo> GetAllGameByID(int userid)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            List<GameInfo> gameList = null;

            try
            {
                mapper.BeginTransaction();
                gameList = mapper.QueryForList<GameInfo>("GetAllGameByID", userid).ToList();
                mapper.CommitTransaction();
            }
            catch { }

            return gameList;
        }

        /// <summary>
        /// 根据用户所玩游戏信息获取相应成就信息
        /// </summary>
        /// <param name="gameInfo">包含（用户id,游戏名字,游戏表）的信息</param>
        /// <returns>成就信息</returns>
        public static AchieveInfo GetAllAchieveByID(GameInfo gameInfo)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            AchieveInfo achieveInfo = null;

            try
            {
                mapper.BeginTransaction();
                achieveInfo=mapper.QueryForObject<AchieveInfo>("GetAllAchieveByID", gameInfo);
                mapper.CommitTransaction();
            }
            catch { }

            return achieveInfo;
        }

        /// <summary>
        /// 返回消息集合
        /// </summary>
        /// <returns>系统消息集合</returns>
        public static List<NoticeInfo> GetAllMsg()
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            List<NoticeInfo> noticeList = null;

            try
            {
                mapper.BeginTransaction();
                noticeList = mapper.QueryForList<NoticeInfo>("GetAllMsg", null).ToList();
                mapper.CommitTransaction();
            }
            catch { }

            return noticeList;
        }

        /// <summary>
        /// 根据用户id返回“别人请求添加自己为好友”的该用户的信息集合
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <returns>消息集合</returns>
        public static List<UserInfo> GetAddFromByAddTo(int userid)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            List<UserInfo> userList = null;
            try
            {
                mapper.BeginTransaction();
                userList = mapper.QueryForList<UserInfo>("GetAddFromByAddTo", userid).ToList();
                mapper.CommitTransaction();
            }
            catch { }

            return userList;
        }

        /// <summary>
        /// 1.根据用户id返回“自己请求别人为好友，然后对方同意了”的用户信息集合
        /// 2.存在自己添加别人为好友的请求且已经被处理了，删除该记录
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <returns></returns>
        public static List<UserInfo> GetAddToByAddFrom(int userid)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            List<UserInfo> userList = null;
            try
            {
                mapper.BeginTransaction();
                userList = mapper.QueryForList<UserInfo>("GetAddToByAddFrom", userid).ToList();
                mapper.Delete("DeleteAgreeByAddFrom", userid); //存在自己添加别人为好友的请求且已经被处理了，删除该记录
                mapper.CommitTransaction();
            }
            catch { }

            return userList;
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="cardword">账号</param>
        /// <param name="PrePass">原密码（加密后）</param>
        /// <param name="NewPass">新密码（加密后）</param>
        /// <returns></returns>
        public static int ResetPassword(string cardword,string PrePass,string NewPass)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int result = 0;
            Hashtable hashTable = new Hashtable();

            hashTable.Add("CardWord", cardword);
            hashTable.Add("PassWord", PrePass);
            hashTable.Add("NewPassWord", NewPass);

            try
            {
                mapper.BeginTransaction();
                result = mapper.Update("ResetPassword", hashTable);
                mapper.CommitTransaction();
            }
            catch { }

            return result;
        }

        /// <summary>
        /// 根据账号或昵称返回用户信息集合
        /// </summary>
        /// <param name="cardorname">账号或昵称</param>
        /// <returns></returns>
        public static List<UserInfo> GetUserByIDorName(string cardorname)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            List<UserInfo> userList = null;

            try
            {
                mapper.BeginTransaction();
                userList = mapper.QueryForList<UserInfo>("GetUserByIDorName", cardorname).ToList();
                mapper.CommitTransaction();
            }
            catch { }

            return userList;
        }

        /// <summary>
        /// 判断addfriendinfo表中是否存在相同记录
        /// </summary>
        /// <param name="fromid">请求者id</param>
        /// <param name="toid">被请求者id</param>
        /// <returns>记录数</returns>
        public static int IsExistAddFriendInfo(string fromid,string toid)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int result = 0;

            //判断addfriendinfo表中是否存在相同记录
            Hashtable hashTable = new Hashtable();
            hashTable.Add("AddFrom", fromid);
            hashTable.Add("AddTo", toid);

            try
            {
                mapper.BeginTransaction();
                result = mapper.QueryForObject<int>("IsExistAddFriendInfo", hashTable);
                mapper.CommitTransaction();
            }
            catch { }

            return result;
        }

        /// <summary>
        /// 将记录请求插入addfriendinfo表中
        /// </summary>
        /// <param name="fromid">请求者id</param>
        /// <param name="toid">被请求者id</param>
        /// <returns></returns>
        public static object AddFriendInfo(string fromid, string toid)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            object oj = null;
             
            Hashtable hashTable = new Hashtable();
            hashTable.Add("AddFrom", fromid);
            hashTable.Add("AddTo", toid);

            //将记录请求插入addfriendinfo表中
            try
            {
                mapper.BeginTransaction();
                oj=mapper.Insert("AddFriendInfo", hashTable);
                mapper.CommitTransaction();
            }
            catch { }

            return oj;
        }

        /// <summary>
        /// 被请求者同意后，修改添加表中的记录为“已同意”
        /// </summary>
        /// <param name="fromid">请求者id</param>
        /// <param name="toid">被请求者id</param>
        /// <returns>影响行数</returns>
        public static int UpdateAddFriendInfo(string fromid, string toid)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int result = 0;

            //修改添加表中的记录为“已同意”。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。
            Hashtable hashTable = new Hashtable();
            hashTable.Add("AddFrom", fromid);
            hashTable.Add("AddTo", toid);

            try
            {
                mapper.BeginTransaction();
                result = mapper.Update("UpdateAddFriendInfo", hashTable);
                mapper.CommitTransaction();
            }
            catch { }

            return result;
        }

        /// <summary>
        /// 查看好友映射表中是否已存在记录
        /// </summary>
        /// <param name="oneid">用户id</param>
        /// <param name="otherid">用户id</param>
        /// <returns>记录数</returns>
        public static int IsExistFriendInfo(string oneid,string otherid)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int result = 0;

            //查看好友映射表中是否已存在记录。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。
            Hashtable hashTable = new Hashtable();
            hashTable.Add("OneID", oneid);
            hashTable.Add("OtherID", otherid);

            try
            {
                mapper.BeginTransaction();
                result = mapper.QueryForObject<int>("IsExistFriendInfo", hashTable);
                mapper.CommitTransaction();
            }
            catch { }

            return result;
        }

        /// <summary>
        /// 插入好友映射表
        /// </summary>
        /// <param name="oneid">用户id</param>
        /// <param name="otherid">用户id</param>
        /// <returns></returns>
        public static object InsertFriendInfo(string oneid, string otherid)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            object oj = null;
 
            Hashtable hashTable = new Hashtable();
            hashTable.Add("OneID", oneid);
            hashTable.Add("OtherID", otherid);

            //插入好友映射表。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。
            try
            {
                mapper.BeginTransaction();
                oj = mapper.Insert("InsertFriendInfo", hashTable);
                mapper.CommitTransaction();
            }
            catch { }

            return oj;
        }

        /// <summary>
        /// 根据用户id，返回用户信息
        /// </summary>
        /// <param name="oneid">用户id</param>
        /// <param name="otherid">用户id</param>
        /// <returns>两个用户的信息</returns>
        public static List<UserInfo> GetUserInfoByID(string oneid, string otherid)
        { 
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            List<UserInfo> userList = null;

            ArrayList arrayList = new ArrayList();
            arrayList.Add(oneid);
            arrayList.Add(otherid);

            try
            {
                mapper.BeginTransaction();
                userList = mapper.QueryForList<UserInfo>("GetUserInfoByID", arrayList).ToList();
                mapper.CommitTransaction();
            }
            catch { }

            return userList;
        }

        /// <summary>
        /// 删除该好友请求记录
        /// </summary>
        /// <param name="fromid">请求者id</param>
        /// <param name="toid">被请求者id</param>
        /// <returns></returns>
        public static int DeleteAddFriendInfo(string fromid, string toid)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int result = 0;

            Hashtable hashTable = new Hashtable();
            hashTable.Add("AddFrom", fromid);
            hashTable.Add("AddTo", toid);

            try
            {
                mapper.BeginTransaction();
                result = mapper.Delete("DeleteAddFriendInfo", hashTable);
                mapper.CommitTransaction();
            }
            catch { }

            return result;
        }

        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="friendid">好友id</param>
        /// <returns></returns>
        public static int DeleteFriend(string userid, string friendid)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int result = 0;
            Hashtable hashTable = new Hashtable();

            hashTable.Add("UserID", userid);
            hashTable.Add("FriendID", friendid);

            try
            {
                mapper.BeginTransaction();
                result = mapper.Delete("DeleteFriend", hashTable);
                mapper.CommitTransaction();
            }
            catch { }

            return result;
        }

        /// <summary>
        /// 移动好友
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="friendid">好友id</param>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public static int MoveFriend(string userid, string friendid, string groupid)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int result = 0;

            Hashtable hashTable = new Hashtable();

            hashTable.Add("UserID", userid);
            hashTable.Add("FriendID", friendid);
            hashTable.Add("GroupID", groupid);

            try
            {
                mapper.BeginTransaction();
                result = mapper.Update("MoveFriend1", hashTable);
                mapper.CommitTransaction();
            }
            catch { }

            if (result == 0)
            {
                try
                {
                    mapper.BeginTransaction();
                    result = mapper.Update("MoveFriend2", hashTable);
                    mapper.CommitTransaction();
                }
                catch { }
            }

            return result;
        }

        public static object InsertUserToGame(string userid, string gameid)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int result = 0;
            Hashtable hashTable = new Hashtable();

            hashTable.Add("UserID", userid);
            hashTable.Add("GameID", gameid);

            try
            {
                mapper.BeginTransaction();
                result = mapper.QueryForObject<int>("IsExistUserToGame", hashTable);
                mapper.CommitTransaction();
            }
            catch { }

            if (result == 0)
            {
                object oj = null;

                try
                {
                    mapper.BeginTransaction();
                    oj = mapper.Insert("InsertUserToGame", hashTable);
                    mapper.CommitTransaction();
                }
                catch { }

                return oj;
            }

            return null;
        }

        /// <summary>
        /// 根据游戏ID获取路径
        /// </summary>
        /// <param name="gameid"></param>
        /// <returns></returns>
        public static string GetGamePath(string gameid)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            string gamepath = null;

            try
            {
                mapper.BeginTransaction();
                gamepath = mapper.QueryForObject<string>("GetGamePath", gameid);
                mapper.CommitTransaction();
            }
            catch { }

            return gamepath;
        }

        /// <summary>
        /// 获取游戏版本时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetGameTime(string gameid)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            string gametime = null;

            try
            {
                mapper.BeginTransaction();
                gametime = mapper.QueryForObject<string>("GetGameTime", gameid);
                mapper.CommitTransaction();
            }
            catch { }

            return gametime;
        }

        #endregion

        #region 消息管理

        /// <summary>
        /// 发布消息，插入数据库
        /// </summary>
        /// <param name="noticeInfo"></param>
        /// <returns></returns>
        public static object InsertNotice(NoticeInfo noticeInfo)
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            object oj = null;

            try
            {
                mapper.BeginTransaction();
                oj=mapper.Insert("InsertNotice", noticeInfo);
                mapper.CommitTransaction();
            }
            catch { }

            return oj; 
        }

        /// <summary>
        /// 清空消息
        /// </summary>
        /// <returns></returns>
        public static int ClearNotice()
        {
            ISqlMapper mapper = GetSqlMapper.GetMapper();
            int result=0;

            try
            {
                mapper.BeginTransaction();
                result=mapper.Delete("ClearNotice", null);
                mapper.CommitTransaction();
            }
            catch { }

            return result;
        }

        #endregion
         * 
         * */
    }
}
