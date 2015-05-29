using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using iq_add_user.Models;
using System.Web.Configuration;
using iq_add_user.Helpers;
using System.IO;
using System.Text;

namespace iq_add_user.Controllers
{
    public class UserController : Controller
    {
        private IQMS_Entities db = new IQMS_Entities();
        private List<string> _addUserResults = new List<string>();
        private List<string> _addUserErrors = new List<string>();


        // GET: USER LIST
        public ActionResult Index()
        {
            var s_USER_GENERAL = db.S_USER_GENERAL.Include(s => s.EPLANT);
            return View(s_USER_GENERAL.OrderBy(u=>u.USER_NAME).ToList());
        }

        public ActionResult ListUsers()
        {
            var allUsers = db.Database.SqlQuery<IQMSUserRow>(
                @"
select s.USER_NAME as username, u.ACCOUNT_STATUS AS AccountStatus, 
e.LAST_NAME as LastName, e.FIRST_NAME as FirstName,
s.EMAIL as UserEmail, e.EMAIL as EmployeeEmail, 
s.EPLANT_ID as EplantId, ep.NAME as EplantName, 
t.id as TeamMemberId, x.EXP_APPROVER as ExpenseApprover, x.ID as ExpenseUserId
from s_user_general s
left outer join pr_emp e on s.PR_EMP_ID = e.id
left outer join eplant ep on s.EPLANT_ID = ep.ID
left outer join TEAM_MEMBER t on s.USER_NAME = t.USERID
left outer join EXP_USER x on s.USER_NAME = x.USER_NAME
join SYS.DBA_USERS u on s.USER_NAME = u.USERNAME
");
            
            return View(allUsers.OrderBy(x=>x.EplantName + x.Username).Take(50));
        }

        public ActionResult ListUsersRemote()
        {
            return View();
        }


        public ActionResult UserListData(DTParameterModel param)
        {
            #region Repository
            StringBuilder sb = new StringBuilder();
            sb.Append(@"select s.USER_NAME as username, u.ACCOUNT_STATUS AS AccountStatus, ");
            sb.Append(@"e.LAST_NAME as LastName, e.FIRST_NAME as FirstName, ");
            sb.Append(@"s.EMAIL as UserEmail, e.EMAIL as EmployeeEmail, ");
            sb.Append(@"s.EPLANT_ID as EplantId, ep.NAME as EplantName, ");
            sb.Append(@"t.id as TeamMemberId, x.EXP_APPROVER as ExpenseApprover, x.ID as ExpenseUserId ");
            sb.Append(@"from s_user_general s ");
            sb.Append(@"left outer join pr_emp e on s.PR_EMP_ID = e.id ");
            sb.Append(@"left outer join eplant ep on s.EPLANT_ID = ep.ID ");
            sb.Append(@"left outer join TEAM_MEMBER t on s.USER_NAME = t.USERID ");
            sb.Append(@"left outer join EXP_USER x on s.USER_NAME = x.USER_NAME ");
            sb.Append(@"join SYS.DBA_USERS u on s.USER_NAME = u.USERNAME ");

            string sql = sb.ToString();

            var allUsers = db.Database.SqlQuery<IQMSUserRow>(sql);
            #endregion


            var searchString = param.Search.Value;
            var searchRegEx = param.Search.Regex;
            int sortColumnIndex = param.Order.First().Column;   //var sortColumnIndex = Convert.ToInt32(Request["order[0][column]"]);
            string sortDirection = param.Order.First().Dir;     //Request["order[0][dir]"]; // asc or desc

            IEnumerable<IQMSUserRow> filteredUsers;
            if (!string.IsNullOrEmpty(searchString))
            {
                filteredUsers = allUsers
                    .Where(u => u.Username.ToUpper().Contains(searchString.ToUpper()));
            }
            else
            {
                filteredUsers = allUsers;
            }

            //var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<IQMSUserRow, string> orderingFunction = (c => sortColumnIndex == 0 ? c.Username :
                                                                sortColumnIndex == 1 ? c.AccountStatus :
                                                                c.UserEmail);

            //var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredUsers = filteredUsers.OrderBy(orderingFunction);
            else
                filteredUsers = filteredUsers.OrderByDescending(orderingFunction);

            var displayedUsers = filteredUsers.Skip(param.Start).Take(param.Length); ;

            var result = from u in displayedUsers
                         select new[]{
                             u.Username, 
                             u.AccountStatus,
                             u.UserEmail
                         };


            return Json(new
            {
                draw = param.Draw,
                recordsTotal = allUsers.Count(),
                recordsFiltered = filteredUsers.Count(),
                data = result
            }, JsonRequestBehavior.AllowGet);

        }


































        #region SCAFFOLD ACTIONS

        // GET: S_USER_GENERAL/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            S_USER_GENERAL s_USER_GENERAL = db.S_USER_GENERAL.Find(id);
            if (s_USER_GENERAL == null)
            {
                return HttpNotFound();
            }
            return View(s_USER_GENERAL);
        }

        // GET: S_USER_GENERAL/Create
        public ActionResult Create()
        {
            ViewBag.EPLANT_ID = new SelectList(db.EPLANT, "ID", "NAME");
            return View();
        }

        // POST: S_USER_GENERAL/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "USER_NAME,AUTO_SHUT_TIME,EPLANT_ID,PO_LIMIT,EMAIL,RMA_LIMIT,PR_EMP_ID,PO_APPROVER,PO_CAP_LIMIT,PO_EXP_LIMIT,WEB_MASTER,PO_CANT_INCR_COST,AUTO_SHUT_ACTION_CODE,NOTIFICATION_PRINTER,DONT_CHANGE_POAP,PO_BUYER,PO_CAN_DENY,PHONE_NUMBER,AUTO_SHUT_SHOPDATA,LANGUAGE_CODE,FORCE_PASSWORD_CHANGE,IS_NEW_USER,NEW_USER_TIMESTAMP,SEC_PREVENT_SAME_USER_LOGIN,SEC_ALLOW_NON_DBA_SEC_INS,INV_ADJ_LIMIT,IS_MOBILE_USER,RECEIPT_TOLERANCE,AP_TOLERANCE,EMAIL_SIGNATURE,SHIP_MANAGER_WARNING,CUSER1,SP_PROFILE,SP_WORKSPACE,EXCLUDE_CUSTOMERS,EXCLUDE_VENDORS")] S_USER_GENERAL s_USER_GENERAL)
        {
            if (ModelState.IsValid)
            {
                db.S_USER_GENERAL.Add(s_USER_GENERAL);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EPLANT_ID = new SelectList(db.EPLANT, "ID", "NAME", s_USER_GENERAL.EPLANT_ID);
            return View(s_USER_GENERAL);
        }

        // GET: S_USER_GENERAL/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            S_USER_GENERAL s_USER_GENERAL = db.S_USER_GENERAL.Find(id);
            if (s_USER_GENERAL == null)
            {
                return HttpNotFound();
            }
            ViewBag.EPLANT_ID = new SelectList(db.EPLANT, "ID", "NAME", s_USER_GENERAL.EPLANT_ID);
            return View(s_USER_GENERAL);
        }

        // POST: S_USER_GENERAL/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "USER_NAME,AUTO_SHUT_TIME,EPLANT_ID,PO_LIMIT,EMAIL,RMA_LIMIT,PR_EMP_ID,PO_APPROVER,PO_CAP_LIMIT,PO_EXP_LIMIT,WEB_MASTER,PO_CANT_INCR_COST,AUTO_SHUT_ACTION_CODE,NOTIFICATION_PRINTER,DONT_CHANGE_POAP,PO_BUYER,PO_CAN_DENY,PHONE_NUMBER,AUTO_SHUT_SHOPDATA,LANGUAGE_CODE,FORCE_PASSWORD_CHANGE,IS_NEW_USER,NEW_USER_TIMESTAMP,SEC_PREVENT_SAME_USER_LOGIN,SEC_ALLOW_NON_DBA_SEC_INS,INV_ADJ_LIMIT,IS_MOBILE_USER,RECEIPT_TOLERANCE,AP_TOLERANCE,EMAIL_SIGNATURE,SHIP_MANAGER_WARNING,CUSER1,SP_PROFILE,SP_WORKSPACE,EXCLUDE_CUSTOMERS,EXCLUDE_VENDORS")] S_USER_GENERAL s_USER_GENERAL)
        {
            if (ModelState.IsValid)
            {
                db.Entry(s_USER_GENERAL).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EPLANT_ID = new SelectList(db.EPLANT, "ID", "NAME", s_USER_GENERAL.EPLANT_ID);
            return View(s_USER_GENERAL);
        }

        // GET: S_USER_GENERAL/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            S_USER_GENERAL s_USER_GENERAL = db.S_USER_GENERAL.Find(id);
            if (s_USER_GENERAL == null)
            {
                return HttpNotFound();
            }
            return View(s_USER_GENERAL);
        }

        // POST: S_USER_GENERAL/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            S_USER_GENERAL s_USER_GENERAL = db.S_USER_GENERAL.Find(id);
            db.S_USER_GENERAL.Remove(s_USER_GENERAL);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion


        // GET: User/AddUser
        public ActionResult AddUser()
        {
            try
            {
                FillAddUserDropdownLists();
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                ViewBag.ErrorDescription = "Unable to connect to database." + Environment.NewLine + e.Message;
                ViewBag.ErrorData = e.Data;
                return View("Error");
            }

            // Send a pre-populated model to the view
            IQMSUser model = new IQMSUser();
            return View(model);
        }

        // POST: Employee/AddUser
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser([Bind(Include = "Username,FirstName,LastName,Email,CopyPermissions,CopyFrom,EmpNo,EplantId,JobTitle,AddTeamMember,AddExpenseUser,ApproverUsername,EplantName")] IQMSUser userInfo)
        {

            #region Set Variables
            long empId = 0;
            long teamMemberId;
            string FN = userInfo.FirstName.ToUpper();
            string LN = userInfo.LastName.ToUpper();
            userInfo.CopyFrom = userInfo.CopyFrom.ToUpper();
            userInfo.Username = userInfo.Username.ToUpper();
            userInfo.Email = userInfo.Email.ToLower();
            string Ln = string.Format("{0}{1}", LN.Substring(0, 1), LN.ToLower().Remove(0, 1));
            string Fn = string.Format("{0}{1}", FN.Substring(0, 1), FN.ToLower().Remove(0, 1));
            string firstLast = FN + LN;
            userInfo.FirstName = Fn;
            userInfo.LastName = Ln;


            bool hasEmployeeErrors = false;

            try
            {
                FillAddUserDropdownLists();
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                ViewBag.ErrorDescription = "Unable to connect to database." + Environment.NewLine + e.Message;
                ViewBag.ErrorData = e.Data;
                return View("Error");
            }
            #endregion

            #region Validations

            // A team member must exist or be added if adding an expense user. Otherwise they cannot be added to the document team.
            if (userInfo.AddExpenseUser && !userInfo.AddTeamMember) userInfo.AddTeamMember = true;


            // if email is not in use on someone else
            var empsWithSameEmail = db.PR_EMP.Where(e => e.EMAIL.ToLower() == userInfo.Email);
            if (empsWithSameEmail.Any(i => i.FIRST_NAME.ToUpper() != FN && i.LAST_NAME.ToUpper() != LN))
            {
                ModelState.AddModelError("Email", String.Format("Email {0} has already been used for another employee.", userInfo.Email));
                hasEmployeeErrors = true;
            }

            // if employee# exists on another name, model error
            var empsWithSameEmpno = db.PR_EMP.Where(e => e.EMPNO == userInfo.EmpNo);
            if (empsWithSameEmpno.Any(i => i.FIRST_NAME.ToUpper() != FN && i.LAST_NAME.ToUpper() != LN))
            {
                ModelState.AddModelError("EmpNo", String.Format("Employee No. {0} has already been used for another employee.", userInfo.EmpNo));
                hasEmployeeErrors = true;
            }

            // if Copying permissions, make sure copyfrom is populated
            if (userInfo.CopyPermissions && String.IsNullOrEmpty(userInfo.CopyFrom))
            {
                ModelState.AddModelError("CopyFrom", "Please specify the user to copy permissions from");
                hasEmployeeErrors = true;
            }

            if (userInfo.AddExpenseUser && db.TEAM_MEMBER.Where(t => t.USERID == userInfo.ApproverUsername) == null)
            {
                ModelState.AddModelError("", "The Expense approver must first exist as a team member.");
                hasEmployeeErrors = true;
            }

            if (!ModelState.IsValid || hasEmployeeErrors)
            {
                return View(userInfo);
            }
            #endregion

            // Add (or get existing ) Employee record and return the Id
            empId = AddEmployee(FN, LN, userInfo.Email, userInfo.EmpNo);

            // Add or Update IQMS User Account
            // if anything fails here, return to the view because everything else depends on a valid username
            if (!AddIqmsUser(userInfo.Username, LN, userInfo.Email, userInfo.EplantId,
                userInfo.CopyPermissions, userInfo.CopyFrom, empId))
            {
                ViewBag.AddUserErrors = _addUserErrors;
                ViewBag.AddUserResults = _addUserResults;
                return View("AddUserSuccess", userInfo);
            }

            // Add as Team Member
            if (userInfo.AddTeamMember)
            {
                // If adding as an expense user and eplant is not 1, team member must be added with a null eplant
                long? eplantId = userInfo.EplantId;
                if (userInfo.AddExpenseUser && userInfo.EplantId != 1)
                {
                    eplantId = null;
                }
                teamMemberId = AddIqmsTeamMember(userInfo.Username, FN, LN, userInfo.Email,
                    userInfo.JobTitle, userInfo.CopyPermissions, userInfo.CopyFrom, eplantId);
            }

            // Add Expense User
            if (userInfo.AddExpenseUser)
            {
                // Add Vendor or get existing vendor Id
                long vendorId = GetExpenseVendorId(FN, LN);

                // Add Expense User
                AddExpenseUser(userInfo.Username, userInfo.ApproverUsername, empId, vendorId);

                // Add Doc Library (and library team)
                AddExpenseLibrary(Fn, Ln, userInfo.Username, userInfo.ApproverUsername);
            }
            ViewBag.AddUserErrors = _addUserErrors;
            ViewBag.AddUserResults = _addUserResults;
            return View("AddUserSuccess", userInfo);
        }

        private bool EmployeeNameExists(string firstName, string lastName)
        {
            using (IQMS_Entities context = new IQMS_Entities())
            {
                return context.PR_EMP
                    .Where(i => i.FIRST_NAME.ToUpper() == firstName &&
                        i.LAST_NAME.ToUpper() == lastName.ToUpper()).Count() > 0;

            }
        }

        private long AddEmployee(string firstName, string lastName, string email, string empNo)
        {
            long newEmpId = 0;

            // If an employee already exists, get the Id
            if (EmployeeNameExists(firstName, lastName))
            {
                // Just get the EmpID
                PR_EMP existingEmployee = new PR_EMP();
                existingEmployee = db.PR_EMP.Where(i => i.FIRST_NAME.ToUpper() == firstName && i.LAST_NAME.ToUpper() == lastName).FirstOrDefault();
                newEmpId = existingEmployee.ID;
                _addUserResults.Add(String.Format("The employee record for {0} {1} already existed with ID {2}.", firstName, lastName, newEmpId));

                // update email if missing
                if (string.IsNullOrEmpty(existingEmployee.EMAIL))
                {
                    existingEmployee.EMAIL = email;
                    _addUserResults.Add(String.Format("Added missing email address on the employee record {0} {1}", firstName, lastName));
                    db.SaveChanges();
                }
                return newEmpId;
            }

            // Add the Employee record
            string sql = "SELECT S_PR_EMP.NextVal as NewID from DUAL";
            newEmpId = db.Database.SqlQuery<long>(sql).FirstOrDefault();
            try
            {
                using (IQMS_Entities context = new IQMS_Entities())
                {
                    PR_EMP employee = new PR_EMP();
                    employee.ID = newEmpId;
                    employee.FIRST_NAME = firstName;
                    employee.LAST_NAME = lastName;
                    employee.SSNMBR = "111111111";
                    employee.EMPNO = empNo;
                    employee.EMAIL = email;
                    employee.BADGENO = empNo;
                    employee.PK_HIDE = "N";
                    context.PR_EMP.Add(employee);
                    context.SaveChanges();
                }
                _addUserResults.Add(String.Format("Added Employee record for {0} {1}", firstName, lastName));
                return newEmpId;
            }
            catch (Exception e)
            {
                _addUserErrors.Add(String.Format("Something went wrong adding the Employee Record: {0}{1}", Environment.NewLine, e.Message));
                return (long)0;
            }
        }

        private bool EmpNoExists(string empNo)
        {
            return db.PR_EMP.Any(i => i.EMPNO.ToUpper() == empNo.ToUpper());
        }

        private bool EmployeeEmailExists(string email)
        {
            return db.PR_EMP.Any(i => i.EMAIL.ToUpper() == email.ToUpper());
        }

        private bool UserExists(string un)
        {
            return db.S_USER_GENERAL.Any(i => i.USER_NAME.ToUpper() == un.ToUpper());
        }

        private bool AddIqmsUser(string username, string lastName, string email, long eplantId, bool copyPermissions, string copyFrom, long empId)
        {
            if (UserExists(username))
            {
                _addUserResults.Add(String.Format("A user record for {0} already existed and was not added.", username));
                // ensure email is populated
                S_USER_GENERAL existingUser = db.S_USER_GENERAL.Where(i => i.USER_NAME.ToUpper() == username).FirstOrDefault();
                if (string.IsNullOrEmpty(existingUser.EMAIL))
                {
                    existingUser.EMAIL = email;
                    db.SaveChanges();
                    _addUserResults.Add(String.Format("Added missing email address for user {0}", username));
                }
                return true;
            }
            try
            {
                string newPwdFormat = WebConfigurationManager.AppSettings["NewPwdFormat"];
                string newPassword = string.Format(newPwdFormat, lastName);
                List<String> sqlCommands = new List<string>();
                int noOfRowInserted;

                //string test = string.Format(@"create user {0} identified by {1} default tablespace USERS temporary tablespace TEMP", username, newPassword);
                sqlCommands.Add(string.Format(@"create user {0} identified by {1} default tablespace USERS temporary tablespace TEMP", username, newPassword));
                sqlCommands.Add(string.Format(@"grant connect to ""{0}"" ", username));
                sqlCommands.Add(string.Format(@"grant create session to ""{0}"" ", username));
                sqlCommands.Add(string.Format(@"grant select on iqorder2 to ""{0}"" ", username));
                sqlCommands.Add(string.Format(@"grant IQWEBDIRECT_ROLE to ""{0}"" ", username));
                sqlCommands.Add(string.Format(@"insert into s_user_general (user_name, force_password_change, pr_emp_id, auto_shut_time, eplant_id, email, cuser1, dont_change_poap, po_cant_incr_cost, rma_limit, inv_adj_limit, ap_tolerance, receipt_tolerance, auto_shut_action_code) 
                                values ('{0}', 'Y', {1}, 2700, {2}, '{3}', 'Copied from {4}', 'Y', 'Y', 0, 0, 0, 0, 0)", username, empId, eplantId, email, copyFrom));

                foreach (var sqlCommand in sqlCommands)
                {
                    noOfRowInserted = db.Database.ExecuteSqlCommand(sqlCommand);
                }
                _addUserResults.Add(string.Format("Added User record {0}", username));
                sqlCommands.Clear();

                if (copyPermissions)
                {
                    // roles
                    sqlCommands.Add(string.Format(@"delete from s_users where user_name = '{0}'", username));
                    sqlCommands.Add(string.Format(@"insert into s_users (user_name, role_name, s_group_id) select '{0}', role_name, s_group_id from s_users where user_name = '{1}'", username, copyFrom));
                    // scanner profiles
                    sqlCommands.Add(string.Format(@"delete from rf_profile where userid = '{0}'", username));
                    sqlCommands.Add(string.Format(@"insert into rf_profile (userid, module_name, topic, to_prompt, source_id, source, attribute ) 
                                        select '{0}', module_name, topic, to_prompt, source_id, source, attribute from rf_profile where userid = '{1}'", username, copyFrom));
                    // po types
                    sqlCommands.Add(string.Format(@"delete from s_user_po_type where user_name = '{0}'", username));
                    sqlCommands.Add(string.Format(@"insert into s_user_po_type ( po_type_id, limit, user_name, is_default ) 
                                        select po_type_id, limit, '{0}', is_default from s_user_po_type where user_name = '{1}'", username, copyFrom));
                    // accessible eplants
                    sqlCommands.Add(string.Format(@"delete from s_user_eplants where user_name = '{0}'", username));
                    sqlCommands.Add(string.Format(@"insert into s_user_eplants ( user_name, eplant_id ) 
                                        select '{0}', eplant_id from s_user_eplants where user_name = '{1}'", username, copyFrom));

                    foreach (var sqlCommand in sqlCommands)
                    {
                        noOfRowInserted = db.Database.ExecuteSqlCommand(sqlCommand);
                    }
                    _addUserResults.Add(@"Copied Groups, Roles, RF profile, PO Types and Eplants from " + copyFrom);

                }
            }
            catch (Exception e)
            {
                _addUserErrors.Add(String.Format("Something went wrong adding or updating USER {0}: {1}{2}", username, Environment.NewLine, e.Message));
                return false;
            }
            return true;
        }

        private long AddIqmsTeamMember(string username, string firstName, string lastName, string email, string jobTitle, bool copyPerms, string copyFrom, long? eplantId)
        {
            try
            {

                // If Team Member exists skip this and add a message to that effect
                TEAM_MEMBER teamMember = new TEAM_MEMBER();
                teamMember = db.TEAM_MEMBER.Where(t => t.USERID.ToUpper() == username).FirstOrDefault();

                if (teamMember != null)
                {
                    _addUserResults.Add(String.Format("A Team Member record already existed for {0} {1} and was not duplicated.", firstName, lastName));
                    return teamMember.ID;
                }
                else
                {
                    // Get Next Team Member ID
                    string sql = "Select S_TEAM_MEMBER.NextVal as NewID from DUAL";
                    long newTeamMemberId = db.Database.SqlQuery<long>(sql).FirstOrDefault();

                    TEAM_MEMBER newTeamMember = new TEAM_MEMBER();
                    newTeamMember.ID = newTeamMemberId;
                    newTeamMember.USERID = username;
                    newTeamMember.NAME = string.Format("{0} {1}", firstName, lastName);
                    newTeamMember.EMAIL = email;
                    newTeamMember.TITLE = jobTitle;
                    newTeamMember.USED_DOC = "Y";
                    if (eplantId.HasValue) { newTeamMember.EPLANT_ID = eplantId; }

                    // If copying from another user, copy over the team member settings
                    if (copyPerms)
                    {
                        TEAM_MEMBER copyTeamMember = new TEAM_MEMBER();
                        copyTeamMember = db.TEAM_MEMBER.Where(t => t.USERID == copyFrom).FirstOrDefault();

                        bool copiedTeamMemberInfo = false;
                        if (copyTeamMember != null)
                        {
                            copiedTeamMemberInfo = true;
                            newTeamMember.USED_AUDIT = copyTeamMember.USED_AUDIT;
                            newTeamMember.USED_CAR = copyTeamMember.USED_CAR;
                            newTeamMember.USED_MRB = copyTeamMember.USED_MRB;
                            newTeamMember.USED_PO = copyTeamMember.USED_PO;
                        }
                        db.TEAM_MEMBER.Add(newTeamMember);
                        db.SaveChanges();
                        _addUserResults.Add(String.Format("Added Team Member record for {0} {1}", firstName, lastName));

                        if (copiedTeamMemberInfo)
                        {
                            _addUserResults.Add("Copied Team member settings from " + copyTeamMember.NAME);
                        }
                        else
                        {
                            _addUserResults.Add(string.Format("* A Team Member for user {0} did not exist so Settings and Doc Team membership were NOT copied", copyFrom));
                        }

                        // If a copyFrom Team Member exists, copy doc team membership
                        if (copyTeamMember != null)
                        {
                            List<String> sqlCommands = new List<string>();
                            int noOfRowInserted;
                            sqlCommands.Add(string.Format(@"delete from doc_team where team_member_id = {0}", newTeamMemberId));
                            sqlCommands.Add(string.Format(@"insert into doc_team ( doc_library_id, team_member_id, type, seq ) 
                                        select dt.doc_library_id, {0}, dt.type, dt.seq 
                                        from doc_team dt join doc_library dl on dl.id = dt.doc_library_id
                                        where dt.team_member_id = {1} and upper(dl.descrip) not like ('%{2}%')", newTeamMemberId, copyTeamMember.ID, copyTeamMember.USERID.Remove(0, 1)));
                            foreach (var sqlCommand in sqlCommands)
                            {
                                noOfRowInserted = db.Database.ExecuteSqlCommand(sqlCommand);
                            }
                            sqlCommands.Clear();
                            _addUserResults.Add(string.Format("Copied new user to all the same Document Libraries as {0}, except personal expense libraries", copyTeamMember.NAME));
                        }
                    }
                    return newTeamMemberId;
                }

            }
            catch (Exception e)
            {
                _addUserErrors.Add(String.Format("Something went wrong adding {0} {1} as a Team Member: {2}{3}", firstName, lastName, Environment.NewLine, e.Message));
                return (long)0;
            }
        }

        private long GetExpenseVendorId(string firstName, string lastName)
        {
            try
            {
                // If a vendor already exists, get the vendor Id.  Otherwise add a vendor.
                if (db.VENDOR.Any(v => (v.COMPANY.ToUpper().StartsWith(lastName + ",") || v.COMPANY.ToUpper().StartsWith(lastName + ";")) && v.COMPANY.ToUpper().Contains(firstName)))
                {
                    var existingVendor = new VENDOR();
                    existingVendor = db.VENDOR.Where(v => (v.COMPANY.ToUpper().StartsWith(lastName + ",") || v.COMPANY.ToUpper().StartsWith(lastName + ";")) && v.COMPANY.Contains(firstName)).FirstOrDefault();
                    return existingVendor.ID;
                }

                // Find an unused vendorno.  First 5 characters of last name, plus 3 digit sequence
                int iSeq = 0;
                string vendorNo = string.Empty;
                string company = string.Format("{0}; {1}", lastName, firstName);
                string lastFirst = lastName + firstName;

                do
                {
                    iSeq++;
                    int prefixLength = (lastFirst.Length > 5 ? 5 : lastFirst.Length);
                    vendorNo = lastFirst.Substring(0, prefixLength) + iSeq.ToString("D3");
                } while (db.VENDOR.Any(v => v.VENDORNO.Trim() == vendorNo));

                string sql = "Select S_VENDOR.NextVal as NewID from DUAL";
                long newVendorId = db.Database.SqlQuery<long>(sql).FirstOrDefault();

                VENDOR vendor = new VENDOR();
                vendor.ID = newVendorId;
                vendor.VENDORNO = vendorNo;
                vendor.ADDR3 = "EXPENSE REPORTS";
                vendor.COMPANY = company;
                vendor.AP_GLACCT_ID = 149;  // All expense users are reported in eplant 1
                vendor.EPLANT_ID = 1;
                vendor.USE_USA_MASK = "N";

                db.VENDOR.Add(vendor);

                sql = "Select S_REMIT_TO.NextVal as NewID from DUAL";
                long newRemitId = db.Database.SqlQuery<long>(sql).FirstOrDefault();

                REMIT_TO remit = new REMIT_TO();
                remit.ID = newRemitId;
                remit.VENDOR_ID = newVendorId;
                remit.VENDOR = vendor;
                remit.ATTN = vendor.COMPANY;
                remit.ADDR3 = vendor.ADDR3;
                remit.USE_USA_MASK = "N";
                remit.AP_REMITTANCE_TYPE = "EFT";
                db.REMIT_TO.Add(remit);
                db.SaveChanges();
                _addUserResults.Add(string.Format("Added user as vendor {0} - {1}", vendorNo, company));
                return newVendorId;
            }
            catch (Exception e)
            {
                _addUserErrors.Add(String.Format("Something went wrong adding the Employee Vendor Record: {0}{1}", Environment.NewLine, e.Message));
                return (long)0;
            }
        }

        private void AddExpenseUser(string username, string approverUsername, long empId, long vendorId)
        {
            // If the vendor creation failed (vendorId=0), return false. Do not add team or 
            if (vendorId == 0)
            {
                _addUserErrors.Add(string.Format("Expense User record for {0} could not be created without a Vendor record.", username));
                return;
            }

            // Check if a vendor already exists
            EXP_USER expUser = new EXP_USER();
            expUser = db.EXP_USER.Where(e => e.USER_NAME == username).FirstOrDefault();
            try
            {
                if (expUser == null)
                {
                    // Get the next ID
                    string sql = "Select S_exp_user.NextVal as NewID from DUAL";
                    long newExpUserId = db.Database.SqlQuery<long>(sql).FirstOrDefault();

                    expUser = new EXP_USER();
                    expUser.ID = newExpUserId;
                    expUser.USER_NAME = username;
                    expUser.PR_EMP_ID = empId;
                    expUser.VENDOR_ID = vendorId;
                    expUser.EXP_APPROVER = approverUsername;
                    expUser.MILEAGE_RATE = Convert.ToDecimal(.51);
                    db.EXP_USER.Add(expUser);
                    db.SaveChanges();
                    _addUserResults.Add(string.Format("Added expense user record for user {0}, vendor {1}", username, expUser.VENDOR.VENDORNO));
                }
                else
                {
                    _addUserResults.Add(string.Format("The expense user already existed and was NOT added."));

                    // If EmpID is missing, add it...
                    if (expUser.PR_EMP_ID == null)
                    {
                        expUser.PR_EMP_ID = empId;
                        db.SaveChanges();
                        _addUserResults.Add(string.Format("Updated expense user with the new Employee ID."));
                    }
                }
            }
            catch (Exception e)
            {
                _addUserErrors.Add(string.Format("Something went wrong adding the Expense User: {0}{1}", Environment.NewLine, e.Message));
            }
        }

        private void AddExpenseLibrary(string Fn, string Ln, string username, string approverUsername)
        {
            // Get doc path out of the settings in web config
            string lastCommaFirst = string.Format("{0}, {1}", Ln, Fn);
            string path = WebConfigurationManager.AppSettings["PlainfieldExpenseDocPath"];
            string libDescrip = string.Format("Expense - {0}", lastCommaFirst);
            string fullPath = path + lastCommaFirst;
            long newDocLibId = 0;

            // Ensure library record does not exist
            if (db.DOC_LIBRARY.Any(d => d.DESCRIP.ToUpper() == libDescrip.ToUpper())
                || db.DOC_LIBRARY.Any(d => d.PATH == fullPath)
                )
            {
                _addUserResults.Add(string.Format("An Expense library or path already existed and was NOT added."));
                return;
            }

            #region Create the Doc Library
            try
            {
                string sql = "Select S_DOC_LIBRARY.NextVal as NewID from DUAL";
                newDocLibId = db.Database.SqlQuery<long>(sql).FirstOrDefault();

                DOC_LIBRARY docLib = new DOC_LIBRARY();
                docLib.ID = newDocLibId;
                docLib.DESCRIP = libDescrip;
                docLib.PATH = path + lastCommaFirst;
                docLib.IS_CONFIDENTIAL = "Y";
                docLib.TEAM_MEMBER_ONLY = "Y";
                db.DOC_LIBRARY.Add(docLib);
                db.SaveChanges();
                _addUserResults.Add(string.Format("Added Expense Library [{0}], Id={1}", libDescrip, newDocLibId));
            }
            catch (Exception e)
            {
                _addUserErrors.Add(string.Format("* FAILED adding the Expense Doc Library: {0}{1}", Environment.NewLine, e.Message));
            }
            #endregion

            try
            {
                #region Add Doc Team Members
                // create a list of doc team members to add
                string docTeamUserIds = WebConfigurationManager.AppSettings["PlainfieldDefaultDocTeamIds"];
                List<long> docTeamList = docTeamUserIds.Split(',').Select(long.Parse).ToList();

                TEAM_MEMBER teamMemberApprover = new TEAM_MEMBER();
                teamMemberApprover = db.TEAM_MEMBER.Where(t => t.USERID == approverUsername).FirstOrDefault();

                TEAM_MEMBER teamMemberUser = new TEAM_MEMBER();
                teamMemberUser = db.TEAM_MEMBER.Where(t => t.USERID == username).FirstOrDefault();

                if (teamMemberApprover != null) docTeamList.Add(teamMemberApprover.ID);
                if (teamMemberUser != null) docTeamList.Add(teamMemberUser.ID);
                //int listCount = docTeamList.Count();  //debug

                byte seq = 0;
                foreach (var docTeamUserId in docTeamList)
                {
                    seq++;
                    string sql = "Select S_DOC_TEAM.NextVal as NewID from DUAL";
                    long newDocTeamId = db.Database.SqlQuery<long>(sql).FirstOrDefault();

                    DOC_TEAM docTeamMember = new DOC_TEAM();
                    docTeamMember.ID = newDocTeamId;
                    docTeamMember.DOC_LIBRARY_ID = newDocLibId;
                    docTeamMember.TEAM_MEMBER_ID = docTeamUserId;
                    docTeamMember.SEQ = seq;
                    docTeamMember.TYPE = "Access";
                    db.DOC_TEAM.Add(docTeamMember);
                }
                db.SaveChanges();
                _addUserResults.Add("Added Doc Team members to expense library.");
                #endregion

                #region Add the network folder
                try
                {
                    if (!AddExpenseDirectory(fullPath))
                    {
                        _addUserResults.Add(string.Format("The network folder [{0}] already existed.", fullPath));
                    }
                    else
                    {
                        _addUserResults.Add(string.Format("Added network folder [{0}].", fullPath));

                        // Add AD Group
                        string expenseGroupName = string.Format("{0} {1} - Expense", Fn, Ln);
                        string addExpenseGroupResult = ActiveDirectory.AddExpenseGroup(expenseGroupName, username, approverUsername);
                        _addUserResults.Add(addExpenseGroupResult);
                        if (addExpenseGroupResult.Contains("EXCEPT expense approver"))
                        {
                            _addUserErrors.Add(string.Format("Expense Approver {0} did not exist in AD and was not added to the AD Expense Group.", approverUsername));
                        }
                        if (addExpenseGroupResult.Contains("EXCEPT user"))
                        {
                            _addUserErrors.Add(string.Format("Expense User {0} did not exist in AD and was not added to the AD Expense Group.", username));
                        }


                        // Apply Folder Permissions and report results
                        string addGroupSecurityResult = ActiveDirectory.GrantModifyOnPathToGroup(fullPath, expenseGroupName);
                        _addUserResults.Add(addGroupSecurityResult);

                    }

                }
                catch (Exception e)
                {
                    _addUserErrors.Add(string.Format("* FAILED adding the Folder Path {0}: {1}{2}", path + lastCommaFirst, Environment.NewLine, e.Message));
                }
                #endregion
            }
            catch (Exception e)
            {
                _addUserErrors.Add(string.Format("Something went wrong adding the DOC TEAM to library {0}: {1}{2}", libDescrip, Environment.NewLine, e.Message));
            }

        }

        private void FillAddUserDropdownLists()
        {
            // build dropdownlists  
            ViewBag.EplantId = GetEplantSelectList();
            ViewBag.CopyFrom = GetUserSelectList();
            ViewBag.ApproverUsername = ViewBag.CopyFrom;
        }

        private List<SelectListItem> GetEplantSelectList()
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            var excludedEplants = new long[] { 2, 5, 10, 31, 3, 7 };

            foreach (EPLANT eplant in db.EPLANT.Where(e => !excludedEplants.Contains(e.ID)))
            {
                SelectListItem selectListItem = new SelectListItem
                {
                    Text = eplant.NAME,
                    Value = eplant.ID.ToString(),
                    Selected = eplant.ID == 1 ? true : false
                };
                selectListItems.Add(selectListItem);
            }
            return selectListItems;
        }

        private List<SelectListItem> GetUserSelectList()
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();

            foreach (S_USER_GENERAL user in db.S_USER_GENERAL
                .Where(u => (u.EPLANT_ID == 1 || u.EPLANT_ID == 6) && u.FORCE_PASSWORD_CHANGE != "Y")
                .OrderBy(u => u.USER_NAME))
            {
                SelectListItem selectListItem = new SelectListItem
                {
                    Text = user.USER_NAME,
                    Value = user.USER_NAME,
                };
                selectListItems.Add(selectListItem);
            }
            return selectListItems;
        }

        /// <summary>
        /// Returns false if a directory already exists and is not added.
        /// Otherwise, it adds the directory and returns true.
        /// </summary>
        /// <param name="pathname"></param>
        /// <returns></returns>
        private bool AddExpenseDirectory(string pathname)
        {
            try
            {
                // Determine whether the directory exists. 
                if (Directory.Exists(pathname))
                {
                    return false;
                }
                else
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(pathname);
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
